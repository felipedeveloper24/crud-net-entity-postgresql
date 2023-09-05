using CrudAPI.Data;
using CrudAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Obtenemos la info desde appsettings.json que es como el .env que existe en node

var ConnectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");
builder.Services.AddDbContext<ConexionBD>(options => options.UseNpgsql(ConnectionString));
//Configuramos que vamos a utilizar base de datos postgresql
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Endpoint para crear una persona
app.MapPost("/persona", async (Persona P, ConexionBD db) =>
{
    //Con esto se agrega una nuueva entidad en la base de datos
    db.Personas.Add(P);

    await db.SaveChangesAsync();//Guardamos los datos


    return Results.Created("Persona registrada correctamente",P);
});
//Endpoint para obtener un registro
app.MapGet("/persona/{id:int}", async (int id, ConexionBD db) =>
{
    return await db.Personas.FindAsync(id) is Persona p ? Results.Ok(p) : Results.NotFound();
});
//Endpoint para actualizar un registro
app.MapPut("/persona/{id:int}", async (int id, Persona P, ConexionBD db) =>
{
    if (P.Id != id)
    {
        return Results.BadRequest();
    }
    var persona = await db.Personas.FindAsync(id);
    if (persona is null) return Results.NotFound();

    persona.Nombre = P.Nombre;
    persona.Apellido = P.Apellido;
    persona.Edad = P.Edad;
    persona.Correo = P.Correo;

    await db.SaveChangesAsync();
    return Results.Ok(persona);


});

app.MapDelete("/persona/{id:int}", async (int id, ConexionBD db) =>
{
    var persona = await db.Personas.FindAsync(id);
    if(persona is not null)
    {
        db.Remove(persona);
        await db.SaveChangesAsync();
    }
    return Results.NoContent();
});

app.MapGet("/personas", async (ConexionBD db) =>
{
    var personas = await db.Personas.ToListAsync();

    if (personas.Count == 0) return Results.Ok("No hay personas registradas");

    return Results.Ok(personas);
});

app.Run();

