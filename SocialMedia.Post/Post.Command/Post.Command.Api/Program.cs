using CQRS.Core.Domain;
using CQRS.Core.Infrastructure;
using Post.Command.Api.Commands;
using Post.Command.Api.Interfaces.Commands;
using Post.Command.Domain.Aggregates;
using Post.Command.Infrastructure.Configs;
using Post.Command.Infrastructure.Handlers;
using Post.Command.Infrastructure.Repositories;
using Post.Command.Infrastructure.Stores;
using CQRS.Core.Handlers;
using Post.Command.Infrastructure.Dispatchers;
using Post.Command.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureServices(builder.Configuration);


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
