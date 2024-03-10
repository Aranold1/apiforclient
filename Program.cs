using Microsoft.EntityFrameworkCore;
using WebApiForGptBlazor.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("ConnectionString") ?? throw new Exception("db connection string is null");
var apiKey = builder.Configuration.GetConnectionString("ApiKey");
builder.Services.AddSingleton<OpenAiApi>(x=>new OpenAiApi(apiKey));
builder.Services.AddAutoMapper(typeof(Program).Assembly);
var openAiApi = new OpenAiApi(connectionString);
builder.Services.AddDbContext<ChatGptCloneTstDbContext>(options=>options.UseNpgsql(connectionString));

var app = builder.Build();
app.MapControllers();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.Run();