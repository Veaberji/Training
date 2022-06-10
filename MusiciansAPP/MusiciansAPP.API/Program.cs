using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusiciansAPP.API.Extensions.Configuration;
using MusiciansAPP.API.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;


builder.BindObjects();
services.AddAppCors();
services.AddAppServices(builder.Configuration);
services.AddDbServices(builder.Configuration);
services.AddControllers();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.MapControllers();

app.Run();