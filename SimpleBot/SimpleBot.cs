using OpenMetaverse;
using SimpleBot.Managers;
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

    public enum ImplementedManagers
    {
      AvatarPropertiesRequestManager = 0,
      ChatManager,
      ConnectionManager,
      LocalAvatarManager,
      GroupNameRequestManager
    }

    public Dictionary<ImplementedManagers, ManagerBase> Managers { get; protected set; }
    private List<ManagerBase> ManagerList { get; set; }

    public AvatarPropertiesRequestManager AvatarPropertiesRequestManager => Managers[ImplementedManagers.AvatarPropertiesRequestManager] as AvatarPropertiesRequestManager;
    public ChatManager ChatManager => Managers[ImplementedManagers.ChatManager] as ChatManager;
    public ConnectionManager ConnectionManager => Managers[ImplementedManagers.ConnectionManager] as ConnectionManager;
    public LocalAvatarManager LocalAvatarManager => Managers[ImplementedManagers.LocalAvatarManager] as LocalAvatarManager;
    public GroupNameRequestManager GroupNameRequestManager => Managers[ImplementedManagers.GroupNameRequestManager] as GroupNameRequestManager;

    public void Run()
    {
      Managers = new Dictionary<ImplementedManagers, ManagerBase>();
      ManagerList = new List<ManagerBase>();

      Config = new Config("SimpleBot");
      Config.Load();

      Client = new GridClient();

      var enum_values = Enum.GetValues(typeof(ImplementedManagers));
      foreach (var value in enum_values)
      {
        var type_name = Enum.GetName(typeof(ImplementedManagers), value);

        var type = Type.GetType("SimpleBot.Managers." + type_name);
        if (type != null)
        {
          var manager_instance = Activator.CreateInstance(type) as ManagerBase;
          Managers.Add((ImplementedManagers)value, manager_instance);
          ManagerList.Add(manager_instance);
        }
      }

      ManagerList.ForEach(manager => manager.Init());

      WebsocketBackend.Instance.Init(55000);
    }

    public bool Poll()
    {
      if (Client == null)
      {
        return false;
      }

      return ManagerList.All(manager => manager.Poll());
    }

    public void Shutdown()
    {
      ManagerList.ForEach(manager => manager.Shutdown());
    }
  }
}
