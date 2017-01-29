using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OpenMetaverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenMetaverse.NetworkManager;

namespace SimpleBot.Server.ClientMessages
{
  public enum ClientMessageType
  {
    Unknown,
    ProfileRequest,
    RegionStatsRequest,
    AvatarListRequest,
  }

  public class ProfileRequestMessage
  {
    public string AgentId { get; set; }
  }
}
