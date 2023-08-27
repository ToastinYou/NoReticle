using CitizenFX.Core;
using System;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;
using static NoReticle.Client.ClientLog;
using static NoReticle.Shared.Shared;

namespace NoReticle.Client
{
    public class Client : BaseScript
    {
        /// <returns>The current player's handle (ID), or -1 if the ID cannot be found.</returns>
        public static int Handle => Game.PlayerPed?.Handle ?? -1;

        public Client()
        {
            EventHandlers.Add("onClientResourceStart", new Action<string>(OnClientResourceStart));
        }

        private void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            Configurations.Read();
            ConfigurationsMenu.Initialize();
            TriggerServerEvent("NoReticle:Server:GetAcePermissions");

            Trace("Resource loaded.");
        }

        private Task OnTick()
        {
            Weapon w = Game.PlayerPed?.Weapons?.Current;

            if (!Reticle.Value && w != null)
            {
                bool isMusket = w.Hash == WeaponHash.Musket;
                bool isSniper = w.Group == WeaponGroup.Sniper && IsFirstPersonAimCamActive();
                bool isUnarmed = w.Group == WeaponGroup.Unarmed;
                bool isStunGun = ReticleStunGun.Value && w.Group == WeaponGroup.Stungun;
                bool isAircraft = w.Group == 0 && !Configurations.HideAircraftReticle;

                if (isMusket || !(isSniper || isUnarmed || isStunGun || isAircraft))
                {
                    // Hide reticle.
                    HideHudComponentThisFrame(14);
                }
            }

            return Task.FromResult(0);
        }

        [EventHandler("NoReticle:Client:OpenMenu")]
        private void OpenMenu()
        {
            Trace($"Received NoReticle:Client:OpenMenu.");
            ConfigurationsMenu.OpenMenu();
        }

        /// <returns>The Weapon Hash as a uint.</returns>
        private static uint GetWeaponHash(WeaponHash weaponHash) => (uint)weaponHash;

        [EventHandler("NoReticle:Client:GiveAllWeapons")]
        private void GiveAllWeapons()
        {
            Trace($"Received NoReticle:Client:GiveAllWeapons.");

            if (!Configurations.EnableGiveAllWeaponsCommand)
            {
                Trace($"Player does not have access to this command.");
                return;
            }

            Array weaponHashes = Enum.GetValues(typeof(WeaponHash));

            foreach (WeaponHash weapon in weaponHashes)
            {
                int maxAmmo = 0;
                uint weaponHash = GetWeaponHash(weapon);

                GetMaxAmmo(Handle, weaponHash, ref maxAmmo);
                GiveWeaponToPed(Handle, weaponHash, maxAmmo, false, false);
            }

            SetCurrentPedWeapon(Handle, GetWeaponHash(WeaponHash.Unarmed), true);
            Trace($"Gave player all weapons.");
        }

        public new static void TriggerEvent(string eventName, params object[] args)
        {
            // Why? Well, because I do not want each class to have to inherit from BaseScript.
            BaseScript.TriggerEvent(eventName, args);
        }
    }

    public static class Chat
    {
        private static readonly int[] DefaultColor = { 255, 255, 255 };
        private static readonly string DefaultSendersName = GetPlayerName(PlayerId());

        public static void Send(string message, int[]? color = null, string? sendersName = null, bool multiline = true)
        {
            color ??= DefaultColor;
            sendersName ??= DefaultSendersName;

            string[] args = { sendersName, message };
            Client.TriggerEvent("chat:addMessage", new { color, multiline, args });
        }
    }
}