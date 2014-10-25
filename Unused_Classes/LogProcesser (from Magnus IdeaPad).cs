using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagBot_FFXIV
{
    public class LogProcesser
    {
        [FlagsAttribute]
        public enum QueueType
        {
            None = 0,
            Work = 1,
            Chat = 2,
        }

        public enum EndReason
        {
            None,
            Completed,
            Stopped,
            Finished,
            Aborted,
            Errored,
            Timeout,
        }

        public enum Perspective
        {
            Unknown,
            Sender,
            Receiver,
        }

        public enum LogEntryType
        {
            Invalid,

            ChatSay,
            ChatTellReceived,
            ChatTellSent,
            ChatShout,
            ChatParty,
            ChatLinkshell1,
            ChatLinkshell2,
            ChatLinkshell3,
            ChatLinkshell4,
            ChatLinkshell5,
            ChatLinkshell6,
            ChatLinkshell7,
            ChatLinkshell8,

            FishNoBite,
            FishBigBite,
            FishBite,
            FishNibble,
            FishCaught,
            FishGotAway,
            FishHooked,
            FishJigBetter,
            FishJigWorse,
            FishLost,
            FishTired,
            FishPackEmpty,
            FishPackFilled,
            FishPoolEmpty,
            GatheringFinished,
            FishNoBait,
            YouGetXp,
            YouObtain,
            YouCreate,
            EquipPackFail,
            NoCrystal,
            FingerprintsOfTheGodsActivated,
            FingerprintsOfTheGodsDeactivated,
            YouBotch,
            GatherCloseTo,
            GatherNoHint,
            GatherWorse,
            GatherBetter,
            GatherSuccess,
            GatherFail,
            GatherDamages,
            GatherAttemptsLeft,
            GatherNodeExhausted,
            GatherNodeComplete,

            OrbStateWhite,
            OrbStateYellow,
            OrbStateRed,
            OrbStateRainbow,
            ElementUnstableWind,
            ElementUnstableWater,
            ElementUnstableLightning,
            ElementUnstableFire,
            ElementUnstableEarth,
            ElementUnstableIce,
            ElementStabilized,

            ProgressIncreases,
            ProgressDecreases,
            DurabilityIncreases,
            DurabilityDecreases,
            QualityIncreases,
            QualityDecreases,

            AbilityFades,

            DamageDealtBySelf,
            DamageDealtByParty,

            FoodEffectOn,
            FoodEffectOff,
            UseFoodFail,
            UseFoodSuccess,
        }

        public static Dictionary<LogEntryType, string> LogEntryTypeKeyMap = new Dictionary<LogEntryType, string>()
        {            
            {LogProcesser.LogEntryType.Invalid, ""},
            
            {LogProcesser.LogEntryType.ChatSay, "Chat Say"},
            {LogProcesser.LogEntryType.ChatTellReceived, "Chat Tell A"},
            {LogProcesser.LogEntryType.ChatTellSent, "Chat Tell B"},
            {LogProcesser.LogEntryType.ChatShout, "Chat Shout"},
            {LogProcesser.LogEntryType.ChatParty, "Chat Party"},
            {LogProcesser.LogEntryType.ChatLinkshell1, "Chat Linkshell 1"},
            {LogProcesser.LogEntryType.ChatLinkshell2, "Chat Linkshell 2"},
            {LogProcesser.LogEntryType.ChatLinkshell3, "Chat Linkshell 3"},
            {LogProcesser.LogEntryType.ChatLinkshell4, "Chat Linkshell 4"},
            {LogProcesser.LogEntryType.ChatLinkshell5, "Chat Linkshell 5"},
            {LogProcesser.LogEntryType.ChatLinkshell6, "Chat Linkshell 6"},
            {LogProcesser.LogEntryType.ChatLinkshell7, "Chat Linkshell 7"},
            {LogProcesser.LogEntryType.ChatLinkshell8, "Chat Linkshell 8"},

            {LogProcesser.LogEntryType.FishNoBite, "Fish No Bite"},
            {LogProcesser.LogEntryType.FishBigBite, "Fish Big Bite"},
            {LogProcesser.LogEntryType.FishBite, "Fish Bite"},
            {LogProcesser.LogEntryType.FishNibble, "Fish Nibble"},
            {LogProcesser.LogEntryType.FishCaught, "Fish Caught"},
            {LogProcesser.LogEntryType.FishGotAway, "FIsh Got Away"},
            {LogProcesser.LogEntryType.FishHooked, "Fish Hooked"},
            {LogProcesser.LogEntryType.FishJigBetter, "Fish Jig Better"},
            {LogProcesser.LogEntryType.FishJigWorse, "Fish Jig Worse"},
            {LogProcesser.LogEntryType.FishLost, "Fish Lost"},
            {LogProcesser.LogEntryType.FishTired, "Fish Tired"},
            {LogProcesser.LogEntryType.FishPackEmpty, "Fish Pack Empty"},
            {LogProcesser.LogEntryType.FishPackFilled, "Fish Pack Filled"},
            {LogProcesser.LogEntryType.FishPoolEmpty, "Fish Pool Empty"},
            {LogProcesser.LogEntryType.GatheringFinished, "Gathering Finished"},
            {LogProcesser.LogEntryType.FishNoBait, "Fish No Bait"},
            {LogProcesser.LogEntryType.YouGetXp, "You Get XP"},
            {LogProcesser.LogEntryType.YouObtain, "You Obtain Item"},
            {LogProcesser.LogEntryType.YouCreate, "You Create Item"},
            {LogProcesser.LogEntryType.EquipPackFail, "Equip Pack Fail"},
            {LogProcesser.LogEntryType.NoCrystal, "No Crystal"},
            {LogProcesser.LogEntryType.FingerprintsOfTheGodsActivated, "FotG Activated"},
            {LogProcesser.LogEntryType.FingerprintsOfTheGodsDeactivated, "FotG Deactivated"},
            {LogProcesser.LogEntryType.YouBotch, "You Botch"},
            {LogProcesser.LogEntryType.GatherCloseTo, "Gather Close To"},
            {LogProcesser.LogEntryType.GatherNoHint, "Gahter No Hint"},
            {LogProcesser.LogEntryType.GatherWorse, "Gather Worse"},
            {LogProcesser.LogEntryType.GatherBetter, "Gather Better"},
            {LogProcesser.LogEntryType.GatherSuccess, "Gather Success"},
            {LogProcesser.LogEntryType.GatherFail, "Gather Fail"},
            {LogProcesser.LogEntryType.GatherDamages, "Gather Damages"},
            {LogProcesser.LogEntryType.GatherAttemptsLeft, "Gather Attempts Left"},
            {LogProcesser.LogEntryType.GatherNodeExhausted, "Gather Node Exhausted"},
            {LogProcesser.LogEntryType.GatherNodeComplete, "Gather Node Complete"},

            {LogProcesser.LogEntryType.OrbStateWhite, "Orb State White"},
            {LogProcesser.LogEntryType.OrbStateYellow, "Orb State Yellow"},
            {LogProcesser.LogEntryType.OrbStateRed, "Orb State Red"},
            {LogProcesser.LogEntryType.OrbStateRainbow, "Orb State Rainbow"},
            {LogProcesser.LogEntryType.ElementUnstableWind, "Element Unstable Wind"},
            {LogProcesser.LogEntryType.ElementUnstableWater, "Element Unstable Water"},
            {LogProcesser.LogEntryType.ElementUnstableLightning, "Element Unstable Lightning"},
            {LogProcesser.LogEntryType.ElementUnstableFire, "Element Unstable Fire"},
            {LogProcesser.LogEntryType.ElementUnstableEarth, "Element Unstable Earth"},
            {LogProcesser.LogEntryType.ElementUnstableIce, "Element Unstable Ice"},
            {LogProcesser.LogEntryType.ElementStabilized, "Element Stabilized"},

            {LogProcesser.LogEntryType.ProgressIncreases, "Progress Increases"},
            {LogProcesser.LogEntryType.ProgressDecreases, "Progress Decreases"},
            {LogProcesser.LogEntryType.DurabilityIncreases, "Durability Increases"},
            {LogProcesser.LogEntryType.DurabilityDecreases, "Durability Decreases"},
            {LogProcesser.LogEntryType.QualityIncreases, "Quality Increases"},
            {LogProcesser.LogEntryType.QualityDecreases, "Quality Decreases"},

            {LogProcesser.LogEntryType.AbilityFades, "Ability Fades"},

            {LogProcesser.LogEntryType.DamageDealtBySelf, "Damage Dealt By Self"},
            {LogProcesser.LogEntryType.DamageDealtByParty, "Damage Dealt By Party"},

            {LogProcesser.LogEntryType.FoodEffectOff, "Food Effect Off"},
            {LogProcesser.LogEntryType.FoodEffectOn, "Food Effect On"},
            {LogProcesser.LogEntryType.UseFoodFail, "Use Food Fail"},
            {LogProcesser.LogEntryType.UseFoodSuccess, "Use Food Success"},
        };

        public class LogQueueEntry
        {
            public LogEntryType LogType = LogEntryType.Invalid;
            public string CategoryCode = "";
            public QueueType TargetQueues = QueueType.None;
            public DateTime TimeReceived = DateTime.Now;
            public Perspective Perspective = Perspective.Unknown;
            public string CharacterName = "";
            public string MessagePrefix = ""; // Prefix is set in the configuration settings. used mostly to identify what linkshell a message is related to
            public string Message = ""; // Log entry text, less the category code portion.
            public string VariableSubstring = ""; // Contains the variable part of the log entry as a string
            public int VariableInt = 0; // Contains an int value of interest from the string, e.g., exp gained, fished up count, etc.
        }
    }
}
