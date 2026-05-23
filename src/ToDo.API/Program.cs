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
builder.Services.AddControllers();
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

app.MapControllers();

app.Run();
