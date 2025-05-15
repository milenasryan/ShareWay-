using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Backend.Data;  // Подключаем AppDbContext

var builder = WebApplication.CreateBuilder(args);

// ✅ Добавляем CORS политику
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// Подключение к PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавляем поддержку контроллеров API
builder.Services.AddControllers();

// Добавляем OpenAPI (Swagger)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ✅ Включаем CORS перед другими middleware
app.UseCors("AllowAll");

// ✅ Разрешаем статические файлы из папки "public"
var publicPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../public"));

if (Directory.Exists(publicPath))
{
    app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(publicPath)
});

    Console.WriteLine($"✅ Static files are served from: {publicPath}");
}
else
{
    Console.WriteLine("⚠️ Warning: The 'public' folder was not found!");
}

// Настраиваем OpenAPI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Console.WriteLine($"Static files path: {Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "images")}");


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers(); // Подключаем контроллеры

// Добавляем тестовый маршрут для проверки работы API
app.MapGet("/", () => "API is running!");

app.Run();
