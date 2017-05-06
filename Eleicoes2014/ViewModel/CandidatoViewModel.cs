namespace Eleicoes2014.ViewModel
{
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using Eleicoes2014.Model;

    public class CandidatoViewModel
    {
        public Candidato GetCandidato(string json)
        {
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Candidato));
            Candidato candidato = (Candidato)serializer.ReadObject(stream);
            stream.Close();
            return candidato;
        }

        public IList<Candidato> GetCandidatos(string json)
        {
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(IList<Candidato>));
            IList<Candidato> candidatos = (IList<Candidato>)serializer.ReadObject(stream);
            stream.Close();
            return candidatos;
        }

        public IList<Bem> GetCandidatoBens(string json)
        {
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(IList<Bem>));
            IList<Bem> candidatoBens = (IList<Bem>)serializer.ReadObject(stream);
            stream.Close();
            return candidatoBens;
        }

        public IList<Candidatura> GetCandidatoCandidaturas(string json)
        {
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(IList<Candidatura>));
            IList<Candidatura> candidatoCandidaturas = (IList<Candidatura>)serializer.ReadObject(stream);
            stream.Close();
            return candidatoCandidaturas;
        }

        public IList<Estatistica> GetCandidatoEstatisticas(string json)
        {
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(IList<Estatistica>));
            IList<Estatistica> candidatoEstatisticas = (IList<Estatistica>)serializer.ReadObject(stream);
            stream.Close();
            return candidatoEstatisticas;
        }
    }
}
