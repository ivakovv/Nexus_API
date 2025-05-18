using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Nexus_API.Interfaces;
using Nexus_API.Models;
using Nexus_API.Services;

var builder = WebApplication.CreateBuilder(args);

// �������� ������������
Console.WriteLine("Configuration loaded:");
Console.WriteLine(builder.Configuration.GetDebugView());

// ���������� ��������
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IDepositService, DepositService>();
builder.Services.AddScoped<ICreditTypeService, CreditTypeService>();
builder.Services.AddScoped<IS3Service, S3Service>();

// ��������� ���� ������
builder.Services.AddDbContext<NexusContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ��������� Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Nexus API", Version = "v1" });
});

builder.Services.AddControllers();

var app = builder.Build();

// ��������� ��������� middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nexus API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();