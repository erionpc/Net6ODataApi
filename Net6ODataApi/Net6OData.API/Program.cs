using Microsoft.AspNetCore.OData;
using Net6OData.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
                //.AddNewtonsoftJson()    // commenting this out causes HTTP Patch to not work
                                        // including it causes OData queries to not work
                .AddOData(options => options
                    .Select()
                    .Filter()
                    .OrderBy()
                    .SetMaxTop(100)
                    .Count());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
