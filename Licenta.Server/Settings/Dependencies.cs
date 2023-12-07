using Licenta.Server.DataLayer.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Licenta.Server.DataLayer.Models;
using System.Security.Claims;

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
                    builder.WithOrigins("http://localhost:5173")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
        });
    }

}
