using System.Collections.Generic;
using System.Windows.Forms;

namespace Unused_Classes
{
    public partial class Config
    {
        // We use an enum internally but map it to strings of keys used in th config settings. This makes it easier to upgrade the config in the future as we keep
        // enums out of the config XML file which poses challenges if we remove iems later since old config files would generate exceptions since it would
        // contain an undefined enum name.
        public enum Timer
        {
            MaterialReadinessCheckDelay,
            WidgetGenericSettlingPeriod
        }
        public static Dictionary<Timer, string> TimerKeyMap = new Dictionary<Timer, string>() {
            {Timer.MaterialReadinessCheckDelay, "Material Readiness Check Delay"},
            {Timer.WidgetGenericSettlingPeriod, "Widget Settling Delay" }
        };

        public enum BoolTypedOption
        {
            BaitRequired,
            RandomizeTimers,
            ScanAbilitiesOnce,
            CraftingFoodRequired,
            GatheringFoodRequired,
            FishingFoodRequired,
            ForegroundAtWorkEnd,
            ForegroundClientAtWorkStart,
            AutoStartGathering,
        }
        public static Dictionary<BoolTypedOption, string> BoolTypedOptionKeyMap = new Dictionary<BoolTypedOption, string>()
        {
            {BoolTypedOption.BaitRequired, "Bait Required"},
            {BoolTypedOption.RandomizeTimers, "Randomize Timers"},
            {BoolTypedOption.ScanAbilitiesOnce, "Scan Abilities Once"},
            {BoolTypedOption.CraftingFoodRequired, "Crafting Food Required"},
            {BoolTypedOption.GatheringFoodRequired, "Gathering Food Required"},
            {BoolTypedOption.FishingFoodRequired, "Fishing Food Required"},
            {BoolTypedOption.ForegroundAtWorkEnd, "Bring To Front At Work End"},
            {BoolTypedOption.ForegroundClientAtWorkStart, "Bring Game Client To Front At Work Start"},
            {BoolTypedOption.AutoStartGathering, "Auto Start Gathering On Approach"},
        };

        public enum HotKeyTypedOption
        {
            GatheringSynthesis,
            CameraMode,
            MoveForward,
            MoveLeft,
            MoveRight,
            ChatOpen
        }
        public static Dictionary<HotKeyTypedOption, string> HotKeyTypedOptionKeyMap = new Dictionary<HotKeyTypedOption, string>()
        {
            {HotKeyTypedOption.GatheringSynthesis, "Gathering/Synthesis"},
            {HotKeyTypedOption.CameraMode, "Camera Mode"},
            {HotKeyTypedOption.MoveForward, "Forward"},
            {HotKeyTypedOption.MoveLeft, "Left"},
            {HotKeyTypedOption.MoveRight, "Right"},
            {HotKeyTypedOption.ChatOpen, "Chat Open"},
        };


        private static Config _DefaultConfig;
        public static Config Defaults
        {
            get
            {
                if (_DefaultConfig == null) return _DefaultConfig = FromDefaults(false);
                return _DefaultConfig;
            }
        }

        public static Config FromDefaults(bool setOwner)
        {
            // Need to set up defaults
            Config config = new Config();
            
            Config owner = null;
            if (setOwner) owner = config;

            config._TypedOptions = new ConfigTypedOptions();
            config._TypedOptions.Owner = owner;

            config._TypedOptions._KeyMap = new Dictionary<string, Keys>()
            {
                {HotKeyTypedOptionKeyMap[HotKeyTypedOption.GatheringSynthesis], Keys.G},
                {HotKeyTypedOptionKeyMap[HotKeyTypedOption.CameraMode], Keys.V},
                {HotKeyTypedOptionKeyMap[HotKeyTypedOption.MoveForward], Keys.W},
                {HotKeyTypedOptionKeyMap[HotKeyTypedOption.MoveLeft], Keys.A},
                {HotKeyTypedOptionKeyMap[HotKeyTypedOption.MoveRight], Keys.D},
                {HotKeyTypedOptionKeyMap[HotKeyTypedOption.ChatOpen], Keys.Space},
            };


            config._MemoryOffsets = new ConfigMemoryOffsets();
            config._MemoryOffsets.Owner = owner;
            config._MemoryOffsets._Map = new Dictionary<string, ConfigMemoryOffsetInfo>();

            config._MemoryOffsets._Map["CharacterX"] = new ConfigMemoryOffsetInfo(owner, "CharacterX", 0x3C,
                new byte[] {0x00, 0x00, 0x00, 0x00},
                "Offset from the address found from resolving the Character Point of View Information pointer chain to the location where the check value is in memory. The check value is the location in memory storing the character’s current X location. The Check Value value is not significant.");
            config._MemoryOffsets._Map["CharacterY"] = new ConfigMemoryOffsetInfo(owner, "CharacterY", 0x44,
                new byte[] {0x00, 0x00, 0x00, 0x00},
                "Offset from the address found from resolving the Character Point of View Information pointer chain to the location where the check value is in memory. The check value is the location in memory storing the character’s current Y location. The Check Value value is not significant.");
            config._MemoryOffsets._Map["CharacterZ"] = new ConfigMemoryOffsetInfo(owner, "CharacterZ", 0x40,
                new byte[] {0x00, 0x00, 0x00, 0x00},
                "Offset from the address found from resolving the Character Point of View Information pointer chain to the location where the check value is in memory. The check value is the location in memory storing the character’s current Z location. The Check Value value is not significant.");
            config._MemoryOffsets._Map["CharacterHeading"] = new ConfigMemoryOffsetInfo(owner, "CharacterHeading", 0x0,
                new byte[] {0x00, 0x00, 0x00, 0x00},
                "Offset from the address found from resolving the Character Point of View Information pointer chain to the location where the check value is in memory. The check value is the location in memory storing the character’s current heading in radians. The Check Value value is not significant. South is the 0-radian heading. Counter-clockwise from South to North is in the range 0.0 to Pi. Clockwise from South to North is in the range 0.0 to –Pi (negative PI).");
            config._MemoryOffsets._Map["CameraFirstPersonHeading"] = new ConfigMemoryOffsetInfo(owner,
                "CameraFirstPersonHeading", 0x0, new byte[] {0x00, 0x00, 0x00, 0x00},
                "Offset from the address found from resolving the Camera First Person Point of View Information pointer chain to the location where the check value is in memory. The check value is the location in memory storing the first person camera’s current heading in radians. The Check Value value is not significant. South is the 0-radian heading. Counter-clockwise from South to North is in the range 0.0 to Pi. Clockwise from South to North is in the range 0.0 to –Pi (negative PI).");

            config._ApplicationParams = new ConfigApplicationInfo();
            config._ApplicationParams.Owner = owner;

            return config;
        }
    }
}
