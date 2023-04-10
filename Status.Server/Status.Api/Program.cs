using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using Status.Infrastructure.Models;
using Status.Core.Models;
using Status.Infrastructure.Workers;
using Status.Infrastructure.Services;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Status.Infrastructure.Repositories;

// Register MongoDb class mappings
BsonClassMap.RegisterClassMap<Incident>(cm =>
{
    cm.AutoMap();
    cm.MapIdProperty(c => c.Id)
        .SetIdGenerator(StringObjectIdGenerator.Instance)
        .SetSerializer(new StringSerializer(BsonType.ObjectId));
});
BsonClassMap.RegisterClassMap<Server>(cm =>
{
    cm.AutoMap();
    cm.MapIdProperty(c => c.Id)
        .SetIdGenerator(StringObjectIdGenerator.Instance)
        .SetSerializer(new StringSerializer(BsonType.ObjectId));
});

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<StatusOptions>()
    .ValidateDataAnnotations();

builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.Configure<StatusOptions>(builder.Configuration);
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("StatusDatabase"));
builder.Services.AddSingleton<IIncidentsRepository, IncidentsRepository>();
builder.Services.AddSingleton<IServerRepository, ServerRepository>();
builder.Services.AddSingleton<IResponseService, ResponseService>();

builder.Services.AddHostedService<ResponseWorker>();

builder.Services.AddSingleton<MongoClient>(provider =>
{
    IOptions<DatabaseSettings> options = provider.GetService<IOptions<DatabaseSettings>>() ?? throw new ArgumentNullException(nameof(provider));
    return new(options.Value.ConnectionString);
});

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen().AddSwaggerGenNewtonsoftSupport();

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
