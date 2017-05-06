namespace Eleicoes2014.Model.Enums
{
    using System.ComponentModel;

    public enum CargoTipo
    {
        [Description("Presidente")]
        Presidente = 1,
        [Description("Vice-presidente")]
        VicePresidente = 2,
        [Description("Governador")]
        Governador = 3,
        [Description("Vice-Governador")]
        ViceGovernador = 4,
        [Description("Senador")]
        Senador = 5,
        [Description("Deputado Federal")]
        DeputadoFederal = 6,
        [Description("Deputado Estadual")]
        DeputadoEstadual = 7,
        [Description("Deputado Distrital")]
        DeputadoDistrital = 8,
        [Description("1º Suplente de Senador")]
        SuplenteSenador1 = 9,
        [Description("2º Suplente de Senador")]
        SuplenteSenador2 = 10
    }
}
