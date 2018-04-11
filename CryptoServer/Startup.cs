using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CryptoServer
{    
    public class Startup
    {        
        private static readonly CryptoChecker CryptoChecker = new CryptoChecker();

        public void Configure(IApplicationBuilder app)
        {            
            app.UseWebSockets();
            app.Run(async context =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    CryptoChecker.AddConnection(webSocket);

                    if (!CryptoChecker.IsUpdating)
                    {
                        await CryptoChecker.InitValues();
                        await CryptoChecker.AskForCurrentValues(webSocket);

                        await CryptoChecker.CheckChanges();
                    }
                    else
                        await CryptoChecker.AskForCurrentValues(webSocket);
                }
                else
                    await context.Response.WriteAsync("This server for websocket use!");
            });        
        }
    }
}