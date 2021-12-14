using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API_Filmes.Erros
{
    public class ErrorResponse : IErrorResponse
    {
        public int Codigo { get; set; }
        public string? Mensagem { get; set; }
        public ErrorResponse InnerError{ get; set; }
        public string[]? Descricao { get; set; }



        public ErrorResponse EmiteErro(Exception exception)
        {
            return new ErrorResponse
            {
                Codigo = exception.HResult,
                Mensagem = exception.Message,
                InnerError = EmiteErro(exception.InnerException)
            };
        }

        public ErrorResponse EmiteErroModelo (ModelStateDictionary modelState)
        {
            var Erros = modelState.Values.SelectMany(x => x.Errors);
            return new ErrorResponse
            {
                Codigo = 100,
                Mensagem = "Erro no Envio da Requisição - Model State",
                Descricao = Erros.Select(x => x.ErrorMessage).ToArray()
            };
        }

    }



}
