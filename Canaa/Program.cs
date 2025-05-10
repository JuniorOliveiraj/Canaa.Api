
using Canaa.Configs;
using Canaa.AppHost.utils;

using Ninject;

using Canaa.Ninject;
using Canaa.Infra.ExternalServices.Context;
using Canaa.Infra.ExternalServices.Dependency;
using Canaa.DataContracts.Auth.Context;
using Microsoft.AspNetCore.Server.Kestrel.Core;


var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5000);

    /* serverOptions.ListenAnyIP(5001, listenOptions =>
     {
         listenOptions.UseHttps(); // Habilita HTTPS
     });*/

    serverOptions.Limits.MinRequestBodyDataRate = new MinDataRate(
    bytesPerSecond: 100, 
    gracePeriod: TimeSpan.FromSeconds(10) 
);
});
// Program.cs (para .NET 6 ou superior)

 builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();//
 builder.Services.AddScoped<IUserContext, UserContext>();
 

Config.Init(builder.Configuration);


ServicesConfig.Configure(builder.Services, builder.Configuration);


JwtConfig.Configure(builder.Services, builder.Configuration);

SwaggerConfig.Configure(builder.Services);


IKernel kernel = new StandardKernel();
NinjectBindings.Register(kernel);
BusinessComponent.Initialize(kernel);
NinjectBindingsInfra.Register(kernel);
BusinessComponentInfra.Initialize(kernel);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendClients", // <-- este nome
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:3000",
                "https://www.juniorbelem.com"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});




var app = builder.Build();
app.UseCors("AllowFrontendClients");

/////
//  Ambiente de desenvolvimento
////
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
