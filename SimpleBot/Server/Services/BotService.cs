using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleBot.Server.ClientMessages;
using SimpleBot.Server.ServerMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace SimpleBot.Server.Services
{
  public class BotService : WebSocketBehavior
  {
    public static string ServicePath => "/Bot";

    protected override void OnOpen()
    {
      base.OnOpen();

      Console.WriteLine("Client connected");

      var instance = Program.Instance;
      var json_data = Newtonsoft.Json.JsonConvert.SerializeObject(new Server.ServerMessages.InitMessage()
      {
        ClientId = instance.Client.Self.AgentID,
        ClientName = instance.Client.Self.Name,
      });

      Send(json_data);

      lock (Program.Instance.ChatManager.ChatHistory)
      {
        foreach (var item in Program.Instance.ChatManager.ChatHistory)
        {
          WebsocketBackend.Instance.SendTo(ID, item);
        }
      }
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

      try
      {

        var message = JObject.Parse(json_data);
        var message_type_string = message["MessageType"].ToString();
        var message_type = (ClientMessageType)Enum.Parse(typeof(Server.ClientMessages.ClientMessageType), message_type_string);
        var payload = message["Payload"];

        switch (message_type)
        {
          case ClientMessageType.ProfileRequest:
          {
            var profile_request = JsonConvert.DeserializeObject<ProfileRequestMessage>(payload.ToString());
            
            Program.Instance.AvatarPropertiesRequestManager.RequestAvatarProperties(this.ID, new OpenMetaverse.UUID(profile_request.AgentId));
            break;
          }
          case ClientMessageType.RegionStatsRequest:
          {
            WebsocketBackend.Instance.SendTo(this.ID, new ServerMessages.SimStatsResponseMessage() {
              Stats = Program.Instance.Client.Network.CurrentSim.Stats
            });
            break;
          }
          case ClientMessageType.AvatarListRequest:
          {
            var agents_in_sim = Program.Instance.Client.Network.CurrentSim.ObjectsAvatars;
            var avatar_locations = new List<AvatarListResponseMessage.AvatarLocation>();
            agents_in_sim.ForEach((OpenMetaverse.Avatar avatar) =>
            {
              avatar_locations.Add(new AvatarListResponseMessage.AvatarLocation() {
               FirstName = avatar.FirstName,
               LastName = avatar.LastName,
               Id = avatar.ID,
               Location = avatar.Position
              });
            });

            WebsocketBackend.Instance.SendTo(this.ID, new ServerMessages.AvatarListResponseMessage() {
              AvatarLocations = avatar_locations
            });
            break;
          }
          case ClientMessageType.ChatRequest:
          {
            var chat_request = JsonConvert.DeserializeObject<ChatRequestMessage>(payload.ToString());
            Program.Instance.Client.Self.Chat(chat_request.Message, chat_request.Channel, chat_request.ChatType);
            break;
          }
          default:
          {
            Console.WriteLine($"Unknown message type '{message_type}'");
            break;
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("Failed to decode message from client: " + ex.Message);
      }
    }
  }
}
