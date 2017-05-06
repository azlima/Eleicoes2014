namespace Eleicoes2014.ViewModel
{
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using Eleicoes2014.Model;

    public class EstadoViewModel
    {
        public IList<Estado> GetEstados(string json)
        {
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(IList<Estado>));
            IList<Estado> estados = (IList<Estado>)serializer.ReadObject(stream);
            stream.Close();
            return estados;
        }
    }
}
