﻿using System.Collections.Generic;

namespace NoReticle.Shared
{
    public class Shared
    {
        public enum KeyEnum
        {
            Reticle,
            ReticleStunGun,
            NoReticleMenu,
        }

        public static AcePermission Reticle = new AcePermission(KeyEnum.Reticle, true); // Default (false): Hide Reticle.
        public static AcePermission ReticleStunGun = new AcePermission(KeyEnum.ReticleStunGun, false); // Default (true): Show Reticle for Stun Gun.
        public static AcePermission NoReticleMenu = new AcePermission(KeyEnum.NoReticleMenu, true); // Default (false): Disallow access to the menu.

        public static IEnumerable<AcePermission> AcePermissions = new List<AcePermission>()
        {
            Reticle,
            ReticleStunGun,
            NoReticleMenu
        };

        public class AcePermission
        {
            public bool Value { get; set; }
            public bool DefaultValue { get; private set; }
            public string Key { get; private set; }
            private KeyEnum _keyEnum;

            public AcePermission(KeyEnum keyEnum, bool value)
            {
                Value = value;
                DefaultValue = value;
                Key = keyEnum.ToString();
                _keyEnum = keyEnum;
            }
        }

        public static class Constants
        {
            public const string Prefix = "[NoReticle]:";
        }
    }
}