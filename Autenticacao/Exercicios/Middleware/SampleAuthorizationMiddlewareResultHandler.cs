using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using System.Security.Claims;

namespace Exercicios.Middleware
{
    public class SampleAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();

        public async Task HandleAsync(RequestDelegate next,
                                HttpContext context,
                                AuthorizationPolicy policy,
                                PolicyAuthorizationResult authorizeResult)
        {
            if (authorizeResult.Forbidden && authorizeResult.AuthorizationFailure!
                                                            .FailedRequirements
                                                            .OfType<Show404Requirement>()
                                                            .Any())
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
            }

            // If que verifica se existem CLAIMS configuradas na requisição
            if (context.User!.Claims!.ToList().Any())
            {
                System.Diagnostics.Debug.WriteLine("[ClaimsNaRequest] #### ROTINA PRA LISTAR CLAIMS RECEBIDA NO CONTEXT ####");
                System.Diagnostics.Debug.WriteLine($"[ClaimsNaRequest] Retornar o status da autorização {authorizeResult.Succeeded}");

                foreach (var item in context.User.Claims.ToList())
                {
                    if (item.Type == "id")
                    {
                        System.Diagnostics.Debug.WriteLine($"[ClaimsNaRequest] ID Recebido no Request {item.Value}");
                    }

                    if (item.Type == ClaimTypes.Role)
                    {
                        System.Diagnostics.Debug.WriteLine($"[ClaimsNaRequest] Role recebido no Request {item.Value}");
                    }
                }

                System.Diagnostics.Debug.WriteLine("[ClaimsNaRequest] #### FIM ####");
            }


            if (authorizeResult!.AuthorizationFailure != null)
            {
                System.Diagnostics.Debug.WriteLine("[AuthorizationFailure] #### ROTINA PRA LISTAR CLAIMS CONFIGURADO NA CONTROLLER ####");

                foreach (var listFailedRequirements in authorizeResult!.AuthorizationFailure!.FailedRequirements!.ToList())
                {
                    /*Aqui é feito um cast para a classe do tipo RolesAuthorizationRequirement*/
                    var listRolesConfiguradorNaController = ((Microsoft.AspNetCore.Authorization.Infrastructure.RolesAuthorizationRequirement)listFailedRequirements).AllowedRoles;

                    foreach (var roleConfiguradorNaController in listRolesConfiguradorNaController)
                    {
                        System.Diagnostics.Debug.WriteLine($"[AuthorizationFailure] Role configurada na controller {roleConfiguradorNaController}");
                    }
                }

                System.Diagnostics.Debug.WriteLine("[AuthorizationFailure] #### FIM ####");
            }


            await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }

        public class Show404Requirement : IAuthorizationRequirement { }

    }
}
