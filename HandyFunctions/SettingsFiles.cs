using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace HandyFunctions
{
    // Handles settings stored in standard INI file format(s)
    public class KeyValueSettingsFile
    {
        // Member variables
        public string SettingsFilePath;
        private Dictionary<string, object> settings = new Dictionary<string,object>();

        // Constructors
        public KeyValueSettingsFile() { }
        public KeyValueSettingsFile(string SettingsPath) { this.SettingsFilePath = SettingsPath; }

        // Auto-load/save functions
        public void loadFromFile() // List itself should contain the default values
        {
            // Check for nonexistent files (to speed things up)
            if (!File.Exists(SettingsFilePath)) { return; }

            // Load values
            for (int i = 0; i < settings.Count; i++)
            {
                KeyValuePair<string, object> kvp = settings.ElementAt<KeyValuePair<string, object>>(i);

                string key = kvp.Key;
                object value = kvp.Value;

                string type = Convert.ToString(kvp.GetType()).ToLowerInvariant();

                if (SettingsFileLowLevelIO.keyExistsInFile(key, SettingsFilePath))
                {
                    // Any errors triggered by the read... functions should NOT be handled here (so the programmer is notified of them)!
                    if (value is int) { value = SettingsFileLowLevelIO.readInteger(key, SettingsFilePath); }
                    if (value is double) { value = SettingsFileLowLevelIO.readDouble(key, SettingsFilePath); }
                    if (value is string) { value = SettingsFileLowLevelIO.readString(key, SettingsFilePath); }
                    if (value is bool) { value = SettingsFileLowLevelIO.readBoolean(key, SettingsFilePath); }
                }

                // Update value
                settings[key] = value;
            }
        }
        public void saveToFile() 
        {
            // If a settings file already exists, it WILL be overwritten by this function!
            foreach (KeyValuePair<string, object> pair in settings)
            {
                string key = pair.Key;
                object value = pair.Value;
                if (value is double) { SettingsFileLowLevelIO.writeDouble(key, double.Parse(value.ToString()), SettingsFilePath); }
                else if (value is int) { SettingsFileLowLevelIO.writeInteger(key, int.Parse(value.ToString()), SettingsFilePath); }
                else if (value is string) { SettingsFileLowLevelIO.writeString(key, value.ToString(), SettingsFilePath); }
                else if (value is bool) { SettingsFileLowLevelIO.writeBoolean(key, bool.Parse(value.ToString()), SettingsFilePath); }
            }
        }
    
        // Per-setting functions
        public bool changeSetting(string name, object value)
        {
            if(nameExists(name)){ settings[name] = value; return true; }
            else { return false; }
        }
        public bool addSetting(string name, object value)
        {
            if(!nameExists(name)){ settings.Add(name, value); return true; }
            else { return false; }
        }
        public bool removeSetting(string name)
        {
            if (nameExists(name)) { settings.Remove(name); return true; }
            else { return false; }
        }

        // Value retrievers
        public bool getBool(string name)
        {
            // Make sure value is a boolean
            if(!settings.ContainsKey(name)){ throw new ArgumentException("No setting with that name exists."); }
            if (!(settings[name] is bool)) { throw new ArgumentException("That setting's value is not a bool."); }
            
            // Return
            return Convert.ToBoolean(settings[name]);
        }
        public int getInteger(string name)
        {
            if(!settings.ContainsKey(name)){ throw new ArgumentException("No setting with that name exists."); }
            if (!(settings[name] is int)) { throw new ArgumentException("That setting's value is not an integer."); }

            return int.Parse(settings[name].ToString());
        }
        public double getDouble(string name)
        {
            if (!settings.ContainsKey(name)) { throw new ArgumentException("No setting with that name exists."); }
            if (!(settings[name] is double)) { throw new ArgumentException("That setting's value is not a double."); }

            return double.Parse(settings[name].ToString());
        }
        public string getString(string name)
        {
            if(!settings.ContainsKey(name)){ throw new ArgumentException("No setting with that name exists."); }

            return settings[name].ToString();
        }

        // Existence checking functions
        public bool nameExists(string name) { return settings.ContainsKey(name); }
        public bool valueExists(string value) { return settings.ContainsValue(value); }
        public bool nameValuePairExists(string name, string value) { return nameExists(name) && settings[name].ToString() == value; }       
    }
}
