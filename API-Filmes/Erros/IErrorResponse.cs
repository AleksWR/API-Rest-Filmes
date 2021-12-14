using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API_Filmes.Erros
{
    public interface IErrorResponse
    {
        ErrorResponse EmiteErro(Exception exception);
        ErrorResponse EmiteErroModelo(ModelStateDictionary modelState);
    }
}
