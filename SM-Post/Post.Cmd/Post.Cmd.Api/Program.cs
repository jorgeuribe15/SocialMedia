using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using Post.Cmd.Api.Commands;
using Post.Cmd.Domain.Agregates;
using Post.Cmd.Infrastructure.Config;
using Post.Cmd.Infrastructure.Dispatchers;
using Post.Cmd.Infrastructure.Handlers;
using Post.Cmd.Infrastructure.Repositories;
using Post.Cmd.Infrastructure.Stores;
using System.Diagnostics.Tracing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));

////
builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
builder.Services.AddScoped<IEventStore, EventStore>();
builder.Services.AddScoped<IEventSourcingHandler<PostAgregate>, EventSourcingHandler>();
builder.Services.AddScoped<ICommnadHandler, CommnadHandler>();

/// Register command handler methods
var commnadHandler = builder.Services.BuildServiceProvider().GetRequiredService<ICommnadHandler>();
var dispatcher = new CommandDispatcher();
dispatcher.RegisterHandler<NewPostCommand>(commnadHandler.HandleAsync);



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
