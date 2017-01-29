using OpenMetaverse;
using SimpleBot.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBot.Managers
{
  class ConnectionManager : ManagerBase
  {
    public override bool Init()
    {
      var client = Program.Instance.Client;
      client.Network.LoginProgress += Network_LoginProgress;
      client.Network.LoggedOut += Network_LoggedOut;
      client.Network.Disconnected += Network_Disconnected;

      var logins = Program.Instance.Config.GetSetting<List<LoginDetails>>("SavedLogins");
      var current_login = logins[0];

      Console.WriteLine($"Logging in as {current_login.Firstname} {current_login.Lastname}...");

      return client.Network.Login(current_login.Firstname, current_login.Lastname, current_login.Password, "SimpleBot", "0.1");
    }

    public override bool Poll()
    {
      return Program.Instance.Client.Network.Connected;
    }
    
    private void Network_Disconnected(object sender, DisconnectedEventArgs e)
    {
      Console.WriteLine($"Disconnected: {e.Reason} - {e.Message}");
 
      WebsocketBackend.Instance.Broadcast(new Server.ServerMessages.DisconnectedMessage()
      {
        Message = e.Message,
        Reason = e.Reason
      });
    }

    private void Network_LoggedOut(object sender, LoggedOutEventArgs e)
    {
      Console.WriteLine("Logged out");

      WebsocketBackend.Instance.Broadcast(new Server.ServerMessages.LoggedOutMessage());
    }

    private void Network_LoginProgress(object sender, LoginProgressEventArgs e)
    {
      Console.WriteLine($"Login progress: [{e.Status}] {e.Message}");
 
      WebsocketBackend.Instance.Broadcast(new Server.ServerMessages.LoginProcessMessage()
      {
        FailReason = e.FailReason,
        Message = e.Message,
        Status = e.Status
      });
    }
  }
}
