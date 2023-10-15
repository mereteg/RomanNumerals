using RomanNumeralsBusinessLogic.Contract.Interfaces;
using RomanNumeralsBusinessLogic.Engines;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRomanNumeralsEngine, RomanNumeralsEngine>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Simple API currently ONLY to be used for testing/validation
// Missing, security, error handling etc.

app.MapGet("/romannumerals/convertfromint/{intvalue}", (int intvalue, IRomanNumeralsEngine engine) =>
{
    return engine.ConvertToRomanNumeral(intvalue);
})
.WithOpenApi();

app.MapGet("/romannumerals/converttoint/{romannumeral}", (string romannumeral, IRomanNumeralsEngine engine) =>
{
    return engine.ConvertToInt(romannumeral);
})
.WithOpenApi();

app.Run();


