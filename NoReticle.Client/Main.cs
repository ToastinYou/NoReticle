using CitizenFX.Core;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace NoReticle.Client
{
    public class Main : BaseScript
    {
        private bool reticleAllowed, stunGunReticleAllowed;

        public Main()
        {
            Log("Resource loaded.");
            TriggerServerEvent("NoReticle:Server:GetAcePermissions", Game.Player.Handle);
        }

        [EventHandler("NoReticle:Client:SetPlayerReticleAceAllowed")]
        private void SetPlayerReticleAceAllowed()
        {
            Log("Allowing reticle.");
            reticleAllowed = true;
        }

        [EventHandler("NoReticle:Client:SetStunGunReticleAllowed")]
        private void SetStunGunReticleAllowed()
        {
            Log("Allowing stun gun reticle.");
            stunGunReticleAllowed = true;
        }

        [Tick]
        private Task OnTick()
        {
            // gets client's current weapon.
            Weapon w = Game.PlayerPed?.Weapons?.Current;

            if (w != null)
            {
                // disable reticle for musket and all weapons EXCEPT snipers, unarmed, stungun (if permissions allowed), and aircraft (w.Group == 0).
                if (w.Hash == WeaponHash.Musket || !((w.Group == WeaponGroup.Sniper && IsFirstPersonAimCamActive()) || w.Group == WeaponGroup.Unarmed || (stunGunReticleAllowed && w.Group == WeaponGroup.Stungun) || w.Group == 0))
                {
                    // if ace perm "Reticle" is not allowed (cannot have reticle) then..
                    if (!reticleAllowed)
                    {
                        // hides reticle (white dot [HUD] when aiming in).
                        HideHudComponentThisFrame(14);
                    }
                }
            }

            return Task.FromResult(0);
        }

        /// <summary>
        /// Writes debug message to the client's console.
        /// </summary>
        /// <param name="msg">Message to display.</param>
        private void Log(string msg)
        {
            Debug.Write($"[NoReticle]: {msg}\n");
        }
    }
}