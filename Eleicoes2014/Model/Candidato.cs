namespace Eleicoes2014.Model
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class Candidato : INotifyPropertyChanged
    {
        public int id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    OnPropertyChanged("id");
                }
            }
        }

        public string apelido 
        {
            get
            {
                return _apelido;
            }
            set
            {
                if (value != _apelido)
                {
                    _apelido = value;
                    OnPropertyChanged("apelido");
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
        public string numero
        {
            get
            {
                return _numero;
            }
            set
            {
                if (value != _numero)
                {
                    _numero = value;
                    OnPropertyChanged("numero");
                }
            }
        }

        public string titulo 
        {
            get
            {
                return _titulo;
            }
            set
            {
                if (value != _titulo)
                {
                    _titulo = value;
                    OnPropertyChanged("titulo");
                }
            }
        }

        public string CPF 
        {
            get
            {
                return _cpf;
            }
            set
            {
                if (value != _cpf)
                {
                    _cpf = value;
                    OnPropertyChanged("CPF");
                }
            }
        }

        public string matricula 
        {
            get
            {
                return _matricula;
            }
            set
            {
                if (value != _matricula)
                {
                    _matricula = value;
                    OnPropertyChanged("matricula");
                }
            }
        }
        public string cargo 
        {
            get
            {
                return _cargo;
            }
            set
            {
                if (value != _cargo)
                {
                    _cargo = value;
                    OnPropertyChanged("cargo");
                }
            }
        }
        public string estado 
        {
            get
            {
                return _estado;
            }
            set
            {
                if (value != _estado)
                {
                    _estado = value;
                    OnPropertyChanged("estado");
                }
            }
        }

        public string partido 
        {
            get
            {
                return _partido;
            }
            set
            {
                if (value != _partido)
                {
                    _partido = value;
                    OnPropertyChanged("partido");
                }
            }
        }

        public string idade 
        {
            get
            {
                return _idade;
            }
            set
            {
                if (value != _idade)
                {
                    _idade = value;
                    OnPropertyChanged("idade");
                }
            }
        }
        public string instrucao 
        {
            get
            {
                return _instrucao;
            }
            set
            {
                if (value != _instrucao)
                {
                    _instrucao = value;
                    OnPropertyChanged("instrucao");
                }
            }
        }
        public string ocupacao 
        {
            get
            {
                return _ocupacao;
            }
            set
            {
                if (value != _ocupacao)
                {
                    _ocupacao = value;
                    OnPropertyChanged("ocupacao");
                }
            }
        }
        public string miniBio 
        {
            get
            {
                return _miniBio;
            }
            set
            {
                if (value != _miniBio)
                {
                    _miniBio = value;
                    OnPropertyChanged("miniBio");
                }
            }
        }

        public string cargos 
        {
            get
            {
                return _cargos;
            }
            set
            {
                if (value != _cargos)
                {
                    _cargos = value;
                    OnPropertyChanged("cargos");
                }
            }
        }

        public string previsao 
        {
            get
            {
                return _previsao;
            }
            set
            {
                if (value != _previsao)
                {
                    _previsao = value;
                    OnPropertyChanged("previsao");
                }
            }
        }

        public string bancadas 
        {
            get
            {
                return _bancadas;
            }
            set
            {
                if (value != _bancadas)
                {
                    _bancadas = value;
                    OnPropertyChanged("bancadas");
                }
            }
        }

        public string processos 
        {
            get
            {
                return _processos;
            }
            set
            {
                if (value != _processos)
                {
                    _processos = value;
                    OnPropertyChanged("processos");
                }
            }
        }

        public string casaAtual 
        {
            get
            {
                return _casaAtual;
            }
            set
            {
                if (value != _casaAtual)
                {
                    _casaAtual = value;
                    OnPropertyChanged("casaAtual");
                }
            }
        }

        public bool reeleicao 
        {
            get
            {
                return _reeleicao;
            }
            set
            {
                if (value != _reeleicao)
                {
                    _reeleicao = value;
                    OnPropertyChanged("reeleicao");
                }
            }
        }

        public string foto 
        {
            get
            {
                return _foto;
            }
            set
            {
                if (value != _foto)
                {
                    _foto = value;
                    OnPropertyChanged("foto");
                }
            }
        }

        public string candidatoReeleicao
        {
            get
            {
                return (reeleicao) ? "CANDIDATO A REELEIÇÃO" : string.Empty;
            }
        }

        public IList<ProcessoCandidato> candidatoProcessos
        {
            get
            {
                var processosCandidato = new List<ProcessoCandidato>();
                IList<string> processosColecao = new List<string>();
                string[] stringSeparators = new string[] { "<a href=" };
                if (!string.IsNullOrEmpty(processos))
                {
                    var processosString = processos.Replace("<b>", "").Replace("</b>", "").Replace("<a>", "").Replace("</a>", "").Replace(">", ">\n").Replace("\" target=\"_blank", "").Replace("<!--", "").Replace("-->", "");
                    processosColecao = processosString.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var processoColecao in processosColecao)
                    {
                        ProcessoCandidato pc = new ProcessoCandidato()
                        {
                            url = processoColecao.Split('>')[0].Replace("\"", "").Replace("\n", "").Replace("\r", ""),
                            descricao = processoColecao.Split('>')[1].Replace("\n","").Replace("\r", "")
                        };
                        processosCandidato.Add(pc);
                    }
                }
                return processosCandidato;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private int _id;
        private string _apelido;
        private string _nome;
        private string _numero;
        private string _titulo;
        private string _cpf;
        private string _matricula;
        private string _cargo;
        private string _estado;
        private string _partido;
        private string _idade;
        private string _instrucao;
        private string _ocupacao;
        private string _miniBio;
        private string _cargos;
        private string _previsao;
        private string _bancadas;
        private string _processos;
        private string _casaAtual;
        private bool _reeleicao;
        private string _foto;

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
