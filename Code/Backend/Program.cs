using System.Text;
using Backend.Data;
using Backend.Middleware;
using Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using SmartDocs.API.Services;

namespace Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ----------------------------
            // SERVICES
            // ----------------------------
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // DB
            var conn = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(conn, ServerVersion.AutoDetect(conn))
            );

            // DI
            builder.Services.AddScoped<JwtService>();
            builder.Services.AddScoped<DocumentService>();
            builder.Services.AddScoped<DocumentTextExtractionService>();
            builder.Services.AddScoped<DocumentVersionService>();
            builder.Services.AddScoped<InlineCommentService>();
            builder.Services.AddScoped<SearchService>();
            builder.Services.AddScoped<ActivityLogService>();

            // JWT Auth
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "")
                        )
                    };
                });

            // CORS (React dev)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReact", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:3000", "http://localhost:3001")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            // ----------------------------
            // BUILD APP  ✅ (must exist before app.Use...)
            // ----------------------------
            var app = builder.Build();

            // ----------------------------
            // MIDDLEWARE
            // ----------------------------
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Keep OFF if you call backend via http://localhost:5102 from React
            // app.UseHttpsRedirection();

            app.UseCors("AllowReact"); // ✅ BEFORE auth

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            // Ensure Uploads folder exists
            var uploadsPath = Path.Combine(app.Environment.ContentRootPath, "Uploads");
            Directory.CreateDirectory(uploadsPath);

            // Serve static files (wwwroot)
            app.UseStaticFiles();

            // Serve uploads at /uploads
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(uploadsPath),
                RequestPath = "/uploads"
            });

            // Test endpoint
            app.MapGet("/", () => "SmartDocHub Backend is running ✅ Go to /swagger");

            app.MapControllers();

            app.Run();
        }
    }
}
