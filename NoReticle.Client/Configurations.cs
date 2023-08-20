using System;
using static CitizenFX.Core.Native.API;

namespace NoReticle.Client
{
    public class Configurations
    {
        public const string KeyEnableGiveAllWeaponsCommand = "enable_give_all_weapons_command";
        public static bool EnableGiveAllWeaponsCommand { get; set; }
        public const string KeyHideAircraftReticle = "hide_aircraft_reticle";
        public static bool HideAircraftReticle { get; set; }
        
        public static bool GetValue(string key)
        {
            string value = string.Empty;

            try
            {
                string resourceName = GetCurrentResourceName();
                value = GetResourceMetadata(resourceName, key, 0).ToLower();
                Client.Log($"Set '{key}' to '{value}'");
                return Convert.ToBoolean(value);
            }
            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    Client.Log(string.IsNullOrWhiteSpace(value) ? $"Invalid setting for '{key}'." : $"Invalid setting of '{value}' for '{key}'.");
                }

                // Unanticipated exception.
                Client.Log(ex.Message);
                throw;
            }
        }
    }
}
