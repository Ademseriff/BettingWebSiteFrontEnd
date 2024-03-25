using BettingWebSiteFUserInterface.Consumers;
using MassTransit;
using MatchOddsApi.Consumers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Shared;

namespace BettingWebSiteFUserInterface
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddMassTransit(configurator =>
            {

                configurator.AddConsumer<GetMatchOddsConsumers>();
                configurator.AddConsumer<UserLoginCheckEventConsumer>();
                configurator.AddConsumer<BasketItemGetResponseEventConsumer>();
                configurator.UsingRabbitMq((contex, _configure) =>
                {
                    _configure.Host(builder.Configuration["RabbitMq"]);

                    _configure.ReceiveEndpoint(RabbitMQSettings.UserInterface_matchoddsresponseEventQueue, e => e.ConfigureConsumer<GetMatchOddsConsumers>(contex));

                    _configure.ReceiveEndpoint(RabbitMQSettings.UserInterface_CustomerCheckResponse, e => e.ConfigureConsumer<UserLoginCheckEventConsumer>(contex));

                    _configure.ReceiveEndpoint(RabbitMQSettings.UserInterface_BasketItemGetResponseEvent, e => e.ConfigureConsumer<BasketItemGetResponseEventConsumer>(contex));
                });
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(opt =>
            {
                opt.Cookie.Name = "website";
                opt.LoginPath = new PathString("/Auth/Auth1/Login");
                opt.LogoutPath = new PathString("/Auth/Auth1/Logout");
                //opt.ExpireTimeSpan = TimeSpan.FromHours(2);
                opt.ExpireTimeSpan = TimeSpan.FromSeconds(300);
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
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {

                endpoints.MapAreaControllerRoute(
                    name: "auth",
                    areaName: "Auth",
                    pattern: "Auth/{controller=home}/{action=Index}/{id?}"

                );



                endpoints.MapDefaultControllerRoute();

            });

            //app.MapControllerRoute(
            //    name: "default",
            //    pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
