using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var notebooks = new List<Notebook>();

// POST: api/notebooks
app.MapPost("/api/notebooks", (Notebook notebook) =>
{
    notebooks.Add(notebook);
    return Results.Created($"/api/notebooks/{notebook.Subject}", notebook);
});

// GET: api/notebooks
app.MapGet("/api/notebooks", () => notebooks);

// GET: api/notebooks/{subject}
app.MapGet("/api/notebooks/{subject}", (string subject) =>
{
    var notebook = notebooks.FirstOrDefault(n => n.Subject.Equals(subject, StringComparison.OrdinalIgnoreCase));
    return notebook is not null ? Results.Ok(notebook) : Results.NotFound();
});

// PUT: api/notebooks/{subject}
app.MapPut("/api/notebooks/{subject}", (string subject, Notebook updatedNotebook) =>
{
    var notebook = notebooks.FirstOrDefault(n => n.Subject.Equals(subject, StringComparison.OrdinalIgnoreCase));
    if (notebook is null) return Results.NotFound();

    notebook.Year = updatedNotebook.Year;
    notebook.NumberOfPages = updatedNotebook.NumberOfPages;
    notebook.Type = updatedNotebook.Type;
    return Results.NoContent();
});

// DELETE: api/notebooks/{subject}
app.MapDelete("/api/notebooks/{subject}", (string subject) =>
{
    var notebook = notebooks.FirstOrDefault(n => n.Subject.Equals(subject, StringComparison.OrdinalIgnoreCase));
    if (notebook is null) return Results.NotFound();

    notebooks.Remove(notebook);
    return Results.NoContent();
});

app.Run();
public class AppDbContext : DbContext
{
    public DbSet<Notebook> Notebooks { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
public class Notebook
{
    public int Id { get; set; } // Уникальный идентификатор
    public string Subject { get; set; }
    public int Year { get; set; }
    public int NumberOfPages { get; set; }
    public string Type { get; set; }

    // Конструктор по умолчанию
    public Notebook() { }

    public Notebook(string subject, int year, int numberOfPages, string type)
    {
        Subject = subject;
        Year = year;
        NumberOfPages = numberOfPages;
        Type = type;
    }
}