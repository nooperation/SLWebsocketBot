using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBot
{
  public class LoginDetails
  {
    private SecureString _password = new SecureString();

    public string Firstname { get; set; } = "Example";
    public string Lastname { get; set; } = "Resident";
    public string Grid { get; set; } = "agni";
    public string Password
    {
      get
      {
        var bstr_ptr = IntPtr.Zero;

        try
        {
          bstr_ptr = Marshal.SecureStringToBSTR(_password);
          return Marshal.PtrToStringBSTR(bstr_ptr);
        }
        finally
        {
          Marshal.ZeroFreeBSTR(bstr_ptr);
        }
      }
      set
      {
        _password.Clear();
        foreach (var ch in value)
        {
          _password.AppendChar(ch);
        }
      }
    }
  }
}
