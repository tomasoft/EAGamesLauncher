#region Using
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using Shell32; 
#endregion

namespace EAGamesLauncher
{
    public partial class FormMain : Form
    {

        #region PInvoke
        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref Devmode devMode);
        #endregion

        #region Screen Resolutions
        [StructLayout(LayoutKind.Sequential)]
        public struct Devmode
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public ScreenOrientation dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }

        public class ScreenResolution
        {
            public int Height { get; set; }
            public int Width { get; set; }
        }
        #endregion

        #region Get Supported Screen Resolutions
        /// <summary>
        /// Gets all supported screen resolutions.
        /// </summary>
        /// <returns>An IEnumerable of supported resolutions.</returns>
        private static IEnumerable<ScreenResolution> GetSupportedResolutions()
        {
            try
            {
                var vDevMode = new Devmode();
                var i = 0;
                var resolutions = new List<ScreenResolution>();
                while (EnumDisplaySettings(null, i, ref vDevMode))
                {
                    resolutions.Add(new ScreenResolution() { Height = vDevMode.dmPelsHeight, Width = vDevMode.dmPelsWidth });
                    i++;
                }

                return resolutions.Select(r => new { r.Height, r.Width }).Distinct().ToList().Select(r => new ScreenResolution() { Height = r.Height, Width = r.Width }).OrderBy(r => r.Width);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Private variables
        private readonly Dictionary<string, string> _games;
        private IEnumerable<string> _installedGames;
        private string _gameFolder;
        private string _gameExec;
        private delegate void SetEnabledCallback(bool enabled);
        private delegate void SetFormCallback(FormWindowState state);
        private string _eaGamesPath;
        #endregion

        #region Ctor
        public FormMain()
        {
            InitializeComponent();
            _games = new Dictionary<string, string>();
            _installedGames = new List<string>();
            _gameFolder = string.Empty;
            _gameExec = string.Empty;
            _eaGamesPath = string.Empty;
        }
        #endregion

        #region Form Load
        private void Form1_Load(object sender, EventArgs e)
        {
            // Load supported screen resolutions.
            var resolutions = GetSupportedResolutions();

            // Add them to resolutions combo.
            foreach (var screenResolution in resolutions)
                cmbResolutions.Items.Add($"{screenResolution.Width}*{screenResolution.Height}");
            

            _eaGamesPath = LoadSavedModsRootDirectory();

            _installedGames = GetGames(_eaGamesPath);

            LoadInstalledMods(_installedGames);
        }
        #endregion

        #region Mods root folder selection
        private void BtnChooseRootFolder_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ChooseModsRootDirectory()) return;

                _installedGames = GetGames(_eaGamesPath);

                LoadInstalledMods(_installedGames);
            }
            catch (Exception)
            {
                MessageBox.Show("Please select a valid EA Mods directory.");
            }
            
        }
        #endregion

        #region ReselectModsRootDirectory
        private bool ChooseModsRootDirectory()
        {
            if (eaGamesFolder.ShowDialog() != DialogResult.OK) return false;

            _eaGamesPath = eaGamesFolder.SelectedPath;

            Properties.Settings.Default["eaGamesPath"] = _eaGamesPath;

            Properties.Settings.Default.Save();

            return true;
        } 
        #endregion

        #region Game Selection
        private void LstInstalledGames_DoubleClick(object sender, EventArgs e)
        {
            // Get game folder.
            var key = lstInstalledGames.SelectedItems[0].Text;

            GetGameFileDetails(key);
        }

        private void GetGameFileDetails(string key)
        {
            WindowState = FormWindowState.Minimized;

            // Get game executable.
            var value = _games.FirstOrDefault(k => k.Key.Contains(key)).Value;

            _gameFolder = key;
            _gameExec = value;

            LaunchGame();
        }
        #endregion

        #region Launch Game
        private void LaunchGame()
        {
            // Delete all other games
            DeleteUnusedGames(_installedGames, _gameFolder);

            // Run the game
            RunGame(_gameExec);
        } 
        #endregion

        #region Shell Object (Recycle Bin)
        private static Folder NameSpace(object path)
        {
            var shellAppType = Type.GetTypeFromProgID("Shell.Application");

            var shell = Activator.CreateInstance(shellAppType);

            var val = (Folder)shellAppType.InvokeMember("NameSpace", System.Reflection.BindingFlags.InvokeMethod, null, shell, new[] { path });

            Marshal.ReleaseComObject(shell);

            return val;
        }
        #endregion

        #region File Deletion
        private static void Delete(string item)
        {
            FileSystem.DeleteDirectory(item, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
        }
        #endregion
        
        #region File restore
        public bool Restore(string item)
        {
            var recycler = NameSpace(10);

            for (var i = 0; i < recycler.Items().Count; i++)
            {
                var folderItem = recycler.Items().Item(i);

                var fileName = recycler.GetDetailsOf(folderItem, 0);

                if (string.IsNullOrEmpty(Path.GetExtension(fileName))) fileName += Path.GetExtension(folderItem.Path);
                
                //Necessary for systems with hidden file extensions.
                var filePath = recycler.GetDetailsOf(folderItem, 1);

                if (item != Path.Combine(filePath, fileName)) continue;

                DoVerb(folderItem, "ESTORE");

                return true;
            }
            return false;
        }
        #endregion

        #region Restore Command
        private static void DoVerb(FolderItem item, string verb)
        {
            foreach (FolderItemVerb fiVerb in item.Verbs())
            {
                if (!fiVerb.Name.ToUpper().Contains(verb.ToUpper())) continue;

                fiVerb.DoIt();

                return;
            }
        }
        #endregion

        #region LoadSavedModsRootDirectory
        private string LoadSavedModsRootDirectory()
        {
            if (!PropertiesHasKey("eaGamesPath")) return "";

            _eaGamesPath = Properties.Settings.Default["eaGamesPath"].ToString();

            if (string.IsNullOrWhiteSpace(_eaGamesPath)) ChooseModsRootDirectory();

            return _eaGamesPath;
        }
        #endregion

        #region ResetModsDirectory
        /// <summary>
        /// Resets currently selected mods directory.
        /// </summary>
        private bool ResetModsDirectory()
        {
            MessageBox.Show($@"Looks like previously selected directory '{_eaGamesPath}' has been moved or deleted. Please choose another directory location.");

            _eaGamesPath = "";

            _games.Clear();

            Properties.Settings.Default["eaGamesPath"] = "";

            Properties.Settings.Default.Save();

            return ChooseModsRootDirectory();
        }

        #endregion

        #region Get Installed Mods
        /// <summary>
        /// Returns a list of games inside the EA Last decade directory
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        private static IEnumerable<string> GetGames(string folderPath)
        {
            try
            {
                return Directory.GetDirectories(folderPath).Where(d => d.Contains("Generals Zero Hour")).ToList();
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Please select a valid EA mods directory.");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("");
            }

            return new List<string>();
        }
        #endregion

        #region Load installed mods
        private void LoadInstalledMods(IEnumerable<string> installedGames)
        {
            if (!installedGames.Any())
                if (!ResetModsDirectory())
                    return;

            var cnt = 0;

            _games.Clear();

            lstInstalledGames.Items.Clear();

            modsmenuStrip.Items.Clear();

            txtEaRootFolder.Text = _eaGamesPath;

            _installedGames = GetGames(_eaGamesPath);

            var games = _installedGames as IList<string> ?? _installedGames.ToList();

            AddIcoFilesToImageList(games);

            foreach (var installedGame in games)
            {
                var name = installedGame.Split('\\').Last();

                lstInstalledGames.Items.Add(new ListViewItem { Text = name, ImageIndex = cnt });

                var executable = GetExecutable(installedGame);

                if (string.IsNullOrEmpty(executable))
                {
                    MessageBox.Show(@"Unable to detect game executable, game loading cannot continue. :(");
                    return;
                }
                
                _games.Add(installedGame, executable);

                modsmenuStrip.Items.Add(name);

                cnt++;
            }
        }
        #endregion

        #region Get mod directory files
        private static IEnumerable<string> GetGameFiles(string path)
        {
            return Directory.GetFiles(path).ToList().ConvertAll(d => d.ToLower());
        }
        #endregion
        
        #region Mods icon files
        /// <summary>
        /// For each folder supplied try and get the ico file that will be used for the listview
        /// </summary>
        /// <param name="folders">The folders containing the ico files</param>
        private void AddIcoFilesToImageList(IEnumerable<string> folders)
        {

            foreach (var folder in folders)
            {
                try
                {
                    var icoFile = GetGameFiles(folder).Any(f =>
                                              !f.Contains("generals.ico") && !f.Contains("generalszh.ico")
                                              && f.EndsWith("ico")) == false ? $@"{folder}\generalszh.ico" : Directory.GetFiles(folder).First(f =>
                                !f.Contains("generals.ico") && !f.Contains("generalszh.ico")
                                && f.EndsWith("ico"));

                    using (var img = Image.FromFile(icoFile))
                    {
                        icosList.Images.Add(img);

                        lstInstalledGames.SmallImageList = icosList;
                    }
                }
                catch (FileNotFoundException)
                {
                    icosList.Images.Add(Properties.Resources.GeneralsZH);

                    lstInstalledGames.SmallImageList = icosList;
                }
            }
        }
        #endregion

        #region Get mod executable
        private static string GetExecutable(string path)
        {
            var executable = string.Empty;

            var files = GetGameFiles(path).ToList();

            // We have generals.exe, ofs_start, launcher
            if (files.Any(f => f.EndsWith("bat") && f.Contains("start")))
            {
                executable = files.Find(f => f.EndsWith("bat") && f.Contains("start"));
                return !string.IsNullOrEmpty(executable) ? executable : "";
            }
            if (files.Any(f => f.EndsWith("exe") && f.Contains("launcher")))
            {
                executable = files.Find(f => f.EndsWith("exe") && f.Contains("launcher"));
                return !string.IsNullOrEmpty(executable) ? executable : "";
            }
            if (!files.Any(f => f.EndsWith("exe") && f.Contains("generals"))) return executable;
            {
                executable = files.Find(f => f.EndsWith("exe") && f.Contains("generals"));
                return !string.IsNullOrEmpty(executable) ? executable : "";
            }
        }
        #endregion
        
        #region Delete not running mods
        private void DeleteUnusedGames(IEnumerable<string> installedGames, string gameFolder)
        {
            // Disable control so no other games can be run
            lstInstalledGames.Enabled = false;

            var ugames = installedGames.Where(g => !g.EndsWith(gameFolder) && !g.EndsWith("Command & Conquer(tm) Generals Zero Hour")).ToList();

            foreach (var ugame in ugames)
            {
                Delete(ugame);
            }
        }
        #endregion

        #region Restore deleted mods
        private void UndeleteUnusedGames(IEnumerable<string> installedGames, string gameFolder)
        {
            var ugames = installedGames.Where(g => !g.EndsWith(gameFolder) && !g.EndsWith("Command & Conquer(tm) Generals Zero Hour")).ToList();

            foreach (var ugame in ugames)
            {

                if (!Restore(ugame))
                    MessageBox.Show($@"Error restoring folder '{ugame}. Please navigate to your recycle bin and restore the folder(s) manually.");
            }

            SetEnabled(true);

            SetFormState(FormWindowState.Normal);
        }
        #endregion
        
        #region Game exited event
        private void MyProcess_Exited(object sender, EventArgs e)
        {
            UndeleteUnusedGames(_installedGames, _gameFolder);

            lstInstalledGames.Enabled = true;
        }
        #endregion
        
        #region Start process
        private void RunGame(string executable)
        {
            try
            {
                Process process;

                if (executable.EndsWith("bat"))
                {
                    process = new Process
                    {
                        StartInfo =
                        {
                            FileName = executable,
                            WorkingDirectory = Path.GetDirectoryName(executable)?? "",
                            UseShellExecute = false,
                            CreateNoWindow = true
                        }
                    };

                    process.Start();
                }
                else
                {
                    process = new Process
                    {
                        StartInfo =
                        {
                            FileName = executable,
                            WorkingDirectory = Path.GetDirectoryName(executable)?? ""
                        }
                    };

                    process.Start();
                }

                process.EnableRaisingEvents = true;

                process.Exited += MyProcess_Exited;
            }
            catch (Exception e)
            {
                
                // Restore the deleted mods
                UndeleteUnusedGames(_installedGames, _gameFolder);

                Console.WriteLine(e.Message);
            }
        }
        #endregion

        #region Form Controls STA
        private void SetEnabled(bool enabled)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (lstInstalledGames.InvokeRequired)
            {
                SetEnabledCallback d = SetEnabled;
                Invoke(d, enabled);
            }
            else
            {
                lstInstalledGames.Enabled = enabled;
            }
        }

        private void SetFormState(FormWindowState state)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (lstInstalledGames.InvokeRequired)
            {
                SetFormCallback d = SetFormState;
                Invoke(d, state);
            }
            else
            {
                Visible = state != FormWindowState.Minimized;

                WindowState = FormWindowState.Normal;
            }
        }
        #endregion

        #region Check if application setting exists
        public bool PropertiesHasKey(string key)
        {
            return Properties.Settings.Default.Properties.Cast<SettingsProperty>().Any(sp => sp.Name == key);
        }
        #endregion

        #region Form Minimize
        private void FrmMain_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Visible = false;
            }
        }
        #endregion

        #region Form Restore
        private void LauncherNotify_DoubleClick(object sender, EventArgs e)
        {
            Visible = true;
            WindowState = FormWindowState.Normal;
        }
        #endregion

        #region Launch mod from notify menu
        private void ModsmenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            modsmenuStrip.Hide();

            GetGameFileDetails(e.ClickedItem.ToString());
        }
        #endregion

        #region Get rid of the notify icon
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            launcherNotify.Visible = false;

            launcherNotify.Dispose();
        } 
        #endregion
    }
}