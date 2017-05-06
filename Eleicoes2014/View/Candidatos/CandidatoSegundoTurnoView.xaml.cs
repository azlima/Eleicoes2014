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
    using System.Linq;
    using System.Net;
    using System.Windows;
    using System.Windows.Navigation;

    public partial class CandidatoSegundoTurnoView : PhoneApplicationPage
    {

        ConfigValue configValue = new ConfigValue();
        CandidatoViewModel viewModel = new CandidatoViewModel();
        string cargoId, cargoNome, estado;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NavigationContext.QueryString.TryGetValue("cargoId", out cargoId);
            NavigationContext.QueryString.TryGetValue("cargoNome", out cargoNome);
            NavigationContext.QueryString.TryGetValue("estado", out estado);

            LoadPage();
            CandidatosPivot.Title = string.Format("ELEIÇÕES 2014 - 2º TURNO - {0}{1}", cargoNome.ToUpper(), (int.Parse(cargoId) == (int)CargoTipo.Presidente) ? string.Empty : string.Format(" - {0}", estado.ToUpper()));

            if (estado != "br")
            {
                InitializeMenuItems();
            }
        }

        public CandidatoSegundoTurnoView()
        {
            InitializeComponent();
        }

        private void DisablePage()
        {
            this.displayProgresso.Visibility = Visibility.Visible;
            this.barraProgresso.Visibility = Visibility.Visible;
            this.barraProgresso.IsIndeterminate = true;
        }

        private void EnablePage()
        {
            this.displayProgresso.Visibility = Visibility.Collapsed;
            this.barraProgresso.Visibility = Visibility.Collapsed;
            this.barraProgresso.IsIndeterminate = false;
        }

        private void LoadPage()
        {
            DisablePage();
            this.candidatoDetalheUserControl1View.principalStackPanel.Visibility = Visibility.Collapsed;
            this.candidatoDetalheUserControl2View.principalStackPanel.Visibility = Visibility.Collapsed;
            LoadCandidatos(cargoId, estado);
        }

        private void LoadCandidatos(string cargo, string estado)
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            client.Headers["App-Token"] = configValue.ApplicationIdForTransparenciaBrasil;

            Uri uri = new Uri(string.Format("http://api.transparencia.org.br/sandbox/v1/candidatos?cargo={0}&estado={1}", cargo, estado));
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
            var candidatos = viewModel.GetCandidatos(json).OrderBy(x => x.apelido).ToList();

            EnablePage();
            candidatoHeader1.DataContext = candidatos[0];
            candidatoHeader2.DataContext = candidatos[1];
            this.candidatoDetalheUserControl1View.principalStackPanel.Visibility = Visibility.Visible;
            this.candidatoDetalheUserControl2View.principalStackPanel.Visibility = Visibility.Visible;
            this.candidatoDetalheUserControl1View.DataContext = candidatos[0] as Candidato;
            this.candidatoDetalheUserControl2View.DataContext = candidatos[1] as Candidato;

            this.candidatoDetalheUserControl1View.LoadBens(candidatos[0].id.ToString());
            this.candidatoDetalheUserControl1View.LoadCandidaturas(candidatos[0].id.ToString());
            this.candidatoDetalheUserControl1View.LoadCandidaturas(candidatos[0].id.ToString());
            this.candidatoDetalheUserControl1View.LoadBens(candidatos[0].id.ToString());

            this.candidatoDetalheUserControl2View.LoadBens(candidatos[1].id.ToString());
            this.candidatoDetalheUserControl2View.LoadCandidaturas(candidatos[1].id.ToString());
            this.candidatoDetalheUserControl2View.LoadCandidaturas(candidatos[1].id.ToString());
            this.candidatoDetalheUserControl2View.LoadBens(candidatos[1].id.ToString());
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

        private void atualizarBotao_Click(object sender, EventArgs e)
        {
            LoadPage();
        }
    }
}