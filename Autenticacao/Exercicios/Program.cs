using Exercicios.Estatico;
using Exercicios.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();

/* Configura o servidor para devolver um XML no lugar o JSON */
builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true;
    //options.ReturnHttpNotAcceptable = true;
}).AddXmlSerializerFormatters();

JwtConfiguracao.Secret = builder.Configuration.GetValue<string>("JWT:Secret");
JwtConfiguracao.Issuer = builder.Configuration.GetValue<string>("JWT:Issuer");
JwtConfiguracao.Audience = builder.Configuration.GetValue<string>("JWT:Audience");
JwtConfiguracao.Key = Encoding.ASCII.GetBytes(JwtConfiguracao.Secret);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = JwtConfiguracao.Issuer,
            ValidAudience = JwtConfiguracao.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(JwtConfiguracao.Key)
        };
    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "JWT Token Authentication API",
        Description = "API feita em .net core 6.0"
    });

    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Utilizando JWT Authorization para autenticar",
    });

    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                            Array.Empty<string>()
                    }
                });
});

var devCorsPolicy = "devCorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(devCorsPolicy, builder =>
    {
        builder.WithOrigins("http://localhost:800")
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(devCorsPolicy);
}

app.UseHttpsRedirection()
    .UseAuthentication()
    .UseAuthorization();

app.UseMiddleware<ApiKeyMiddleware>();


app.MapControllers();
app.Run();
