using CitizenFX.Core;
using System;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;
using static NoReticle.Client.ClientLog;

namespace NoReticle.Client
{
    public class Client : BaseScript
    {
        /// <returns>The current player's handle (ID), or -1 if the ID cannot be found.</returns>
        public static int Handle => Game.PlayerPed?.Handle ?? -1;

        private bool _reticleAllowed, _stunGunReticleAllowed;

        public Client()
        {
            EventHandlers.Add("onClientResourceStart", new Action<string>(OnClientResourceStart));
            Tick += OnTick;
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

            if (!_reticleAllowed && w != null)
            {
                bool isMusket = w.Hash == WeaponHash.Musket;
                bool isSniper = w.Group == WeaponGroup.Sniper && IsFirstPersonAimCamActive();
                bool isUnarmed = w.Group == WeaponGroup.Unarmed;
                bool isStunGun = _stunGunReticleAllowed && w.Group == WeaponGroup.Stungun;
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

        [EventHandler("NoReticle:Client:SetPlayerReticleAceAllowed")]
        private void SetPlayerReticleAceAllowed()
        {
            _reticleAllowed = true;
            Trace("Reticle is allowed.");
        }

        [EventHandler("NoReticle:Client:SetStunGunReticleAllowed")]
        private void SetStunGunReticleAllowed()
        {
            _stunGunReticleAllowed = true;
            Trace("Reticle for the Stun Gun is allowed.");
        }
    }
}