﻿using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Data;


namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
                                                               IConfiguration          configuration)
    {
        
        
        var connectionString = configuration.GetConnectionString("Database");
        
        //Add services to the container.
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();
        
        services.AddDbContext<ApplicationDbContext>((sp, opts) =>
                                                    {
                                                        opts.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                                                        opts.UseSqlServer(connectionString);
                                                    });
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        return services;
    }
}