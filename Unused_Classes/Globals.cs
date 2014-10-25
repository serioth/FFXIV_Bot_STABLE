using System.IO;
using System.Windows.Forms;

namespace Unused_Classes
{
    public class Globals
    {
        public Globals()
        {
            // Construct pathes
            _SettingsPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath),
                                          Path.GetFileNameWithoutExtension(Application.ExecutablePath) + _Version.Replace(".", string.Empty) + _SettingsFileExtension);
            //_RuleSetsPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath),
            //                              Path.GetFileNameWithoutExtension(Application.ExecutablePath) + "Rules" + RuleSets._Version.Replace(".", string.Empty) + _RuleSetsFileExtension);
            //_RoutesFolder = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Routes");
            //_RulesFolder = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Rules");
        }

        public DialogResult ShowMessage(string message) { return ShowMessage(message, "", MessageBoxButtons.OK, MessageBoxIcon.None); }
        private delegate DialogResult ShowMessagehandler(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon);
        public DialogResult ShowMessage(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            DialogResult result = DialogResult.None;
            if (MainForm.Get.InvokeRequired)
                result = (DialogResult)MainForm.Get.Invoke(new ShowMessagehandler(doUiShowMessage), new object[] { message, caption, buttons, icon });
            else
                result = doUiShowMessage(message, caption, buttons, icon);
            return result;
        }
        private DialogResult doUiShowMessage(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            if (string.IsNullOrWhiteSpace(message)) return DialogResult.None;
            //Log.Add(message, true);
            return MessageBox.Show(message, caption, buttons, icon);
        }

        private static string _SettingsFileExtension = ".xml";
        private static string _SettingsPath;
        public static string SettingsPath { get { return _SettingsPath; } }

        //private static string _RuleSetsFileExtension = ".xml";
        //private static string _RuleSetsPath;
        //public static string RuleSetsPath { get { return _RuleSetsPath; } }

        private static Globals _Global;
        public static Globals Get { get { if (_Global == null) { _Global = new Globals(); } return _Global; } }

        private static string _Version = "1.0.0.0";
        public static string Version { get { return _Version; } }

        private static string _ConfigVersion = "0.1.0.0";
        public static string ConfigVersion { get { return _ConfigVersion; } }

        private static Config _Settings;
        public Config Settings
        {
            get
            {
                if (_Settings == null)
                {
                    // See if file exists and try to load them if so
                    if (File.Exists(SettingsPath)) _Settings = Config.Load(SettingsPath);
                    else
                    {
                        // Try to and create the settings
                        _Settings = new Config();
                        _Settings.ConformConfig(Config.Defaults);
                        _Settings.Path = SettingsPath;
                        _Settings.SaveXML();
                    }
                }
                return _Settings;
            }
        }
    }
}
