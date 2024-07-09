using System.Text;
using DapperApi.Authentication.Services;
using DapperApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>

{

    c.SwaggerDoc("v1", new OpenApiInfo

    {

        Version = "v1",

        Title = "JWT Api",

        Description = "Secures API using JWT",

        Contact = new OpenApiContact

        {

            Name = "Berke Koralp",

            Email = "berkekoralp@gmail.com",

            Url = new Uri("https://www.google.mw/")

        }

    });

    // To Enable authorization using Swagger (JWT)

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()

    {

        Name = "Authorization",

        Type = SecuritySchemeType.ApiKey,

        Scheme = "Bearer",

        BearerFormat = "JWT",

        In = ParameterLocation.Header,

        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",

    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement

                {

                    {

                          new OpenApiSecurityScheme

                            {

                                Reference = new OpenApiReference

                                {

                                    Type = ReferenceType.SecurityScheme,

                                    Id = "Bearer"

                                }

                            },

                            new string[] {}

                    }

                });

});


builder.Services.AddScoped(typeof(IDatabaseService<>), typeof(DatabaseService<>));

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

.AddJwtBearer(options =>

{

    options.TokenValidationParameters = new TokenValidationParameters

    {

        ValidateIssuer = true,

        ValidateAudience = true,

        ValidateLifetime = true,

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])),

        ValidateIssuerSigningKey = true,

       ValidIssuer = builder.Configuration["Jwt:Issuer"],   

       ValidAudience = builder.Configuration["Jwt:Audience"],

    };

});

builder.Services.AddScoped<JwtService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DapperApi v1");
        c.RoutePrefix = string.Empty; // Serve Swagger at the app's root
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
