namespace Eleicoes2014.Model
{
    using System.ComponentModel;

    public class Cargo : INotifyPropertyChanged
    {
        public int cargoId
        {
            get
            {
                return _cargoId;
            }
            set
            {
                if (value != _cargoId)
                {
                    _cargoId = value;
                    OnPropertyChanged("cargoId");
                }
            }
        }

        public string nome
        {
            get
            {
                return _nome;
            }
            set
            {
                if (value != _nome)
                {
                    _nome = value;
                    OnPropertyChanged("nome");
                }
            }
        }

        public string imagem
        {
            get
            {
                return string.Format("/View/Contents/Images/Icons/Cargos/{0}.png", this.cargoId.ToString()); 
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private int _cargoId;
        private string _nome;
        
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
