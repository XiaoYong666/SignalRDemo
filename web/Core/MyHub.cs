using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using web.Model;

namespace web.Core
{
    public class MyHub : Hub
    {        
        public override Task OnConnectedAsync()
        {
            Console.WriteLine("Connected->User:"+this.Context.User);   
            Console.WriteLine("Connected->UserIdentifier:"+this.Context.UserIdentifier);            
            Console.WriteLine("Connected->ConnectionId:"+this.Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine("Disconnected->ConnectionId:"+this.Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        //发送消息--发送给所有连接的客户端
        public Task SendMessage(string msg)
        {
            return Clients.All.SendAsync("ReceiveMessage", msg);
        }

        //发送消息--发送给指定用户
        //默认情况下，使用 SignalRClaimTypes.NameIdentifier从ClaimsPrincipal与作为用户标识符连接相关联
        //所以使用自带授权Authorize登陆时，可以把用户id保存在NameIdentifier中
        public Task SendPrivateMessage(string userId, string message)
        {
            return Clients.User(userId).SendAsync("ReceiveMessage", message);
        }
    }
}