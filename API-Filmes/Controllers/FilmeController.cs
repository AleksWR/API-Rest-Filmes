using API_Filmes.Data;
using API_Filmes.Erros;
using API_Filmes.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_Filmes.Controllers
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("[controller]")]
    [ApiController]
    public class FilmeController : ControllerBase
    {

        private EFContext _context;
        private IErrorResponse _errorResponse;

        public FilmeController(EFContext context, IErrorResponse errorResponse)
        {
            _context = context;
            _errorResponse = errorResponse;
        }

        /// <summary>
        /// Retorna os Filmes do Banco - Permite Filtro.
        /// </summary>
        /// <response code="200">Retorna os Filmes cadastrados no Banco de Dados</response>
        /// <response code="400">Retorna descrição de erro ocorrido na Requisição</response>
        /// <response code="404">Filme não foi encontrado</response>

        [ProducesResponseType(statusCode:200, type: typeof(Filme))]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404)]
        [HttpGet]
        public IActionResult Get([FromQuery] Filtro? filtro)
        {
            if (!string.IsNullOrEmpty(filtro.Autor))
            {
                var filmes = _context.Filmes
                    .Where(filme => filme.Autor == filtro.Autor)
                    .ToList();

                return VerificaExistenciaFilme(filmes);
            }
            if (!string.IsNullOrEmpty(filtro.Nome))
            {
                var filmes = _context.Filmes
                    .Where(filme => filme.Nome == filtro.Nome)
                    .ToList();

                return VerificaExistenciaFilme(filmes);
            }
            if (filtro.Duracao != 0)
            {
                var filmes = _context.Filmes
                    .Where(filme => filme.Duracao == filtro.Duracao)
                    .ToList();

                return VerificaExistenciaFilme(filmes);
            }


            var filmeLista = _context.Filmes.ToList();

            return VerificaExistenciaFilme(filmeLista);
        }


        /// <summary>
        /// Retorna Filme do Banco de Dados por Id.
        /// </summary>
        /// <response code="200">Retorna Filme com Id Correspondente </response>
        /// <response code="404">Filme com Id pesquisado não foi encontrado</response>
       
        [ProducesResponseType(statusCode: 200, type: typeof(Filme))]
        [ProducesResponseType(statusCode: 404)]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var filme = _context.Filmes
                .Where(filme => filme.Id == id)
                .FirstOrDefault();

            if (filme != null)
            {
                return Ok(filme);
            }
            return NotFound();
        }

        /// <summary>
        /// Permite atualizar informações de um filme no Banco de Dados.
        /// </summary>
        /// <response code="204">Filme foi Atualizado no Banco de Dados</response>
        /// <response code="400">Retorna descrição de erro ocorrido na Requisição</response>
        /// <response code="404">Filme não foi encontrado para ser atualizado</response>
        
        [ProducesResponseType(statusCode: 204)]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404)]
        [HttpPut("{id}")]
        public IActionResult Put(int id, Filme filme)
        {
            if (ModelState.IsValid)
            {
                var filmeAtual = _context.Filmes
                    .Where(filme => filme.Id == id)
                    .FirstOrDefault();

                if(filmeAtual != null)
                {
                    filmeAtual.Autor = filme.Autor;
                    filmeAtual.Nome = filme.Nome;
                    filmeAtual.Duracao = filme.Duracao;
                    _context.Filmes.Update(filmeAtual);
                    _context.SaveChanges();
                    return NoContent();
                }
                return NotFound();
            }

            return BadRequest();
        }

        /// <summary>
        /// Apaga um Filme do Banco de Dados.
        /// </summary>
        /// <response code="204">Filme foi apagado com Sucesso do Banco de Dados</response>
        /// <response code="404">Filme não foi encontrado </response>
      
        [ProducesResponseType(statusCode: 204)]
        [ProducesResponseType(statusCode: 404)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var FilmeADeletar = _context.Filmes
                .Where(filme => filme.Id == id)
                .FirstOrDefault();

            if(FilmeADeletar != null)
            {
                _context.Filmes.Remove(FilmeADeletar);
                _context.SaveChanges();
                return NoContent();
            }
            return NotFound();
        }

        /// <summary>
        /// Insere um Filme no Banco de Dados.
        /// </summary>
        /// <response code="201">Retorna Filme que foi Inserido no Banco de Dados</response>
        /// <response code="400">Retorna descrição de erro ocorrido na Requisição</response>
        
        [ProducesResponseType(statusCode: 201)]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorResponse))]
        [HttpPost]
        public IActionResult Post(Filme filme)
        {
            if (ModelState.IsValid) {

                Filme NovoFilme = new Filme()
                {
                    Autor = filme.Autor,
                    Duracao = filme.Duracao,
                    Nome = filme.Nome
                };


                _context.Filmes.Add(NovoFilme);
                _context.SaveChanges();

                return CreatedAtAction(nameof(Get), new {id = NovoFilme.Id}, NovoFilme);

            }
            return BadRequest(_errorResponse.EmiteErroModelo(ModelState));
        }


        private IActionResult VerificaExistenciaFilme(List<Filme> filmes )
        {
            if (filmes.Count == 0)
            {
                return NotFound();
            }
            return Ok(filmes);
        }
        
    }
}
