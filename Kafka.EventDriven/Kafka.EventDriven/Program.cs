using Confluent.Kafka;
using Consumer.Service;
using Producer.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var objBuilder = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true);
IConfiguration configuration = objBuilder.Build();

//kafka
builder.Services.AddSingleton<IKafkaProducer>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var bootstrapServers = configuration["KafkaProducerConfig:bootstrapServers"];

    return new KafkaProducer(bootstrapServers);
});

builder.Services.AddHostedService(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var bootstrapServers = configuration["KafkaConsumerConfig:BootstrapServers"];
    var groupId = configuration["KafkaConsumerConfig:GroupId"];
    var autoOffsetRest = configuration["KafkaConsumerConfig:AutoOffsetRest"];
    var enableAutoOffsetStore = configuration["KafkaConsumerConfig:EnableAutoOffsetStore"];
    return new KafkaConsumer(bootstrapServers, groupId, autoOffsetRest, enableAutoOffsetStore);
});


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

app.UseAuthorization();

app.MapControllers();
await app.RunAsync();
