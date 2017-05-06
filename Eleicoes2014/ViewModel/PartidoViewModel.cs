namespace Eleicoes2014.ViewModel
{
    using Eleicoes2014.Model;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Text;

    public class PartidoViewModel
    {
        public IList<Partido> GetPartidos(string json)
        {
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(IList<Partido>));
            IList<Partido> partidos = (IList<Partido>)serializer.ReadObject(stream);
            stream.Close();
            return partidos;
        }
    }
}
