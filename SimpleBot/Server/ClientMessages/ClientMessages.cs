using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OpenMetaverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBot.Server.ClientMessages
{
  public enum ClientMessageType
  {
    Unknown,
    ProfileRequest,
    GroupNameRequest,
    RegionStatsRequest,
    AvatarListRequest,
    ChatRequest,
  }

  public class ProfileRequestMessage
  {
    public string AgentId { get; set; }
  }

  public class GroupNamesRequestMessage
  {
    public List<string> GroupIds { get; set; }
  }

  public class ChatRequestMessage
  {
    public string Message { get; set; }
    public int Channel { get; set; }
    public ChatType ChatType { get; set; }
  }
}
