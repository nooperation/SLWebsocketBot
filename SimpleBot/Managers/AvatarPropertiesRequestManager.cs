using OpenMetaverse;
using SimpleBot.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBot.Managers
{
  class AvatarPropertiesRequestManager : ManagerBase
  {
    private class ClientRequest
    {
      public string SessionId { get; set; }
      public UUID AvatarId { get; set; }
      public DateTime RequestTime { get; set; }
    }

    private Dictionary<UUID, List<ClientRequest>> PendingRequests = new Dictionary<UUID, List<ClientRequest>>();
    private DateTime LastExpirationCheck { get; set; }
    private int ExpirationTimeInMilliseconds => 5000;

    public override bool Init()
    {
      Program.Instance.Client.Avatars.AvatarPropertiesReply += Avatars_AvatarPropertiesReply;
      LastExpirationCheck = DateTime.Now;

      return true;
    }

    public override bool Poll()
    {
      var time_since_last_expiration_check = DateTime.Now - LastExpirationCheck;
      if(time_since_last_expiration_check.TotalMilliseconds >= ExpirationTimeInMilliseconds)
      {
        lock (PendingRequests)
        {
          var requests_to_remove = new List<UUID>();

          foreach (var request in PendingRequests)
          {
            request.Value.RemoveAll(item => (DateTime.Now - item.RequestTime).TotalMilliseconds > ExpirationTimeInMilliseconds);
            if(request.Value.Count == 0)
            {
              requests_to_remove.Add(request.Key);
            }
          }

          foreach (var request_key in requests_to_remove)
          {
            PendingRequests.Remove(request_key);
            Console.WriteLine($"Dropping request {request_key}");
          }
        }
      }

      return true;
    }
 
    public void RequestAvatarProperties(string session_id, UUID avatar_id)
    {
      var new_request = new ClientRequest() {
        SessionId = session_id,
        AvatarId = avatar_id,
        RequestTime = DateTime.Now
      };

      lock (PendingRequests)
      {
        if (PendingRequests.ContainsKey(avatar_id))
        {
          var existing_request = PendingRequests[avatar_id];
          if(existing_request.FindIndex(n => n.SessionId == session_id) == -1)
          {
            existing_request.Add(new_request);
            Console.WriteLine($"Appending request for avatar properties for {avatar_id} by client {session_id}");
          }
        }
        else
        {
          PendingRequests.Add(avatar_id, new List<ClientRequest>() {
            new_request
          });
          Console.WriteLine($"Adding new request for avatar properties for {avatar_id} by client {session_id}");
        }
      }

      Program.Instance.Client.Avatars.RequestAvatarProperties(avatar_id);
    }

    private void Avatars_AvatarPropertiesReply(object sender, AvatarPropertiesReplyEventArgs e)
    {
      List<ClientRequest> requests;

      lock (PendingRequests)
      {
        if (PendingRequests.ContainsKey(e.AvatarID) == false)
        {
          return;
        }

        requests = PendingRequests[e.AvatarID];
        PendingRequests.Remove(e.AvatarID);
        Console.WriteLine($"Server sent us avatar properties for {e.AvatarID}");
      }

      var response = new Server.ServerMessages.ProfileResponseMessage()
      {
        AgentId = e.AvatarID,
        AboutText = e.Properties.AboutText,
        BornOn = e.Properties.BornOn,
        CharterMember = e.Properties.CharterMember,
        FirstLifeImage = e.Properties.FirstLifeImage,
        FirstLifeText = e.Properties.FirstLifeText,
        Flags = e.Properties.Flags,
        Partner = e.Properties.Partner,
        ProfileImage = e.Properties.ProfileImage,
        ProfileURL = e.Properties.ProfileURL
      };

      foreach (var request in requests)
      {
        WebsocketBackend.Instance.SendTo(request.SessionId, response);
        Console.WriteLine($"Sent avatar properties to {request.SessionId} for {request.AvatarId}");
      }
    }
  }
}
