using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace HandyFunctions
{
    // This class houses the low-level functions (e.g. file writing) used to work with key-value settings setups
    public class SettingsFileLowLevelIO
    {
        // Read functions
        public static bool keyExistsInFile(string key, string SettingsFilePath)
        {
            // Validate file
            if (!File.Exists(SettingsFilePath))
            {
                throw new ArgumentException("Settings file not found!");
            }

            // Open connection to file
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(SettingsFilePath);
            }
            catch (Exception ex)
            {
                reader.Close();
                throw new ArgumentException("Unable to read file due to exception of type \"" + ex.GetType().ToString() + "\"");
            }

            // Search for key
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                if (line.StartsWith(key + "=") && Regex.Matches(line, "=").Count == 1)
                {
                    reader.Close();
                    return true;
                }
            }

            // Report that key was not found
            reader.Close();
            return false;
        }

        public static int readInteger(string key, string SettingsFilePath)
        {
            string value = readString(key, SettingsFilePath);
            if (!DataValidation.ValidateInteger(value))
            {
                throw new ArgumentException("That is not an integer!");
            }
            return int.Parse(value);
        }
        public static double readDouble(string key, string SettingsFilePath)
        {
            string value = readString(key, SettingsFilePath);
            if (!DataValidation.ValidateDouble(value))
            {
                throw new ArgumentException("That is not a double!");
            }
            return double.Parse(value);
        }
        public static string readString(string key, string SettingsFilePath)
        {
            // Validate file
            if (!File.Exists(SettingsFilePath))
            {
                throw new ArgumentException("Settings file not found!");
            }

            // Open connection to file
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(SettingsFilePath);
            }
            catch (Exception ex)
            {
                reader.Close();
                throw new ArgumentException("Unable to read file due to exception of type \"" + ex.GetType().ToString() + "\"");
            }

            // Search for key (note: we do not call the keyExists function here because we would still need to search for the key anyways)
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                if (line.StartsWith(key + "=") && Regex.Matches(line, @"\=").Count == 1)
                {
                    reader.Close();
                    return Regex.Match(line, @"(?<=(\=)).+$").Value;
                }
            }

            // Report that key was not found
            reader.Close();
            throw new ArgumentException("Specified key does not exist in settings file!");
        }
        public static bool readBoolean(string key, string SettingsFilePath)
        {
            int value = readInteger(key, SettingsFilePath); // Will throw an exception if a non-integer is entered (so no need to notify the programmer that a non-integer was submitted below)
            if(!DataValidation.ValidateBooleanInteger(value.ToString()))
            {
                throw new ArgumentException("That is not a boolean integer! Boolean integers must be either 0 (false) or 1 (true).");
            }
            return (value == 1);
        }

        public static string readStringAndValidate(string key, string validationRegexPattern, string SettingsFilePath)
        {
            string value = readString(key, SettingsFilePath);
            if (!DataValidation.ValidateBasedOnRegex(value, validationRegexPattern))
            {
                throw new ArgumentException("That value does not match the provided regex!");
            }
            return value;
        }
        public static string readStringAndValidate(string key, List<string> validationRegexPatternList, string SettingsFilePath)
        {
            string value = readString(key, SettingsFilePath);
            if (!DataValidation.ValidateBasedOnRegexList(value, validationRegexPatternList))
            {
                throw new ArgumentException("That value does not match one or more of the provided regexes!");
            }
            return value;
        }

        // Write functions
        public static void writeInteger(string key, int value, string SettingsFilePath)
        {
            writeString(key, value.ToString(), SettingsFilePath);
        }
        public static void writeDouble(string key, double value, string SettingsFilePath)
        {
            writeString(key, value.ToString(), SettingsFilePath);
        }
        public static void writeString(string key, string value, string SettingsFilePath)
        {
            List<string> newLines = new List<string>();
            bool settingWasWritten = false;

            if (File.Exists(SettingsFilePath))
            {
                // Handle existing settings files
                StreamReader reader = new StreamReader(SettingsFilePath);

                while (reader.Peek() != -1)
                {
                    string line = reader.ReadLine();

                    if (line.StartsWith(key + "="))
                    {
                        newLines.Add(key + "=" + value);
                        settingWasWritten = true;
                    }
                    else { newLines.Add(line); }
                }

                // Close the reader
                reader.Close();
            }

            // Handle non-existent settings files or files that don't contain the specified setting
            if (!settingWasWritten)
            {
                newLines.Add(key + "=" + value);
            }

            // Wrap things up
            File.WriteAllLines(SettingsFilePath, newLines);
        }
        public static void writeBoolean(string key, bool value, string SettingsFilePath)
        {
            int isTrue = 0;
            if(value){ isTrue = 1; }

            writeInteger(key, isTrue, SettingsFilePath);
        }

        public static void writeStringAndValidate(string key, string value, string validationRegexPattern, string SettingsFilePath)
        {
            if (!DataValidation.ValidateBasedOnRegex(value, validationRegexPattern))
            {
                throw new ArgumentException("That value does not match the provided regex!");
            }
            else { writeString(key, value, SettingsFilePath); }
        }
        public static void writeStringAndValidate(string key, string value, List<string> validationRegexPatternList, string SettingsFilePath)
        {
            if (!DataValidation.ValidateBasedOnRegexList(value, validationRegexPatternList))
            {
                throw new ArgumentException("That value does not match one or more of the provided regexes!");
            }
            else { writeString(key, value, SettingsFilePath); }
        }
    }
}
