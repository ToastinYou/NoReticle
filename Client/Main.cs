using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;

namespace Client
{
    public class Main : BaseScript
    {
        private Ped _p = Game.PlayerPed;
        private bool _reticleAllowed = false;

        public Main()
        {
            Log("Resource loaded.");
            
            EventHandlers.Add("NoReticle:Client:SetPlayerReticleAceAllowed", new Action(SetPlayerReticleAceAllowed));

            TriggerServerEvent("NoReticle:Server:GetPlayerReticleAceAllowed", Game.Player.Handle);

            Tick += ProcessTask;
        }

        private void SetPlayerReticleAceAllowed()
        {
            Screen.ShowNotification("Allowing reticle.", true);
            _reticleAllowed = true;
        }

        private async Task ProcessTask()
        {
            if (_p != null)
            {
                // gets client's current weapon.
                Weapon w = _p.Weapons.Current;

                if (w != null)
                {
                    WeaponHash wHash = w.Hash;

                    if (wHash != WeaponHash.Unarmed && wHash != WeaponHash.StunGun &&
                        wHash != WeaponHash.SniperRifle && wHash != WeaponHash.HeavySniper &&
                        wHash != WeaponHash.HeavySniperMk2 &&
                        wHash != WeaponHash.MarksmanRifle &&
                        API.GetHashKey(wHash.ToString()) != -1783943904) // add MarksmanRifle MKII
                    {
                        // if ace perm "Reticle" is not allowed (cannot have reticle) then..
                        if (_reticleAllowed == false)
                        {
                            // hides reticle (white dot [HUD] when aiming in).
                            API.HideHudComponentThisFrame(14);
                        }
                    }
                }
            }
            else
            {
                _p = Game.PlayerPed;
            }
        }

        /// <summary>
        /// Writes debug message to client's console.
        /// </summary>
        /// <param name="msg">Message to display.</param>
        private void Log(string msg)
        {
            Debug.Write($"[NoReticle] - {msg}");
        }
    }
}
