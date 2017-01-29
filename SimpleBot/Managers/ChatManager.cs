using OpenMetaverse;
using SimpleBot.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBot.Managers
{
  class ChatManager : ManagerBase
  {
    public LinkedList<Server.ServerMessages.ChatMessage> ChatHistory { get; protected set; } = new LinkedList<Server.ServerMessages.ChatMessage>();
    public int MaxChatHistoryLength => 100;

    public override bool Init()
    {
      var client = Program.Instance.Client;
      client.Self.ChatFromSimulator += Self_ChatFromSimulator;
      client.Self.IM += Self_IM;

      return true;
    }

    private void Self_IM(object sender, InstantMessageEventArgs e)
    {
      WebsocketBackend.Instance.Broadcast(new Server.ServerMessages.InstantMessage()
      {
        IM = e.IM
      });
    }

    private void Self_ChatFromSimulator(object sender, ChatEventArgs e)
    {
      if (e.Type != ChatType.StartTyping && e.Type != ChatType.StopTyping && e.Message.Length > 0)
      {
        var source_avatar_id = e.OwnerID;
        var chat_message = new Server.ServerMessages.ChatMessage()
        {
          SourceType = e.SourceType,
          Type = e.Type,
          FromName = e.FromName,
          SourceId = e.SourceID,
          OwnerId = e.OwnerID,
          Message = e.Message
        };

        WebsocketBackend.Instance.Broadcast(chat_message);

        lock (ChatHistory)
        {
          ChatHistory.AddLast(chat_message);
          if(ChatHistory.Count > MaxChatHistoryLength)
          {
            ChatHistory.RemoveFirst();
          }
        }
      }
    }

  }
}
