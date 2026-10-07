using System.Text.Json.Serialization;
using Gochs.ProtectiveStructures.Data;
using Gochs.ProtectiveStructures.Exceptions;
using Gochs.ProtectiveStructures.Repositories.Implementations;
using Gochs.ProtectiveStructures.Repositories.Interfaces;
using Gochs.ProtectiveStructures.Services.Implementations;
using Gochs.ProtectiveStructures.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter(allowIntegerValues: false)));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProtectiveStructureRepository, ProtectiveStructureRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IInspectionRepository, InspectionRepository>();

builder.Services.AddScoped<IProtectiveStructureService, ProtectiveStructureService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IInspectionService, InspectionService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
