namespace Eleicoes2014.View.Candidatos
{
    using Eleicoes2014.Model;
    using Eleicoes2014.Model.Enums;
    using Eleicoes2014.Utils;
    using Eleicoes2014.ViewModel;
    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Shell;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Navigation;

    public partial class CandidatoListagemView : PhoneApplicationPage
    {
        private ScrollViewer _listScrollViewer;
        private double _lastFetch;

        ConfigValue configValue = new ConfigValue();
        CandidatoViewModel viewModel = new CandidatoViewModel();
        string cargoId, cargoNome, estado, nome, partido;

        public CandidatoListagemView()
        {
            InitializeComponent();
            candidatosListagem.Loaded += new RoutedEventHandler(candidatosListagem_Loaded);
            candidatoPesquisaView.pesquisarImagem.Tap += new EventHandler<GestureEventArgs>(pesquisarImagem_Tap);
            candidatoPesquisaView.numeroOuNomeTextBox.KeyDown += new KeyEventHandler(numeroOuNomeTextBox_KeyDown);
        }

        #region Page Methods

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NavigationContext.QueryString.TryGetValue("cargoId", out cargoId);
            NavigationContext.QueryString.TryGetValue("cargoNome", out cargoNome);
            NavigationContext.QueryString.TryGetValue("estado", out estado);
            NavigationContext.QueryString.TryGetValue("nome", out nome);
            NavigationContext.QueryString.TryGetValue("partido", out partido);
            PageTitle.Text = (int.Parse(cargoId) == (int) CargoTipo.Presidente) ? cargoNome.ToLower() : string.Format("{0} - {1}", cargoNome.ToLower(), estado.ToLower());
            var pesquisarButton = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
            switch (int.Parse(cargoId))
            {
                case (int)CargoTipo.DeputadoFederal:
                case (int)CargoTipo.DeputadoEstadual:
                case (int)CargoTipo.DeputadoDistrital:
                    pesquisarButton.IsEnabled = true;
                    break;
                default:
                    pesquisarButton.IsEnabled = false;
                    break;
            }

            if (estado != "br")
            {
                InitializeMenuItems();
            }

            if (candidatosListagem.Items.Count() == 0)
            {
                LoadPage();
            }
        }

        private void LoadPage()
        {
            DisablePage();
            LoadCandidatos(cargoId, estado, nome, partido);
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

        private void atualizarBotao_Click(object sender, EventArgs e)
        {
            if (candidatosListagem.Items.Count() > 0)
            {
                candidatosListagem.Items.Clear();
            }
            _lastFetch = 0;
            LoadPage();
        }

        #endregion

        #region Loading Candidatos

        private void LoadCandidatos(string cargo, string estado, string nome, string partido)
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            client.Headers["App-Token"] = configValue.ApplicationIdForTransparenciaBrasil;
            string buscaNome = (!string.IsNullOrEmpty(nome)) ? string.Format("&nome={0}", nome) : string.Empty;
            string buscaPartido = (!string.IsNullOrEmpty(partido)) ? string.Format("&partido={0}", partido) : string.Empty;
            string paginacao = string.Empty;

            switch (int.Parse(cargo))
            {
                case (int)CargoTipo.DeputadoFederal:
                case (int)CargoTipo.DeputadoEstadual:
                case (int)CargoTipo.DeputadoDistrital:
                    paginacao = string.Format("&_offset={0}", candidatosListagem.Items.Count());
                    break;
            }

            Uri uri = new Uri(string.Format("http://api.transparencia.org.br/sandbox/v1/candidatos?cargo={0}&estado={1}{2}{3}{4}", cargo, estado, buscaNome, buscaPartido, paginacao));
            client.DownloadStringAsync(uri);
        }

