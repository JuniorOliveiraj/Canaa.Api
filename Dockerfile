# =========================
# Etapa de Build
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia somente arquivos de projeto e solução para acelerar restore
COPY Canaa/Canaa.sln Canaa/
COPY Canaa/Canaa.csproj Canaa/
COPY Canaa.DataContracts/Canaa.DataContracts.csproj Canaa.DataContracts/
COPY Canaa.FN.BusinessComponents/Canaa.FN.BusinessComponents.csproj Canaa.FN.BusinessComponents/
COPY Canaa.Infra.Entities/Canaa.Infra.Entities.csproj Canaa.Infra.Entities/
COPY Canaa.Infra.ExternalServices/Canaa.Infra.ExternalServices.csproj Canaa.Infra.ExternalServices/

# Restaura dependências
RUN dotnet restore Canaa/Canaa.sln

# Copia todo o restante do projeto
COPY Canaa/ Canaa/
COPY Canaa.DataContracts/ Canaa.DataContracts/
COPY Canaa.FN.BusinessComponents/ Canaa.FN.BusinessComponents/
COPY Canaa.Infra.Entities/ Canaa.Infra.Entities/
COPY Canaa.Infra.ExternalServices/ Canaa.Infra.ExternalServices/

# Build e publish
RUN dotnet publish Canaa/Canaa.sln -c Release -o /app/publish

# =========================
# Etapa de Runtime
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copia arquivos publicados
COPY --from=build /app/publish .

# Porta padrão para Railway
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

# Executa a aplicação
ENTRYPOINT ["dotnet", "Canaa.dll"]
