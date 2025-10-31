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

// Configuração do Kestrel
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MinRequestBodyDataRate = new MinDataRate(
        bytesPerSecond: 100,
        gracePeriod: TimeSpan.FromSeconds(10)
    );
});

// Carrega variáveis de ambiente
builder.Configuration.AddEnvironmentVariables();
builder.Configuration["ConnectionStrings:DefaultConnection"] = Env.GetString("ConnectionStrings__DefaultConnection");
builder.Configuration["Jwt:Key"] = Env.GetString("Jwt__Key");
builder.Configuration["Jwt:Issuer"] = Env.GetString("Jwt__Issuer");
builder.Configuration["Jwt:Audience"] = Env.GetString("Jwt__Audience");
builder.Configuration["Resend:ApiKey"] = Env.GetString("api__key");

// Inicialização de configurações personalizadas
Config.Init(builder.Configuration);
Canaa.Infra.Entities.Config.Init(builder.Configuration);

// Injeção de dependências
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
IKernel kernel = new StandardKernel();

ServicesConfig.Configure(builder.Services, builder.Configuration);
JwtConfig.Configure(builder.Services, builder.Configuration);
SwaggerConfig.Configure(builder.Services);
TabelasConfig.Configure(builder.Services, builder.Configuration);
EmailConfig.Configure(builder.Services, builder.Configuration);

NinjectBindings.Register(kernel);
BusinessComponent.Initialize(kernel);
NinjectBindingsInfra.Register(kernel);
BusinessComponentInfra.Initialize(kernel);
BusinessComponentInfraEntities.Initialize(kernel);
NinjectBindingsInfraEntitis.Register(kernel);

// ✅ CONFIGURAÇÃO DO CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendClients",
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:5173",    // Porta padrão do Vite
                "http://localhost:3000",    // Porta padrão do CRA
                "http://localhost:5000",
                "http://152.67.61.114:5000",
                "http://192.168.3.18:5000",
                "https://app.juniorbelem.com"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
            // .AllowCredentials(); // Se usar cookies/autenticação baseada em sessão
        });
});

// Banco de dados
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        Config.GetConnectionString(),
        ServerVersion.AutoDetect(Config.GetConnectionString())
    )
);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// =========================================================
// 🚀 PIPELINE DE EXECUÇÃO
// =========================================================
var app = builder.Build();

// ⚙️ IMPORTANTE: Ordem correta dos middlewares
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ✅ 1. Roteamento antes do CORS
app.UseRouting();

// ✅ 2. Aplicar política de CORS (tem que vir DEPOIS do UseRouting e ANTES do Auth)
app.UseCors("AllowFrontendClients");

// ✅ 3. Autenticação / Autorização
app.UseAuthentication();
app.UseAuthorization();

// ✅ 4. Mapear os controllers
app.MapControllers();

// ✅ 5. Executar aplicação
app.Run();
