using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autobot;
using Magic.Auto;
using Magic.Core;

namespace Magic.App
{
    public partial class DraftRecorder : Form
    {
        private readonly string magicProgramName = "Magic Online";
        private readonly TimeSpan pauseTime = TimeSpan.FromSeconds(0.5);
        private readonly string rootDirectory = "Screenshot";
        private BackgroundWorker backgroundWorker;

        public DraftRecorder()
        {
            this.InitializeComponent();
            this.CreateRootFolderIfNeeded();
            this.backgroundWorker = new BackgroundWorker();
            this.backgroundWorker.DoWork += new DoWorkEventHandler(this.DoWork);
            this.backgroundWorker.RunWorkerAsync();
        }

        private void Log(string message)
        {
            this.logTextBox.BeginInvoke(new System.Action(() => 
            {
                this.logTextBox.AppendText(message + System.Environment.NewLine);
            }));
        }

        private void CreateRootFolderIfNeeded()
        {
            if (!System.IO.Directory.Exists(this.rootDirectory))
            {
                System.IO.Directory.CreateDirectory(this.rootDirectory);
            }
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            DraftParser draftImageParser = new DraftParser();
            DraftParser.Result lastResult = null;
            string setName = null;
            string subFolder = null;
            this.Log("Starting");
            while (true)
            {
                System.Threading.Thread.Sleep(this.pauseTime);
                if (All.GetForegroundProcess().MainWindowTitle != string.Empty)
                {
                    continue;
                }

                Bitmap bmp = All.ScreenCaptureProcess(this.magicProgramName);
                if (bmp == null)
                {
                    continue;
                }

                FastAccessImage fai = new FastAccessImage(bmp);
                DraftParser.Result result = draftImageParser.Read(fai, setName);
                if (result.HasCards && (lastResult == null || result.Count != lastResult.Count))
                {
                    if (setName == null)
                    {
                        setName = result.Cards.First().SetName;
                        this.setNameCombo.BeginInvoke(new System.Action(() =>
                        {
                            this.setNameCombo.Text = setName;
                        }));
                    }

                    if (subFolder == null || result.Count == 0)
                    {
                        subFolder = this.rootDirectory + "\\" + DateTime.Now.ToFileTime().ToString();
                        System.IO.Directory.CreateDirectory(subFolder);
                        this.folderCombo.BeginInvoke(new System.Action(() =>
                        {
                            this.folderCombo.Text = subFolder;
                        }));
                    }

                    string file = subFolder + "\\" + DateTime.Now.ToFileTime() + ".png";
                    bmp.Save(file, ImageFormat.Png);
                    string logMessage = string.Format("{0} : wrote to {1}", DateTime.Now, file);
                    this.Log(logMessage);
                    lastResult = result;
                }
            }
        }

        private void ScreenShotButtonClick(object sender, EventArgs e)
        {
            Bitmap bmp = All.ScreenCaptureProcess("Magic Online");
            if (bmp == null)
            {
                this.Log("No Screen Found");
                return;
            }

            bmp.Save("Screenshot\\" + System.IO.Path.GetRandomFileName() + ".png", ImageFormat.Png);
            string logMessage = string.Format("{0} : Screen shot saved.", DateTime.Now);
            this.Log(logMessage);
        }

        private void FolderButtonClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", "Screenshot");
        }
    }
}
