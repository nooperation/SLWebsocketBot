using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace wsdebug
{
  public class BotService : WebSocketBehavior
  {
    public static string ServicePath => "/Bot";

    protected override void OnOpen()
    {
      base.OnOpen();
      
      Console.WriteLine("Client connected");
    }

    protected override void OnClose(CloseEventArgs e)
    {
      base.OnClose(e);
      Console.WriteLine("Client disconnected");
    }

    protected override void OnMessage(MessageEventArgs e)
    {
      var json_data = e.Data;
      Console.WriteLine("Message from client: " + json_data);
    } 
  }

  class Program
  {
    private WebSocketServer Server { get; set; } = null;

    public void Init(int port)
    {
      if(Server != null)
      {
        Server.Stop();
        Server = null;
      }
 
      
      Server = new WebSocketServer(port);
      Server.AddWebSocketService<BotService>(BotService.ServicePath, () => new BotService() {
        IgnoreExtensions = true
      });
      Server.Start();
    }

    public Program()
    {
      Init(1234);
      while(true) { }
    }

    static void Main(string[] args)
    {
      new Program();
    }
  }
}
