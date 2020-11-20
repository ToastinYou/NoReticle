using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Threading.Tasks;

namespace Client
{
    public class Main : BaseScript
    {
        private bool _reticleAllowed, _stunGunReticleAllowed;

        public Main()
        {
            Log("Resource loaded.");
            TriggerServerEvent("NoReticle:Server:GetAcePermissions", Game.Player.Handle);
        }

        [EventHandler("NoReticle:Client:SetPlayerReticleAceAllowed")]
        private void SetPlayerReticleAceAllowed()
        {
            Log("Allowing reticle.");
            _reticleAllowed = true;
        }

        [EventHandler("NoReticle:Client:SetStunGunReticleAllowed")]
        private void SetStunGunReticleAllowed()
        {
            Log("Allowing stun gun reticle.");
            _stunGunReticleAllowed = true;
        }

        [Tick]
        private async Task ProcessTask()
        {
            // gets client's current weapon.
            Weapon w = Game.PlayerPed?.Weapons?.Current;

            if (w != null)
            {
                // disable reticle for musket and all weapons EXCEPT snipers, unarmed, stungun (if permissions allowed), and aircraft (w.Group == 0).
                if (w.Hash == WeaponHash.Musket || !(w.Group == WeaponGroup.Sniper || w.Group == WeaponGroup.Unarmed || (_stunGunReticleAllowed && w.Group == WeaponGroup.Stungun) || w.Group == 0))
                {
                    // if ace perm "Reticle" is not allowed (cannot have reticle) then..
                    if (!_reticleAllowed)
                    {
                        // hides reticle (white dot [HUD] when aiming in).
                        API.HideHudComponentThisFrame(14);
                    }
                }
            }
        }

        /// <summary>
        /// Writes debug message to client's console.
        /// </summary>
        /// <param name="msg">Message to display.</param>
        private void Log(string msg)
        {
            Debug.Write($"[NoReticle]: {msg}\n");
        }
    }
}
