using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Middleware.API.Real.Auth
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        private static string UserNameConfig = ConfigurationManager.AppSettings["BasicAuthUsername"];
        private static string PasswordConfig = ConfigurationManager.AppSettings["BasicAuthPassword"];
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(nameof(actionContext));
            }

            // Verifique se o cabeçalho de autorização está presente
            if (actionContext.Request.Headers.Authorization != null &&
                actionContext.Request.Headers.Authorization.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase))
            {
                // Extrair credenciais
                string encodedUserPass = actionContext.Request.Headers.Authorization.Parameter;
                string userPass = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUserPass));
                string[] parts = userPass.Split(':');
                string username = parts[0];
                string password = parts[1];

                // Verifique as credenciais (substitua isso pela lógica de autenticação real)
                if (IsValidUser(username, password))
                {
                    // Crie uma identidade para o usuário
                    var identity = new GenericIdentity(username, "Basic");

                    // Defina o principal do usuário
                    var principal = new GenericPrincipal(identity, null);
                    Thread.CurrentPrincipal = principal;

                    // Se necessário, defina o principal do usuário na solicitação
                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.User = principal;
                    }

                    return;
                }
            }

            // Credenciais inválidas, retorne uma resposta de Não Autorizado
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Headers.Add("WWW-Authenticate", "Unauthorized");
        }

        private bool IsValidUser(string username, string password)
        {
            // Lógica de validação das credenciais aqui
            // Substitua pelo mecanismo de autenticação real
            return username == UserNameConfig && password == PasswordConfig;
        }
    }
}