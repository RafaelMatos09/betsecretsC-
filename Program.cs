using betsecrets.Interfaces.Repository;
using betsecrets.Interfaces.Services;
using betsecrets.ORM;
using betsecrets.Repositories;
using betsecrets.Schema;
using betsecrets.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using System.Text;

// Evita limite de inotify em containers Docker (Render, etc.)
Environment.SetEnvironmentVariable("DOTNET_USE_POLLING_FILE_WATCHER", "1");
Environment.SetEnvironmentVariable("DOTNET_hostBuilder__reloadOnChange", "false");

var builder = WebApplication.CreateBuilder(args);

// No Render/Docker usa a variável PORT; localmente usa launchSettings.json (5027)
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port))
    builder.WebHost.UseUrls($"http://*:{port}");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<AppDbContext>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IBairroService, BairroService>();
builder.Services.AddScoped<IBairroRepository, BairroRepository>();
builder.Services.AddScoped<ITimeService, TimeService>();
builder.Services.AddScoped<ITimeRepository, TimeRepository>();
builder.Services.AddScoped<IJogadorService, JogadorService>();
builder.Services.AddScoped<IJogadorRepository, JogadorRepository>();
builder.Services.AddScoped<IJogadorTimeService, JogadorTimeService>();
builder.Services.AddScoped<IJogadorTimeRepository, JogadorTimeRepository>();
builder.Services.AddScoped<ICampeonatoService, CampeonatoService>();
builder.Services.AddScoped<ICampeonatoRepository, CampeonatoRepository>();
builder.Services.AddScoped<ICampeonatoTimeService, CampeonatoTimeService>();
builder.Services.AddScoped<ICampeonatoTimeRepository, CampeonatoTimeRepository>();
builder.Services.AddScoped<IRodadaService, RodadaService>();
builder.Services.AddScoped<IRodadaRepository, RodadaRepository>();
builder.Services.AddScoped<IPartidaService, PartidaService>();
builder.Services.AddScoped<IPartidaRepository, PartidaRepository>();
builder.Services.AddScoped<IPartidaEventoService, PartidaEventoService>();
builder.Services.AddScoped<IPartidaEventoRepository, PartidaEventoRepository>();
builder.Services.AddScoped<IClassificacaoService, ClassificacaoService>();
builder.Services.AddScoped<IClassificacaoRepository, ClassificacaoRepository>();
builder.Services.AddScoped<ICalendarioJogoService, CalendarioJogoService>();
builder.Services.AddScoped<ICalendarioJogoRepository, CalendarioJogoRepository>();
builder.Services.AddScoped<IPracaService, PracaService>();
builder.Services.AddScoped<IPracaRepository, PracaRepository>();
builder.Services.AddScoped<IEscalacaoJogoService, EscalacaoJogoService>();
builder.Services.AddScoped<IEscalacaoJogoRepository, EscalacaoJogoRepository>();
builder.Services.AddScoped<IRelatorioService, RelatorioService>();
builder.Services.AddScoped<IRelatorioRepository, RelatorioRepository>();
builder.Services.AddScoped<ICalendarioService, CalendarioService>();
builder.Services.AddScoped<ICalendarioRepository, CalendarioRepository>();
builder.Services.AddScoped<IEscalacaoService, EscalacaoService>();
builder.Services.AddScoped<IEscalacaoRepository, EscalacaoRepository>();

builder.Services.AddHttpClient<ApiFutebolService>((sp, client) =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var baseUrl = config["ApiFutebol:BaseUrl"] ?? "https://api.api-futebol.com.br/v1";
    var apiKey = config["ApiFutebol:ApiKey"];

    client.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/");
    if (!string.IsNullOrWhiteSpace(apiKey))
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
});

builder.Services.AddHttpClient("CampeonatoBrasileiro", (sp, client) =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var baseUrl = config["CampeonatoProxy:BaseUrl"] ?? "https://campeonatobrasileiroapi.onrender.com";
    client.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/");
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

var corsOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()?.ToList()
    ?? ["http://localhost:5173", "http://localhost:5174", "http://localhost:5175"];

var extraOrigins = builder.Configuration["CORS_ALLOWED_ORIGINS"];
if (!string.IsNullOrWhiteSpace(extraOrigins))
{
    corsOrigins.AddRange(
        extraOrigins.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    );
}

var allowedOrigins = corsOrigins.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();

if (builder.Environment.IsProduction())
{
    if (string.IsNullOrWhiteSpace(builder.Configuration.GetConnectionString("Postgres")))
        throw new InvalidOperationException(
            "ConnectionStrings:Postgres é obrigatória em produção. " +
            "Configure ConnectionStrings__Postgres no painel do Render.");

    if (string.IsNullOrWhiteSpace(jwtKey))
        throw new InvalidOperationException("Jwt:Key é obrigatória em produção.");
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

var spaIndex = Path.Combine(app.Environment.WebRootPath ?? Path.Combine(app.Environment.ContentRootPath, "wwwroot"), "index.html");
var serveSpa = !app.Environment.IsDevelopment() && File.Exists(spaIndex);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else if (serveSpa)
{
    app.UseDefaultFiles();
    app.UseStaticFiles();

    app.Use(async (context, next) =>
    {
        var path = context.Request.Path.Value;
        if (HttpMethods.IsGet(context.Request.Method)
            && path is not null
            && path.StartsWith("/api/campeonato/", StringComparison.OrdinalIgnoreCase))
        {
            var serie = path["/api/campeonato/".Length..].Trim('/');
            if (serie is "a" or "b" or "c" or "d")
            {
                var client = context.RequestServices.GetRequiredService<IHttpClientFactory>().CreateClient("CampeonatoBrasileiro");
                using var response = await client.GetAsync(serie, context.RequestAborted);
                context.Response.StatusCode = (int)response.StatusCode;
                if (response.Content.Headers.ContentType is { } contentType)
                    context.Response.ContentType = contentType.ToString();
                await response.Content.CopyToAsync(context.Response.Body, context.RequestAborted);
                return;
            }
        }

        await next(context);
    });
}

app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapGet("/health/db", async (AppDbContext db) =>
{
    try
    {
        await db.TestConnectionAsync();
        return Results.Ok(new { status = "ok", database = "connected" });
    }
    catch (Exception ex)
    {
        return Results.Problem(
            statusCode: StatusCodes.Status503ServiceUnavailable,
            title: "Banco de dados indisponível",
            detail: ex.Message);
    }
});

app.MapControllers();

try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await PracasSchema.AplicarAsync(db);
}
catch (Exception ex)
{
    app.Logger.LogWarning(ex, "Não foi possível preparar a tabela de praças. O mapa de campos fica indisponível até o banco aceitar o esquema.");
}

if (serveSpa)
{
    app.MapFallback(async context =>
    {
        var path = context.Request.Path.Value ?? "";
        if (path.StartsWith("/api", StringComparison.OrdinalIgnoreCase)
            || path.StartsWith("/health", StringComparison.OrdinalIgnoreCase))
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.SendFileAsync(spaIndex);
    });
}

app.Run();
