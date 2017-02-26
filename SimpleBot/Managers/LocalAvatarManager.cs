using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenMetaverse;
using OpenMetaverse.Packets;
using SimpleBot.Server;

namespace SimpleBot.Managers
{
  class LocalAvatarManager : ManagerBase
  {
    public override bool Init()
    {
      Program.Instance.Client.Grid.CoarseLocationUpdate += Grid_CoarseLocationUpdate;
      return true;
    }

    // Todo: Ditch GridManager, move to poll for a more predictable flow
    private void Grid_CoarseLocationUpdate(object sender, CoarseLocationUpdateEventArgs e)
    {
      var instance = Program.Instance;
      if(instance.Client.Network.Connected == false || instance.Client.Network.CurrentSim == null)
      {
        return;
      }

      if(e.Simulator == instance.Client.Network.CurrentSim)
      {
        if(e.NewEntries.Count > 0 && e.RemovedEntries.Count > 0)
        {
          WebsocketBackend.Instance.Broadcast(new Server.ServerMessages.NearbyAgentsDelta()
          {
            Added = e.NewEntries,
            Removed = e.RemovedEntries
          });
        }
      }
    }

    public override bool Shutdown()
    {
      Program.Instance.Client.Grid.CoarseLocationUpdate -= Grid_CoarseLocationUpdate;
      return true;
    }
  }
}
