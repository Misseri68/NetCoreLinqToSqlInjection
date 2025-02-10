using NetCoreLinqToSqlInjection.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

Coche car = new Coche();
car.Marca = "Honda";
car.Modelo = " Civic 2020 Type R";
car.Imagen = "honda.jpg";
car.Velocidad = 0;
car.VelocidadMaxima = 279;

builder.Services.AddSingleton<ICoche, Coche>(x => car);

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Coches}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
