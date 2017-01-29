using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using OpenMetaverse;
using OpenMetaverse.Packets;
using OpenMetaverse.Utilities;
using System.Runtime.InteropServices;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace SimpleBot
{
  class Program
  {
    static void Main(string[] args)
    {
      new Program();
    }

    private static SimpleBot _instance = null;
    public static SimpleBot Instance => _instance;

    private void RestartClient()
    {
      if(_instance != null)
      {
        Console.WriteLine("Logging client out...");
        _instance.Client.Network.Logout();
        _instance = null;
      }

      try
      {
        _instance = new SimpleBot();
        _instance.Run();
      }
      catch (Exception ex)
      {
        Console.WriteLine("Exception: " + ex.Message);
        _instance = null;
      }
    }

    public Program()
    {
      RestartClient();

      while(true)
      {
        if(Instance == null || Instance.Poll() == false)
        {
          Console.WriteLine($"Client not connected. Waiting {1234}ms before reconnecting...");
          Thread.Sleep(1234);
          RestartClient();
        }
        Thread.Sleep(1000);
      }
    }
  }
}
