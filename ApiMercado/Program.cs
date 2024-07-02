using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseMySQL(builder.Configuration.GetConnectionString("Conexao")));

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("MLB", client =>
{
    client.BaseAddress = new Uri("https://api.mercadolibre.com/items/");
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "APP_USR-6500094978999069-070207-6050275e26a42938ef5ff24164b7276b-1855456057");
});

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
