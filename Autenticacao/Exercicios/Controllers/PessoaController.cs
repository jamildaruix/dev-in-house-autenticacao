using Bogus;
using Exercicios.Request;
using Microsoft.AspNetCore.Mvc;
using Bogus.Extensions.Brazil;


namespace Exercicios.Controllers
{
    /// <summary>
    /// Exemplos de Contente Negations
    /// Lib utilizada https://github.com/bchavez/Bogus
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PessoaController : ControllerBase
    {

        private static List<PessoaRequest>? pessoas;

        public PessoaController()
        {
            if (pessoas is null)
                pessoas = CriarDadosFake();
        }


        [HttpGet()]
        public ActionResult<IEnumerable<PessoaRequest>> GetAll()
        {
            return Ok(pessoas);
        }


        [HttpGet("{id}")]
        public ActionResult<PessoaRequest> Get(int id)
        {
            return Ok(pessoas!.Where(w => w.Id == id).FirstOrDefault());
        }


        [HttpPost()]
        public ActionResult<int> Post(PessoaRequest pessoaRequest)
        {
            int novoId = pessoas!.OrderByDescending(o => o.Id)
                                .Take(1)
                                .Select(s => s.Id)
                                .FirstOrDefault();

            pessoaRequest.Id = novoId += 1;
            pessoas!.Add(pessoaRequest);
            return Ok(pessoaRequest.Id);
        }

        [HttpPut()]
        public ActionResult Put(PessoaRequest pessoaRequest)
        {
            foreach (var pessoa in pessoas!.Where(w => w.Id == pessoaRequest.Id))
            {
                pessoa.CPF = pessoaRequest.CPF;
                pessoa.Funcao = pessoa.Funcao;
                pessoa.Nome = pessoaRequest.Nome;
            }

            return Ok();
        }


        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            pessoas!.RemoveAll(s => s.Id == id);
            return Ok();
        }

        private static List<PessoaRequest> CriarDadosFake()
        {
            int orderIds = 1;
            var faker = new Faker<PessoaRequest>("pt_BR")
                                        .StrictMode(true)
                                        .RuleFor(r => r.Id, f => orderIds++)
                                        .RuleFor(r => r.CPF, f => f.Person.Cpf())
                                        .RuleFor(r => r.Nome, f => f.Person.FullName)
                                        .RuleFor(r => r.Funcao, f => f.Commerce.Department());

            var pessoas = faker.Generate(5);
            return pessoas;
        }
    }
}
