using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> LoginPost()
        {
            var username = Request.Form["username"];
            var userpwd = Request.Form["userpwd"];            

            //简单验证
            if(string.IsNullOrWhiteSpace(username))
            {
                return Json(new { code = "failed", msg = "用户名不能为空" });
            }
            if(string.IsNullOrWhiteSpace(userpwd))
            {
                return Json(new { code = "failed", msg = "密码不能为空" });
            }

            //本demo没连接数据库，就不做用户验证了
            
            //登陆授权
            //声明【类似于身份证中姓名，民族等定义】
            string userId=Guid.NewGuid().ToString().Replace("-","");
            var claims =new List<Claim>()
            {
                new Claim(ClaimTypes.Name,username),   //储存用户name
                new Claim(ClaimTypes.NameIdentifier,userId)  //储存用户id
            };
            //身份【似身份证，多个声明（姓名，民族等）构成】
            var indentity = new ClaimsIdentity(claims,"formlogin");
            //证件所有者【类似身份证所有者】
            var principal = new ClaimsPrincipal(indentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal);

            //验证是否授权成功
            if (principal.Identity.IsAuthenticated)
            {
                return Json(new { code = "success", msg = "登陆成功" });
            }
            else
                return Json(new { code = "failed", msg = "登陆失败" });
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> logOff()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            return Redirect("/Home/Index");
        }
    }
}