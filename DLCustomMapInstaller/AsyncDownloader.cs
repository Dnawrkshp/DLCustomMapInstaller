using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace DLCustomMapInstaller
{
    public partial class AsyncDownloader : Form
    {
        public AsyncDownloader()
        {
            InitializeComponent();
        }

        public string dlPath = "";
        public string savePath = "";
        public int isComplete = 0;

        void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                double bytesIn = double.Parse(e.BytesReceived.ToString());
                double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
                double percentage = bytesIn / totalBytes * 100;
                label1.Text = "Downloaded " + (e.BytesReceived / 1024) + "kb of " + (e.TotalBytesToReceive / 1024) + "kb";
                progressBar1.Value = int.Parse(Math.Truncate(percentage).ToString());
            });
        }
        void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                isComplete = 1;
                Close();
            });
        }

        private void AsyncDownloader_Shown(object sender, EventArgs e)
        {
            if (dlPath == null || dlPath == "" || savePath == null || savePath == "")
            {
                isComplete = 2;
                Close();
            }

            System.Threading.Thread thread = new System.Threading.Thread(() =>
            {
                WebClient c = new WebClient();
                c.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressChanged);
                c.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleted);
                c.DownloadFileAsync(new Uri(dlPath), savePath);
            });
            thread.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isComplete = 2;
            Close();
        }

    }
}
