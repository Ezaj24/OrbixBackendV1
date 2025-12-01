using OrbixBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// ---------- LOAD CONFIG ----------
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables(); // for deployment

// ---------- SERVICES ----------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Backend services
builder.Services.AddScoped<GroqService>();
builder.Services.AddScoped<PromptRefiner>();

// ---------- CORS ----------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin(); 
    });
});

// ---------- BUILD ----------
var app = builder.Build();

// ---------- MIDDLEWARE ----------

// Swagger only in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Disable https redirect in development (to fix your warning)
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// CORS
app.UseCors("AllowFrontend");

// Controllers
app.MapControllers();

// ---------- RUN ----------
app.Run();
