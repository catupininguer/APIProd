using Microsoft.EntityFrameworkCore; // Aseg�rate de que el paquete Microsoft.EntityFrameworkCore est� instalado
using FrontApi.Data; // Aseg�rate de usar el namespace correcto para MyDbContext

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

// Registrar MyDbContext y la conexi�n a la base de datos
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL")));

// Registrar controladores con vistas para MVC y controladores para la API
builder.Services.AddControllersWithViews(); // Para vistas (frontend)
builder.Services.AddControllers();          // Para controladores API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();           // Para documentaci�n de Swagger (si es necesario)
builder.Services.AddHttpClient(); // Agregar servicio para HttpClient

// Construir la aplicaci�n
var app = builder.Build();

// Configurar el pipeline de la aplicaci�n
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // P�gina de error para entornos de producci�n
}

// Aseg�rate de agregar UseCors antes de UseRouting
app.UseCors(misReglasCors); // Configurar CORS globalmente

// Swagger solo en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware est�ndar
app.UseStaticFiles();  // Archivos est�ticos (CSS, JS, im�genes)
app.UseRouting();      // Configuraci�n de rutas
app.UseAuthorization(); // Autorizaci�n (si aplicas pol�ticas)

// Rutas para MVC y API
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Productos}/{action=Index}/{id?}"); // Ruta para vistas

app.MapControllers(); // Ruta para controladores de la API

// Iniciar la aplicaci�n
app.Run();
