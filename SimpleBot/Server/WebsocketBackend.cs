using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OpenMetaverse;
using SimpleBot.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace SimpleBot.Server
{
  public class WebsocketBackend
  {
    private static WebsocketBackend _instance = new WebsocketBackend();
    public static WebsocketBackend Instance => _instance;

    private WebSocketServer Server { get; set; } = null;

    public void Init(int port)
    {
      if(Server != null)
      {
        Server.Stop();
        Server = null;
      }
 
      Server = new WebSocketServer(port);
      Server.AddWebSocketService<BotService>(BotService.ServicePath);
      Server.Start();

      JsonConvert.DefaultSettings = (() =>
      {
        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new StringEnumConverter());
        return settings;
      });
    }

    public void SendTo(string session_id, ServerMessages.IServerMessage message)
    {
      if(Server == null)
      {
        return;
      }
 
      var json_data = JsonConvert.SerializeObject(message);
      Server.WebSocketServices[BotService.ServicePath].Sessions.SendTo(json_data, session_id);
    }
 
    public void Broadcast(ServerMessages.IServerMessage message)
    {
      if(Server == null)
      {
        return;
      }

      var json_data = JsonConvert.SerializeObject(message);
      Server.WebSocketServices[BotService.ServicePath].Sessions.Broadcast(json_data);
    }
  }
}
