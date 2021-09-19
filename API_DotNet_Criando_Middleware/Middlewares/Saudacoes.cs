using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_DotNet_Criando_Middleware.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class Saudacoes
    {
        private readonly RequestDelegate _next;

        public Saudacoes(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = 400;

            if (!httpContext.Request.Path.Equals("/saudacoes", StringComparison.Ordinal))
            {
                await httpContext.Response.WriteAsync($"{httpContext.Request.Path}{": Caminho de requisição inválido."}");
                return;
            }

            if (!httpContext.Request.Method.Equals("GET"))
            {
                await httpContext.Response.WriteAsync($"{httpContext.Request.Method}{": Método não suportado."}");
                return;
            }

            if(!httpContext.Request.Query.Any() ||
                string.IsNullOrEmpty(httpContext.Request.Query["nomes"]))
            {
                await httpContext.Response.WriteAsync("A consulta está vazia ou inválida.");
                return;
            }

            httpContext.Response.StatusCode = 200;

            List<string> nomes = httpContext.Request.Query["nomes"].ToString().Split(',').ToList();
            var sb = new StringBuilder();

            nomes.ForEach(n => sb.Append($"{"Olá, "}{n}{Environment.NewLine}"));

            // Também poderíamos fazer:
            //foreach (var item in nomes)
            //{
            //    sb.Append($"{"Olá, "}{item}{\n}");
            //}

            await httpContext.Response.WriteAsync(sb.ToString());

            return;
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.

    // Não há necessidade do código abaixo, pois vamos iniciar ele pela classe StartUp.

    //public static class SaudacoesExtensions
    //{
    //    public static IApplicationBuilder UseSaudacoes(this IApplicationBuilder builder)
    //    {
    //        return builder.UseMiddleware<Saudacoes>();
    //    }
    //}
}
