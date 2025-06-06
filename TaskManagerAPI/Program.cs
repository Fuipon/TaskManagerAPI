using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManagerAPI.BusinessLogic;
using TaskManagerAPI.Data;
using TaskManagerAPI.Repositories;
using TaskManagerAPI.Seeders;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(typeof(MappingProfile));
// Подключаем EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.WebHost.UseUrls("http://0.0.0.0:10000");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient",
        policy =>
        {
            policy.WithOrigins("https://localhost:7079") // порт клиента
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddAuthorization();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<TaskManagerAPI.Middleware.ErrorHandlingMiddleware>();



app.UseHttpsRedirection();
app.UseCors("AllowBlazorClient");

app.UseAuthorization();

app.MapControllers();

DbSeeder.Seed(app.Services);

app.Run();
