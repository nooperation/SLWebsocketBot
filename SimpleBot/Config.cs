using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace SimpleBot
{
  public class Config
  {
    /// <summary>
    /// Name of the program.
    /// </summary>
    public string ProgramName { get; protected set; }
 
    /// <summary>
    /// User directory for this program.
    /// </summary>
    public string UserDirectory { get; protected set; }
 
    /// <summary>
    /// Path to the settings file.
    /// </summary>
    public string SettingsPath { get; protected set; }

    /// <summary>
    /// User defined settings.
    /// </summary>
    private Dictionary<string, object> Settings { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// Constructs a new config instance.
    /// </summary>
    /// <param name="program_name">Name of the program this config is for.</param>
    public Config(string program_name)
    {
      ProgramName = program_name;
      UserDirectory = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), ProgramName);
      SettingsPath = Path.Combine(UserDirectory, "Settings") + ".json";
    }

    /// <summary>
    /// Gets the specified setting.
    /// </summary>
    /// <typeparam name="T">Type of data to get.</typeparam>
    /// <param name="setting_name">Setting name</param>
    /// <returns>Setting value.</returns>
    /// <exception cref="Exception">Thrown on bad conversion of setting not found.</exception>
    public T GetSetting<T>(string setting_name)
    {
      if (Settings.ContainsKey(setting_name))
      {
        var setting_value = Settings[setting_name];
        var setting_type = setting_value.GetType();
        var desired_type = typeof(T);

        var json_token = setting_value as JToken;
        if(json_token != null)
        {
          return (T)json_token.ToObject(typeof(T));
        }

        try
        {
          if(desired_type.IsPrimitive || setting_type.IsPrimitive)
          {
            var converted_value = Convert.ChangeType(setting_value, desired_type);
            if(converted_value != null)
            {
              return (T)converted_value;
            }
          }

          return (T)setting_value;
        }
        catch (Exception ex)
        {
          var reason = ex.Message;
          throw new Exception($"Setting {setting_name} has type {setting_type}. Cannot convert to {desired_type}: {reason}.", ex);
        }
      }

      throw new Exception($"Setting {setting_name} not found.");
    }

    /// <summary>
    /// Gets the specified setting or return a default value if setting does not exist.
    /// </summary>
    /// <typeparam name="T">Type of data to get.</typeparam>
    /// <param name="setting_name">Setting name.</param>
    /// <param name="default_value">Default value to return if setting doesn't exist.</param>
    /// <returns>Setting value</returns>
    /// <exception cref="Exception">Thrown on bad conversion.</exception>
    public T GetSetting<T>(string setting_name, T default_value)
    {
      if (Settings.ContainsKey(setting_name))
      {
        return GetSetting<T>(setting_name);
      }

      return default_value;
    }

    /// <summary>
    /// Sets or creates the specified setting with the given value.
    /// </summary>
    /// <typeparam name="T">Type of data to set.</typeparam>
    /// <param name="setting_name">Setting name.</param>
    /// <param name="setting_value">Setting value.</param>
    public void SetSetting<T>(string setting_name, T setting_value)
    {
      if (!Settings.ContainsKey(setting_name))
      {
        Settings.Add(setting_name, setting_value);
      }
      else
      {
        Settings[setting_name] = setting_value;
      }
    }

    /// <summary>
    /// Saves settings to disk.
    /// </summary>
    public void Save()
    {
      if (!Directory.Exists(UserDirectory))
      {
        Directory.CreateDirectory(UserDirectory);
      }

      var contents = JsonConvert.SerializeObject(Settings, Formatting.Indented);
      File.WriteAllText(SettingsPath, contents);
    }

    /// <summary>
    /// Loads settings from disk.
    /// </summary>
    public void Load()
    {
      var contents = File.ReadAllText(SettingsPath);
      Settings = JsonConvert.DeserializeObject<Dictionary<string, object>>(contents);
    }
  }
}
