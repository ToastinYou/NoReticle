using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace NoReticle.Server
{
    public class Main : BaseScript
    {
        [EventHandler("NoReticle:Server:GetAcePermissions")]
        private void GetAcePermissions([FromSource] Player p)
        {
            // if ace permission 'Reticle' is allowed for the player then show the reticle..
            if (IsPlayerAceAllowed(p.Handle, "Reticle"))
            {
                p.TriggerEvent("NoReticle:Client:SetPlayerReticleAceAllowed");
            }
            // if ace permission 'ReticleStunGun' is allowed for the player then show the reticle..
            if (IsPlayerAceAllowed(p.Handle, "ReticleStunGun"))
            {
                p.TriggerEvent("NoReticle:Client:SetStunGunReticleAllowed");
            }
        }
    }
}