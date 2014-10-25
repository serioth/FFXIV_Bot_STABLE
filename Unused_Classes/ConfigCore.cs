using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Unused_Classes
{
    public partial class Config
    {

        private IDictionary<string, Skill> _skillList = new Dictionary<string, Skill>();
        public IDictionary<string, Skill> SkillList
        {
            get { return _skillList; }
            private set { _skillList = value; }
        }

        private void LoadSKills(string path)
        {
            XElement xelement = XElement.Load(path);

        }


        [DataMember(Name = "ConfigurationParameters")]
        internal ConfigurationInfo _ConfigParams;
        public ConfigurationInfo ConfigParams { get { return _ConfigParams; } }

        [DataMember(Name = "TypedOptions")]
        internal ConfigTypedOptions _TypedOptions;
        public ConfigTypedOptions TypedOptions { get { return _TypedOptions; } }

        [DataMember(Name = "MemorySignatures")]
        internal ConfigMemoryMarkers _MemoryMarkers;
        public ConfigMemoryMarkers MemoryMarkers { get { return _MemoryMarkers; } }

        [DataMember(Name = "MemoryChains")]
        internal ConfigMemoryChains _MemoryChains;
        public ConfigMemoryChains MemoryChains { get { return _MemoryChains; } }

        [DataMember(Name = "MemoryOffsets")]
        internal ConfigMemoryOffsets _MemoryOffsets;
        public ConfigMemoryOffsets MemoryOffsets { get { return _MemoryOffsets; } }

        [DataMember(Name = "Abilities")]
        internal ConfigAbilities _Abilities;
        public ConfigAbilities Abilities { get { return _Abilities; } }

        [DataMember(Name = "Timers")]
        internal ConfigTimers _Timers;
        public ConfigTimers Timers { get { return _Timers; } }

        [DataMember(Name = "ApplicationParameters")]
        internal ConfigApplicationInfo _ApplicationParams;
        public ConfigApplicationInfo ApplicationParams { get { return _ApplicationParams; } }

        // Properties
        private string _Path = "";
        public string Path { get { return _Path; } set { _Path = value; } }
        private bool _IsDirty = false;
        internal bool IsDirty { get { return _IsDirty; } set { _IsDirty = value; /*if (_IsDirty && !_BufferChanges) SaveXML();*/ } }

        private bool _BufferChanges = false;
        public bool BufferChanges { get { return _BufferChanges; } set { _BufferChanges = value; /*if (!_BufferChanges && IsDirty) SaveXML();*/ } }

        //public bool Prune(ConfigDataOptions.OptionSettings settingOptions)
        //{
        //    if (!settingOptions.OptionMap[ConfigDataOptions.Options.AbilityOptions]) _Abilities = null;
        //    if (!settingOptions.OptionMap[ConfigDataOptions.Options.MemoryChains]) _MemoryChains = null;
        //    if (!settingOptions.OptionMap[ConfigDataOptions.Options.MemoryMarkers]) _MemoryMarkers = null;
        //    if (!settingOptions.OptionMap[ConfigDataOptions.Options.MemoryOffsets]) _MemoryOffsets = null;
        //    if (!settingOptions.OptionMap[ConfigDataOptions.Options.TypedOptions]) _TypedOptions = null;
        //}

        public void CopyFrom(Config updatedSettings)
        {
            if (_Abilities == null) _Abilities = new ConfigAbilities();
            if (updatedSettings.Abilities != null)
            {
                Abilities._Map.Clear();
                foreach (KeyValuePair<string, ConfigAbilityInfo> pair in updatedSettings.Abilities._Map)
                    Abilities._Map[pair.Key] = ConfigAbilityInfo.CreateFrom(this, pair.Value);
            }
            _Abilities._LabelMaxLength = -1;

            if (_MemoryChains == null) _MemoryChains = new ConfigMemoryChains();
            if (updatedSettings.MemoryChains != null)
            {
                MemoryChains._Map.Clear();
                foreach (KeyValuePair<string, ConfigMemoryChainInfo> pair in updatedSettings.MemoryChains._Map)
                    MemoryChains._Map[pair.Key] = ConfigMemoryChainInfo.CreateFrom(this, pair.Value);
            }
            if (_MemoryMarkers == null) _MemoryMarkers = new ConfigMemoryMarkers();
            if (updatedSettings.MemoryMarkers != null)
            {
                MemoryMarkers._Map.Clear();
                foreach (KeyValuePair<string, ConfigMemoryMarkerInfo> pair in updatedSettings.MemoryMarkers._Map)
                    MemoryMarkers._Map[pair.Key] = pair.Value.Clone();
            }
            if (_MemoryOffsets == null) _MemoryOffsets = new ConfigMemoryOffsets();
            if (updatedSettings.MemoryOffsets != null)
            {
                MemoryOffsets._Map.Clear();
                foreach (KeyValuePair<string, ConfigMemoryOffsetInfo> pair in updatedSettings.MemoryOffsets._Map)
                    MemoryOffsets._Map[pair.Key] = ConfigMemoryOffsetInfo.CreateFrom(this, pair.Value);
            }

            if (_TypedOptions == null) _TypedOptions = new ConfigTypedOptions();
            if (updatedSettings.TypedOptions != null)
            {
                if (updatedSettings.TypedOptions._BoolMap != null && updatedSettings.TypedOptions._BoolMap.Count > 0)
                {
                    TypedOptions._BoolMap.Clear();
                    foreach (KeyValuePair<string, bool> pair in updatedSettings.TypedOptions._BoolMap)
                        TypedOptions._BoolMap[pair.Key] = pair.Value;
                }
                if (updatedSettings.TypedOptions._IntMap != null && updatedSettings.TypedOptions._IntMap.Count > 0)
                {
                    TypedOptions._IntMap.Clear();
                    foreach (KeyValuePair<string, int> pair in updatedSettings.TypedOptions._IntMap)
                        TypedOptions._IntMap[pair.Key] = pair.Value;
                }
                if (updatedSettings.TypedOptions._FloatMap != null && updatedSettings.TypedOptions._FloatMap.Count > 0)
                {
                    TypedOptions._FloatMap.Clear();
                    foreach (KeyValuePair<string, float> pair in updatedSettings.TypedOptions._FloatMap)
                        TypedOptions._FloatMap[pair.Key] = pair.Value;
                }
                if (updatedSettings.TypedOptions._StringMap != null && updatedSettings.TypedOptions._StringMap.Count > 0)
                {
                    TypedOptions._StringMap.Clear();
                    foreach (KeyValuePair<string, string> pair in updatedSettings.TypedOptions._StringMap)
                        TypedOptions._StringMap[pair.Key] = pair.Value;
                }
                if (updatedSettings.TypedOptions._KeyMap != null && updatedSettings.TypedOptions._KeyMap.Count > 0)
                {
                    TypedOptions._KeyMap.Clear();
                    foreach (KeyValuePair<string, Keys> pair in updatedSettings.TypedOptions._KeyMap)
                        TypedOptions._KeyMap[pair.Key] = pair.Value;
                }
            }
        }

        public bool ConformConfig(Config defaultConfig)
        {
            Config owner = this;
            bool madeDirty = false;

            if (_TypedOptions == null) _TypedOptions = new ConfigTypedOptions();
            _TypedOptions.Owner = owner;
            if (_TypedOptions.CleanUp(defaultConfig)) madeDirty = true;
            
            return true;
        }

        public static Config Load(string path)
        {
            Config config;
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                config = new Config();
                config.ConformConfig(Config.Defaults);
                return config;
            }

            FileStream stream;
            XmlDictionaryReader reader;
            DataContractSerializer dcs;

            try { stream = new FileStream(path, FileMode.Open); }
            catch { return null; }

            try { reader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas()); }
            catch { stream.Close(); return null; }

            try { dcs = new DataContractSerializer(typeof(Config)); }
            catch { reader.Close(); stream.Close(); return null; }

            try { config = (Config)dcs.ReadObject(reader, true); }
            catch { reader.Close(); stream.Close(); return null; }

            reader.Close();
            stream.Close();

            // Since loaded okay, save the path with the configuration data
            config.Path = path;

            // Need to make sure missing items are set to defaults
            config.ConformConfig(Config.Defaults);

            return config;
        }
        public bool SaveXML()
        {
            if (string.IsNullOrEmpty(_Path)) return false;

            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(_Path)))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(_Path));
                }
                catch
                {
                    return false;
                }
            }

            if (File.Exists(_Path))
            {
                try
                {
                    File.Delete(_Path);
                }
                catch (Exception err)
                {
                    Globals.Instance.ShowMessage("Unable to save settings: " + err.Message);
                    return false;
                }
            }

            bool savedFlag = true;

            // Always update the timestamp when saving.
            if (_ConfigParams == null)
            {
                _ConfigParams = new ConfigurationInfo();
                _ConfigParams.CopyFrom(Config.Defaults._ConfigParams);
            }
            DateTime nowTime = DateTime.Now;
            _ConfigParams._Timestamp = nowTime.Ticks;

            FileStream stream;
            try
            {
                stream = new FileStream(_Path, FileMode.OpenOrCreate);
            }
            catch (Exception err)
            {
                Globals.Instance.ShowMessage("Unable to create settings: " + err.Message);
                return false;
            }
            DataContractSerializer dcs = new DataContractSerializer(typeof(Config));
            try { dcs.WriteObject(stream, this); }
            catch { savedFlag = false; }

            stream.Close();

            IsDirty = !savedFlag;

            // Note: We do not modify the BufferChanges flag as we could still be buffering even after a save
            return true;
        }
    }

    [DataContract(Name = "ConfigurationInfo")]
    public class ConfigurationInfo : ConfigBase
    {
        public ConfigurationInfo() { }

        [DataMember(Name = "Version")]
        internal string _Version = Globals.ConfigVersion;

        [DataMember(Name = "UpdatedTimestamp")]
        internal long _UpdatedTimestamp = -1;
        public long UpdatedTimestamp { get { return _UpdatedTimestamp; } set { if (_UpdatedTimestamp == value) return; _UpdatedTimestamp = value; Dirtied(); } }

        [DataMember(Name = "Timestamp")]
        internal long _Timestamp = -1;
        public long Timestamp { get { return _Timestamp; } set { _Timestamp = value; } }

        public void CopyFrom(ConfigurationInfo copyFrom)
        {
            _Version = copyFrom._Version;
            _Timestamp = copyFrom._Timestamp;
            _UpdatedTimestamp = copyFrom._UpdatedTimestamp;
        }
    }

    [DataContract(Name = "ApplicationInfo")]
    public class ConfigApplicationInfo : ConfigBase
    {
        [DataMember(Name = "Size")]
        private Size _GuiSize = new Size(0, 0);

        [DataMember(Name = "Location")]
        private Point _GuiLocation = new Point(-1, -1);

        [DataMember(Name = "Version")]
        private string _Version = Globals.Version;

        public ConfigApplicationInfo() { }

        public Size GuiSize { get { return _GuiSize; } set { if (!_GuiSize.Equals(value)) { _GuiSize = value; Dirtied(); } } }
        public Point GuiLocation { get { return _GuiLocation; } set { if (!_GuiLocation.Equals(value)) { _GuiLocation = value; Dirtied(); } } }
        public string Version { get { return _Version; } }
    }


    [DataContract(Name = "TypedOptions")]
    public class ConfigTypedOptions : ConfigBase
    {
        [DataMember(Name = "BoolMap")]
        internal Dictionary<string, bool> _BoolMap = new Dictionary<string, bool>();

        [DataMember(Name = "IntMap")]
        internal Dictionary<string, int> _IntMap = new Dictionary<string, int>();

        [DataMember(Name = "FloatMap")]
        internal Dictionary<string, float> _FloatMap = new Dictionary<string, float>();

        [DataMember(Name = "StringMap")]
        internal Dictionary<string, string> _StringMap = new Dictionary<string, string>();

        [DataMember(Name = "KeyMap")]
        internal Dictionary<string, Keys> _KeyMap = new Dictionary<string, Keys>();

        public ConfigTypedOptions() { }

        // Methods
        public string[] BoolKeys() { return _BoolMap.Keys.ToArray(); }
        public bool GetBool(string key) { if (!_BoolMap.ContainsKey(key)) return false; return _BoolMap[key]; }
        public bool SetBool(string key, bool value) { if (!_BoolMap.ContainsKey(key)) return false; _BoolMap[key] = value; if (_Owner != null) _Owner.IsDirty = true; return true; }

        internal bool CleanUp(Config defaultConfig)
        {
            bool madeDirty = false;

            // Convert old values to new ones in case we support a label change between versions

            // Remove the unrecognized ones now that we have converted old to new labels
            List<string> keysToDelete = new List<string>();
            foreach (string s in _BoolMap.Keys) if (!defaultConfig._TypedOptions._BoolMap.ContainsKey(s)) keysToDelete.Add(s);
            foreach (string s in keysToDelete) { _BoolMap.Remove(s); madeDirty = true; }
            keysToDelete.Clear();
            foreach (string s in _FloatMap.Keys) if (!defaultConfig._TypedOptions._FloatMap.ContainsKey(s)) keysToDelete.Add(s);
            foreach (string s in keysToDelete) { _FloatMap.Remove(s); madeDirty = true; }
            keysToDelete.Clear();
            foreach (string s in _IntMap.Keys) if (!defaultConfig._TypedOptions._IntMap.ContainsKey(s)) keysToDelete.Add(s);
            foreach (string s in keysToDelete) { _IntMap.Remove(s); madeDirty = true; }
            keysToDelete.Clear();
            foreach (string s in _StringMap.Keys) if (!defaultConfig._TypedOptions._StringMap.ContainsKey(s)) keysToDelete.Add(s);
            foreach (string s in keysToDelete) { _StringMap.Remove(s); madeDirty = true; }
            keysToDelete.Clear();
            foreach (string s in _KeyMap.Keys) if (!defaultConfig._TypedOptions._KeyMap.ContainsKey(s)) keysToDelete.Add(s);
            foreach (string s in keysToDelete) { _KeyMap.Remove(s); madeDirty = true; }

            // Add in the missing items now that we have cleaned out the unrecognized ones
            foreach (KeyValuePair<string, bool> pair in defaultConfig._TypedOptions._BoolMap)
            {
                if (!_BoolMap.ContainsKey(pair.Key.ToString()))
                {
                    _BoolMap.Add(pair.Key, pair.Value); madeDirty = true;
                }
            }
            foreach (KeyValuePair<string, float> pair in defaultConfig._TypedOptions._FloatMap)
            {
                if (!_FloatMap.ContainsKey(pair.Key))
                {
                    _FloatMap.Add(pair.Key, pair.Value); madeDirty = true;
                }
            }
            foreach (KeyValuePair<string, int> pair in defaultConfig._TypedOptions._IntMap)
            {
                if (!_IntMap.ContainsKey(pair.Key))
                {
                    _IntMap.Add(pair.Key, pair.Value); madeDirty = true;
                }
            }
            foreach (KeyValuePair<string, string> pair in defaultConfig._TypedOptions._StringMap)
            {
                if (!_StringMap.ContainsKey(pair.Key))
                {
                    _StringMap.Add(pair.Key, pair.Value); madeDirty = true;
                }
            }
            foreach (KeyValuePair<string, Keys> pair in defaultConfig._TypedOptions._KeyMap)
            {
                if (!_KeyMap.ContainsKey(pair.Key))
                {
                    _KeyMap.Add(pair.Key, pair.Value); madeDirty = true;
                }
            }

            return madeDirty;
        }
    }

    [DataContract(Name = "TimerInfo")]
    public class ConfigTimerInfo : ConfigBase
    {
        [DataMember(Name = "Duration")]
        internal int _Duration = 0;

        [DataMember(Name = "RandomPercent")]
        internal int _RandomPercent = 0;

        [DataMember(Name = "Description")]
        internal string _Description = "";
        public string Description { get { return _Description; } }

        public ConfigTimerInfo() { }
        public ConfigTimerInfo(Config owner, int duration, int randomPercent, string description)
        {
            Owner = owner;
            _Duration = duration;
            _RandomPercent = randomPercent;
            _Description = description;
        }

        // Properties
        public int Duration
        {
            get
            {
                int duration = _Duration;
                if (RandomPercent > 0 && Globals.Instance.Settings.TypedOptions.GetBool(Config.BoolTypedOptionKeyMap[Config.BoolTypedOption.RandomizeTimers]))
                {
                    double working = (100.0d + RandomPercent) / 100.0d;
                    duration = Utils.getRandom(duration, (int)(duration * working));
                }
                return duration;
            }
            set { if (_Duration != value) { _Duration = value; if (_Owner != null) _Owner.IsDirty = true; } }
        }
        public int RandomPercent { get { return _RandomPercent; } set { if (_RandomPercent != value) { _RandomPercent = value; if (_Owner != null) _Owner.IsDirty = true; } } }

        // Methods
        // Copies all fields then does a single save which is more efficient than user setting each field invidually resulting in multiple save attempts.
        public void CopyFrom(ConfigTimerInfo infoToCopy)
        {
            if (infoToCopy == null) return;

            bool isDirty = false;
            if (_Duration != infoToCopy._Duration)
            {
                _Duration = infoToCopy._Duration;
                isDirty = true;
            }
            if (_RandomPercent != infoToCopy._RandomPercent)
            {
                _RandomPercent = infoToCopy._RandomPercent;
                isDirty = true;
            }
            if (_Description != infoToCopy._Description)
            {
                _Description = infoToCopy._Description;
                isDirty = true;
            }
            _Owner = infoToCopy._Owner;

            if (isDirty && _Owner != null) _Owner.IsDirty = isDirty;
        }
        public static ConfigTimerInfo CreateFrom(Config owner, ConfigTimerInfo infoToCopy)
        {
            ConfigTimerInfo item = new ConfigTimerInfo();
            item.Owner = owner;
            if (owner != null) owner.IsDirty = true;
            if (infoToCopy == null) return item;
            item.CopyFrom(infoToCopy);
            return item;
        }
    }

    [DataContract(Name = "Timers")]
    public class ConfigTimers : ConfigBase
    {
        [DataMember(Name = "Map")]
        public Dictionary<string, ConfigTimerInfo> _Map = new Dictionary<string, ConfigTimerInfo>();

        public ConfigTimers() { }

        // Properties
        public ConfigTimerInfo this[string key] { get { return _Map[key]; } }
        public string[] Keys { get { return _Map.Keys.ToArray(); } }
        public bool Exists(string key) { return _Map.ContainsKey(key); }

        internal bool CleanUp(Config defaultConfig)
        {
            bool madeDirty = false;

            // See if there are any labels we need to convert from old value to a new value

            // Remove any unrecognized now as it is a bit more efficient to remove first
            List<string> keysToDelete = new List<string>();
            foreach (string s in _Map.Keys) if (!defaultConfig._Timers._Map.ContainsKey(s)) keysToDelete.Add(s);
            foreach (string s in keysToDelete) { _Map.Remove(s); madeDirty = true; }

            // Add in any missing items
            foreach (KeyValuePair<string, ConfigTimerInfo> pair in defaultConfig._Timers._Map)
            {
                if (!_Map.ContainsKey(pair.Key))
                {
                    _Map.Add(pair.Key, ConfigTimerInfo.CreateFrom(Owner, pair.Value)); madeDirty = true;
                }
                pair.Value.Owner = Owner;
            }
            return madeDirty;
        }
    }

    [DataContract(Name = "AbilityInfo")]
    public class ConfigAbilityInfo : IEnumerable
    {
        // Will eventually need members for: Next Ability In Series and flags for: isHQ, isSavior, ...

        public ConfigAbilityInfo() { }
        public ConfigAbilityInfo(Config owner, string name, int rounds, bool alwaysAvailable, bool overwrites, bool extends, string description, List<int> codes)
        {
            _Owner = owner;
            _Name = name;
            _Rounds = rounds;
            _Overwrites = overwrites;
            _Extends = extends;
            _AlwaysAvailable = alwaysAvailable;
            if (!string.IsNullOrEmpty(description)) _Description = description;

            if (codes != null) _Codes.AddRange(codes);
        }

        // Properties         NOTE: If adding data members be sure to update the CopyFrom method
        public int this[int index]
        {
            get { if (index < 0 || index >= _Codes.Count) return 0; return _Codes[index]; }
            set { if (index < 0 || index >= _Codes.Count) return; if (_Codes[index] == value) return; _Codes[index] = value; Dirtied(); }
        }
        public bool Add(int code) { if (_Codes.Contains(code)) return false; _Codes.Add(code); Dirtied(); return true; }
        public bool Add(List<int> codes) { if (codes == null || codes.Count < 1) return false; foreach (int code in codes) { if (!_Codes.Contains(code)) _Codes.Add(code); } Dirtied(); return true; }
        public bool Delete(int code) { if (!_Codes.Contains(code)) return false; _Codes.Remove(code); Dirtied(); return true; }
        public bool Clear() { _Codes.Clear(); Dirtied(); return true; }

        [DataMember(Name = "Name")]
        internal string _Name = "";
        public string Name { get { return _Name; } }

        [DataMember(Name = "Codes")]
        internal List<int> _Codes = new List<int>(); // Game client in memory code for this ability

        [DataMember(Name = "Rounds")]
        internal int _Rounds = 1;
        public int Rounds { get { return _Rounds; } set { if (_Rounds != value) { _Rounds = value; if (_Owner != null) _Owner.IsDirty = true; } } }

        [DataMember(Name = "Overwrites")]
        internal bool _Overwrites = true;
        public bool Overwrites { get { return _Overwrites; } set { if (_Overwrites != value) { _Overwrites = value; if (_Owner != null) _Owner.IsDirty = true; } } }

        [DataMember(Name = "Extends")]
        internal bool _Extends = true;
        public bool Extends { get { return _Extends; } set { if (_Extends != value) { _Extends = value; if (_Owner != null) _Owner.IsDirty = true; } } }

        [DataMember(Name = "Description")]
        internal string _Description = "";
        public string Description { get { return _Description; } set { if (_Description == value) return; _Description = value; Dirtied(); } }

        [DataMember(Name = "AlwaysAvailable")]
        internal bool _AlwaysAvailable = false;
        public bool AlwaysAvailable { get { return _AlwaysAvailable; } set { if (_AlwaysAvailable == value) return; _AlwaysAvailable = value; Dirtied(); } }

        // Methods
        public bool ContainsCode(int code)
        {
            foreach (int x in _Codes) if (x == code) return true;
            return false;
        }

        // Copies all fields then does a single save which is more efficient than user setting each field invidually resulting in multiple save attempts.
        public void CopyFrom(ConfigAbilityInfo infoToCopy)
        {
            if (infoToCopy == null) return;

            bool isDirty = false;
            if (_Name != infoToCopy._Name)
            {
                _Name = infoToCopy._Name;
                isDirty = true;
            }
            if (_Description != infoToCopy._Description)
            {
                _Description = infoToCopy._Description;
                isDirty = true;
            }
            if (_Rounds != infoToCopy._Rounds)
            {
                _Rounds = infoToCopy._Rounds;
                isDirty = true;
            }
            if (_Overwrites != infoToCopy._Overwrites)
            {
                _Overwrites = infoToCopy._Overwrites;
                isDirty = true;
            }
            if (_Extends != infoToCopy._Extends)
            {
                _Extends = infoToCopy._Extends;
                isDirty = true;
            }
            if (_AlwaysAvailable != infoToCopy._AlwaysAvailable)
            {
                _AlwaysAvailable = infoToCopy._AlwaysAvailable;
                isDirty = true;
            }
            /*
            if (_Editable != infoToCopy._Editable)
            {
                _Editable = infoToCopy._Editable;
                isDirty = true;
            }
             * */
            if (!_Codes.SequenceEqual(infoToCopy._Codes))
            {
                _Codes = new List<int>(infoToCopy._Codes);
                isDirty = true;
            }

            _Owner = infoToCopy._Owner;

            if (isDirty && _Owner != null) _Owner.IsDirty = isDirty;
        }
        public static ConfigAbilityInfo CreateFrom(Config owner, ConfigAbilityInfo infoToCopy)
        {
            ConfigAbilityInfo item = new ConfigAbilityInfo();
            item.Owner = owner;
            if (infoToCopy == null) return item;
            item.CopyFrom(infoToCopy);
            return item;
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            for (int i = 0; i < _Codes.Count; i++)
            {
                yield return _Codes[i];
            }
        }

        internal Config _Owner;
        public Config Owner { get { return _Owner; } set { _Owner = value; } }

        internal void Dirtied() { if (Owner != null) Owner.IsDirty = true; }
    }

    [DataContract(Name = "Abilities")]
    public class ConfigAbilities : ConfigBase
    {
        [DataMember(Name = "Map")]
        public Dictionary<string, ConfigAbilityInfo> _Map = new Dictionary<string, ConfigAbilityInfo>();

        public ConfigAbilities() { }

        // Properties
        public ConfigAbilityInfo this[string key] { get { return _Map[key]; } }
        public string[] Keys { get { return _Map.Keys.ToArray(); } }
        public bool Exists(string key) { return _Map.ContainsKey(key); }
        public int Count { get { return _Map.Count; } }

        internal int _LabelMaxLength = -1; // Initialize to < 0 so that we will calculate the max length on first access
        public int LabelMaxLength { get { if (_LabelMaxLength < 0) { foreach (string s in Keys) if (s.Length > _LabelMaxLength) _LabelMaxLength = s.Length; } return _LabelMaxLength; } }

        // Methods
        public string FromCode(int code)
        {
            foreach (KeyValuePair<string, ConfigAbilityInfo> pair in _Map)
            {
                if (pair.Value.ContainsCode(code)) return pair.Key;
            }
            return "";
        }
        internal bool CleanUp(Config defaultConfig)
        {
            bool madeDirty = false;

            if (_Map == null) _Map = new Dictionary<string, ConfigAbilityInfo>();

            // Convert old values to new ones in case we support a label change between versions

            // Remove the unrecognized ones now that we have converted old to new labels
            List<string> keysToDelete = new List<string>();
            foreach (string s in _Map.Keys) if (!defaultConfig._Abilities._Map.ContainsKey(s)) keysToDelete.Add(s);
            foreach (string s in keysToDelete) { _Map.Remove(s); madeDirty = true; }

            // Add in the missing items now that we have cleaned out the unrecognized ones
            ConfigAbilityInfo newInfo;
            foreach (KeyValuePair<string, ConfigAbilityInfo> pair in defaultConfig._Abilities._Map)
            {
                if (!_Map.ContainsKey(pair.Key.ToString()))
                {
                    newInfo = new ConfigAbilityInfo();
                    newInfo.CopyFrom(pair.Value);
                    _Map.Add(pair.Key, newInfo); madeDirty = true;
                }
            }
            _LabelMaxLength = -1;
            return madeDirty;
        }
        public bool Add(ConfigAbilityInfo info)
        {
            if (info == null || string.IsNullOrEmpty(info.Name)) return false;
            info.Owner = Owner;
            _Map[info.Name] = info;
            Dirtied();
            return true;
        }
        public bool Delete(string key)
        {
            if (!_Map.ContainsKey(key)) return false;
            _Map.Remove(key);
            Dirtied();
            return true;
        }
    }

    [DataContract(Name = "SignatureInfo")]
    public class ConfigMemoryMarkerInfo : ConfigBase
    {
        public ConfigMemoryMarkerInfo() { }
        public ConfigMemoryMarkerInfo(Config owner, string signature, string description)
        {
            _Owner = owner;
            _Signature = signature;
            _Description = description;
        }

        // Properties
        [DataMember(Name = "Signature")]
        internal string _Signature;
        public string Signature { get { return _Signature; } set { _Signature = value; Dirtied(); } }

        [DataMember(Name = "Description")]
        internal string _Description = "";
        public string Description { get { return _Description; } }

        // Methods
        // Copies all fields then does a single save which is more efficient than user setting each field invidually resulting in multiple save attempts.
        public ConfigMemoryMarkerInfo Clone()
        {
            ConfigMemoryMarkerInfo newInfo = new ConfigMemoryMarkerInfo();
            newInfo.CopyFrom(this);
            return newInfo;
        }
        public void CopyFrom(ConfigMemoryMarkerInfo infoToCopy)
        {
            if (infoToCopy == null) return;

            bool isDirty = false;
            if (_Signature != infoToCopy._Signature)
            {
                _Signature = infoToCopy._Signature;
                isDirty = true;
            }
            if (_Description != infoToCopy._Description)
            {
                _Description = infoToCopy._Description;
                isDirty = true;
            }
            _Owner = infoToCopy._Owner;

            if (isDirty && _Owner != null) Dirtied();
        }
        public static ConfigMemoryMarkerInfo CreateFrom(Config owner, ConfigMemoryMarkerInfo infoToCopy)
        {
            ConfigMemoryMarkerInfo item = new ConfigMemoryMarkerInfo();
            item.Owner = owner;
            if (infoToCopy == null) return item;
            item.CopyFrom(infoToCopy);
            return item;
        }
    }

    [DataContract(Name = "Markers")]
    public class ConfigMemoryMarkers : ConfigBase
    {
        [DataMember(Name = "Map")]
        internal Dictionary<string, ConfigMemoryMarkerInfo> _Map = new Dictionary<string, ConfigMemoryMarkerInfo>();

        public ConfigMemoryMarkers() { }

        // Properties and Operators
        public ConfigMemoryMarkerInfo this[string key] { get { return _Map[key]; } set { if (_Map[key] != value) { _Map[key] = value; if (_Owner != null) _Owner.IsDirty = true; } } }
        public string[] Keys { get { return _Map.Keys.ToArray(); } }

        private int _MaxKeyLength = -1;
        public int MaxKeyLength { get { if (_MaxKeyLength < 0) { foreach (string s in Keys) if (s.Length > _MaxKeyLength) _MaxKeyLength = s.Length; } return _MaxKeyLength; } }

        // Methods
        public string ToUiString(string key)
        {
            string keyString = key.ToString();
            string toString = (keyString + ":").PadRight(MaxKeyLength + 2) + _Map[keyString].Signature; // +2 for the ': ' plus a gap
            return toString;
        }
        internal bool CleanUp(Config defaultConfig)
        {
            bool madeDirty = false;

            // Convert old labels to new labels for any upgrades we support

            // Remove unrecognized items now since it makes checking for missing items a tad faster
            List<string> keysToDelete = new List<string>();
            foreach (string s in _Map.Keys) if (!defaultConfig._MemoryMarkers._Map.ContainsKey(s)) keysToDelete.Add(s);
            foreach (string s in keysToDelete) { _Map.Remove(s); madeDirty = true; }

            // Add in any missing items
            foreach (KeyValuePair<string, ConfigMemoryMarkerInfo> pair in defaultConfig._MemoryMarkers._Map)
                if (!_Map.ContainsKey(pair.Key)) { _Map.Add(pair.Key, pair.Value); madeDirty = true; }

            // Now refresh our tracker for max length
            _MaxKeyLength = -1;

            return madeDirty;
        }
    }

    [DataContract(Name = "ChainInfo")]
    public class ConfigMemoryChainInfo : IEnumerable
    {
        public ConfigMemoryChainInfo() { }
        public ConfigMemoryChainInfo(Config owner, string name, int size, string description, int[] offsets)
        {
            _Owner = owner;
            _Name = name;
            _MemoryDataSize = size;
            _MemoryOffsets = offsets;
            _Description = description;
        }

        // Properties
        protected Config _Owner;
        public Config Owner { get { return _Owner; } set { _Owner = value; } }

        [DataMember(Name = "Name")]
        internal string _Name;
        public string Name { get { return _Name; } }

        [DataMember(Name = "Description")]
        internal string _Description = "";
        public string Description { get { return _Description; } }

        public int Size { get { return _MemoryDataSize; } }
        [DataMember(Name = "Size")]
        internal int _MemoryDataSize = 1;

        [DataMember(Name = "Offsets")]
        internal int[] _MemoryOffsets = new int[1] { 0 };

        public int Length { get { return _MemoryOffsets.Length; } }
        public int this[int index] { get { return _MemoryOffsets[index]; } }

        // Methods
        // Copies all fields then does a single save which is more efficient than user setting each field invidually resulting in multiple save attempts.
        public ConfigMemoryChainInfo Clone()
        {
            ConfigMemoryChainInfo newInfo = new ConfigMemoryChainInfo();
            newInfo.CopyFrom(this);
            return newInfo;
        }
        public void CopyFrom(ConfigMemoryChainInfo infoToCopy)
        {
            if (infoToCopy == null) return;

            bool isDirty = false;
            if (_MemoryDataSize != infoToCopy._MemoryDataSize)
            {
                _MemoryDataSize = infoToCopy._MemoryDataSize;
                isDirty = true;
            }
            if (!_MemoryOffsets.SequenceEqual(infoToCopy._MemoryOffsets))
            {
                _MemoryOffsets = (int[])infoToCopy._MemoryOffsets.Clone();
                isDirty = true;
            }
            if (_Name != infoToCopy._Name)
            {
                _Name = infoToCopy._Name;
                isDirty = true;
            }
            if (_Description != infoToCopy._Description)
            {
                _Description = infoToCopy._Description;
                isDirty = true;
            }
            _Owner = infoToCopy._Owner;

            if (isDirty && _Owner != null) _Owner.IsDirty = isDirty;
        }
        public static ConfigMemoryChainInfo CreateFrom(Config owner, ConfigMemoryChainInfo infoToCopy)
        {
            ConfigMemoryChainInfo item = new ConfigMemoryChainInfo();
            item.Owner = owner;
            if (infoToCopy == null) return item;
            item.CopyFrom(infoToCopy);
            return item;
        }
        public System.Collections.IEnumerator GetEnumerator()
        {
            for (int i = 0; i < _MemoryOffsets.Length; i++)
            {
                yield return _MemoryOffsets[i];
            }
        }
    }

    [DataContract(Name = "Chains")]
    public class ConfigMemoryChains : ConfigBase
    {
        [DataMember(Name = "Map")]
        internal Dictionary<string, ConfigMemoryChainInfo> _Map = new Dictionary<string, ConfigMemoryChainInfo>();

        public ConfigMemoryChains() { }

        // Properties
        public ConfigMemoryChainInfo this[string key] { get { return _Map[key]; } }
        public string[] Keys { get { return _Map.Keys.ToArray(); } }
        private int _MaxKeyLength = -1;
        public int MaxKeyLength { get { if (_MaxKeyLength < 0) { foreach (string s in Keys) if (s.Length > _MaxKeyLength) _MaxKeyLength = s.Length; } return _MaxKeyLength; } }

        // Methods
        public string ToUiString(string key)
        {
            if (!_Map.ContainsKey(key)) return "";
            ConfigMemoryChainInfo info = this[key];
            string joiner = "";
            string formatString = "X8";
            string toString = (info.Name.ToString() + ":").PadRight(MaxKeyLength + 2); // +2 for the ': ' plus a gap
            for (int x = 0; x < info.Length; x++)
            {
                toString += joiner + info._MemoryOffsets[x].ToString(formatString);

                // First part was special, now set up for rest of offsets
                joiner = " -> ";
                formatString = "X4";
            }
            return toString;
        }
        internal bool CleanUp(Config defaultConfig)
        {
            bool madeDirty = false;

            // Convert old labels to new labels for any upgrades we support

            // Remove unrecognized items now since it makes cheking for missing items a tad faster
            List<string> keysToDelete = new List<string>();
            foreach (string s in _Map.Keys) if (!defaultConfig._MemoryChains._Map.ContainsKey(s)) keysToDelete.Add(s);
            foreach (string s in keysToDelete) { _Map.Remove(s); madeDirty = true; }

            // Add in any missing items
            foreach (KeyValuePair<string, ConfigMemoryChainInfo> pair in defaultConfig._MemoryChains._Map)
            {
                if (!_Map.ContainsKey(pair.Key)) { _Map.Add(pair.Key, ConfigMemoryChainInfo.CreateFrom(Owner, pair.Value)); madeDirty = true; }
                pair.Value.Owner = Owner;
            }

            // Now refresh our tracke for max length
            _MaxKeyLength = -1;

            return madeDirty;
        }
    }

    [DataContract(Name = "OffsetInfo")]
    public class ConfigMemoryOffsetInfo : ConfigBase
    {
        [DataMember(Name = "Name")]
        internal string _Name;
        public string Name { get { return _Name; } }

        [DataMember(Name = "Description")]
        internal string _Description;
        public string Description { get { return _Description; } }

        [DataMember(Name = "Offset")]
        internal int _MemoryOffset = 0;

        [DataMember(Name = "CheckValue")]
        internal byte[] _CheckValue = new byte[1] { 0 };

        public ConfigMemoryOffsetInfo() { }
        public ConfigMemoryOffsetInfo(Config owner, string name, int offset, byte[] checkValue, string description)
        {
            _Owner = owner;
            _Name = name;
            _MemoryOffset = offset;
            _CheckValue = checkValue;
        }

        // Properties
        public int Offset { get { return _MemoryOffset; } set { if (_MemoryOffset != value) { _MemoryOffset = value; if (Owner != null) _Owner.IsDirty = true; } } }
        public byte[] CheckValue { get { return _CheckValue; } set { if (!_CheckValue.SequenceEqual(value)) { _CheckValue = value; if (Owner != null) _Owner.IsDirty = true; } } }

        // Methods
        // Copies all fields then does a single save which is more efficient than user setting each field invidually resulting in multiple save attempts.
        public void CopyFrom(ConfigMemoryOffsetInfo infoToCopy)
        {
            if (infoToCopy == null) return;

            bool isDirty = false;
            if (_MemoryOffset != infoToCopy._MemoryOffset)
            {
                _MemoryOffset = infoToCopy._MemoryOffset;
                isDirty = true;
            }
            if (_Name != infoToCopy._Name)
            {
                _Name = infoToCopy._Name;
                isDirty = true;
            }
            if (_Description != infoToCopy._Description)
            {
                _Description = infoToCopy._Description;
                isDirty = true;
            }
            if (!_CheckValue.SequenceEqual(infoToCopy._CheckValue))
            {
                _CheckValue = (byte[])infoToCopy._CheckValue.Clone();
                isDirty = true;
            }
            _Owner = infoToCopy._Owner;

            if (isDirty && _Owner != null) _Owner.IsDirty = isDirty;
        }
        public static ConfigMemoryOffsetInfo CreateFrom(Config owner, ConfigMemoryOffsetInfo infoToCopy)
        {
            ConfigMemoryOffsetInfo item = new ConfigMemoryOffsetInfo();
            item.Owner = owner;
            if (infoToCopy == null) return item;
            item.CopyFrom(infoToCopy);
            return item;
        }
    }

    [DataContract(Name = "Offsets")]
    public class ConfigMemoryOffsets : ConfigBase
    {
        [DataMember(Name = "Map")]
        internal Dictionary<string, ConfigMemoryOffsetInfo> _Map = new Dictionary<string, ConfigMemoryOffsetInfo>();

        public ConfigMemoryOffsets() { }

        // Properties
        public ConfigMemoryOffsetInfo this[string key] { get { return _Map[key]; } }
        public string[] Keys { get { return _Map.Keys.ToArray(); } }

        private int _MaxKeyLength = -1;
        public int MaxKeyLength { get { if (_MaxKeyLength < 0) { foreach (string s in Keys) if (s.Length > _MaxKeyLength) _MaxKeyLength = s.Length; } return _MaxKeyLength; } }

        // Methods
        public string ToUiString(string key)
        {
            if (!_Map.ContainsKey(key)) return "";
            ConfigMemoryOffsetInfo info = this[key];
            string toString = (info.Name.ToString() + ":").PadRight(MaxKeyLength + 2); // +2 for the ': ' plus a gap
            toString += "Offset = " + info.Offset.ToString("X8") + " - Check Value = " + BitConverter.ToString(info.CheckValue).Replace("-", string.Empty);
            return toString;
        }

        internal bool CleanUp(Config defaultConfig)
        {
            bool madeDirty = false;

            // Convert old labels to new labels for any upgrades we support

            // Remove unrecognized items now since it makes cheking for missing items a tad faster
            List<string> keysToDelete = new List<string>();
            foreach (string s in _Map.Keys) if (!defaultConfig._MemoryOffsets._Map.ContainsKey(s)) keysToDelete.Add(s);
            foreach (string s in keysToDelete) { _Map.Remove(s); madeDirty = true; }

            // Add in any missing items
            foreach (KeyValuePair<string, ConfigMemoryOffsetInfo> pair in defaultConfig._MemoryOffsets._Map)
            {
                if (!_Map.ContainsKey(pair.Key)) { _Map.Add(pair.Key, ConfigMemoryOffsetInfo.CreateFrom(Owner, pair.Value)); madeDirty = true; }
                pair.Value.Owner = Owner;
            }

            // Now refresh our tracked for max length
            _MaxKeyLength = 0;
            foreach (string s in _Map.Keys) if (s.Length > _MaxKeyLength) _MaxKeyLength = s.Length;

            return madeDirty;
        }
    }

    [DataContract(Name = "ConfigurationBase")]
    public abstract class ConfigBase
    {
        protected Config _Owner;
        public Config Owner { get { return _Owner; } set { _Owner = value; } }

        internal void Dirtied() { if (Owner != null) Owner.IsDirty = true; }

        private bool _BufferChanges = false;
        internal bool BufferChanges { get { if (Owner != null) return _BufferChanges = false; return _BufferChanges; } set { if (Owner != null) { Owner.BufferChanges = value; } } }
    }

}
