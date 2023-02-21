using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using Status.Api.Models;
using Status.Infrastructure.Models;
using Status.Api.Services;
using Status.Core.Models;
using Status.Infrastructure;

// Register MongoDb class mappings
BsonClassMap.RegisterClassMap<Incident>(cm =>
{
    cm.AutoMap();
    cm.MapIdProperty(c => c.Id)
        .SetIdGenerator(StringObjectIdGenerator.Instance)
        .SetSerializer(new StringSerializer(BsonType.ObjectId));
});

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<ServersOptions>()
    .Bind(builder.Configuration.GetSection(ServersOptions.configurationSectionName))
    .ValidateDataAnnotations();

builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("StatusDatabase"));
builder.Services.AddSingleton<IIncidentsRepository, IncidentsRepository>();
builder.Services.AddSingleton<ResponseService>();

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
