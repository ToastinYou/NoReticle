using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace Client
{
    public class Main : BaseScript
    {
        private Ped _p = Game.PlayerPed;

        public Main()
        {
            Log("Resource loaded.");
            Tick += ProcessTask;
        }

        private async Task ProcessTask()
        {
            if (_p != null)
            {
                // gets client's current weapon.
                Weapon w = _p.Weapons.Current;

                if (w != null)
                {
                    // if current weapon group defines weapon as a sniper then..
                    if (w.Group != WeaponGroup.Sniper)
                    {
                        // if ace perm "Reticle" is not allowed (cannot have reticle) then..
                        if (API.IsAceAllowed("Reticle") == false)
                        {
                            Log("Ace 'Reticle' not allowed.");

                            // hides reticle (white dot [HUD] when aiming in).
                            API.HideHudComponentThisFrame(14);
                        }
                        else
                        {
                            Log("Ace 'Reticle' allowed.");
                        }
                    }
                }
            }
            else
            {
                Log("Player ped does not exist.. ?");
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
