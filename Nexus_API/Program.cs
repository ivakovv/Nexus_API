using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Nexus_API.Interfaces;
using Nexus_API.Models;
using Nexus_API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});


// Проверка конфигурации
Console.WriteLine("Configuration loaded:");
Console.WriteLine(builder.Configuration.GetDebugView());

// Добавление сервисов
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<ICreditTypeService, CreditTypeService>();
builder.Services.AddScoped<ICreditService, CreditService>();
builder.Services.AddScoped<IS3Service, S3Service>();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IDepositService, DepositService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ICardPoolService, CardPoolService>();
builder.Services.AddScoped<IClientAccountService, ClientAccountService>();

// Настройка базы данных
builder.Services.AddDbContext<NexusContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Настройка Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Nexus API", Version = "v1" });
});

builder.Services.AddControllers();

var app = builder.Build();

// Настройка конвейера middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nexus API v1");
    });
}
app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();