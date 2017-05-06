namespace Eleicoes2014.Model
{
    using System.ComponentModel;

    public class Partido : INotifyPropertyChanged
    {
        public int partidoId
        {
            get
            {
                return _partidoId;
            }
            set
            {
                if (value != _partidoId)
                {
                    _partidoId = value;
                    OnPropertyChanged("partidoId");
                }
            }
        }

        public string sigla
        {
            get
            {
                return _sigla;
            }
            set
            {
                if (value != _sigla)
                {
                    _sigla = value;
                    OnPropertyChanged("sigla");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private int _partidoId;
        private string _sigla;

        public void OnPropertyChanged(string nomePropriedade)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(nomePropriedade));
            }
        }
    }
}
