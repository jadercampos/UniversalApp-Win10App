using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using System.Xml.Linq;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UniversalApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static string urlMapa { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var httpClient = new Windows.Web.Http.HttpClient())
            {
                Uri url = new Uri("http://cep.republicavirtual.com.br/web_cep.php?cep=" + textBox.Text.Replace("-", "").Trim() + "&formato=xml");
                HttpResponseMessage teste = await httpClient.GetAsync(url);
                if (teste.StatusCode == HttpStatusCode.Ok)
                {

                    XElement el = XElement.Parse(teste.Content.ToString());
                    if (el.HasElements)
                    {
                        Windows.UI.Popups.MessageDialog msg = new Windows.UI.Popups.MessageDialog("Achei o endereço, mas estou tentando evitar a fadiga!!!", "Oi, eu sou o Jaiminho:");
                        await msg.ShowAsync();
                        textBlock.Text = string.Format("{0} {1} \n{2}\n{3} - {4}", el.Element("tipo_logradouro").Value, el.Element("logradouro").Value, el.Element("bairro").Value, el.Element("cidade").Value, el.Element("uf").Value);
                        urlMapa = string.Format("https://www.google.com.br/maps/place/{0}+{1}+-+{2},{3}+-+{4}", el.Element("tipo_logradouro").Value, el.Element("logradouro").Value, el.Element("bairro").Value, el.Element("cidade").Value, el.Element("uf").Value);
                        MapaButton.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void MapaButton_Click(object sender, RoutedEventArgs e)
        {
            wv.Visibility = Visibility.Visible;
            wv.Navigate(new Uri(urlMapa));
        }
    }
}
