namespace Eleicoes2014.View.Candidatos
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Eleicoes2014.Model;
    using Eleicoes2014.Utils;
    using Eleicoes2014.ViewModel;

    public partial class CandidatoPesquisaView : UserControl
    {
        public string emptyText = "Número ou nome...";
        public string todosPartidosText = "Todos os partidos...";

        ConfigValue configValue = new ConfigValue();
        PartidoViewModel viewModel = new PartidoViewModel();
        string cargoId, cargoNome, estado;

        public CandidatoPesquisaView()
        {
            InitializeComponent();
            Uri uri;
            if ((Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"] == Visibility.Visible)
            {
                uri = new Uri("/View/Contents/Images/Icons/dark/appbar.feature.search.rest.png", UriKind.Relative);
            }
            else
            {
                uri = new Uri("/View/Contents/Images/Icons/light/appbar.feature.search.rest.png", UriKind.Relative);
            }
            pesquisarImagem.Source = new BitmapImage(uri);

            if (partidosListagem.Items.Count() == 0)
            {
                LoadPartidos();
            }
        }

        public void LoadPartidos()
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            client.Headers["App-Token"] = configValue.ApplicationIdForTransparenciaBrasil;

            Uri uri = new Uri("http://api.transparencia.org.br/sandbox/v1/partidos");
            client.DownloadStringAsync(uri);
        }

        private void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(configValue.ErrorMessage);
                return;
            }
            string json = e.Result;
            partidosListagem.DataContext = viewModel.GetPartidos(json).OrderBy(x => x.sigla);
        }

        private void numeroOuNomeTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (numeroOuNomeTextBox.Text == emptyText)
            {
                numeroOuNomeTextBox.Text = string.Empty;
                SolidColorBrush Brush1 = new SolidColorBrush();
                Brush1.Color = Colors.Black;
                numeroOuNomeTextBox.Foreground = Brush1;
            }
        }

        private void numeroOuNomeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (numeroOuNomeTextBox.Text == String.Empty)
            {
                numeroOuNomeTextBox.Text = emptyText;
                SolidColorBrush Brush2 = new SolidColorBrush();
                Brush2.Color = Colors.Gray;
                numeroOuNomeTextBox.Foreground = Brush2;
            }
        }

        private void partidosBotao_Click(object sender, RoutedEventArgs e)
        {
            if (partidosListagem.Visibility == Visibility.Visible)
            {
                partidosListagem.SelectedIndex = -1;
                partidosListagem.Visibility = Visibility.Collapsed;
                numeroOuNomeTextBox.Visibility = Visibility.Visible;
                pesquisarImagem.Visibility = Visibility.Visible;
            }
            else if (partidosListagem.Visibility == Visibility.Collapsed)
            {
                numeroOuNomeTextBox.Visibility = Visibility.Collapsed;
                pesquisarImagem.Visibility = Visibility.Collapsed;
                partidosListagem.Visibility = Visibility.Visible;
            }
            partidosBotao.Content = todosPartidosText;
        }

        private void partidosListagem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            partidosListagem.Visibility = Visibility.Collapsed;
            numeroOuNomeTextBox.Visibility = Visibility.Visible;
            pesquisarImagem.Visibility = Visibility.Visible;
            if (partidosListagem.SelectedIndex >= 0)
            {
                var partidoSelecionado = partidosListagem.SelectedItem as Partido;
                partidosBotao.Content = partidoSelecionado.sigla;
            }
        }
    }
}
