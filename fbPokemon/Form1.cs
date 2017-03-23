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
                
        // Na url abaixo, eu coloquei o limite de 100, ou seja, a chamada 
        // chamada vai retornar 100 pokemons.
        const string url = "http://pokeapi.co/api/v2/pokemon?limit=100";
        
        public async Task<List<Result>> ObterPokemons()
        {
            List<Result> resultado = null;

            using (var httpClient = new System.Net.Http.HttpClient())
            {
                var jsonConteudo = await httpClient.GetStringAsync(url);

                // Aqui, é a raiz do conteúdo json.
                // Quando eu chamo o json pela primeira vez, há uma propriedade 'next' que indica
                // a url que retorna json do próximo pokemons.
                // Então no loop abaixo no while, eu uso isto para ir para os próximos pokemons.
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
