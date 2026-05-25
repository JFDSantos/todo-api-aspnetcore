using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ToDo.Application.Dtos;
using ToDo.Application.Mappings;
using ToDo.Application.Services;
using ToDo.Application.Validators;
using ToDo.Infrastructure.Data;
using ToDo.Infrastructure.Repositories;
using ToDo.Domain.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        // Garante que DateTime seja serializado com 'Z' (UTC) para o frontend interpretar corretamente
        opts.JsonSerializerOptions.Converters.Add(new ToDo.API.Infrastructure.UtcDateTimeConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Database Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add Repository (Data Access - Dependency Inversion)
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

// Add Application Services
builder.Services.AddScoped<ITaskService, TaskService>();

// Add FluentValidation
builder.Services.AddScoped<IValidator<CreateTaskDto>, CreateTaskValidator>();
builder.Services.AddScoped<IValidator<UpdateTaskDto>, UpdateTaskValidator>();

// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add CORS - Permitir requisições do Frontend Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://localhost:55624")  // Frontend Angular (portas)
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Apply migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Usar CORS antes de MapControllers
app.UseCors("AllowFrontend");

app.MapControllers();

app.Run();
