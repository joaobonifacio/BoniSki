using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Stripe;

namespace API.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        public int timeToLiveInSeconds;

        public CachedAttribute(int timeToLiveSeconds)
        {
            timeToLiveInSeconds = timeToLiveSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //ANTES DE MANDAR O REQUEST PARA O CONTROLLER
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            //Queremos gerar uma key, baseada nos request params, p identificar na redis database
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            var cacheResponse = await cacheService.GetCachedResponseAsync(cacheKey);

            //Aqui verificamos se já temos uma cached response para a key
            // Se tivermos resposta em cache, não vamos à BD, devolvemos o q temos em cache
            if(!string.IsNullOrEmpty(cacheResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cacheResponse,
                    ContentType = "application.json",
                    StatusCode = 200
                };

                context.Result = contentResult;
                return;
            } 

            //AGORA SIM, VAMOS MANDAR O REQUEST PARA O CONTROLLER
            //Se não tivermos nada em cache
            var excutedContext = await next(); // E aqui vamos para o controller.

            if(excutedContext.Result is OkObjectResult okObjectResult) 
            {
                //Neste caso pormos o resultado okObjectResulv.Value na cache
                await cacheService.CacheResponseAsync(cacheKey, okObjectResult.Value, 
                    TimeSpan.FromSeconds(timeToLiveInSeconds));
            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            //O nosso request vai ter query string parameters (productParams, por exemplo, no ProductsController)
            var keyBuilder = new StringBuilder();

            keyBuilder.Append($"{request.Path}");

            foreach(var (key, value) in request.Query.OrderBy(x=>x.Key))
            {
                keyBuilder.Append($"{key}-{value}");
            }

            return keyBuilder.ToString();
        }
    }
}