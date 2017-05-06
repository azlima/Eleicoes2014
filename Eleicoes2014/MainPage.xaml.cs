namespace Eleicoes2014
{
    using System;
    using Eleicoes2014.Utils;
    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Tasks;

    public partial class MainPage : PhoneApplicationPage
    {
        ConfigValue configValue = new ConfigValue();

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void classificarMenuItem_Click(object sender, System.EventArgs e)
        {
            MarketplaceReviewTask marketPlaceReviewTask = new MarketplaceReviewTask();
            marketPlaceReviewTask.Show();
        }

        private void contribuirMenuItem_Click(object sender, System.EventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();

            string business = configValue.DeveloperEmailTo;
            string description = "[WP]%20ELEI%C7%D5ES%202014%20App";
            string country = "BR";
            string currency = "BRL";
            string url = "https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=";
            string uri = string.Format("{0}{1}&lc={2}&item_name={3}&currency_code={4}&bn=PP%2dDonationsBF", url, business, country, description, currency);

            webBrowserTask.Uri = new Uri(uri, UriKind.RelativeOrAbsolute);
            webBrowserTask.Show();
        }

        private void falarDesenvolvedorMenuItem_Click(object sender, System.EventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();
            emailComposeTask.To = configValue.DeveloperEmailTo;
            emailComposeTask.Subject = "[WP] Eleições 2014 App";
            emailComposeTask.Show();
        }

        private void sobreMenuItem_Click(object sender, System.EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Sobre/SobreView.xaml", UriKind.RelativeOrAbsolute));
        }

        private void atualizarBotao_Click(object sender, EventArgs e)
        {
            cargoView.LoadPage();
        }
    }
}