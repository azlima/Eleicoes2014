namespace Eleicoes2014.View.Cargos
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using Eleicoes2014.Model;
    using Eleicoes2014.Model.Enums;
    using Eleicoes2014.Utils;
    using Eleicoes2014.ViewModel;

    public partial class CargoView : UserControl
    {
        ConfigValue configValue = new ConfigValue();
        CargoViewModel viewModel = new CargoViewModel();
        CandidatoViewModel candidatoViewModel = new CandidatoViewModel();

        public CargoView()
        {
            InitializeComponent();
            if (cargosListagem.Items.Count() == 0)
            {
                LoadPage();
            }
        }

        public void LoadPage()
        {
            DisablePage();
            LoadCargos();
        }

        private void DisablePage()
        {
            this.CargosListagemStackPanel.Visibility = Visibility.Collapsed;
            this.displayProgresso.Visibility = Visibility.Visible;
            this.barraProgresso.Visibility = Visibility.Visible;
            this.barraProgresso.IsIndeterminate = true;
        }

        private void EnablePage()
        {
            this.displayProgresso.Visibility = Visibility.Collapsed;
            this.barraProgresso.Visibility = Visibility.Collapsed;
            this.barraProgresso.IsIndeterminate = false;
            this.CargosListagemStackPanel.Visibility = Visibility.Visible;
        }

        public void LoadCargos()
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            client.Headers["App-Token"] = configValue.ApplicationIdForTransparenciaBrasil;

            Uri uri = new Uri("http://api.transparencia.org.br/sandbox/v1/cargos");
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
            cargosListagem.DataContext = viewModel.GetCargos(json).OrderBy(x => x.cargoId);
            EnablePage();
        }

        private void cargosListagem_Tap(object sender, GestureEventArgs e)
        {
            if (cargosListagem.SelectedIndex >= 0)
            {
                Page page = GetDependencyObjectFromVisualTree(this, typeof(Page)) as Page;
                Cargo cargoSelecionado = cargosListagem.SelectedItem as Cargo;

                switch (cargoSelecionado.cargoId)
                {
                    case (int)CargoTipo.Presidente:
                        LoadCandidatosToNextPage(cargoSelecionado.cargoId.ToString(), "br");
                        break;
                    case (int)CargoTipo.DeputadoDistrital:
                        page.NavigationService.Navigate(new Uri(string.Format("/View/Candidatos/CandidatoListagemView.xaml?cargoId={0}&cargoNome={1}&estado=df", cargoSelecionado.cargoId.ToString(), cargoSelecionado.nome), UriKind.RelativeOrAbsolute));
                        break;
                    default:
                        page.NavigationService.Navigate(new Uri(string.Format("/View/Estados/EstadoView.xaml?cargoId={0}&cargoNome={1}", cargoSelecionado.cargoId.ToString(), cargoSelecionado.nome), UriKind.RelativeOrAbsolute));
                        break;
                }
            }
        }

        private void LoadCandidatosToNextPage(string cargo, string estado)
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_Candidatos_DownloadStringCompleted);
            client.Headers["App-Token"] = configValue.ApplicationIdForTransparenciaBrasil;

            switch (int.Parse(cargo))
            {
                case (int)CargoTipo.Presidente:
                    Uri uri = new Uri(string.Format("http://api.transparencia.org.br/sandbox/v1/candidatos?cargo={0}&estado={1}", cargo, estado));
                    client.DownloadStringAsync(uri);
                    break;
            }
        }

        private void client_Candidatos_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            Page page = GetDependencyObjectFromVisualTree(this, typeof(Page)) as Page;
            if (e.Error != null)
            {
                MessageBox.Show(configValue.ErrorMessage);
                EnablePage();
                return;
            }
            string json = e.Result;
            var candidatos = candidatoViewModel.GetCandidatos(json).OrderBy(x => x.apelido).ToList();
            var cargoSelecionado = cargosListagem.SelectedItem as Cargo;

            if (candidatos.Count() == 1)
            {
                Candidato candidatoSelecionado = candidatos[0] as Candidato;
                page.NavigationService.Navigate(new Uri(string.Format("/View/Candidatos/CandidatoDetalheView.xaml?candidatoId={0}&cargoId={1}&cargoNome={2}&estado=br", candidatoSelecionado.id.ToString(), cargoSelecionado.cargoId.ToString(), cargoSelecionado.nome), UriKind.RelativeOrAbsolute));
            }

            else if (candidatos.Count() == 2)
            {
                page.NavigationService.Navigate(new Uri(string.Format("/View/Candidatos/CandidatoSegundoTurnoView.xaml?cargoId={0}&cargoNome={1}&estado=br", cargoSelecionado.cargoId.ToString(), cargoSelecionado.nome), UriKind.RelativeOrAbsolute));
            }
        }

        private DependencyObject GetDependencyObjectFromVisualTree(DependencyObject startObject, Type type)
        {
            DependencyObject parent = startObject;
            while (parent != null)
            {
                if (type.IsInstanceOfType(parent))
                {
                    break;
                }
                else
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }
            }
            return parent;
        }
    }
}
