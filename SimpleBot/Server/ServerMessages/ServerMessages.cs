using OpenMetaverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenMetaverse.NetworkManager;

namespace SimpleBot.Server.ServerMessages
{
  public enum ServerMessageType
  {
    Generic,
    Init,
    LoginProcess,
    Disconnected,
    LoggedOut,
    InstantMessage,
    Chat,
    ProfileResponse,
    SimStatsResponse,
    AvatarListResponse,
  }

  public interface IServerMessage
  {
    ServerMessageType MessageType { get; }
  }

  class ChatMessage : IServerMessage
  {
    public ServerMessageType MessageType => ServerMessageType.Chat;

    public ChatMessage()
    {
      Time = DateTime.Now;
    }

    public string FromName { get; set; }
    public string Message { get; set; }
    public UUID SourceId { get; set; }
    public UUID OwnerId { get; set; }

    public ChatType Type { get; set; }
    public ChatSourceType SourceType { get; set; }

    public DateTime Time { get; set; }
  }

  class InstantMessage : IServerMessage
  {
    public ServerMessageType MessageType => ServerMessageType.InstantMessage;

    public OpenMetaverse.InstantMessage IM { get; internal set; }
  }

  class DisconnectedMessage : IServerMessage
  {
    public ServerMessageType MessageType => ServerMessageType.Disconnected;

    public DisconnectType Reason { get; set; }
    public string Message { get; set; }
  }

  class LoginProcessMessage : IServerMessage
  {
    public ServerMessageType MessageType => ServerMessageType.LoginProcess;

    public LoginStatus Status { get; set; }
    public string Message { get; set; }
    public string FailReason { get; set; }
  }

  class LoggedOutMessage : IServerMessage
  {
    public ServerMessageType MessageType => ServerMessageType.LoggedOut;
  }

  public class InitMessage : IServerMessage
  {
    public ServerMessageType MessageType => ServerMessageType.Init;

    public string ClientName { get; set; }
    public UUID ClientId { get; set; }

    public string CurrentSimName { get; set; }
    public UUID CurrentSimID { get; set; }
  }

  public class ProfileResponseMessage : IServerMessage
  {
    public ServerMessageType MessageType => ServerMessageType.ProfileResponse;

    public UUID AgentId { get; set; }
    public string FirstLifeText { get; set; }
    public UUID FirstLifeImage { get; set; }
    public UUID Partner { get; set; }
    public string AboutText { get; set; }
    public string BornOn { get; set; }
    public string CharterMember { get; set; }
    public UUID ProfileImage { get; set; }
    public ProfileFlags Flags { get; set; }
    public string ProfileURL { get; set; }
  }

  public class SimStatsResponseMessage : IServerMessage
  {
    public ServerMessageType MessageType => ServerMessageType.SimStatsResponse;

    public Simulator.SimStats Stats { get; set; }
  }

  public class AvatarListResponseMessage : IServerMessage
  {
    public ServerMessageType MessageType => ServerMessageType.AvatarListResponse;

    public struct AvatarLocation
    {
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public UUID Id { get; set; }
      public Vector3 Location { get; set; }
    }

    public List<AvatarLocation> AvatarLocations { get; set; }
  }
}
