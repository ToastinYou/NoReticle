using CitizenFX.Core;
using CitizenFX.Core.UI;
using MenuAPI;
using System;
using System.Collections.Generic;
using static NoReticle.Client.ClientLog;

namespace NoReticle.Client
{
    public class ConfigurationsMenu
    {
        // Create the main menu.
        private static readonly Menu Menu = new("NoReticle", "Configuration Menu");

        public static void Initialize()
        {
            MenuItem giveAllWeaponsCommandConfiguration = null;
            MenuItem hideAircraftReticleConfiguration = null;
            bool menuInitialized = false;

            Menu.OnMenuOpen += (sender) =>
            {
                // Wait for menu's first open to add menu items because the Configurations need time to be read.
                if (!menuInitialized)
                {
                    menuInitialized = true;

                    giveAllWeaponsCommandConfiguration = new MenuListItem("Give All Weapons Command", new List<string> { "Disabled", "Enabled" }, Configurations.EnableGiveAllWeaponsCommand ? 1 : 0, "Enable/Disable /weapon command for all players.");
                    Menu.AddMenuItem(giveAllWeaponsCommandConfiguration);

                    hideAircraftReticleConfiguration = new MenuListItem("Aircraft Reticle", new List<string> { "Visible", "Hidden" }, Configurations.HideAircraftReticle ? 1 : 0, "Enable/Disable hiding of the aircraft reticle for all players.");
                    Menu.AddMenuItem(hideAircraftReticleConfiguration);
                }

                Trace($"Opened {sender.MenuTitle} Menu.");
            };

            Menu.OnMenuClose += (sender) =>
            {
                MenuController.DisableMenuButtons = true; // Prevent menu from opening via keypress.
            };

            Menu.OnListIndexChange += (menu, listItem, previousIndex, currentIndex, realIndex) =>
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

            MenuController.AddMenu(Menu);
            MenuController.MainMenu = Menu;
            MenuController.DisableMenuButtons = true; // Prevent menu from opening via keypress.
        }

        public static void OpenMenu()
        {
            MenuController.DisableMenuButtons = false; // Allow movement within menu via keypress.
            Menu.OpenMenu();
            Trace($"Opened {Menu.MenuTitle} Menu.");
        }
    }
}
