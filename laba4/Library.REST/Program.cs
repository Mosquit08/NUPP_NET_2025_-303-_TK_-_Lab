using Microsoft.EntityFrameworkCore;
using Library.REST.Services;
using Library.REST.Models;
using Library.REST.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Добавляем контекст базы данных
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрируем сервисы
builder.Services.AddScoped<ICrudServiceAsync<BookModel>, CrudServiceAsync<BookModel>>();
builder.Services.AddScoped<ICrudServiceAsync<MagazineModel>, CrudServiceAsync<MagazineModel>>();
builder.Services.AddScoped<ICrudServiceAsync<ReaderModel>, CrudServiceAsync<ReaderModel>>();
builder.Services.AddScoped<ICrudServiceAsync<BorrowingRecordModel>, CrudServiceAsync<BorrowingRecordModel>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    // Development specific middleware
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
