using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBot.Managers
{
  class ManagerBase
  {
    public virtual bool Init()
    {
      return true;
    }

    public virtual bool Poll()
    {
      return true;
    }

    public virtual bool Shutdown()
    {
      return true;
    }
  }
}
