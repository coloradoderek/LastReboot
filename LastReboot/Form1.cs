using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Color = System.Drawing.Color;

namespace LastReboot
{


    public partial class Form1 : Form
    {
        public static class Globals
        {
            // How to Give borders to FLAT style ComboBoxes
            // https://stackoverflow.com/questions/38679135/combobox-dropdownlist-style-with-white-background

            // User and Machine information
            public static String CurUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            public static String CurMachine = Environment.MachineName;

            // Auditing Purposes - UPDATE section
            public static object[] PreChangeAssetArray = new object[25];
            public static object[] PostChangeAssetArray = new object[25];

            public static bool TestTemp = false;
            public static String test = "";
            public static String sql = "";
            public static String SearchResults = "";
            public static String nl = "\r\n";

            public static bool Debug = true;

            public static bool OKToContinue = true;

            //public static String ServerToConn = "";
            public static String ServerToConn = "69.146.195.176,1420";
            public static String UserName = "autobot";
            public static String Password = "1conlon2";
            public static String InitialCatalog = "inventory";

            public static String CurrentAV = "";
            public static List<string> PreviousAVList = new List<string>(10);
            public static String NewestOS = "";

            // customLocationList (cLL)
            public static string[] cLL = new string[20];

            public static int CountX = 0;
            public static int CountY = 0;
            public static int CountZ = 0;
            public static int CountW = 0;

            public static bool PopulatedServerList = false;

            // Used on the Software Options tab
            public static bool SkipNextRun = false;

            // Regex Definitions
            public static Regex regex_allnumber = new Regex(@"^\d+$");
            public static Regex regex_allletters = new Regex(@"^[A-Za-z\s]+$");
            public static Regex regex_onlyletters = new Regex(@"^[A-Za-z]+$");
            public static Regex regex_isdate = new Regex(@"\d\d\/\d\d\/\d\d(\d\d)?");
            public static Regex regex_letternum = new Regex(@"[A-Za-z0-9\s]+$");
            public static Regex regex_model = new Regex(@"[A-Za-z0-9\/\-\(\)\s]+");
            public static Regex regex_phone = new Regex(@"^\D?([2-9]\d{2})\D?\D?(\d{3})\D?(\d{4})$");
            // user name validation - allow letters, numbers, hyphens, periods, parentheses and spaces
            public static Regex regex_usersName = new Regex(@"^[A-Za-z\s\-\.\(\)]+$");
            // Asset Tag validation - allow numbers, letters, hyphens
            public static Regex regex_assettag = new Regex(@"^[0-9A-Za-z\-]+$");
            public static Regex regex_osversion = new Regex(@"^Version\s+\:\s.+$");
            public static Regex regex_osname = new Regex(@"^Caption\s+\:\s.+$");
            public static Regex regex_boottime = new Regex(@"^LastBootUpTime\s+\:\s.+$");
            public static Regex regex_currentuser = new Regex(@"^UserName\s+\:\s(.+)$");
            public static Regex regex_lastuser = new Regex(@"^LocalPath\s+\:\s(.+)$");
        }

        public Form1()
        {
            InitializeComponent();
        }

