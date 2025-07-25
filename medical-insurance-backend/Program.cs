using Microsoft.EntityFrameworkCore;
using medical_insurance_backend.Data;
using medical_insurance_backend.Services;
using medical_insurance_backend.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure Entity Framework with MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Server=localhost;Database=demo_task;User=root;Password=password;";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Register services following Dependency Inversion Principle
builder.Services.AddScoped<ICompanyService, CompanyService>();

// Configure controllers with model validation
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configure JSON serialization to handle camelCase from Angular
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        // Customize model validation responses
        options.SuppressModelStateInvalidFilter = false;
    });

// Configure CORS for Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "Medical Insurance API", 
        Version = "v1",
        Description = "API for Medical Insurance Company Management System"
    });
    
    // Include XML comments if available
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Ensure database is created and migrate
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        context.Database.EnsureCreated();
        app.Logger.LogInformation("Database ensured created successfully");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred while creating the database");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Medical Insurance API v1");
        c.RoutePrefix = string.Empty; // Make swagger UI the default page
    });
}

// Enable CORS
app.UseCors("AllowAngularApp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
