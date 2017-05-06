namespace Eleicoes2014.View.Sobre
{
    using System.Windows.Navigation;
    using Eleicoes2014.ViewModel;
    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Tasks;

    public partial class SobreView : PhoneApplicationPage
    {
        SobreViewModel sobreViewModel = new SobreViewModel();

        public SobreView()
        {
            InitializeComponent();
            this.DataContext = sobreViewModel.sobre;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            sobreViewModel.LoadViewModel();
        }

        private void HyperlinkButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();
            emailComposeTask.To = sobreViewModel.sobre.CommentsAndSuggestionsToValue;
            emailComposeTask.Subject = "[WP] Eleições 2014 App";
            emailComposeTask.Show();
        }
    }
}