        private bool ValidateInfo(string value, int w2v)
        {
            switch (w2v)
            {
                case 1: // all letters or spaces
                    if (Globals.regex_allletters.IsMatch(value))
                    {
                        return true;
                    }
                    break;
                case 2: // all numbers
                    if (Globals.regex_allnumber.IsMatch(value))
                    {
                        return true;
                    }
                    break;
                case 3: // is a date (mm/dd/yyyy) all numbers
                    if (Globals.regex_isdate.IsMatch(value))
                    {
                        return true;
                    }
                    break;
                case 4: // letters or numbers or a space, only
                    if (Globals.regex_letternum.IsMatch(value))
                    {
                        return true;
                    }
                    break;
                case 5:
                    if (Globals.regex_onlyletters.IsMatch(value))
                    {
                        return true;
                    }
                    break;
                case 6:
                    if (Globals.regex_model.IsMatch(value))
                    {
                        return true;
                    }
                    break;
                case 7:
                    if (Globals.regex_letternum.IsMatch(value) && value.Length == 7)
                    {
                        return true;
                    }
                    break;
                case 8:
                    if (Globals.regex_phone.IsMatch(value))
                    {
                        return true;
                    }
                    break;
                case 9:
                    if (value == "Windows 10 Pro" || value == "Windows 7 Pro" || value == "Windows 8.1 Pro" || value == "Windows 8 Pro" || value == "Windows XP" || value == "N/A")
                    {
                        return true;
                    }
                    break;
                case 10:
                    if (Globals.PreviousAVList.Contains(value) || value == Globals.CurrentAV)
                    {
                        return true;
                    }
                    break;
                case 11:
                    if (Globals.regex_usersName.IsMatch(value))
                    {
                        return true;
                    }
                    break;
                case 12:
                    if (value == "FTE" || value == "Extra" || value == "")
                    {
                        return true;
                    }
                    break;
                case 13:
                    if (Globals.regex_assettag.IsMatch(value))
                    {
                        return true;
                    }
                    break;
                case 14:
                    if (Globals.regex_osversion.IsMatch(value))
                    {
                        return true;
                    }
                    break;
                case 15:
                    if (Globals.regex_osname.IsMatch(value))
                    {
                        return true;
                    }
                    break;
                case 16:
                    if (Globals.regex_boottime.IsMatch(value))
                    {
                        return true;
                    }
                    break;
                case 17:
                    if (Globals.regex_currentuser.IsMatch(value))
                    {
                        return true;
                    }
                    break;
                case 18:
                    if (Globals.regex_lastuser.IsMatch(value))
                    {
                        Console.WriteLine("[18] RETURNING TRUE");
                        return true;
                    }
                    break;
                default:
                    break;
            }
            return false;
        }
        private SqlConnection GetSqlConnection()
        {
            // Build SQL Connection String
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = Globals.ServerToConn;
            builder.UserID = Globals.UserName;
            builder.Password = Globals.Password;
            builder.InitialCatalog = Globals.InitialCatalog;
            builder.MultipleActiveResultSets = true;

            // Connect to SQL
            //Console.Write("Connecting to SQL Server ... ");
            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            return connection;
        }
        private void ProcessErrorCaught(Exception err, string WhereFrom = "skip")
        {
            //Console.WriteLine("Error type: " + err.GetType().ToString());
            if (Globals.Debug) { Console.WriteLine(string.Format("[ProcessErrorCaught] {0}", err.Message)); }

            // Need to add some kind of LOGGING here            
            /*if (WhereFrom != "logdata")
            {
                if (LogData(err.Message.SQLCleanse()) && Globals.Debug)
                {
                    Console.WriteLine("[LOG_SUCCESSFULLY ADDED]");
                }
                else
                {
                    if (Globals.Debug) { Console.WriteLine(string.Format("[ProcessErrorCaught][LOG_NOT_ADDED] {0}", err.Message)); }
                }
            }*/

            // CALLED FROM THE ASSET SOFTWARE MANAGER TAB
            if (WhereFrom == "sal")
            {
                if (err.Message == "There is no row at position 0.")
                {
                    throw new Exception("norow");
                }
            }

            // CALLED FROM OPTIONS PAGE - can be used anywhere that you are trying to prevent duplicate entries
            if (WhereFrom == "opt")
            {
                if (err.Message.Contains("Cannot insert duplicate key row in object "))
                {
                    throw new Exception("dupe");
                }
            }

            // CALLED FROM THE SOFTWARE OPTIONS PAGE - checking for duplicate software name error
            if (WhereFrom == "addnewsoft")
            {
                if (err.Message.Contains("Cannot insert duplicate key "))
                {
                    throw new Exception("dupe");
                }
            }

            // Process the Log errors
            if (WhereFrom == "logdata")
            {
                if (Globals.Debug) { Console.WriteLine(string.Format("[ProcessErrorCaught][ERROR_LOGGING] {0}", err.Message)); }
            }
        }

