using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API_Filmes.Erros
{
    public class DisparaErros : IExceptionFilter
    {
        private IErrorResponse _errorResponse;

        public DisparaErros(IErrorResponse errorResponse)
        {
            _errorResponse = errorResponse;
        }
        public void OnException(ExceptionContext context)
        {
            var Erro = _errorResponse.EmiteErro(context.Exception);
            context.Result = new ObjectResult(Erro)
            {
                StatusCode = 500
            };
        }
    }
}
