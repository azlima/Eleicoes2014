namespace Eleicoes2014.ViewModel
{
    using System.Collections.Generic;
    using Eleicoes2014.Model;
    using System.IO;
    using System.Text;
    using System.Linq;
    using System.Runtime.Serialization.Json;

    public class CargoViewModel
    {
        public IList<Cargo> GetCargos(string json)
        {
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(IList<Cargo>));
            IList<Cargo> cargos = (IList<Cargo>)serializer.ReadObject(stream);
            IList<Cargo> cargosPrincipais = new List<Cargo>();
            foreach (var cargo in cargos)
            {
                if (!cargo.nome.Contains("Vice") && !cargo.nome.Contains("Suplente"))
                {
                    cargosPrincipais.Add(cargo);
                }
            }
            stream.Close();
            return cargosPrincipais;
        }
    }
}
