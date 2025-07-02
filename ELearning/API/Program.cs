using CORE.DTOs.Auth;
using CORE.DTOs.Paths;
using CORE.Hubs;
using CORE.Services;
using CORE.Services.IServices;
using DATA.DataAccess.Context;
using DATA.DataAccess.Context.Interceptors;
using DATA.DataAccess.Repositories;
using DATA.DataAccess.Repositories.IRepositories;
using DATA.DataAccess.Repositories.UnitOfWork;
using DATA.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            builder.Configuration.AddEnvironmentVariables();

            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // register DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("SQLServer"),
                sqlOptions => sqlOptions
                .MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
                )
                .AddInterceptors(new SoftDeleteInterceptor())
            );

            // identity
            builder.Services.AddIdentity<AppUser, IdentityRole<int>>(
            options =>
            {
            }
            ).AddEntityFrameworkStores<AppDbContext>();

            // Configure Swagger to use JWT Bearer token
            builder.Services.AddSwaggerGen(c =>
            {

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
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
                    }});
            });


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                };
            });
            builder.Services.AddAuthorization();

            builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JWT"));
            builder.Services.Configure<Paths>(builder.Configuration.GetSection("Paths"));

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddSignalR();
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ILanguageService, LanguageService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ILanguagePreferenceService, LanguagePreferenceService>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IReportService, ReportService>();
            builder.Services.AddScoped<IFeedbackService, FeedbackService>();
            builder.Services.AddScoped<IMessageService, MessageService>();

            builder.Services.AddMemoryCache();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ELearningAPI", Version = "v1" });
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"));
            });
            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.Migrate();
            }


            app.MapOpenApi();

            app.UseSwagger();   
            app.UseSwaggerUI();

            app.Use(async (context, next) =>
            {
                var baseUrl = $"{context.Request.Scheme}://{context.Request.Host.Value}/";
                context.Items["BaseUrl"] = baseUrl;
                await next();
            });

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHub<ChatHub>("/chatHub");


            app.MapControllers();

            app.Run();
        }
    }
}
