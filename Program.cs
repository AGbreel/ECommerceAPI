using ECommerceAPI.Data;
using ECommerceAPI.Repositories;
using ECommerceAPI.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using ECommerceAPI.Filters;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // يخلي ASP.NET يقبل Enum كـ string
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Add services to the container.
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IWishlistRepository, WishlistRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ✅ Swagger مع دعم الـ JWT Authorize + File Upload
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ECommerce API", Version = "v1" });

    // تعريف الـ Bearer Auth
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new string[] {}
        }
    });

    // ✅ دعم الـ file upload
    c.OperationFilter<FileUploadOperationFilter>();
});

// ✅ CORS config قبل Build
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                          //   policy.WithOrigins("http://localhost:5173")
                          //         .AllowAnyHeader()
                          //         .AllowAnyMethod();
                      });
});

// JWT Authentication
var key = builder.Configuration["Jwt:Key"];
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        await DbSeeder.SeedAdminsAsync(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Seeding failed: {ex.Message}");
    }

}

// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles(); // يخدم wwwroot
// app.UseHttpsRedirection();

app.UseRouting();

// ✅ CORS لازم ييجي هنا
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ✅ عشان Render يحدد البورت
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");
