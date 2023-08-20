using CitizenFX.Core;
using CitizenFX.Core.UI;
using MenuAPI;
using System;
using System.Collections.Generic;

namespace NoReticle.Client
{
    public class ConfigurationsMenu
    {
        private const string MessagePrefix = "[NoReticle]:";
        private const Control MenuToggleKey = Control.SelectWeaponUnarmed;

        public static void Initialize()
        {
            // Create the main menu.
            Menu menu = new("NoReticle", "Configuration Menu");
            MenuItem giveAllWeaponsCommandConfiguration = null;
            MenuItem hideAircraftReticleConfiguration = null;
            bool menuInitialized = false;

            menu.OnMenuOpen += (sender) =>
            {
                // Wait for menu's first open to add menu items because the Configurations need time to be read.
                if (!menuInitialized)
                {
                    menuInitialized = true;

                    giveAllWeaponsCommandConfiguration = new MenuListItem("Give All Weapons Command", new List<string> { "Disabled", "Enabled" }, Configurations.EnableGiveAllWeaponsCommand ? 1 : 0, "Enable/Disable /weapon command for all players.");
                    menu.AddMenuItem(giveAllWeaponsCommandConfiguration);

                    hideAircraftReticleConfiguration = new MenuListItem("Aircraft Reticle", new List<string> { "Visible", "Hidden" }, Configurations.HideAircraftReticle ? 1 : 0, "Enable/Disable hiding of the aircraft reticle for all players.");
                    menu.AddMenuItem(hideAircraftReticleConfiguration);
                }
            };

            menu.OnListIndexChange += (menu, listItem, previousIndex, currentIndex, realIndex) =>
            {
                if (giveAllWeaponsCommandConfiguration is { Selected: true })
                {
                    bool value = Convert.ToBoolean(currentIndex);
                    Configurations.Update(Configurations.KeyEnableGiveAllWeaponsCommand, value, false);

                    string result = value ? "Enabled" : "Disabled";
                    Screen.ShowNotification($"{MessagePrefix} {result} give all weapons command, /weapons.");
                }
                else if (hideAircraftReticleConfiguration is { Selected: true })
                {
                    bool value = Convert.ToBoolean(currentIndex);
                    Configurations.Update(Configurations.KeyHideAircraftReticle, value, false);

                    string result = value ? "hidden" : "visible";
                    Screen.ShowNotification($"{MessagePrefix} Aircraft reticle {result}.");
                }
            };

            MenuController.AddMenu(menu);
            MenuController.MainMenu = menu;
            MenuController.MenuToggleKey = MenuToggleKey;
        }
    }
}
