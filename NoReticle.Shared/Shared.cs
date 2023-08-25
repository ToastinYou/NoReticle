using System.Collections.Generic;

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

        public static AcePermission Reticle = new AcePermission(KeyEnum.Reticle, true);
        public static AcePermission ReticleStunGun = new AcePermission(KeyEnum.ReticleStunGun, true);
        public static AcePermission NoReticleMenu = new AcePermission(KeyEnum.NoReticleMenu, true);

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
