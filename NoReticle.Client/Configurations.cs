using CitizenFX.Core;
using static NoReticle.Client.ClientLog;

namespace NoReticle.Client
{
    public class Configurations : BaseScript
    {
        public const string KeyEnableGiveAllWeaponsCommand = "noreticle_enable_give_all_weapons_command";
        public static bool EnableGiveAllWeaponsCommand { get; set; }

        public const string KeyHideAircraftReticle = "noreticle_hide_aircraft_reticle";
        public static bool HideAircraftReticle { get; set; }

        // TODO: Make asynchronous.
        public static void Read(string filePath = "server.cfg")
        {
            TriggerServerEvent("NoReticle:Server:ReadConfiguration", new
            {
                key = KeyEnableGiveAllWeaponsCommand,
                defaultValue = false,
                filePath
            });

            TriggerServerEvent("NoReticle:Server:ReadConfiguration", new
            {
                key = KeyHideAircraftReticle,
                defaultValue = false,
                filePath
            });
        }

        public static void Update(string key, bool value, bool defaultValue)
        {
            TriggerServerEvent("NoReticle:Server:UpdateConfiguration", new
            {
                key,
                value,
                defaultValue,
                filePath = "server.cfg"
            });
        }

        [EventHandler("NoReticle:Client:UpdateConfiguration")]
        private void Update(dynamic args)
        {
            string key = args.key.ToLower();
            bool value = args.value;
            string stringValue = value.ToString().ToLower();

            switch (key)
            {
                case KeyEnableGiveAllWeaponsCommand:
                    EnableGiveAllWeaponsCommand = value;
                    break;
                case KeyHideAircraftReticle:
                    HideAircraftReticle = value;
                    break;
                default:
                    Trace($"Failed to update '{key}' to '{stringValue}'.");
                    return;
            }

            Trace($"Updated '{key}' to '{stringValue}'.");
        }
    }
}
