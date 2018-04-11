using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoClient
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            RunWebSocketsAsync().GetAwaiter().GetResult();
        }
        
        private static async Task RunWebSocketsAsync()
        {
            var client = new ClientWebSocket();
            await client.ConnectAsync(new Uri("ws://localhost:5000"), CancellationToken.None);
            Console.WriteLine("Connected to CryptoService.");
            
            await ReceivingAsync(client);
        }

        private static async Task ReceivingAsync(WebSocket client)
        {
            var buffer = new byte[1024 * 4];
            
            while (true)
            {
                var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                    Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, result.Count));

                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    break;
                }
            }
        }
    }
}