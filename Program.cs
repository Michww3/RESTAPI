using Microsoft.EntityFrameworkCore;
using RESTAPI.DTOs;

var builder = WebApplication.CreateBuilder();
var connection = "Data Source=app.db";
builder.Services.AddDbContext<DBContext>(options => options.UseSqlite(connection));

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/users", async (DBContext db) => await db.Persons.ToListAsync());

app.MapGet("/api/users/{id}", async (int id, DBContext db) =>
{
    Person? person = await db.Persons.FirstOrDefaultAsync(u => u.Id == id);
    if (person == null) return Results.NotFound(new { message = "Пользователь не найден" });

    return Results.Json(person);
});

app.MapDelete("/api/users/{id:int}", async (int id, DBContext db) =>
{
    Person? person = await db.Persons.FirstOrDefaultAsync(u => u.Id == id);

    if (person == null) return Results.NotFound(new { message = "Пользователь не найден" });

    db.Persons.Remove(person);
    await db.SaveChangesAsync();
    return Results.Json(person);
});

app.MapPost("/api/users", async (Person person, DBContext db) =>
{
    if (string.IsNullOrEmpty(person.Name))
        return Results.BadRequest("Name is required");

    await db.Persons.AddAsync(person);
    await db.SaveChangesAsync();
    return Results.Json(person);
});

app.MapPut("/api/users", async (Person person, DBContext db) =>
{
    if (await db.Persons.FirstOrDefaultAsync(u => u.Id == person.Id) == null)
        return Results.NotFound(new { message = "Пользователь не найден" });

    person.Age = person.Age;
    person.Name = person.Name;
    await db.SaveChangesAsync();
    return Results.Json(person);
});

app.Run();