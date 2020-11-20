using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace Server
{
    public class Main : BaseScript
    {
        public Main() { }

        [EventHandler("NoReticle:Server:GetAcePermissions")]
        private void GetAcePermissions([FromSource] Player p)
        {
            // if ace permission 'Reticle' is allowed for the player then show the reticle..
            if (API.IsPlayerAceAllowed(p.Handle, "Reticle"))
            {
                p.TriggerEvent("NoReticle:Client:SetPlayerReticleAceAllowed");
            }
            // if ace permission 'ReticleStunGun' is allowed for the player then show the reticle..
            if (API.IsPlayerAceAllowed(p.Handle, "ReticleStunGun"))
            {
                p.TriggerEvent("NoReticle:Client:SetStunGunReticleAllowed");
            }
        }
    }
}