using Licenta.Server.DataLayer.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Licenta.Server.DataLayer.Models;
using System.Security.Claims;
using Licenta.Server.Repository.Interfeces;
using Licenta.Server.Repository;
using Licenta.Server.Services.Interfaces;
using Licenta.Server.Services;

namespace Licenta.Server.Settings;

public class Dependencies
{
    public static void Inject(WebApplicationBuilder app)
    {
        app.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(app.Configuration.GetConnectionString("DefaultConnection"),
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
        });
        app.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
        app.Services.AddAuthorizationBuilder();
        app.Services.AddIdentityCore<User>().AddEntityFrameworkStores<AppDbContext>().AddApiEndpoints();
        app.Services.AddCors(options =>
        {
            options.AddPolicy("Access-Control-Allow-Origin",
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
        });
        AddRepositories(app.Services);
        AddServices(app.Services);

    }
    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<UserRepository>();
        services.AddScoped<UnitOfWork>();
        services.AddScoped<SkillRepository>();
        services.AddScoped<SprintRepository>();
        services.AddScoped<ProjectRepository>();
        services.AddScoped<UserSkillRepository>();
        services.AddScoped<IssueRepository>();
        services.AddScoped<IssueStatusRepository>();
        services.AddScoped<IssueTypeRepository>();
        services.AddScoped<CommentRepository>();

    }
    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<UserService>();
        services.AddScoped<SkillService>();
        services.AddScoped<UserSkillService>();
        services.AddScoped<SprintService>();
        services.AddScoped<ProjectService>();
        services.AddScoped<IssueService>();
        services.AddScoped<IssueStatusService>();
        services.AddScoped<IssueTypeService>();
        services.AddScoped<CommentService>();

    }
}
