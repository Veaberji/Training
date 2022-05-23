using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddCors(options =>
    options.AddDefaultPolicy((b =>
    {
        b.SetIsOriginAllowed(origin =>
                new Uri(origin).Host == builder.Configuration["Host"])
            .AllowAnyHeader()
            .AllowAnyMethod();
    })));

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
