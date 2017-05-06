namespace Eleicoes2014.View.Candidatos
{
    using Eleicoes2014.Model;
    using Eleicoes2014.Model.Enums;
    using Eleicoes2014.Utils;
    using Eleicoes2014.ViewModel;
    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Shell;
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Windows;
    using System.Windows.Navigation;

    public partial class CandidatoDetalheView : PhoneApplicationPage
    {
        ConfigValue configValue = new ConfigValue();
        CandidatoViewModel viewModel = new CandidatoViewModel();
        string candidatoId, cargoId, cargoNome, estado;

        public CandidatoDetalheView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NavigationContext.QueryString.TryGetValue("candidatoId", out candidatoId);
            NavigationContext.QueryString.TryGetValue("cargoId", out cargoId);
            NavigationContext.QueryString.TryGetValue("cargoNome", out cargoNome);
            NavigationContext.QueryString.TryGetValue("estado", out estado);
            PageTitle.Text = estado.ToLower() == "br" ? cargoNome.ToLower() : string.Format("{0}{1}", cargoNome.ToLower(), (int.Parse(cargoId) == (int)CargoTipo.Presidente) ? string.Empty : string.Format(" - {0}", estado.ToLower()));
            LoadPage();

            if (estado != "br")
            {
                InitializeMenuItems();
            }
        }

        private void LoadPage()
        {
            DisablePage();
            LoadCandidato(candidatoId);
            LoadBens(candidatoId);
            LoadCandidaturas(candidatoId);
            LoadEstatisticas(candidatoId);
        }

        private void DisablePage()
        {
            this.displayProgresso.Visibility = Visibility.Visible;
            this.barraProgresso.Visibility = Visibility.Visible;
            this.barraProgresso.IsIndeterminate = true;
            this.principalStackPanel.Visibility = Visibility.Collapsed;
        }

        private void EnablePage()
        {
            this.displayProgresso.Visibility = Visibility.Collapsed;
            this.barraProgresso.Visibility = Visibility.Collapsed;
            this.barraProgresso.IsIndeterminate = false;
            this.principalStackPanel.Visibility = Visibility.Visible;
        }

        private void LoadPageAfterError()
        {
            this.displayProgresso.Visibility = Visibility.Collapsed;
            this.barraProgresso.Visibility = Visibility.Collapsed;
            this.barraProgresso.IsIndeterminate = false;
            this.principalStackPanel.Visibility = Visibility.Collapsed;
        }

        private void LoadCandidato(string candidatoId)
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompletedCandidato);
            client.Headers["App-Token"] = configValue.ApplicationIdForTransparenciaBrasil;

            Uri uri = new Uri(string.Format("http://api.transparencia.org.br/sandbox/v1/candidatos/{0}", candidatoId));
            client.DownloadStringAsync(uri);
        }

        private void client_DownloadStringCompletedCandidato(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(configValue.ErrorMessage);
                LoadPageAfterError();
                return;
            }
            string json = e.Result;
            this.DataContext = viewModel.GetCandidato(json);
            EnablePage();
        }

        private void LoadBens(string candidatoId)
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

        private void LoadCandidaturas(string candidatoId)
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

        private void LoadEstatisticas(string candidatoId)
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

        private void atualizarBotao_Click(object sender, EventArgs e)
        {
            LoadPage();
        }

        #region MenuItems

        private void InitializeMenuItems()
        {
            if (ApplicationBar.MenuItems.Count == 0)
            {
                ApplicationBarMenuItem estadoMenuItem = new ApplicationBarMenuItem();
                estadoMenuItem.Text = "Alterar estado";
                estadoMenuItem.Click += estadoMenuItem_Click;
                ApplicationBar.MenuItems.Add(estadoMenuItem);

                ApplicationBarMenuItem presidenteMenuItem = new ApplicationBarMenuItem();
                presidenteMenuItem.Text = GetEnumDescription<CargoTipo>(CargoTipo.Presidente);
                presidenteMenuItem.IsEnabled = (int.Parse(cargoId) != (int)CargoTipo.Presidente);
                presidenteMenuItem.Click += presidenteMenuItem_Click;
                ApplicationBar.MenuItems.Add(presidenteMenuItem);

                ApplicationBarMenuItem governadorMenuItem = new ApplicationBarMenuItem();
                governadorMenuItem.Text = string.Format("{0} - {1}", GetEnumDescription<CargoTipo>(CargoTipo.Governador), estado);
                governadorMenuItem.IsEnabled = (int.Parse(cargoId) != (int)CargoTipo.Governador);
                governadorMenuItem.Click += governadorMenuItem_Click;
                ApplicationBar.MenuItems.Add(governadorMenuItem);

                ApplicationBarMenuItem senadorMenuItem = new ApplicationBarMenuItem();
                senadorMenuItem.Text = string.Format("{0} - {1}", GetEnumDescription<CargoTipo>(CargoTipo.Senador), estado);
                senadorMenuItem.IsEnabled = (int.Parse(cargoId) != (int)CargoTipo.Senador);
                senadorMenuItem.Click += senadorMenuItem_Click;
                ApplicationBar.MenuItems.Add(senadorMenuItem);

                ApplicationBarMenuItem deputadoFederalMenuItem = new ApplicationBarMenuItem();
                deputadoFederalMenuItem.Text = string.Format("{0} - {1}", GetEnumDescription<CargoTipo>(CargoTipo.DeputadoFederal), estado);
                deputadoFederalMenuItem.IsEnabled = (int.Parse(cargoId) != (int)CargoTipo.DeputadoFederal);
                deputadoFederalMenuItem.Click += deputadoFederalMenuItem_Click;
                ApplicationBar.MenuItems.Add(deputadoFederalMenuItem);

                ApplicationBarMenuItem deputadoEstadualMenuItem = new ApplicationBarMenuItem();
                deputadoEstadualMenuItem.Text = string.Format("{0} - {1}", GetEnumDescription<CargoTipo>(CargoTipo.DeputadoEstadual), estado);
                deputadoEstadualMenuItem.IsEnabled = (int.Parse(cargoId) != (int)CargoTipo.DeputadoEstadual);
                deputadoEstadualMenuItem.Click += deputadoEstadualMenuItem_Click;
                if (estado != "df")
                {
                    ApplicationBar.MenuItems.Add(deputadoEstadualMenuItem);
                }

                ApplicationBarMenuItem deputadoDistritalMenuItem = new ApplicationBarMenuItem();
                deputadoDistritalMenuItem.Text = string.Format("{0} - {1}", GetEnumDescription<CargoTipo>(CargoTipo.DeputadoDistrital), estado);
                deputadoDistritalMenuItem.IsEnabled = (int.Parse(cargoId) != (int)CargoTipo.DeputadoDistrital);
                deputadoDistritalMenuItem.Click += deputadoDistritalMenuItem_Click;
                if (estado == "df")
                {
                    ApplicationBar.MenuItems.Add(deputadoDistritalMenuItem);
                }
            }
        }

        private void estadoMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri(string.Format("/View/Estados/EstadoView.xaml?cargoId={0}&cargoNome={1}", cargoId, cargoNome), UriKind.RelativeOrAbsolute));
        }

        private void presidenteMenuItem_Click(object sender, EventArgs e)
        {
            cargoId = ((int)CargoTipo.Presidente).ToString();
            cargoNome = GetEnumDescription<CargoTipo>(CargoTipo.Presidente).ToLower();
            LoadCandidatosToNextPage(cargoId, estado);
        }

        private void governadorMenuItem_Click(object sender, EventArgs e)
        {
            cargoId = ((int)CargoTipo.Governador).ToString();
            cargoNome = GetEnumDescription<CargoTipo>(CargoTipo.Governador).ToLower();
            LoadCandidatosToNextPage(cargoId, estado);
        }

        private void senadorMenuItem_Click(object sender, EventArgs e)
        {
            cargoId = ((int)CargoTipo.Senador).ToString();
            cargoNome = GetEnumDescription<CargoTipo>(CargoTipo.Senador).ToLower();
            LoadCandidatosToNextPage(cargoId, estado);
        }

        private void deputadoFederalMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri(string.Format("/View/Candidatos/CandidatoListagemView.xaml?cargoId={0}&cargoNome={1}&estado={2}", ((int)CargoTipo.DeputadoFederal).ToString(), GetEnumDescription<CargoTipo>(CargoTipo.DeputadoFederal).ToLower(), estado.ToLower()), UriKind.RelativeOrAbsolute));
        }

        private void deputadoEstadualMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri(string.Format("/View/Candidatos/CandidatoListagemView.xaml?cargoId={0}&cargoNome={1}&estado={2}", ((int)CargoTipo.DeputadoEstadual).ToString(), GetEnumDescription<CargoTipo>(CargoTipo.DeputadoEstadual).ToLower(), estado.ToLower()), UriKind.RelativeOrAbsolute));
        }

        private void deputadoDistritalMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri(string.Format("/View/Candidatos/CandidatoListagemView.xaml?cargoId={0}&cargoNome={1}&estado=df", ((int)CargoTipo.DeputadoDistrital).ToString(), GetEnumDescription<CargoTipo>(CargoTipo.DeputadoDistrital).ToLower()), UriKind.RelativeOrAbsolute));
        }

        private void LoadCandidatosToNextPage(string cargo, string estado)
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_Candidatos_DownloadStringCompleted);
            client.Headers["App-Token"] = configValue.ApplicationIdForTransparenciaBrasil;

            switch (int.Parse(cargo))
            {
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
            var candidatos = viewModel.GetCandidatos(json).OrderBy(x => x.apelido).ToList();

            if (candidatos.Count() == 1)
            {
                Candidato candidatoSelecionado = candidatos[0] as Candidato;
                NavigationService.Navigate(new Uri(string.Format("/View/Candidatos/CandidatoDetalheView.xaml?candidatoId={0}&cargoId={1}&cargoNome={2}&estado={3}", candidatoSelecionado.id.ToString(), cargoId, cargoNome, estado.ToLower()), UriKind.RelativeOrAbsolute));
            }

            else if (candidatos.Count() == 2)
            {
                NavigationService.Navigate(new Uri(string.Format("/View/Candidatos/CandidatoSegundoTurnoView.xaml?cargoId={0}&cargoNome={1}&estado={2}", cargoId, cargoNome, estado.ToLower()), UriKind.RelativeOrAbsolute));
            }
        }

        public static string GetEnumDescription<T>(T enumeratedType)
        {
            var description = enumeratedType.ToString();

            var enumType = typeof(T);
            // Can't use type constraints on value types, so have to do check like this
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            var fieldInfo = enumeratedType.GetType().GetField(enumeratedType.ToString());

            if (fieldInfo != null)
            {
                var attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0)
                {
                    description = ((DescriptionAttribute)attributes[0]).Description;
                }
            }

            return description;
        }

        #endregion
    }
}