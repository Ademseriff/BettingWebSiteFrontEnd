﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using BettingWebSiteFUserInterface.Models;
using Shared.Events;
using MassTransit;
using BettingWebSiteFUserInterface.Consumers;

namespace BettingWebSiteFUserInterface.Areas.Auth.Controllers
{
    [Area("Auth")]
    [Authorize]
    public class Auth1Controller : Controller
    {
        private readonly IPublishEndpoint publishEndpoint;
        public static UserLoginCheckEvent userLoginCheckEventstatic = new();
        public Auth1Controller(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginCheck userLoginCheck)
        {
          

            if (userLoginCheck != null)
            {
                userLoginCheckEventstatic.Tc = userLoginCheck.Tc;
                userLoginCheckEventstatic.Password = userLoginCheck.Password;
                await publishEndpoint.Publish(userLoginCheckEventstatic);
                await Task.Delay(8000);
                if (UserLoginCheckEventConsumer.IsValid)
                {
                    var claims = new List<Claim>
                          {
                          new Claim("tc",userLoginCheck.Tc)
                       // Daha fazla kullanıcı bilgisi ekleyebilirsiniz.
                       

                           };
                    //    // Kimlik oluşturma
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Kimlik oluşturup kullanıcıyı giriş yapmış olarak işaretleme
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Home", new { Area = "" });
                }
                
            }
            return View();
        
        }



        [AllowAnonymous]
        public IActionResult Logout()
        {
            return RedirectToAction("Index", "Home", new { Area = "" });
        }
    }
}
