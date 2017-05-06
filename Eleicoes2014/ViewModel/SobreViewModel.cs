namespace Eleicoes2014.ViewModel
{
    using Eleicoes2014.Model;
    using Eleicoes2014.Utils;

    public class SobreViewModel
    {
        public Sobre sobre = new Sobre();
        ConfigValue configValue = new ConfigValue();

        public void LoadViewModel()
        {
            sobre.VersionDescription = "Versão";
            sobre.VersionValue = configValue.AppVersion;
            sobre.DevelopedByDescription = "Desenvolvido por:";
            sobre.DevelopedByValue = configValue.AppDeveloper;
            sobre.Copyrigth = configValue.AppCopyright;
            sobre.AllRightsReserved = "Todos os direitos reservados.";
            sobre.CommentsAndSuggestionsToDescription = "Comentários e Sugestões para:";
            sobre.CommentsAndSuggestionsToValue = configValue.DeveloperEmailTo;
        }
    }
}
