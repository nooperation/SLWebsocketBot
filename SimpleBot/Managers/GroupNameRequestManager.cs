using OpenMetaverse;
using SimpleBot.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBot.Managers
{
  class GroupNameRequestManager : ManagerBase
  {
    private class ClientRequest
    {
      public string SessionId { get; set; }
      public UUID GroupId { get; set; }
      public DateTime RequestTime { get; set; }
    }

    private Dictionary<UUID, List<ClientRequest>> PendingRequests = new Dictionary<UUID, List<ClientRequest>>();
    private DateTime LastExpirationCheck { get; set; }
    private int ExpirationTimeInMilliseconds => 5000;

    public override bool Init()
    {
      Program.Instance.Client.Groups.GroupNamesReply += Groups_GroupNamesReply;
      LastExpirationCheck = DateTime.Now;

      return true;
    }

    public override bool Poll()
    {
      var time_since_last_expiration_check = DateTime.Now - LastExpirationCheck;
      if (time_since_last_expiration_check.TotalMilliseconds >= ExpirationTimeInMilliseconds)
      {
        var requests_to_remove = new List<UUID>();

        lock (PendingRequests)
        {
          foreach (var request in PendingRequests)
          {
            request.Value.RemoveAll(item => (DateTime.Now - item.RequestTime).TotalMilliseconds > ExpirationTimeInMilliseconds);
            if (request.Value.Count == 0)
            {
              requests_to_remove.Add(request.Key);
            }
          }

          foreach (var id in requests_to_remove)
          {
            PendingRequests.Remove(id);
          }
        }

        foreach (var id in requests_to_remove)
        {
          Console.WriteLine($"Dropping request {id}");
        }
      }

      return true;
    }

    public void RequestGroupNames(string session_id, List<UUID> group_ids)
    {
      lock (PendingRequests)
      {
        foreach (var id in group_ids)
        {
          var new_request = new ClientRequest()
          {
            SessionId = session_id,
            GroupId = id,
            RequestTime = DateTime.Now
          };

          PendingRequests.Add(id, new List<ClientRequest>() {
            new_request
          });
        }
      }

      foreach (var id in group_ids)
      {
        Console.WriteLine($"Adding new request for {group_ids.Count} group names by client {session_id}");
        Program.Instance.Client.Groups.RequestGroupNames(group_ids);
      }
    }


    private void Groups_GroupNamesReply(object sender, GroupNamesEventArgs e)
    {
      foreach (var group in e.GroupNames)
      {
        List<ClientRequest> requests;
        Console.WriteLine($"Server sent us group name for {group.Value} [{group.Key}]");

        lock (PendingRequests)
        {
          if (PendingRequests.ContainsKey(group.Key) == false)
          {
            continue;
          }

          requests = PendingRequests[group.Key];
          PendingRequests.Remove(group.Key);
        }

        var response = new Server.ServerMessages.GroupNameResponseMessage()
        {
          Id = group.Key,
          Name = group.Value
        };

        foreach (var request in requests)
        {
          WebsocketBackend.Instance.SendTo(request.SessionId, response);
          Console.WriteLine($"Sent group name to {request.SessionId} for {group.Key}");
        }
      }
    }
  }
}
