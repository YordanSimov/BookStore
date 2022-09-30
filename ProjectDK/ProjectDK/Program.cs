using ProjectDK.BL.Interfaces;
using ProjectDK.BL.Services;
using ProjectDK.DL.Interfaces;
using ProjectDK.DL.Repositories.InMemoryRepositories;
using ProjectDK.Models;
using ProjectDK.Models.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<IPersonInMemoryRepository,PersonInMemoryRepository>();
builder.Services.AddSingleton<IAuthorInMemoryRepository,AuthorInMemoryRepository>();
builder.Services.AddSingleton<IBookInMemoryRepository,BookInMemoryRepository>();
builder.Services.AddSingleton<IPersonService,PersonService>();
builder.Services.AddSingleton<IAuthorService,AuthorService>();
builder.Services.AddSingleton<IBookService,BookService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseAuthorization();

app.MapControllers();

app.Run();
