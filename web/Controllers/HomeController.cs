using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using web.Core;
using web.Model;

namespace web.Controllers
{
    public class HomeController : Controller
    {
        //注入IHubContext实例
        private readonly IHubContext<MyHub> myHub;
        public HomeController(IHubContext<MyHub> _myHub)
        {
            myHub=_myHub;
        } 
        public IActionResult Index()
        {
            //当前登陆用户的id（未登陆，则未null）
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View();
        }

        [Authorize]
        public IActionResult Chat()
        {
            return View();
        }

        [Authorize]
        public IActionResult MyMsg()
        {
            return View();
        }

        [Authorize]
        public IActionResult Post()
        {
            return View();
        }

        /**
            推送消息到特定用户:
            · 默认情况下，使用 SignalRClaimTypes.NameIdentifier从ClaimsPrincipal与作为用户标识符连接相关联
              所以使用自带授权Authorize登陆时，可以把用户id保存在NameIdentifier中              

            post页面发送消息，MyMsg页面接收
         */        
        [Authorize,HttpPost]
        public async Task<IActionResult> PostData()
        {
            //当前登陆用户的id（未登陆，则未null）
            //string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userId = Request.Form["userid"];
            var message = Request.Form["message"];
            
            if(!string.IsNullOrWhiteSpace(userId))
                await myHub.Clients.User(userId).SendAsync("ReceiveMessage", new {message=message});
            
            return Json(new { code = "success", msg = "发送成功" });
        }
    }
}
