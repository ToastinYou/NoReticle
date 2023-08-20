using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using static CitizenFX.Core.Native.API;

namespace NoReticle.Server
{
    public class Main : BaseScript
    {
        public Main()
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
                    TriggerClientEvent(player, "NoReticle:Client:GiveAllWeapons");
                }
                catch (Exception ex)
                {
                    if (ex is ArgumentNullException || ex is InvalidOperationException)
                    {
                        Log($"Failed to find player {source} for command /weapons.");
                        return;
                    }

                    // Unanticipated exception.
                    Log(ex.Message);
                }
            }), false);
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
        private static void Log(string message)
        {
            Debug.WriteLine($"[NoReticle]: {message}");
        }
    }
}