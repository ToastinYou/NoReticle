using CitizenFX.Core;
using System;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace NoReticle.Client
{
    public class Client : BaseScript
    {
        private bool _reticleAllowed, _stunGunReticleAllowed;

        public Client()
        {
            EventHandlers.Add("onClientResourceStart", new Action<string>(OnClientResourceStart));
            Tick += OnTick;
        }

        private void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            ReadMetadataConfigurations();
            TriggerServerEvent("NoReticle:Server:GetAcePermissions");
            Log("Resource loaded.");
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
        
        private static void ReadMetadataConfigurations()
        {
            const string keyEnableGiveAllWeaponsCommand = "enable_give_all_weapons_command";
            const string keyHideAircraftReticle = "hide_aircraft_reticle";

            try
            {
                Configurations.EnableGiveAllWeaponsCommand = Configurations.GetValue(keyEnableGiveAllWeaponsCommand);
                Configurations.HideAircraftReticle = Configurations.GetValue(keyHideAircraftReticle);
            }
            catch (Exception)
            {
                // Ignored, if Configurations.GetValue() failed it will handle the logging.
            }
        }

        /// <returns>The Weapon Hash as a uint.</returns>
        private static uint GetWeaponHash(WeaponHash weaponHash) => (uint)weaponHash;

        [EventHandler("NoReticle:Client:GiveAllWeapons")]
        private void GiveAllWeapons()
        {
            int playerId = Game.PlayerPed.Handle;

            if (!Configurations.EnableGiveAllWeaponsCommand)
            {
                Log($"Player {playerId} does not have access to this command.");
                return;
            }

            Array weaponHashes = Enum.GetValues(typeof(WeaponHash));

            foreach (WeaponHash weapon in weaponHashes)
            {
                int maxAmmo = 0;
                uint weaponHash = GetWeaponHash(weapon);

                GetMaxAmmo(playerId, weaponHash, ref maxAmmo);
                GiveWeaponToPed(playerId, weaponHash, maxAmmo, false, false);
            }

            SetCurrentPedWeapon(playerId, GetWeaponHash(WeaponHash.Unarmed), true);
            Log($"Gave player {playerId} all weapons.");
        }

        [EventHandler("NoReticle:Client:SetPlayerReticleAceAllowed")]
        private void SetPlayerReticleAceAllowed()
        {
            _reticleAllowed = true;
            Log("Reticle is allowed.");
        }

        [EventHandler("NoReticle:Client:SetStunGunReticleAllowed")]
        private void SetStunGunReticleAllowed()
        {
            _stunGunReticleAllowed = true;
            Log("Reticle for the Stun Gun is allowed.");
        }

        /// <summary>
        /// Writes debug message to the client's console.
        /// </summary>
        /// <param name="message">Message to display.</param>
        public static void Log(string message)
        {
            Debug.WriteLine($"[NoReticle]: {message}");
        }
    }
}