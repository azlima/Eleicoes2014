namespace Eleicoes2014.View.Estados
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Windows;
    using System.Windows.Navigation;
    using Eleicoes2014.Utils;
    using Eleicoes2014.ViewModel;
    using Microsoft.Phone.Controls;
    using Model;
    using Eleicoes2014.Model.Enums;

    public partial class EstadoView : PhoneApplicationPage
    {
        ConfigValue configValue = new ConfigValue();
        EstadoViewModel viewModel = new EstadoViewModel();
        CandidatoViewModel candidatoViewModel = new CandidatoViewModel();
        string cargoId, cargoNome;

        public EstadoView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NavigationContext.QueryString.TryGetValue("cargoId", out cargoId);
            NavigationContext.QueryString.TryGetValue("cargoNome", out cargoNome);
            PageTitle.Text = cargoNome.ToLower();
            if (estadosListagem.Items.Count() == 0)
            {
                LoadPage();
            }
        }

        private void LoadPage()
        {
            DisablePage();
            LoadEstados();
        }

        private void DisablePage()
        {
            this.EstadosListagemStackPanel.Visibility = Visibility.Collapsed;
            this.displayProgresso.Visibility = Visibility.Visible;
            this.barraProgresso.Visibility = Visibility.Visible;
            this.barraProgresso.IsIndeterminate = true;
        }

        private void EnablePage()
        {
            this.displayProgresso.Visibility = Visibility.Collapsed;
            this.barraProgresso.Visibility = Visibility.Collapsed;
            this.barraProgresso.IsIndeterminate = false;
            this.EstadosListagemStackPanel.Visibility = Visibility.Visible;
        }

        private void LoadEstados()
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            client.Headers["App-Token"] = configValue.ApplicationIdForTransparenciaBrasil;

            Uri uri = new Uri("http://api.transparencia.org.br/sandbox/v1/estados");
            client.DownloadStringAsync(uri);
        }

        private void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(configValue.ErrorMessage);
                EnablePage();
                return;
            }
            string json = e.Result;
            estadosListagem.DataContext = viewModel.GetEstados(json).Where(x => x.estadoId != 0).OrderBy(x => x.nome);
            EnablePage();
        }

        private void estadosListagem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (estadosListagem.SelectedIndex >= 0)
            {
                Estado estadoSelecionado = estadosListagem.SelectedItem as Estado;
                LoadCandidatosToNextPage(cargoId, estadoSelecionado.sigla);
            }
        }

        private void LoadCandidatosToNextPage(string cargo, string estado)
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_Candidatos_DownloadStringCompleted);
            client.Headers["App-Token"] = configValue.ApplicationIdForTransparenciaBrasil;

            switch (int.Parse(cargo))
            {
                case (int)CargoTipo.DeputadoFederal:
                case (int)CargoTipo.DeputadoEstadual:
                case (int)CargoTipo.DeputadoDistrital:
                    NavigationService.Navigate(new Uri(string.Format("/View/Candidatos/CandidatoListagemView.xaml?cargoId={0}&cargoNome={1}&estado={2}", cargoId, cargoNome, estado.ToLower()), UriKind.RelativeOrAbsolute));
                    break;
                case (int)CargoTipo.Presidente:
                case (int)CargoTipo.Governador:
                case (int)CargoTipo.Senador:
                    Uri uri = new Uri(string.Format("http://api.transparencia.org.br/sandbox/v1/candidatos?cargo={0}&estado={1}", cargo, estado));
                    client.DownloadStringAsync(uri);
                    break;
            }
        }

        private void client_Candidatos_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(configValue.ErrorMessage);
                EnablePage();
                return;
            }
            string json = e.Result;
            var candidatos = candidatoViewModel.GetCandidatos(json).OrderBy(x => x.apelido).ToList();
            var estadoSelecionado = estadosListagem.SelectedItem as Estado;

            if (candidatos.Count() == 1)
            {
                Candidato candidatoSelecionado = candidatos[0] as Candidato;
                NavigationService.Navigate(new Uri(string.Format("/View/Candidatos/CandidatoDetalheView.xaml?candidatoId={0}&cargoId={1}&cargoNome={2}&estado={3}", candidatoSelecionado.id.ToString(), cargoId, cargoNome, estadoSelecionado.sigla.ToLower()), UriKind.RelativeOrAbsolute));
            }

            else if (candidatos.Count() == 2)
            {
                NavigationService.Navigate(new Uri(string.Format("/View/Candidatos/CandidatoSegundoTurnoView.xaml?cargoId={0}&cargoNome={1}&estado={2}", cargoId, cargoNome, estadoSelecionado.sigla.ToLower()), UriKind.RelativeOrAbsolute));
            }
        }

        private void atualizarBotao_Click(object sender, EventArgs e)
        {
            LoadPage();
        }
    }
}