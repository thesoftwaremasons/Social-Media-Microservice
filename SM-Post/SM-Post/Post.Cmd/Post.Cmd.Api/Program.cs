using Confluent.Kafka;
using CQRS.Core.Domain;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using Post.Cmd.Api.Command;
using Post.Cmd.Infrastructure.Config;
using Post.Cmd.Infrastructure.Dispatchers;
using Post.Cmd.Infrastructure.Producers;
using Post.Cmd.Infrastructure.Repository;
using Post.Cmd.Infrastructure.Store;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoConfig>(builder.Configuration.GetSection(nameof(MongoConfig)));
builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));
builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>(); 
builder.Services.AddScoped<IEventProducer, EventProducer>(); 
builder.Services.AddScoped<IEventStore, EventStore>();

//register command handler methods

var commandHandler = builder.Services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
var dispatcher = new CommandDispatcher();

dispatcher.RegisterHandler<NewPostCommand>(commandHandler.HandlerAsync);
dispatcher.RegisterHandler<EditMessageCommand>(commandHandler.HandlerAsync);
dispatcher.RegisterHandler<LikePostCommand>(commandHandler.HandlerAsync);
dispatcher.RegisterHandler<AddCommentCommand>(commandHandler.HandlerAsync);
dispatcher.RegisterHandler<EditCommentCommand>(commandHandler.HandlerAsync);
dispatcher.RegisterHandler<RemoveCommentCommand>(commandHandler.HandlerAsync);
dispatcher.RegisterHandler<DeletePostCommand>(commandHandler.HandlerAsync);

builder.Services.AddSingleton<ICommandDispatcher>(_ => dispatcher);
// Add services to the container.

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
