
//创建连接对象connection
const signalr_connection = new signalR.HubConnectionBuilder()
            .withUrl("/MyHub")
            .configureLogging(signalR.LogLevel.Information)
            .build();

//启动connection
signalr_connection.start()
                    .then(function(){
                        console.log("连接成功");
                    }).catch(function(ex){
                        console.log("连接失败"+ex);
                        //SignalR JavaScript 客户端不会自动重新连接，必须编写代码将手动重新连接你的客户端
                        setTimeout(() => start(), 5000);
                    });

async function start() {
    try {
        await signalr_connection.start();
        console.log("connected");
    } catch (err) {
        console.log(err);
        setTimeout(() => start(), 5000);
    }
};

signalr_connection.onclose(async () => {
    start();
});