using VitaOrbitApi.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

//Buscando conecxão com o banco de dados Oracle a partir do arquivo appsettings.json
//var connectionString = builder.Configuration.GetConnectionString("OracleConnection");

var connectionString =
    Environment.GetEnvironmentVariable("OracleConnection")
    ?? builder.Configuration.GetConnectionString("OracleConnection");

//Injetando o contexto do banco de dados na aplicação, utilizando a string de conexão obtida
builder.Services.AddDbContext<AppDbContext>(
    options =>
    options.UseOracle(connectionString,
    compatibility => compatibility.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion19)));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler =
        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        // Personalizando o título do documento OpenAPI
        document.Info.Title = "Global Solution VitaOrbit - Documentação OpenAPI";
        document.Info.Version = "v1";
        document.Info.Description = "Documentação da API VitaOrbit - Plataforma de monitoramento de saúde em ambientes extremos.";
        return Task.CompletedTask;
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("VitaOrbit API")
            .WithTheme(ScalarTheme.BluePlanet)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

//app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();
var port = Environment.GetEnvironmentVariable("PORT") ?? "5052";
app.Urls.Add($"http://0.0.0.0:{port}");
app.Run();

