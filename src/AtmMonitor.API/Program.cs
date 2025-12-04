using AtmMonitor.API.Data;
using AtmMonitor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("VueFront", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:8080").AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

await DbSeeder.SeedAsync(app.Services, app.Logger);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("VueFront");

app.MapControllers();

app.Run();
