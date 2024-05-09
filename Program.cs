
using FantasyChas_Backend.Data;
using FantasyChas_Backend.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenAI_API;

namespace FantasyChas_Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            DotNetEnv.Env.Load();

            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            var AllowLocalhostOrigin = "_allowLocalhostOrigin";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: AllowLocalhostOrigin,
                                  policy =>
                                  {
                                      policy.WithOrigins("http://localhost:3000")
                                                          .AllowAnyHeader()
                                                          .AllowAnyMethod()
                                                          .AllowCredentials();
                                  });
            });

            builder.Services.AddAuthorization();
            builder.Services.AddControllers();

            // Choose what we want to include in the Identity object in the database?
            builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
            {

            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 0;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
            builder.Services.AddScoped<IProfessionRepository, ProfessionRepository>();
            builder.Services.AddScoped<ISpeciesRepository, SpeciesRepository>();

            builder.Services.AddSingleton(sp => new OpenAIAPI(Environment.GetEnvironmentVariable("OPENAI_KEY")));

            var app = builder.Build();

            // Middleware for logging CORS related problems
            app.Use(async (context, next) =>
            {
                // Log information about incoming request
                Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");

                // Log CORS related headers
                Console.WriteLine($"Origin: {context.Request.Headers["Origin"]}");
                Console.WriteLine($"Access-Control-Request-Method: {context.Request.Headers["Access-Control-Request-Method"]}");
                Console.WriteLine($"Access-Control-Request-Headers: {context.Request.Headers["Access-Control-Request-Headers"]}");

                await next.Invoke(context);
            });

            app.MapIdentityApi<IdentityUser>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // REMOVE this endpoint when ready
            app.MapGet("/user/character", () =>
            {
                return Results.Ok("Hello!");
            }).RequireAuthorization();

            app.UseHttpsRedirection();

            app.UseCors(AllowLocalhostOrigin);

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