        private void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(configValue.ErrorMessage);
                EnablePage();
                this.displayProgressoPaginacao.Visibility = Visibility.Collapsed;
                this.barraProgressoPaginacao.Visibility = Visibility.Collapsed;
                this.barraProgressoPaginacao.IsIndeterminate = false;
                return;
            }
            string json = e.Result;
            var candidatos = viewModel.GetCandidatos(json).OrderBy(x => x.apelido);
            foreach (var candidato in candidatos)
            {
                candidatosListagem.Items.Add(candidato);
            }
            EnablePage();
            HidePaging();
            if (candidatosListagem.Items.Count == 0)
            {
                MessageBox.Show("Nenhum candidato encontrado para essa pesquisa.");
            }
        }

        private void candidatosListagem_Tap(object sender, GestureEventArgs e)
        {
            if (candidatosListagem.SelectedIndex >= 0)
            {
                Candidato candidatoSelecionado = candidatosListagem.SelectedItem as Candidato;
                NavigationService.Navigate(new Uri(string.Format("/View/Candidatos/CandidatoDetalheView.xaml?candidatoId={0}&cargoId={1}&cargoNome={2}&estado={3}", candidatoSelecionado.id.ToString(), cargoId, cargoNome, estado.ToLower()), UriKind.RelativeOrAbsolute));
            }
        }

        #endregion

        #region Paging

        public static readonly DependencyProperty ListVerticalOffsetProperty = DependencyProperty.Register("ListVerticalOffset", typeof(double), typeof(CandidatoListagemView), new PropertyMetadata(new PropertyChangedCallback(OnListVerticalOffsetChanged)));

        public double ListVerticalOffset
        {
            get { return (double)this.GetValue(ListVerticalOffsetProperty); }
            set { this.SetValue(ListVerticalOffsetProperty, value); }
        }

        static T FindChildOfType<T>(DependencyObject root) where T : class
        {
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                DependencyObject current = queue.Dequeue();
                for (int i = VisualTreeHelper.GetChildrenCount(current) - 1; 0 <= i; i--)
                {
                    var child = VisualTreeHelper.GetChild(current, i);
                    var typedChild = child as T;
                    if (typedChild != null)
                    {
                        return typedChild;
                    }
                    queue.Enqueue(child);
                }
            }
            return null;
        }

        private void candidatosListagem_Loaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;
            _listScrollViewer = FindChildOfType<ScrollViewer>(element);

            System.Windows.Data.Binding binding = new System.Windows.Data.Binding();
            binding.Source = _listScrollViewer;
            binding.Path = new PropertyPath("VerticalOffset");
            binding.Mode = BindingMode.OneWay;
            this.SetBinding(ListVerticalOffsetProperty, binding);
        }

        private static void OnListVerticalOffsetChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CandidatoListagemView page = obj as CandidatoListagemView;
            ScrollViewer viewer = page._listScrollViewer;

            if (viewer != null)
            {
                if (page._lastFetch < viewer.ScrollableHeight)
                {
                    // Trigger within 1/4 the viewport.
                    if (viewer.VerticalOffset >= viewer.ScrollableHeight - viewer.ViewportHeight / 4)
                    {
                        page._lastFetch = viewer.ScrollableHeight;
                        switch (int.Parse(page.cargoId))
                        {
                            case (int)CargoTipo.DeputadoFederal:
                            case (int)CargoTipo.DeputadoEstadual:
                            case (int)CargoTipo.DeputadoDistrital:
                                page.ShowPaging();
                                page.LoadCandidatos(page.cargoId, page.estado, page.nome, page.partido);
                                break;
                        }
                    }
                }
            }
        }

        private void ShowPaging()
        {
            this.barraProgressoPaginacao.IsIndeterminate = true;
            this.paginacaoStackPanel.Visibility = Visibility.Visible;
        }

        private void HidePaging()
        {
            this.barraProgressoPaginacao.IsIndeterminate = false;
            this.paginacaoStackPanel.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Pesquisa

        private void pesquisarBotao_Click(object sender, EventArgs e)
        {
            CarregarPesquisaUserControl();
        }

        private void CarregarPesquisaUserControl()
        {
            candidatoPesquisaView.Visibility = Visibility.Visible;
            CandidatosListagemStackPanel.Visibility = Visibility.Collapsed;
            candidatoPesquisaView.partidosListagem.SelectedIndex = -1;
            candidatoPesquisaView.partidosBotao.Content = candidatoPesquisaView.todosPartidosText;
            candidatoPesquisaView.numeroOuNomeTextBox.Text = candidatoPesquisaView.emptyText;
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Colors.Gray;
            candidatoPesquisaView.numeroOuNomeTextBox.Foreground = brush;
            ApplicationBar.IsVisible = false;
        }

        private void numeroOuNomeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PesquisaCandidatos();
            }
        }

        private void pesquisarImagem_Tap(object sender, GestureEventArgs e)
        {
            PesquisaCandidatos();
        }

        private void PesquisaCandidatos()
        {
            int partidoId = 0;
            if (candidatoPesquisaView.partidosListagem.SelectedIndex >= 0)
            {
                var partidoSelecionado = candidatoPesquisaView.partidosListagem.SelectedItem as Partido;
                partidoId = partidoSelecionado.partidoId;
            }
            nome = (!string.IsNullOrEmpty(candidatoPesquisaView.numeroOuNomeTextBox.Text) && (candidatoPesquisaView.numeroOuNomeTextBox.Text != candidatoPesquisaView.emptyText)) ? candidatoPesquisaView.numeroOuNomeTextBox.Text : string.Empty;
            partido = (partidoId > 0) ? partidoId.ToString() : string.Empty;
            ReloadCandidatos();
        }

        private void ReloadCandidatos()
        {
            candidatosListagem.Items.Clear();
            _lastFetch = 0;
            candidatoPesquisaView.Visibility = Visibility.Collapsed;
            ApplicationBar.IsVisible = true;
            CandidatosListagemStackPanel.Visibility = Visibility.Visible;
            DisablePage();
            LoadCandidatos(cargoId, estado, nome, partido);
        }

        #endregion

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