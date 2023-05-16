using Azure.Security.KeyVault.Secrets;
using DeliveryProjectAzureApi.Context;
using DeliveryProjectAzureApi.Helpers;
using DeliveryProjectAzureApi.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAzureClients(factory =>
{
    factory.AddSecretClient(builder.Configuration.GetSection("KeyVault"));
});

SecretClient secretClient = builder.Services.BuildServiceProvider().GetService<SecretClient>();
KeyVaultSecret keyVaultSecret = await secretClient.GetSecretAsync("DeliveryAzureDb");

string azureKeys = keyVaultSecret.Value;

builder.Services.AddSingleton<HelperOAuthToken>();
HelperOAuthToken helper = new HelperOAuthToken(builder.Configuration);
builder.Services.AddAuthentication(helper.GetAuthenticationOptions()).AddJwtBearer(helper.GetJwtOptions());
/*string connectionString = builder.Configuration.GetConnectionString("SqlAzure");*/
builder.Services.AddTransient<RepositoryDelivery>();
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(azureKeys));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Api DeliveryProject con Azure",
        Description = "Realizando API para Delivery Project con Azure OAuth",
        Version = "v1",
        Contact = new OpenApiContact()
        {
            Name = "Andreiggar",
            Email = "andreiggar@gmail.com",
        }
    });
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Api v1");
    options.RoutePrefix = "";
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    /*app.UseSwagger();
    app.UseSwaggerUI();*/
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