        private string ConvertTime(string RawTime)
        {
            if (Globals.Debug) { Console.WriteLine(string.Format("[CONVERT_TIME]RawTime : {0}", RawTime)); }

            string AmPm = "AM";
            var matches = Regex.Matches(RawTime, @"^(\d\d)(\d\d)(\d\d)$");
            var hour = matches[0].Groups[1].Value;
            var min = matches[0].Groups[2].Value;
            var sec = matches[0].Groups[3].Value;

            if (Globals.Debug) { Console.WriteLine(string.Format("[CONVERT_TIME]Hour : {0} Min : {1} Sec : {2}", Convert.ToInt32(hour), Convert.ToInt32(min), Convert.ToInt32(sec))); }

            if (Convert.ToInt32(sec) < 10)
            {
                sec = "0" + Convert.ToInt32(sec);
            }
            if (Convert.ToInt32(min) < 10)
            {
                min = "0" + Convert.ToInt32(min);
            }
            if (Convert.ToInt32(hour) < 10)
            {
                hour = "0" + Convert.ToInt32(hour);
            }
            if (Convert.ToInt32(hour) >= 12)
            {
                AmPm = "PM";
                if (Convert.ToInt32(hour) > 12)
                {
                    hour = (Convert.ToInt32(hour) - 12).ToString();
                }
                if (Convert.ToInt32(hour) < 10)
                {
                    hour = "0" + hour;
                }
            }
            return hour + ":" + min + ":" + sec + " " + AmPm;
        }
        private string GetOSVersion(string build)
        {
            SqlConnection connection = GetSqlConnection();
            connection.Open();

            // Build the SQL Query
            Globals.sql = string.Format("SELECT version FROM w10 WHERE build = '{0}'", build.SQLCleanse());

            if (Globals.Debug) { Console.WriteLine(string.Format("[GOSV]SQL String: {0}", Globals.sql.ToString())); }

            // Create SQL Command and Open the connection
            SqlCommand command = new SqlCommand(Globals.sql, connection);

            try
            {
                // Create a SQL Datareader
                SqlDataReader commandReader = command.ExecuteReader();

                // Create a Datatable Object
                DataTable dataTable = new DataTable();
                dataTable.Load(commandReader);

                int rowCount = dataTable.Rows.Count;

                if (Globals.Debug) { Console.WriteLine(string.Format("[GOSV]RowCount: {0}", rowCount)); }

                if (rowCount == 1)
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //Console.WriteLine(commandReader["type"]);
                            //Globals.test += commandReader["type"] + "|";
                            var tempret = reader["version"].ToString().Trim();
                            connection.Close();
                            return tempret;
                        }
                        connection.Close();
                        return 0.ToString();
                    }
                }
                else
                {
                    connection.Close();
                    return 0.ToString();
                }
            }
            catch (Exception err)
            {
                ProcessErrorCaught(err);
                connection.Close();
                return 0.ToString();
            }
        }

        private void PCSystemInfo()
        {
            var AssetToLookup = AssetSearch.Text.Trim();

            // Used for cmd.exe
            //var ProcessArgs = "/c systeminfo /s " + AssetToLookup + " /fo list | find \"OS\"";

            // Used for PowerShell.exe
            //var ProcessArgs = "[Environment]::OSVersion";          // Works for the local machine

            var ProcessArgs = "Get-WmiObject -ComputerName \"" + AssetToLookup + "\" -class Win32_OperatingSystem | Format-List *";
            var ProcessArgs2 = "Get-WmiObject -ComputerName \"" + AssetToLookup + "\" -class Win32_ComputerSystem | Format-List *";
            var ProcessArgs3 = "Get-WmiObject -ComputerName \"" + AssetToLookup + "\" -class Win32_UserProfile | Where-Object {($_.SID -notmatch '^S-1-5-\\d[18|19|20]$')} | Sort-Object -Property LastUseTime -Descending | Select-Object -First 1";

            try
            {
                // START OF FIRST PROCESS (OS INFO)
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        //FileName = "cmd.exe",
                        FileName = @"powershell.exe",
                        Arguments = ProcessArgs,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };

                process.Start();

                var OSBuild = "";
                var OSVersion = "";
                var OSName = "";
                var LastBootTime = "";
                string RealLastBootTime = "";
                var RebootTime = "";
                var CurrentUser = "";
                var LastUser = "";

                while (!process.StandardOutput.EndOfStream)
                {
                    var line = process.StandardOutput.ReadLine();

                    if (ValidateInfo(line, 14) || ValidateInfo(line, 15) || ValidateInfo(line, 16))
                    {
                        if (Globals.Debug) { Console.WriteLine("[OSVLU]" + line); }
                        if (ValidateInfo(line, 14))
                        {
                            var matches = Regex.Matches(line, @"^Version\s+\:\s\d+\.\d+\.(\d+)$");
                            OSBuild = matches[0].Groups[1].Value.Trim();
                            OSVersion = GetOSVersion(OSBuild);
                            if (Globals.Debug) { Console.WriteLine("PROCESSED: " + line); }
                        }
                        else if (ValidateInfo(line, 15))
                        {
                            var matches = Regex.Matches(line, @"^Caption\s+\:\sMicrosoft\s(.+)$");
                            OSName = matches[0].Groups[1].Value.Trim();
                        }
                        else if (ValidateInfo(line, 16))
                        {
                            var matches = Regex.Matches(line, @"^LastBootUpTime\s+\:\s(\d+)\.\d+\-\d+$");
                            LastBootTime = matches[0].Groups[1].Value.Trim();
                            var matches2 = Regex.Matches(LastBootTime, @"^(\d\d\d\d)(\d\d)(\d\d)(\d\d\d\d\d\d)$");
                            var RebootYear = matches2[0].Groups[1].Value.Trim();
                            var RebootMonth = matches2[0].Groups[2].Value.Trim();
                            var RebootDay = matches2[0].Groups[3].Value.Trim();
                            RebootTime = matches2[0].Groups[4].Value.Trim();
                            RealLastBootTime = RebootMonth + "/" + RebootDay + "/" + RebootYear;
                        }
                    }
                    else
                    {
                        // Testing
                        if (Globals.Debug) { Console.WriteLine("IGNORED: " + line); }
                    }
                }

                string stderrx = process.StandardError.ReadToEnd();
                if (Globals.Debug) { Console.WriteLine("[ERROR PROCESSING]: " + stderrx); }

                if (stderrx == "" || string.IsNullOrEmpty(stderrx))
                {
                    // DO NOTHING and CONTINUE ON TO SECOND PROCESS
                }
                else if (stderrx.Trim().Contains("Get-WmiObject : The RPC server is unavailable."))
                {
                    Invoke((MethodInvoker)delegate
                    {
                        PCInfoLabel.Text = "COMPUTER NOT ONLINE";
                        small_wait_dance.Visible = false;
                        PCInfoLabel.Visible = true;
                        //AssetSearch.Text = "";
                    });
                    goto StopProcessing;
                }
                else if (stderrx.Trim().Contains("Get-WmiObject : Access is denied."))
                {
                    Invoke((MethodInvoker)delegate
                    {
                        PCInfoLabel.Text = "*** ACCESS DENIED ***";
                        small_wait_dance.Visible = false;
                        PCInfoLabel.Visible = true;
                        //AssetSearch.Text = "";
                    });
                    goto StopProcessing;
                }
                process.WaitForExit();
                // END OF FIRST PROCESS (OS INFO)

                // START OF SECOND PROCESS (USER INFO)
                process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        //FileName = "cmd.exe",
                        FileName = @"powershell.exe",
                        Arguments = ProcessArgs2,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                while (!process.StandardOutput.EndOfStream)
                {
                    var line = process.StandardOutput.ReadLine();

                    if (ValidateInfo(line, 17))
                    {
                        if (Globals.Debug) { Console.WriteLine("[CULU]" + line); }
                        if (ValidateInfo(line, 17))
                        {
                            var matches = Regex.Matches(line, @"^UserName\s+\:\s(.+)$");
                            CurrentUser = matches[0].Groups[1].Value.Trim();
                            if (CurrentUser.Contains("MESACOUNTY"))
                            {
                                var Tmatches = Regex.Matches(CurrentUser, @"^MESACOUNTY\\(.+)$");
                                CurrentUser = Tmatches[0].Groups[1].Value.Trim();
                            }
                        }
                    }
                    else
                    {
                        // Testing
                        if (Globals.Debug) { Console.WriteLine("IGNORED: " + line); }
                    }
                }
                stderrx = process.StandardError.ReadToEnd();
                if (Globals.Debug) { Console.WriteLine("[ERROR PROCESSING]: " + stderrx); }

                if (stderrx == "" || string.IsNullOrEmpty(stderrx))
                {
                    // DO NOTHING and KEEP PROCESSING
                }
                else if (stderrx.Trim().Contains("Get-WmiObject : The RPC server is unavailable."))
                {
                    Invoke((MethodInvoker)delegate
                    {
                        PCInfoLabel.Text = "COMPUTER NOT ONLINE";
                        small_wait_dance.Visible = false;
                        PCInfoLabel.Visible = true;
                        //AssetSearch.Text = "";
                    });
                    goto StopProcessing;
                }
                else if (stderrx.Trim().Contains("Get-WmiObject : Access is denied."))
                {
                    Invoke((MethodInvoker)delegate
                    {
                        PCInfoLabel.Text = "*** ACCESS DENIED ***";
                        small_wait_dance.Visible = false;
                        PCInfoLabel.Visible = true;
                        //AssetSearch.Text = "";
                    });
                    goto StopProcessing;
                }
                process.WaitForExit();
                // END OF SECOND PROCESS (USER INFO)

                // START OF THIRD PROCESS (USER INFO)
                process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        //FileName = "cmd.exe",
                        FileName = @"powershell.exe",
                        Arguments = ProcessArgs3,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                while (!process.StandardOutput.EndOfStream)
                {
                    var line = process.StandardOutput.ReadLine();

                    if (ValidateInfo(line, 18))
                    {
                        if (Globals.Debug) { Console.WriteLine("[LULU]" + line); }
                        if (ValidateInfo(line, 18))
                        {
                            var matches = Regex.Matches(line, @"^LocalPath\s+\:\sC:\\Users\\(.+)$");
                            LastUser = matches[0].Groups[1].Value.Trim();
                        }
                    }
                    else
                    {
                        // Testing
                        if (Globals.Debug) { Console.WriteLine("IGNORED: " + line); }
                    }
                }
                stderrx = process.StandardError.ReadToEnd();
                if (Globals.Debug) { Console.WriteLine("[ERROR PROCESSING]: " + stderrx); }

                if (stderrx == "" || string.IsNullOrEmpty(stderrx))
                {
                    Invoke((MethodInvoker)delegate
                    {
                        PCInfoLabel.Text = OSName + Globals.nl + "Version: " + OSVersion + Globals.nl + "Build: " + OSBuild + Globals.nl + "Last Reboot: " + RealLastBootTime + Globals.nl + "Reboot Time: " + ConvertTime(RebootTime) + Globals.nl + "Current User: " + CurrentUser + Globals.nl + "Last User: " + LastUser;
                        small_wait_dance.Visible = false;
                        PCInfoLabel.Visible = true;
                        //AssetSearch.Text = "";
                    });
                }
                else if (stderrx.Trim().Contains("Get-WmiObject : The RPC server is unavailable."))
                {
                    Invoke((MethodInvoker)delegate
                    {
                        PCInfoLabel.Text = "COMPUTER NOT ONLINE";
                        small_wait_dance.Visible = false;
                        PCInfoLabel.Visible = true;
                        //AssetSearch.Text = "";
                    });
                }
                else if (stderrx.Trim().Contains("Get-WmiObject : Access is denied."))
                {
                    Invoke((MethodInvoker)delegate
                    {
                        PCInfoLabel.Text = "*** ACCESS DENIED ***";
                        small_wait_dance.Visible = false;
                        PCInfoLabel.Visible = true;
                        //AssetSearch.Text = "";
                    });
                }
                process.WaitForExit();
            // END OF THIRD PROCESS (USER INFO)

            StopProcessing:;
            }
            catch (Exception e1)
            {
                // ERROR PROCESSING
                ProcessErrorCaught(e1);
            }
        }

        private void AssetSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ButtonPCLookup_Click(this, new EventArgs());
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void ButtonPCLookup_Click(object sender, EventArgs e)
        {
            PCInfoLabel.Visible = false;
            small_wait_dance.Visible = true;

            ThreadStart myThreadStart = new ThreadStart(PCSystemInfo);
            Thread myThread = new Thread(myThreadStart);
            myThread.Start();
        }
    }
    public static class StringExtensions
    {
        public static string SQLCleanse(this string str)
        {
            // replace ' with ''
            str = Regex.Replace(str, @"\'", "''");
            str = str.Trim();
            return str;
        }
    }
}
