using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CryptoServer
{
    public class CryptoChecker
    {
        public bool IsUpdating { get; private set; }
        
        private static HttpClient _client = new HttpClient();
        private static readonly string[] CryptoKeys = {"BTC", "ETH", "REP", "DASH"};
        private static readonly string CheckAdress = @"https://min-api.cryptocompare.com/data/pricemulti?fsyms=BTC,ETH,REP,DASH&tsyms=RUB";
        private static ConcurrentDictionary<string, float> _currentValues = new ConcurrentDictionary<string, float>();
        
        private static readonly ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();

        private static List<WebSocket> _connectedSockets = new List<WebSocket>();

        public int GetNumberOfConnections()
        {
            return _connectedSockets.Count;
        }

        public void AddConnection(WebSocket webSocket)
        {
            if (webSocket == null && !_connectedSockets.Contains(webSocket))
                throw new ArgumentException();
            
            Locker.EnterWriteLock();
            try
            {
                _connectedSockets.Add(webSocket);
            }
            finally
            {
                Locker.ExitWriteLock();
            }
        }

        public void RemoveConnection(WebSocket webSocket)
        {
            Locker.EnterWriteLock();
            try
            {
                _connectedSockets.Remove(webSocket);
            }
            finally
            {
                Locker.ExitWriteLock();
            }
        }

        public async Task CheckChanges()
        {
            IsUpdating = true;
            
            while (true)
            {
                await Task.Delay(1000);
                
                var jsonAnswer = await _client.GetStringAsync(CheckAdress);
                                
                JObject result = JObject.Parse(jsonAnswer);
                foreach (var key in CryptoKeys)
                {
                    if (Math.Abs(result[key]["RUB"].Value<float>() - _currentValues[key]) > 0.005f)
                    {
                        _currentValues[key] = result[key]["RUB"].Value<float>();
                        foreach (var socket in _connectedSockets)
                        {
                            if (socket.State == WebSocketState.Open)
                                await SendMessage(socket, key + " = " + _currentValues[key]);
                            else
                            {
                                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed succesfull",
                                    CancellationToken.None);
                                RemoveConnection(socket);
                            }
                        }
                    }
                }

                if (GetNumberOfConnections() == 0)
                {
                    IsUpdating = false;
                    break;
                }
            }
        }

        private async Task SendMessage(WebSocket socket, string message)
        {
            try
            {
                if (socket.State != WebSocketState.Open)
                    return;

                Console.WriteLine(_connectedSockets.IndexOf(socket) + " " + _connectedSockets.Count + " " + message);
                
                await socket.SendAsync(new ArraySegment<byte>(Encoding.ASCII.GetBytes(message), 0, message.Length),
                    WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (ObjectDisposedException)
            {
                RemoveConnection(socket);
            }
        }

        public async Task InitValues()
        {
            var jsonAnswer = await _client.GetStringAsync(CheckAdress);

            JObject result = JObject.Parse(jsonAnswer);
            foreach (var key in CryptoKeys)
                _currentValues[key] = result[key]["RUB"].Value<float>();
        }

        public async Task AskForCurrentValues(WebSocket webSocket)
        {
            Console.WriteLine("ask " + _connectedSockets.IndexOf(webSocket));
            foreach (var currentValue in _currentValues)
                await SendMessage(webSocket, currentValue.Key + " = " + currentValue.Value);
        }
    }
}