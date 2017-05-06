namespace Eleicoes2014.Model
{
    using System.Globalization;
    using System.Threading;

    public class Bem
    {
        public string bem { get; set; }
        public double montante { get; set; }
        public string ano { get; set; }

        public string montanteFormat
        {
            get
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
                return montante.ToString("C");
            }
        }
    }
}
