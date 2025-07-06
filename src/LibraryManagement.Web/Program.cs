using LibraryManagement.Application;
using LibraryManagement.Infrastructure;
using LibraryManagement.Presentation;
using LibraryManagement.Presentation.Authors;
using LibraryManagement.Presentation.Books;
using LibraryManagement.Presentation.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
//using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Enter the Access Token",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme,
        },
    };

    options.AddSecurityDefinition("Bearer", jwtSecurityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, [] },
    });
});

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPresentation();

//builder.Host.UseSerilog((context, configuration) =>
//{
//    configuration.ReadFrom.Configuration(context.Configuration);
//});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

//app.UseSerilogRequestLogging();

// map endpoints
app.MapUserEndpoints();
app.MapAuthorsEndpoints();
app.MapBooksEndpoints();

app.UseHttpsRedirection();

app.Run();