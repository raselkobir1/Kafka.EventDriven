using Consumer.Service;
using Producer.Service;

var builder = WebApplication.CreateBuilder(args);

//kafka
builder.Services.AddSingleton<IKafkaProducer>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var bootstrapServers = configuration["KafkaProducerConfig:bootstrapServers"] ?? string.Empty;
    var clientId = configuration["KafkaProducerConfig:clientId"] ?? string.Empty;

    return new KafkaProducer(bootstrapServers, clientId);
});

builder.Services.AddHostedService(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var bootstrapServers = configuration["KafkaConsumerConfig:BootstrapServers"] ?? string.Empty;
    var groupId = configuration["KafkaConsumerConfig:GroupId"] ?? string.Empty;
    var autoOffsetRest = configuration["KafkaConsumerConfig:AutoOffsetRest"] ?? string.Empty;
    var enableAutoOffsetStore = configuration["KafkaConsumerConfig:EnableAutoOffsetStore"] ?? string.Empty;
    return new KafkaConsumer(bootstrapServers, groupId, autoOffsetRest, enableAutoOffsetStore);
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
await app.RunAsync();
