using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace NoReticle.Server
{
    public class Server : BaseScript
    {
        private const string MessagePrefix = "[NoReticle]:";

        public Server()
        {
            RegisterWeaponsCommand();
        }

        private void RegisterWeaponsCommand()
        {
            RegisterCommand("weapons", new Action<int, List<object>, string>((source, args, rawCommand) =>
            {
                // Source is not a player.
                if (source <= 0)
                {
                    Log($"Command /{rawCommand} must be executed by a player.");
                    return;
                }

                try
                {
                    Player player = Players.Single(p => p.Handle == source.ToString());
                    player.TriggerEvent("NoReticle:Client:GiveAllWeapons");
                }
                catch (Exception ex)
                {
                    if (ex is ArgumentNullException or InvalidOperationException)
                    {
                        Log($"Failed to find player {source} for command /weapons.");
                        return;
                    }

                    // Unanticipated exception.
                    Log(ex.Message);
                }
            }), false);
        }

        private async Task<bool> Configuration(string key, bool defaultValue, string filePath, bool? updatedValue = null)
        {
            try
            {
                List<string> lines = File.ReadAllLines(filePath).ToList();

                if (lines.Any(line => line.Contains(key)))
                {
                    string line = lines.Single(line => line.Contains(key));
                    int lineIndex = lines.IndexOf(line);
                    int keyIndex = line.IndexOf(key, StringComparison.Ordinal) + key.Length;
                    string value = line.Substring(keyIndex).ToLower().Trim();

                    if (updatedValue.HasValue)
                    {
                        value = updatedValue.Value.ToString().ToLower().Trim();
                        string updatedLine = $"{line.Substring(0, keyIndex)} {value}";
                        lines[lineIndex] = updatedLine; // Replace the line in the array

                        File.WriteAllLines(filePath, lines);
                        Log($"Updated value of '{key}' to '{value}'.");
                    }

                    Log($"Returning value of '{value}' for '{key}'.");
                    return await Task.FromResult(Convert.ToBoolean(value));
                }

                // Configuration line not found.
                string stringDefaultValue = defaultValue.ToString().ToLower().Trim();
                string createLine = $"{key} {stringDefaultValue}";

                // Insert/Create line at bottom of file.
                lines.Add(createLine);
                File.WriteAllLines(filePath, lines);

                Log($"Created line '{key}' with default value of '{stringDefaultValue}' in {filePath}.");
                return await Configuration(key, defaultValue, filePath, updatedValue);
            }
            catch (Exception ex)
            {
                Log("An error occurred: " + ex.Message);
                return await Task.FromResult(defaultValue);
            }
        }

        [EventHandler("NoReticle:Server:ReadConfiguration")]
        private async void ReadConfiguration([FromSource] Player player, dynamic args)
        {
            try
            {
                string key = args.key;
                bool defaultValue = args.defaultValue;
                string filePath = args.filePath;

                bool value = await Configuration(key, defaultValue, filePath);
                player.TriggerEvent("NoReticle:Client:UpdateConfiguration", new { key, value });
            }
            catch (Exception ex)
            {
                Log("An error occurred: " + ex.Message);
            }
        }

        [EventHandler("NoReticle:Server:UpdateConfiguration")]
        private async void UpdateConfiguration(dynamic args)
        {
            try
            {
                string key = args.key;
                bool newValue = args.value;
                bool defaultValue = args.defaultValue;
                string filePath = args.filePath;

                bool value = await Configuration(key, defaultValue, filePath, newValue);

                // newValue not properly assigned.
                if (value != newValue)
                {
                    Log($"Could not update '{key}' to '{newValue.ToString().ToLower().Trim()}'. Ensure this setting has been properly configured. Attempted file path: {filePath}.");
                    return;
                }

                foreach (Player player in Players)
                {
                    player.TriggerEvent("NoReticle:Client:UpdateConfiguration", new { key, value });
                }
            }
            catch (Exception ex)
            {
                Log("An error occurred: " + ex.Message);
            }
        }

        [EventHandler("NoReticle:Server:GetAcePermissions")]
        private void GetAcePermissions([FromSource] Player player)
        {
            string playerId = player.Handle;

            if (IsPlayerAceAllowed(playerId, "Reticle"))
            {
                player.TriggerEvent("NoReticle:Client:SetPlayerReticleAceAllowed");
            }
            if (IsPlayerAceAllowed(playerId, "ReticleStunGun"))
            {
                player.TriggerEvent("NoReticle:Client:SetStunGunReticleAllowed");
            }
        }

        /// <summary>
        /// Writes debug message to the server's console.
        /// </summary>
        /// <param name="message">Message to display.</param>
        public static void Log(string message)
        {
            Debug.WriteLine($"{MessagePrefix} {message}");
        }
    }
}