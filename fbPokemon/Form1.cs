using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace fbPokemon
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        
        // Na url abaixo, iremos retorna 100 pokemons por vez.
        const string url = "http://pokeapi.co/api/v2/pokemon?limit=100";
        

        public async Task<List<Result>> ObterPokemons()
        {
            List<Result> resultado = null;

            using (var httpClient = new System.Net.Http.HttpClient())
            {
                var jsonConteudo = await httpClient.GetStringAsync(url);

                // Aqui, é a raiz do conteúdo json.
                RootObject raizJson = JsonConvert.DeserializeObject<RootObject>(jsonConteudo);

                // Zerar lista.
                resultado?.Clear();
                resultado = raizJson.results;



                // Vamos obter todos os pokemons.
                while (raizJson.next != null)
                {
                    // Vai para o proximo.
                    jsonConteudo = await httpClient.GetStringAsync(raizJson.next);
                    raizJson = JsonConvert.DeserializeObject<RootObject>(jsonConteudo);
                    resultado.AddRange(raizJson.results);

                    this.Text = raizJson.next;
                }

            }

            return resultado;
        }




        private async void button1_Click(object sender, EventArgs e)
        {
            List<Result> resultado = await ObterPokemons();


            if(resultado != null)
            {
                foreach(var cadaResultado in resultado)
                {
                    listBox1.Items.Add(cadaResultado.name + ", " + cadaResultado.url);
                }
            }

            MessageBox.Show("Executado com sucesso!!!", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
    }
}
