using CRUD.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace CRUD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();          
        }


        private void Button_ClickTeste(object sender, RoutedEventArgs e)
        {
            string filePath = string.Empty;

            OpenFileDialog dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                AddExtension = true,
                DereferenceLinks = true,
                Filter = "Arquivos (.csv)|*.csv",
                DefaultExt = ".csv"
            };

            dialog.ShowDialog();


            filePath = dialog.FileName;

            var ext = System.IO.Path.GetExtension(filePath);

            List<ProradisPessoa> pessoas = new List<ProradisPessoa>();
          
            var file = File.OpenRead(filePath);
            var reader = new StreamReader(file);

            string split = ",";

            while (!reader.EndOfStream)
            {

                var line = reader.ReadLine();
                string[] values;

                values = line.Split(new[] { split }, StringSplitOptions.None);

                string nome = RemoveAcentos(values[0]);
                string estado = RemoveAcentos(values[1]);
                int idade = Convert.ToInt32(values[2]);

                pessoas.Add(new ProradisPessoa { Nome = nome, Idade = idade, Estado = estado });

            }

            var group = pessoas.GroupBy(
                     p => p.Estado,
                     (estado, pessoa) => new Media
                     {
                         Cidade = estado,
                         Idade = Math.Round(pessoa.Average(p => p.Idade), 2)
                     });


            Medias medias = new Medias();
            medias.medias = new Media[group.Count()];
            int i = 0;

            foreach (var item in group)
            {
                medias.medias[i] = item;
                i++;
            }

            string jsonString = JsonConvert.SerializeObject(medias);

           var resposta =  Post(jsonString);

            string texto;

            if (resposta.StatusDescription.Equals("OK"))
            {
                texto = "Parabéns. Exercício concluído com sucesso";
            }
            else
            {
                texto = "Essa não é a resposta esperada";
            }

           var mensagem = System.Windows.Forms.MessageBox.Show(texto,"Resultado", MessageBoxButtons.OK);
        }

        public string RemoveAcentos(string text)
        {
            StringBuilder retorno = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letra in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letra) != UnicodeCategory.NonSpacingMark)
                    retorno.Append(letra);
            }
            return retorno.ToString().ToLower();
        }

        public static IRestResponse Post(string pdf)
        {

            var client = new RestClient("https://zeit-endpoint.brmaeji.now.sh/api/avg");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddJsonBody(pdf);


            IRestResponse response = client.Execute(request);

            return response;
        }
    }
}
