using API.Errors;
using Core.Interfaces;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    { 
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {
            // services.AddEndpointsApiExplorer();

            // services.AddSwaggerGen();

            services.AddDbContext<StoreContext>(opt => 
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            //Redis
            services.AddSingleton<IConnectionMultiplexer>(c => {
                var options = ConfigurationOptions.Parse(
                    config.GetConnectionString("Redis"));
                
                return ConnectionMultiplexer.Connect(options);
            });

            //Declarar o interface e o serviço de caching
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();

            //Declarar o interface e repositório basket
            services.AddScoped<IBasketRepository, BasketRepository>(); 
            
            //Declarar o interface e repositório product
            services.AddScoped<IProductRepository, ProductRepository>(); 

            //Declarar serviço Payment 
            services.AddScoped<IPaymentService, PaymentService>();

            //Declarar o serviço token
            services.AddScoped<ITokenService, TokenService>();

            //Declarar o serviço order
            services.AddScoped<IOrderService, OrderService>();

            //Declarar o serviço unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Declarar o serviço genérico
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); 

            //Declarar o AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Configure Api Behaviours
            services.Configure<ApiBehaviorOptions>(options => 
            {
                options.InvalidModelStateResponseFactory = actionContext => 
                {
                    var errors = actionContext.ModelState
                                    .Where(e => e.Value.Errors.Count > 0)
                                    .SelectMany(x => x.Value.Errors)
                                    .Select(x=>x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors,
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            services.AddCors(opt=>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });

            return services;    
        }
    }
}