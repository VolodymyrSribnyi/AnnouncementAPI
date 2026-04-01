using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

namespace Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient("AnnouncementAPI", client =>
            {
                client.BaseAddress = new Uri("https://announcementsapi-vova-hsgnfkdqgphzdzfq.canadacentral-01.azurewebsites.net/");
            });
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
            })
            .AddGoogle(options =>
            {
                options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                options.SaveTokens = true;
                    
                options.Events.OnCreatingTicket = context =>
                {
                    var tokens = context.Properties.GetTokens().ToList();

                    if (context.TokenResponse.Response?.RootElement.TryGetProperty("id_token", out var idTokenElement) == true)
                    {
                        tokens.Add(new AuthenticationToken
                        {
                            Name = "id_token",
                            Value = idTokenElement.GetString()
                        });

                        context.Properties.StoreTokens(tokens);
                    }

                    return Task.CompletedTask;
                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Announcements}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
