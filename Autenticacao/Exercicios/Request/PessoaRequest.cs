namespace Exercicios.Request
{
    public class PessoaRequest
    {
        public PessoaRequest()
                : this(string.Empty, string.Empty, string.Empty)
        {
        }

        public PessoaRequest(string nome,  string cPF, string funcao)
        {
            Nome = nome;
            CPF = cPF;
            Funcao = funcao;
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Funcao { get; set; }
    }
}
