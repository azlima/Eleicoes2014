namespace Eleicoes2014.Model
{
    using System.Globalization;
    using System.Threading;

    public class Candidatura
    {
        public string anoEleitoral { get; set; }
        public string cargo { get; set; }
        public string partidoSigla { get; set; }
        public string municipio { get; set; }
        public string estadoSigla { get; set; }
        public string resultado { get; set; }
        public string votosObtidos { get; set; }
        public double recursosFinanceiros { get; set; }

        public string recursosFinanceirosFormat
        {
            get
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
                return recursosFinanceiros.ToString("C");
            }
        }
    }
}
