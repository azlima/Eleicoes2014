namespace Eleicoes2014.View.Candidatos
{
    using Eleicoes2014.Utils;
    using Eleicoes2014.ViewModel;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;

    public partial class CandidatoDetalheUserControl : UserControl
    {
        ConfigValue configValue = new ConfigValue();
        CandidatoViewModel viewModel = new CandidatoViewModel();

        public CandidatoDetalheUserControl()
        {
            InitializeComponent();
        }

        public void LoadBens(string candidatoId)
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompletedCandidatoBens);
            client.Headers["App-Token"] = configValue.ApplicationIdForTransparenciaBrasil;

            Uri uri = new Uri(string.Format("http://api.transparencia.org.br/sandbox/v1/candidatos/{0}/bens", candidatoId));
            client.DownloadStringAsync(uri);
        }

        private void client_DownloadStringCompletedCandidatoBens(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(configValue.ErrorMessage);
                return;
            }
            string json = e.Result;
            var bens = viewModel.GetCandidatoBens(json);
            bensListagem.DataContext = bens;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
            montanteTotalTextBlock.Text = bens.Sum(x => x.montante).ToString("C");
        }

        public void LoadCandidaturas(string candidatoId)
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompletedCandidatoCandidaturas);
            client.Headers["App-Token"] = configValue.ApplicationIdForTransparenciaBrasil;

            Uri uri = new Uri(string.Format("http://api.transparencia.org.br/sandbox/v1/candidatos/{0}/candidaturas", candidatoId));
            client.DownloadStringAsync(uri);
        }

        private void client_DownloadStringCompletedCandidatoCandidaturas(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(configValue.ErrorMessage);
                return;
            }
            string json = e.Result;
            candidaturasListagem.DataContext = viewModel.GetCandidatoCandidaturas(json);
        }

        public void LoadEstatisticas(string candidatoId)
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompletedCandidatoEstatisticas);
            client.Headers["App-Token"] = configValue.ApplicationIdForTransparenciaBrasil;

            Uri uri = new Uri(string.Format("http://api.transparencia.org.br/sandbox/v1/candidatos/{0}/estatisticas", candidatoId));
            client.DownloadStringAsync(uri);
        }

        private void client_DownloadStringCompletedCandidatoEstatisticas(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(configValue.ErrorMessage);
                return;
            }
            string json = e.Result;
            estatisticasListagem.DataContext = viewModel.GetCandidatoEstatisticas(json);
        }

        private void mostrar_Processos_Botao_Click(object sender, RoutedEventArgs e)
        {
            mostrar_Processos_Botao.Visibility = Visibility.Collapsed;
            processosListagem.Visibility = Visibility.Visible;
            ocultar_Processos_Botao.Visibility = Visibility.Visible;
            processosTextBlock.Visibility = (processosListagem.Items.Count == 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ocultar_Processos_Botao_Click(object sender, RoutedEventArgs e)
        {
            mostrar_Processos_Botao.Visibility = Visibility.Visible;
            processosListagem.Visibility = Visibility.Collapsed;
            ocultar_Processos_Botao.Visibility = Visibility.Collapsed;
            processosTextBlock.Visibility = Visibility.Collapsed;
        }

        private void mostrar_Bens_Botao_Click(object sender, RoutedEventArgs e)
        {
            mostrar_Bens_Botao.Visibility = Visibility.Collapsed;
            bensListagem.Visibility = Visibility.Visible;
            ocultar_Bens_Botao.Visibility = Visibility.Visible;
            bensTextBlock.Visibility = (bensListagem.Items.Count == 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ocultar_Bens_Botao_Click(object sender, RoutedEventArgs e)
        {
            mostrar_Bens_Botao.Visibility = Visibility.Visible;
            bensListagem.Visibility = Visibility.Collapsed;
            ocultar_Bens_Botao.Visibility = Visibility.Collapsed;
            bensTextBlock.Visibility = Visibility.Collapsed;
        }

        private void mostrar_Candidaturas_Botao_Click(object sender, RoutedEventArgs e)
        {
            mostrar_Candidaturas_Botao.Visibility = Visibility.Collapsed;
            candidaturasListagem.Visibility = Visibility.Visible;
            ocultar_Candidaturas_Botao.Visibility = Visibility.Visible;
            candidaturasTextBlock.Visibility = (candidaturasListagem.Items.Count == 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ocultar_Candidaturas_Botao_Click(object sender, RoutedEventArgs e)
        {
            mostrar_Candidaturas_Botao.Visibility = Visibility.Visible;
            candidaturasListagem.Visibility = Visibility.Collapsed;
            ocultar_Candidaturas_Botao.Visibility = Visibility.Collapsed;
            candidaturasTextBlock.Visibility = Visibility.Collapsed;
        }

        private void mostrar_Estatisticas_Botao_Click(object sender, RoutedEventArgs e)
        {
            mostrar_Estatisticas_Botao.Visibility = Visibility.Collapsed;
            estatisticasListagem.Visibility = Visibility.Visible;
            ocultar_Estatisticas_Botao.Visibility = Visibility.Visible;
            estatisticasTextBlock.Visibility = (estatisticasListagem.Items.Count == 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ocultar_Estatisticas_Botao_Click(object sender, RoutedEventArgs e)
        {
            mostrar_Estatisticas_Botao.Visibility = Visibility.Visible;
            estatisticasListagem.Visibility = Visibility.Collapsed;
            ocultar_Estatisticas_Botao.Visibility = Visibility.Collapsed;
            estatisticasTextBlock.Visibility = Visibility.Collapsed;
        }
    }
}
