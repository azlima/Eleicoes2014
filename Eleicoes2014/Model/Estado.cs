namespace Eleicoes2014.Model
{
    using System.ComponentModel;

    public class Estado : INotifyPropertyChanged
    {
        public int estadoId
        {
            get
            {
                return _estadoId;
            }
            set
            {
                if (value != _estadoId)
                {
                    _estadoId = value;
                    OnPropertyChanged("estadoId");
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

        public string descricao
        {
            get
            {
                return string.Format("{0} - ({1})", this.nome, this.sigla);
            }
        }

        public string imagem
        {
            get
            {
                return string.Format("/View/Contents/Images/Flags/{0}.png", this.sigla.ToLower());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private int _estadoId;
        private string _sigla;
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
