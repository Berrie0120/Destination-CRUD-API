using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using DestinationApi.Models;
using Microsoft.AspNetCore.Cors;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DestiationRows") ?? "Data Source=Destination.db";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSqlite<DestinationDb>(connectionString);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DestinationRow API",
        Description = "Planning the destinations you love",
        Version = "v1"
    });
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});


var app = builder.Build();

app.UseCors("AllowReactApp");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Destination API V1");
});

app.MapGet("/", () => "Hello World!");

app.MapGet("/destinationRow", async (DestinationDb db) => await db.DestinationRows.ToListAsync());

app.MapPost("/destinationRow", async (DestinationDb db, DestinationRow destinationRow) =>
{
    await db.DestinationRows.AddAsync(destinationRow);
    await db.SaveChangesAsync();
    return Results.Created($"/destinationRow/{destinationRow.Id}", destinationRow);
});

app.MapGet("/destinationRow/{id}", async (DestinationDb db, int id) => await db.DestinationRows.FindAsync(id));

app.MapPut("/destinationRow/{id}", async (DestinationDb db, DestinationRow updateDetinationRow, int id) =>
{
    var destinationRow = await db.DestinationRows.FindAsync(id);
    if (destinationRow is null) return Results.NotFound();
    destinationRow.Name = updateDetinationRow.Name;
    destinationRow.Destination = updateDetinationRow.Destination;
    destinationRow.Icon = updateDetinationRow.Icon;
    destinationRow.Season = updateDetinationRow.Season;
    destinationRow.Reason = updateDetinationRow.Reason;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/destinationRow/{id}", async (DestinationDb db, int id) =>
{
    var destinationRow = await db.DestinationRows.FindAsync(id);
    if (destinationRow is null)
    {
        return Results.NotFound();
    }
    db.DestinationRows.Remove(destinationRow);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();