using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace Server
{
    public class Main : BaseScript
    {
        public Main() { }

        [EventHandler("NoReticle:Server:GetPlayerReticleAceAllowed")]
        private void GetPlayerReticleAceAllowed([FromSource] Player p)
        {
            // if ace permission 'Reticle' is allowed for the player then show the reticle..
            if (API.IsPlayerAceAllowed(p.Handle, "Reticle"))
            {
                p.TriggerEvent("NoReticle:Client:SetPlayerReticleAceAllowed");
            }
        }
    }
}