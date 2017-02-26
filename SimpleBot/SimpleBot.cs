using OpenMetaverse;
using SimpleBot.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleBot
{
  class SimpleBot
  {
    public Config Config { get; protected set; }
    public GridClient Client { get; protected set; }

    public Managers.AvatarPropertiesRequestManager AvatarPropertiesRequestManager { get; protected set; }
    public Managers.ChatManager ChatManager { get; protected set; }
    public Managers.ConnectionManager ConnectionManager { get; protected set; }
    public Managers.LocalAvatarManager LocalAvatarManager { get; set; }
    public Managers.GroupNameRequestManager GroupNameRequestManager { get; set; }

    public void Run()
    {
      Config = new Config("SimpleBot");
      Config.Load();

      Client = new GridClient();

      AvatarPropertiesRequestManager = new Managers.AvatarPropertiesRequestManager();
      ChatManager  = new Managers.ChatManager();
      ConnectionManager  = new Managers.ConnectionManager();
      LocalAvatarManager = new Managers.LocalAvatarManager();

      ConnectionManager.Init();
      ChatManager.Init();
      AvatarPropertiesRequestManager.Init();
      LocalAvatarManager.Init();
      GroupNameRequestManager.Init();

      WebsocketBackend.Instance.Init(55000);
    }

    public bool Poll()
    {
      if (Client == null)
      {
        return false;
      }

      return AvatarPropertiesRequestManager.Poll() &&
             ChatManager.Poll() &&
             ConnectionManager.Poll() &&
             LocalAvatarManager.Poll() &&
             GroupNameRequestManager.Poll();
    }
  }
}
