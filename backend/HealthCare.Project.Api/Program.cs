using HealthCare.Project.Core.Entities.Identity;
using HealthCare.Project.Core.Helper;
using HealthCare.Project.Core.Repository.Contract;
using HealthCare.Project.Core.Services.Contract;
using HealthCare.Project.Repository.Data;
using HealthCare.Project.Repository.Data.Contexts;
using HealthCare.Project.Repository.Identity.Contexts;
using HealthCare.Project.Repository.Repositories;
using HealthCare.Project.Service.Services.AddMedicine;
using HealthCare.Project.Service.Services.Alternative;
using HealthCare.Project.Service.Services.DrugInteractions;
using HealthCare.Project.Service.Services.Emails;
using HealthCare.Project.Service.Services.Sms;
using HealthCare.Project.Service.Services.Tokens;
using HealthCare.Project.Service.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HealthCare.Project.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient();

            // ?? Configure Database Contexts
            builder.Services.AddDbContext<DrugDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddDbContext<UserIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            // ?? Configure Identity
            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            })
            .AddEntityFrameworkStores<UserIdentityDbContext>()
            .AddDefaultTokenProviders();

            // ?? Configure JWT Authentication
            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "DefaultSecretKey123456");

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });


            // ?? Register Application Services
            builder.Services.AddScoped<IMedicineRepository, MedicineRepository>();
            builder.Services.AddScoped<MedicineService>();
            builder.Services.AddScoped<IDrugInteractionRepository, DrugInteractionRepository>();
            builder.Services.AddScoped<DrugInteractionServices>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddHostedService<ExpiredMedicineCleanupService>();
            builder.Services.AddHttpClient<IInteractionChecker, GeminiInteractionChecker>();
            builder.Services.AddScoped<IMedicineAlternativeRepository, MedicineAlternativeRepository>();
            builder.Services.AddScoped<IMedicineAlternativeService, MedicineAlternativeService>();
            builder.Services.AddScoped<ISmsService, SmsService>();
            builder.Services.AddScoped<ISmsHandler, SmsHandler>();

            var app = builder.Build();

            // ?? Apply Database Migrations
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                var context = services.GetRequiredService<DrugDbContext>();
                var identity = services.GetRequiredService<UserIdentityDbContext>();

                await context.Database.MigrateAsync();
                await DrugDbContextSeed.SeedAsync(context);
                await MedicineAlternativeDbContextSeed.SeedAsync(context);
                await identity.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "There were problems during migrations");
            }







            // ?? Configure Middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthentication(); 
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
