using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTrackerAPI.Data;
using Task = TaskTrackerAPI.Data.Models.Task;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<DataContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(options => options.AddPolicy(name: "TaskOrigins", policy =>
{
    policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
}));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{ 
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("TaskOrigins");

app.MapGet("/tasks", async (DataContext context) =>
    await context.Tasks.ToListAsync());

app.MapPost("/tasks", async (DataContext context, Task taskInput) =>
{
    Task task = new Task
    {
        Text = taskInput.Text,
        Day = taskInput.Day,
        Reminder = taskInput.Reminder
    };

    await context.Tasks.AddAsync(task);
    await context.SaveChangesAsync();
    return Results.Ok(taskInput);
});

app.MapPut("/tasks/{id}", async (DataContext context, Task taskInput, int id) =>
{
    var task = await context.Tasks.FindAsync(id);
    if (task == null)
    {
        return Results.NotFound();
    }
    task.Reminder = taskInput.Reminder;
    await context.SaveChangesAsync();
    return Results.Ok(task);
});

app.MapDelete("/tasks/{id}", async (DataContext context, int id) =>
{
    var task = await context.Tasks.FindAsync(id);
    if(task == null)
    {
        return Results.NotFound();
    }
    context.Tasks.Remove(task);
    await context.SaveChangesAsync();
    return Results.Ok(task);
});


app.Run();
