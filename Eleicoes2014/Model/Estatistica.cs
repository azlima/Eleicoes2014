namespace Eleicoes2014.Model
{
    public class Estatistica
    {
        public double faltasPlenario { get; set; } //Porcentagem de faltas em sessões plenárias do candidato
        public double mediaPlenario { get; set; } //Média de faltas em plenário da Casa (Senado ou Câmara)
        public double faltasComissoes { get; set; } //Porcentagem de faltas em sessões de comissões
        public double mediaComissoes { get; set; } //Média de faltas em comissões da Casa (Senado ou Câmara)
        public double evolucao { get; set; } //Evolução patrimonial entre 2014 e o último ano em que declarou, corrigido pelo IPCA de julho de 2014
        public int anoRef { get; set; } //Ano da última declaração de bens disponível
        public double emendas { get; set; } //Porcentagem de emendas do parlamentar atendidas
        public double mediaEmendas { get; set; } //Porcentagem de emendas atendidas na Casa à qual o candidato pertence
    }
}
