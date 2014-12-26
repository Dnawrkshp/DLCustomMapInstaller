using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using Ionic.Zip;

namespace DLCustomMapInstaller
{
    public partial class Form1 : Form
    {
        public const string zipStore = "CustomMaps.zip";
        public const string version = "CustomMaps.txt";

        public Form1()
        {
            InitializeComponent();
        }

        private void buttBrowseBackup_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Browse to the backups archive.dat";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbBackupFolder.Text = ofd.FileName;
            }
        }

        private void buttAddMaps_Click(object sender, EventArgs e)
        {
            int x = 0;
            tbLog.Text = "";

            FileInfo fi = new FileInfo(tbBackupFolder.Text);
            if (!fi.Exists)
            {
                Log("Backup doesn't exist!");
                return;
            }

            string dirPath = fi.Directory.FullName.Replace("\\", "/");

            //Step 1 - If there are any .bak files in the backup, replace the .dats with their appropriate .baks
            Log("Checking for .bak files in backup directory...");
            string[] files = Directory.GetFiles(dirPath, "*.bak", SearchOption.TopDirectoryOnly);
            if (files.Length > 0)
                Log("Detected " + files.Length.ToString() + " bak files. Removing .dat files and using .bak");
            else
                Log("None found");
            for (x = 0; x < files.Length; x++)
            {
                //Using try catch in case there isn't enough room or something
                try
                {
                    string del = files[x].Remove(files[x].Length - 4, 4);
                    if (File.Exists(del))
                        File.Delete(del);
                    File.Copy(files[x], del);
                    File.Delete(files[x]);
                }
                catch (Exception ee)
                {

                }
            }
            
            //Step 2 - Download updates (if no zip exists)
            if (!CheckForAndDownloadUpdate())
                return;

            //Step 3 - Setup directory to extract files to
            Log("Setting up directories..");
            string dir = Application.StartupPath + "\\cmaps";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            dir += "\\dev_hdd0";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            dir += "\\game";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            dir += "\\NPUA80646";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            dir += "\\USRDIR";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            dir += "\\rc4";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            dir += "\\ps3data";
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
            Directory.CreateDirectory(dir);

            //Step 4 -- Extract maps to directory
            Log("Decompressing Custom Maps... (takes awhile)");
            DecompressFile(Application.StartupPath + "\\" + zipStore, dir);

            //Step 5 - Copy the archive.data nd archive_00.dat to the working directory (linux to windows port limitations, could be fixed later)
            Log("Copying important backup data to working directory...");
            if (File.Exists(Application.StartupPath + "\\archive.dat"))
                File.Delete(Application.StartupPath + "\\archive.dat");
            if (File.Exists(Application.StartupPath + "\\archive_00.dat"))
                File.Delete(Application.StartupPath + "\\archive_00.dat");
            if (File.Exists(Application.StartupPath + "\\archive.bak"))
                File.Delete(Application.StartupPath + "\\archive.bak");
            if (File.Exists(Application.StartupPath + "\\archive_00.bak"))
                File.Delete(Application.StartupPath + "\\archive_00.bak");
            if (File.Exists(Application.StartupPath + "\\archive_00.tmp"))
                File.Delete(Application.StartupPath + "\\archive_00.tmp");
            File.Copy(dirPath + "/archive.dat", Application.StartupPath + "\\archive.dat");
            File.Copy(dirPath + "/archive_00.dat", Application.StartupPath + "\\archive_00.dat");

            //Step 6 - Check for any existing duplicates and delete them from the backup
            Log("Deleting any potential duplicate files in backup...");
            string com = "ps3xport.exe ExtractPSID ./ psid.bin SetPSID psid.bin DeletePath ./ /dev_hdd0/game/NPUA80646/";
            string ret = CMD(com);

            //Step 7 - Install Custom Map Files
            Log("Installing custom map files...");
            com = "ps3xport.exe ExtractPSID ./ psid.bin SetPSID psid.bin Add ./ cmaps";
            ret = CMD(com);
            while (File.Exists(Application.StartupPath + "\\archive_00.tmp"))
                Thread.Sleep(100);

            //Step 8 - Copy backup files back to actual backup, could be cleaner but I don't really care
            Log("Moving backup files from working directory...");
            if (File.Exists(dirPath + "/archive.dat"))
            {
                if (checkBox1.Checked)
                    File.Copy(dirPath + "/archive.dat", dirPath + "/archive.dat.bak");
                File.Delete(dirPath + "/archive.dat");
            }
            if (File.Exists(dirPath + "/archive_00.dat"))
            {
                if (checkBox1.Checked)
                    File.Copy(dirPath + "/archive_00.dat", dirPath + "/archive_00.dat.bak");
                File.Delete(dirPath + "/archive_00.dat");
            }
            File.Copy(Application.StartupPath + "\\archive.dat", dirPath + "/archive.dat");
            File.Copy(Application.StartupPath + "\\archive_00.dat", dirPath + "/archive_00.dat");

            File.Delete(Application.StartupPath + "\\archive.dat");
            if (File.Exists(Application.StartupPath + "\\archive.bak") && checkBox1.Checked)
                File.Delete(Application.StartupPath + "\\archive.bak");
            File.Delete(Application.StartupPath + "\\archive_00.dat");
            if (File.Exists(Application.StartupPath + "\\archive_00.bak") && checkBox1.Checked)
                File.Delete(Application.StartupPath + "\\archive_00.bak");

            //Step 9 - Remove files from working directory
            Log("Deleting final files from working directory...");
            if (File.Exists("psid.bin"))
                File.Delete("psid.bin");
            if (Directory.Exists(Application.StartupPath + "\\cmaps"))
                Directory.Delete(Application.StartupPath + "\\cmaps", true);

            MessageBox.Show("Complete.\r\nBackup saved to " + dirPath);
        }

        private void buttDLMaps_Click(object sender, EventArgs e)
        {
            if (CheckForAndDownloadUpdate())
            {
                Log("Custom Maps are up-to-date!");
            }
        }

        public bool CheckForAndDownloadUpdate()
        {
            string zipPath = Application.StartupPath + "\\" + zipStore;
            string versTxt = Application.StartupPath + "\\" + version;
            string webZip = @"http://www.cod-orc.com/Dnawrkshp/CustomMaps.zip";
            string webVer = @"http://www.cod-orc.com/Dnawrkshp/CMaps.txt";

            Log("Checking for update...");

        updateBoth:
            if (!File.Exists(versTxt) || !File.Exists(zipPath))
            {
                Log("Updating Custom Maps...");
                AsyncDownloader ad = new AsyncDownloader();
                ad.dlPath = webVer;
                ad.savePath = versTxt;
                ad.ShowDialog();
                if (ad.isComplete != 1)
                {
                    Log("Error during download or user aborted");
                    return false;
                }

                ad.dlPath = webZip;
                ad.savePath = zipPath;
                ad.ShowDialog();
                if (ad.isComplete != 1)
                {
                    Log("Error during download or user aborted");
                    return false;
                }

                return true;
            }
            else
            {
                string[] verLines = File.ReadAllLines(versTxt);
                if (verLines.Length <= 0)
                {
                    File.Delete(versTxt);
                    goto updateBoth;
                }
                int vers = int.Parse(verLines[0]);
                AsyncDownloader ad = new AsyncDownloader();
                ad.dlPath = webVer;
                ad.savePath = versTxt;
                ad.ShowDialog();
                if (ad.isComplete != 1)
                {
                    Log("Error during download or user aborted");
                    return false;
                }

                if (int.Parse(File.ReadAllLines(versTxt)[0]) > vers)
                {
                    Log("Updating Custom Maps...");
                    ad.dlPath = webZip;
                    ad.savePath = zipPath;
                    ad.ShowDialog();
                    if (ad.isComplete != 1)
                    {
                        Log("Error during download or user aborted");
                        return false;
                    }
                }

                return true;
            }
        }

        public void Log(string log)
        {
            tbLog.Text += log + "\r\n";
            Application.DoEvents();
        }

        public static void DecompressFile(string file, string directory)
        {
            using (ZipFile archive = ZipFile.Read(file))
            {
                foreach (ZipEntry entry in archive.Entries)
                {
                    try
                    {
                        if (entry.UncompressedSize > 0)
                        {
                            string mergedPath = Path.Combine(directory, entry.FileName);
                            FileInfo fi = new FileInfo(directory);
                            if (!Directory.Exists(fi.Directory.FullName))
                                Directory.CreateDirectory(fi.Directory.FullName);
                            if (File.Exists(mergedPath))
                                File.Delete(mergedPath);
                            entry.Extract(directory, ExtractExistingFileAction.OverwriteSilently);
                        }
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show("Exception: \n" + error.Message);
                    }
                }
            }
        }

        static string CMD(string args)
        {
            string cmdbat = "cd " + Application.StartupPath.Replace("\\", "/") + "\r\n";
            cmdbat += args + " >> out.txt\r\n";
            cmdbat += "exit\r\n";
            File.WriteAllText("cmd.bat", cmdbat);

            System.Diagnostics.Process process = new System.Diagnostics.Process();

            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.Arguments = "";
            startInfo.UseShellExecute = true;
            startInfo.WorkingDirectory = Application.StartupPath;
            startInfo.CreateNoWindow = true;
            startInfo.FileName = Application.StartupPath + "\\cmd.bat";
            process.StartInfo = startInfo;

            process.Start();
            process.WaitForExit();
            System.Threading.Thread.Sleep(5000);

            string cmdOut = File.ReadAllText("out.txt");
            File.Delete("cmd.bat");
            File.Delete("out.txt");
            return cmdOut;
        }

    }
}
