using Microsoft.EntityFrameworkCore; // Asegúrate de que el paquete Microsoft.EntityFrameworkCore está instalado
using FrontApi.Data; // Asegúrate de usar el namespace correcto para MyDbContext

var builder = WebApplication.CreateBuilder(args);

// Configurar CORS (si es necesario)
var misReglasCors = "ReglasCors";
builder.Services.AddCors(options =>
    options.AddPolicy(name: misReglasCors, policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
                     .AllowAnyHeader()
                     .AllowAnyMethod();
    })
);

// Registrar MyDbContext y la conexión a la base de datos
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL")));

// Registrar controladores con vistas para MVC y controladores para la API
builder.Services.AddControllersWithViews(); // Para vistas (frontend)
builder.Services.AddControllers();          // Para controladores API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();           // Para documentación de Swagger (si es necesario)
builder.Services.AddHttpClient(); // Agregar servicio para HttpClient

// Construir la aplicación
var app = builder.Build();

// Configurar el pipeline de la aplicación
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Página de error para entornos de producción
}

// Asegúrate de agregar UseCors antes de UseRouting
app.UseCors(misReglasCors); // Configurar CORS globalmente

// Swagger solo en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware estándar
app.UseStaticFiles();  // Archivos estáticos (CSS, JS, imágenes)
app.UseRouting();      // Configuración de rutas
app.UseAuthorization(); // Autorización (si aplicas políticas)

// Rutas para MVC y API
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Productos}/{action=Index}/{id?}"); // Ruta para vistas

app.MapControllers(); // Ruta para controladores de la API

// Iniciar la aplicación
app.Run();
