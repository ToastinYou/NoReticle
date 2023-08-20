using CitizenFX.Core;
using System;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace NoReticle.Client
{
    public class Main : BaseScript
    {
        private bool _reticleAllowed, _stunGunReticleAllowed, _giveAllWeaponsCommandEnabled;

        public Main()
        {
            EventHandlers.Add("onClientResourceStart", new Action<string>(OnClientResourceStart));
            Tick += OnTick;
        }

        private void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            ReadMetadataConfigurations(resourceName);
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
                bool isAircraft = w.Group == 0;

                if (isMusket || !(isSniper || isUnarmed || isStunGun || isAircraft))
                {
                    // Hide reticle.
                    HideHudComponentThisFrame(14);
                }
            }

            return Task.FromResult(0);
        }

        private void ReadMetadataConfigurations(string resourceName)
        {
            string giveAllWeaponsCommandMetadata = GetResourceMetadata(resourceName, "enable_give_all_weapons_command", 0).ToLower();

            try
            {
                _giveAllWeaponsCommandEnabled = Convert.ToBoolean(giveAllWeaponsCommandMetadata);
                Log($"Set 'enable_give_all_weapons_command' to '{giveAllWeaponsCommandMetadata}'");
            }
            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    Log($"Invalid setting of '{giveAllWeaponsCommandMetadata}' for 'enable_give_all_weapons_command'.");
                }

                // Unanticipated exception.
                Log(ex.Message);
            }
        }

        /// <returns>The Weapon Hash as a uint.</returns>
        private static uint GetWeaponHash(WeaponHash weaponHash) => (uint)weaponHash;

        [EventHandler("NoReticle:Client:GiveAllWeapons")]
        private void GiveAllWeapons()
        {
            int playerId = Game.PlayerPed.Handle;

            if (!_giveAllWeaponsCommandEnabled)
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
        private static void Log(string message)
        {
            Debug.WriteLine($"[NoReticle]: {message}");
        }
    }
}