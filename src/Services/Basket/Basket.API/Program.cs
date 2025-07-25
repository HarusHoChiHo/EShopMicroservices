using Basket.API.Data;
using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Messaging.MassTransit;
using DiscountGrpc;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
var assembly = typeof(Program).Assembly;
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
                            {
                                config.RegisterServicesFromAssembly(assembly);
                                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
                            });

builder.Services.AddMarten(opts =>
                           {
                               opts.Connection(builder.Configuration.GetConnectionString("Database")!);
                               opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
                           })
       .UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();
// Manual version of set up decorator
// builder.Services.AddScoped<IBasketRepository>(provider =>
//                                               {
//                                                   var basketRepository = provider.GetRequiredService<IBasketRepository>();
//                                                   return new CachedBasketRepository(basketRepository, provider.GetRequiredService<IDistributedCache>());
//                                               });

builder.Services.AddStackExchangeRedisCache(options =>
                                            {
                                                options.Configuration = builder.Configuration.GetConnectionString("Redis");
                                            });

//gRPC services
builder.Services
       .AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
                                                                       {
                                                                           options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
                                                                       })
       .ConfigurePrimaryHttpMessageHandler(() =>
                                           {
                                               var handler = new HttpClientHandler
                                               {
                                                   ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                                               };
                                               return handler;
                                           });

//Async Communication Services
builder.Services.AddMessageBroker(builder.Configuration);

//Cross-cutting services
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
       .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
       .AddRedis(builder.Configuration.GetConnectionString("Redis")!);




var app = builder.Build();

// Configure the HTTP request pipeline
app.MapCarter();
app.UseExceptionHandler(options =>
                        {
                        });
app.UseHealthChecks("/health",
                    new HealthCheckOptions
                    {
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                    });
app.Run();