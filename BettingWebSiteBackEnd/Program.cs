using BettingWebSiteBackEnd.Consumers;
using MassTransit;
using Shared;

namespace BettingWebSiteBackEnd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddMassTransit(configurator =>
            {
                //MailGetEventResponseConsumer
                configurator.AddConsumer<OrderGetEventResponseEventConsumer>();
                configurator.AddConsumer<MailGetEventResponseConsumer>();
                configurator.UsingRabbitMq((contex, _configure) =>
                {
                    _configure.Host(builder.Configuration["RabbitMq"]);
                    _configure.ReceiveEndpoint(RabbitMQSettings.BettingWebSiteBackup_GetCouponsEventqueue, e => e.ConfigureConsumer<OrderGetEventResponseEventConsumer>(contex));
                    _configure.ReceiveEndpoint(RabbitMQSettings.BettingWebSite_MailGetEventResponseQueue, e => e.ConfigureConsumer<MailGetEventResponseConsumer>(contex));
                });
            });



            builder.Services.AddControllersWithViews();

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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
