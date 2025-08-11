using Confluent.Kafka;
using KafkaProvider.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<KafkaProducerService>();
app.MapGet("/", () => "Kafka Gateway is running");

app.Run();

