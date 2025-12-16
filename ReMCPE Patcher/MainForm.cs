using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReMCPE_Patcher
{
    public partial class MainForm : Form
    {
        public class PatchConsentForm : Form
        {
            public bool UserAgreed { get; private set; } = false;

            public PatchConsentForm(string patchNotes)
            {
                this.Text = "Patch Confirmation";
                this.Width = 600;
                this.Height = 400;
                this.StartPosition = FormStartPosition.CenterScreen;
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.MaximizeBox = false;
                this.MinimizeBox = false;

                TextBox notesBox = new TextBox()
                {
                    Multiline = true,
                    ReadOnly = true,
                    ScrollBars = ScrollBars.Vertical,
                    WordWrap = true,
                    Dock = DockStyle.Top,
                    Height = 300,
                    Text = patchNotes
                };
                this.Controls.Add(notesBox);

                FlowLayoutPanel panel = new FlowLayoutPanel()
                {
                    Dock = DockStyle.Bottom,
                    FlowDirection = FlowDirection.RightToLeft,
                    Height = 50,
                    Padding = new Padding(10)
                };

                Button yesBtn = new Button() { Text = "Yes", Width = 100 };
                yesBtn.Click += (s, e) => { UserAgreed = true; this.Close(); };

                Button noBtn = new Button() { Text = "No", Width = 100 };
                noBtn.Click += (s, e) => { UserAgreed = false; this.Close(); };

                panel.Controls.Add(yesBtn);
                panel.Controls.Add(noBtn);
                this.Controls.Add(panel);
            }

            public static bool Show(string patchNotes)
            {
                using (var form = new PatchConsentForm(patchNotes))
                {
                    form.ShowDialog();
                    return form.UserAgreed;
                }
            }

            private void InitializeComponent()
            {
                this.SuspendLayout();
                this.ClientSize = new System.Drawing.Size(284, 261);
                this.Name = "PatchConsentForm";
                this.ResumeLayout(false);
            }
        }

        private static readonly string patchRootDefault = "http://YOUR_HOST_HERE.xyz";
        private static string patchRoot = patchRootDefault;

        private static string SanitizeRelativePath(string input)
        {
            // Split path into parts
            var parts = input.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            for (int i = 0; i < parts.Length; i++)
            {
                // Replace invalid filename chars in each part
                foreach (char c in Path.GetInvalidFileNameChars())
                    parts[i] = parts[i].Replace(c, '_');
            }

            // Recombine safely using Path.Combine
            return Path.Combine(parts);
        }

        private static string GetSafeDownloadPath(string baseDir, string untrustedPath)
        {
            string safeRelative = SanitizeRelativePath(untrustedPath);
            string fullPath = Path.GetFullPath(Path.Combine(baseDir, safeRelative));

            // Ensure the final path is inside the baseDir
            if (!fullPath.StartsWith(baseDir + Path.DirectorySeparatorChar))
                Environment.FailFast("Untrusted content server. Exiting.");

            return fullPath;
        }



        private static WebClient client = new WebClient();
        public static void DownloadFile(string url, string outputPath)
        {
            try
            {
                client.Headers["User-Agent"] = "ReMCPEPatcher/1.0";
                client.DownloadFile(url, outputPath);
            }
            catch
            {

            }
        }

        public static void DownloadFileAsync(string url, string outputPath, Action onComplete, Action<Exception> onError)
        {
            try
            {
                client.Headers["User-Agent"] = "ReMCPEPatcher/1.0";
                client.DownloadFileCompleted += (s, e) =>
                {
                    if (e.Error != null) onError?.Invoke(e.Error);
                    else onComplete?.Invoke();
                };
                client.DownloadFileAsync(new Uri(url), outputPath);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
            }
        }

        public static string ShowInputDialog(string title, string defaultValue = "")
        {
            Form prompt = new Form()
            {
                Width = 400,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = title,
                StartPosition = FormStartPosition.WindowsDefaultLocation,
                MaximizeBox = false,
                MinimizeBox = false
            };

            TextBox inputBox = new TextBox() { Left = 50, Top = 20, Width = 300, Text = defaultValue };
            Button confirmation = new Button() { Text = "Ok", Left = 250, Width = 100, Top = 50, DialogResult = DialogResult.OK };

            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(inputBox);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? inputBox.Text : null;
        }

        public static string DownloadText(string url)
        {
            try
            {
                client.Headers["User-Agent"] = "ReMCPEPatcher/1.0";
                return client.DownloadString(url);
            }
            catch
            {
            }

            return "";
        }


        // if you want to use compression later on and download a single patch zip, for now it just downloads each file separately cause it's good enough I guess :P
        public static Dictionary<string, byte[]> UnzipToMemory(byte[] zipData)
        {
            var files = new Dictionary<string, byte[]>();

            using (var zipStream = new MemoryStream(zipData))
            using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Read))
            {
                foreach (var entry in archive.Entries)
                {
                    // Skip directories
                    if (string.IsNullOrEmpty(entry.Name))
                        continue;

                    using (var entryStream = entry.Open())
                    using (var output = new MemoryStream())
                    {
                        entryStream.CopyTo(output);
                        files[entry.FullName] = output.ToArray();
                    }
                }
            }

            return files;
        }


        public MainForm()
        {
            InitializeComponent();
        }

        private void SelectFolderModern()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select the mcpe folder (root of the source folder)";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    workingDirLbl.Text = dialog.SelectedPath;
                    actionBtn.Text = "Patch!";
                }
            }
        }

        private void StartPatchingFiles(Dictionary<string, string> patchMap)
        {
            Task.Run(() =>
            {
                using (var client = new WebClient())
                {
                    client.Headers["User-Agent"] = "ReMCPEPatcher/1.0";

                    int totalFiles = patchMap.Count;
                    int completedFiles = 0;

                    foreach (var kvp in patchMap)
                    {
                        string patchFile = kvp.Key;
                        string targetPath = kvp.Value;

                        this.Invoke((Action)(() =>
                        {
                            statusLbl.Text = $"Status: Downloading file {patchFile}...";
                        }));

                        string outputPath = GetSafeDownloadPath(workingDirLbl.Text, targetPath);

                        try
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

                            byte[] fileData = client.DownloadData(patchRoot + "/" + patchFile);
                            File.WriteAllBytes(outputPath, fileData);

                            completedFiles++;
                            this.Invoke((Action)(() =>
                            {
                                progressBar.Value = (int)((completedFiles / (double)totalFiles) * 100);
                            }));
                        }
                        catch (Exception ex)
                        {
                            this.Invoke((Action)(() =>
                            {
                                MessageBox.Show(
                                    $"Failed to download or write file {patchFile}: {ex.Message}",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error
                                );
                                statusLbl.Text = "Status: Waiting for input...";
                            }));
                            return;
                        }
                    }

                    this.Invoke((Action)(() =>
                    {
                        statusLbl.Text = "Status: Patch completed!";
                        actionBtn.Enabled = true;
                        progressBar.Value = 100;
                        completedFiles = 0;
                    }));
                }
            });
        }

        private void StartPatching()
        {
            actionBtn.Enabled = false;
            statusLbl.Text = "Status: Downloading patch...";


            string patchMapData = DownloadText(patchRoot + "/patch_map.txt");
            if (string.IsNullOrEmpty(patchMapData))
            {
                MessageBox.Show("Failed to download patch map. Is this a valid patch URL?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                actionBtn.Enabled = true;
                statusLbl.Text = "Status: Failure!";
                return;
            }

            var patchMap = new Dictionary<string, string>(); // File within patch -> Target output path on top of "mcpe" root

            foreach (var line in patchMapData.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = line.Split(new[] { '|' }, 2);
                if (parts.Length == 2)
                {
                    patchMap[parts[0]] = parts[1];
                }
            }

            string patchNotes = "";
            foreach (var kvp in patchMap)
            {
                patchNotes += $"{kvp.Key} → {kvp.Value}\r\n";
            }

            // Ask user if they agree:
            if (PatchConsentForm.Show(patchNotes))
            {
                StartPatchingFiles(patchMap);
            }
            else
            {
                MessageBox.Show("Patch canceled by user.", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                actionBtn.Enabled = true;
                statusLbl.Text = "Status: Waiting for input...";
            }
        }

        private void selectDirBtn_Click(object sender, EventArgs e)
        {
            if (actionBtn.Text == "Patch!")
                StartPatching();
            else 
                SelectFolderModern();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void settingsLbl_Click(object sender, EventArgs e)
        {
            patchRoot = ShowInputDialog("Set Patch Server URL", patchRoot);

            if (string.IsNullOrEmpty(patchRoot))
                patchRoot = patchRootDefault;
        }

        private void gitLinkLbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/ReMinecraftPE");
        }
    }
}
