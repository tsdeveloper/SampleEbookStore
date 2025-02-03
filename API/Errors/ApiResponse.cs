using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors;

public class ApiResponse
{
 public ApiResponse(int statusCode, string message = null)
    {
        StatusCode = statusCode;
        Message = message ?? GetDefaultMessageForStatusCode(statusCode);
    }

    public int StatusCode { get; set; }
    public string Message { get; set; }

    private string GetDefaultMessageForStatusCode(int statusCode)
    {
        return statusCode switch
        {
            400 => "Campo obrigatório não preenchido.",
            401 => "Você não tem autorização para esse request.",
            404 => "Endpoint não encontrado.",
            500 => "Houve um erro na sua solicitação no servidor.",
            _ => null
        };
    }
}
