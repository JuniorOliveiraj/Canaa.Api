
using Canaa.AppHost.utils;
using Canaa.Configs;
using Canaa.DataContracts.Auth.Context;
using Canaa.Infra.Entities.BefDb;
using Canaa.Infra.Entities.Context;
using Canaa.Infra.Entities.Dependecy;
using Canaa.Infra.Entities.Entities;
using Canaa.Infra.Entities.Tabelasbef;
using Canaa.Infra.ExternalServices.Dependency;
using Canaa.Ninject;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Ninject;
using DotNetEnv;

Env.Load();
var builder = WebApplication.CreateBuilder(args);




builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MinRequestBodyDataRate = new MinDataRate(
        bytesPerSecond: 100,
        gracePeriod: TimeSpan.FromSeconds(10)
    );
});


builder.Configuration.AddEnvironmentVariables();



builder.Configuration["ConnectionStrings:DefaultConnection"] = Env.GetString("ConnectionStrings__DefaultConnection");
builder.Configuration["Jwt:Key"] = Env.GetString("Jwt__Key");
builder.Configuration["Jwt:Issuer"] = Env.GetString("Jwt__Issuer");
builder.Configuration["Jwt:Audience"] = Env.GetString("Jwt__Audience");



Config.Init(builder.Configuration);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();





ServicesConfig.Configure(builder.Services, builder.Configuration);


JwtConfig.Configure(builder.Services, builder.Configuration);

SwaggerConfig.Configure(builder.Services);
TabelasConfig.Configure(builder.Services, builder.Configuration);


IKernel kernel = new StandardKernel();
NinjectBindings.Register(kernel);
BusinessComponent.Initialize(kernel);
NinjectBindingsInfra.Register(kernel);
BusinessComponentInfra.Initialize(kernel);
BusinessComponentInfraEntities.Initialize(kernel);
NinjectBindingsInfraEntitis.Register(kernel);



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendClients", 
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:5000",           
                "http://152.67.61.114:5000",
                "https://app.juniorbelem.com",
                "http://192.168.3.18:5000" // ADICIONE ISSO

            )
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        Config.GetConnectionString(),
        ServerVersion.AutoDetect(Config.GetConnectionString())
    )
);
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(Config.GetConnectionString(), ServerVersion.AutoDetect(Config.GetConnectionString()))
);

var app = builder.Build();
app.UseCors("AllowFrontendClients");

/////
//  Ambiente de desenvolvimento
////
///


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();
// Middlewares padrão
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
