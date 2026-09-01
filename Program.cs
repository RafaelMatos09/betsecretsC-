using betsecrets.Interfaces.Repository;

using betsecrets.Interfaces.Services;

using betsecrets.ORM;

using betsecrets.Repositories;

using betsecrets.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.IdentityModel.Tokens;

using System.Net.Http.Headers;

using System.Text;

// Evita limite de inotify em containers Docker (Render, etc.)
Environment.SetEnvironmentVariable("DOTNET_USE_POLLING_FILE_WATCHER", "1");
Environment.SetEnvironmentVariable("DOTNET_hostBuilder__reloadOnChange", "false");

var builder = WebApplication.CreateBuilder(args);



var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";

builder.WebHost.UseUrls($"http://*:{port}");



builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<AppDbContext>();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();



builder.Services.AddHttpClient<ApiFutebolService>((sp, client) =>

{

    var config = sp.GetRequiredService<IConfiguration>();

    var baseUrl = config["ApiFutebol:BaseUrl"]

        ?? throw new InvalidOperationException("ApiFutebol BaseUrl não configurada.");

    var apiKey = config["ApiFutebol:ApiKey"]

        ?? throw new InvalidOperationException("ApiFutebol ApiKey não configurada.");



    client.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/");

    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

});



var jwtKey = builder.Configuration["Jwt:Key"]

    ?? throw new InvalidOperationException("JWT Key não configurada.");



if (Encoding.UTF8.GetByteCount(jwtKey) < 16)

    throw new InvalidOperationException("JWT Key deve ter no mínimo 16 caracteres (128 bits) para HS256.");



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

    .AddJwtBearer(options =>

    {

        options.TokenValidationParameters = new TokenValidationParameters

        {

            ValidateIssuer = true,

            ValidateAudience = true,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,



            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidAudience = builder.Configuration["Jwt:Audience"],



            IssuerSigningKey = new SymmetricSecurityKey(

                Encoding.UTF8.GetBytes(jwtKey)

            )

        };

    });



builder.Services.AddAuthorization();



var corsOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()

    ?? ["http://localhost:5173", "http://localhost:5174"];



builder.Services.AddCors(options =>

{

    options.AddPolicy("Frontend", policy =>

    {

        policy.WithOrigins(corsOrigins)

              .AllowAnyHeader()

              .AllowAnyMethod();

    });

});



var app = builder.Build();



if (app.Environment.IsDevelopment())

{

    app.UseSwagger();

    app.UseSwaggerUI();

}



app.UseCors("Frontend");



app.UseAuthentication();

app.UseAuthorization();



app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapControllers();



app.Run();


