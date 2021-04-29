using MusicApp.Beans;
using MusicApp.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicApp.Control
{
    public partial class ConfigControl : UserControl
    {
        #region Constants
        const int labelHeight = 30;
        const int labelWidth = 200;
        #endregion

        #region UI Parts
        Label pathLabel;
        Label serverEnabledLabel;

        Button pathControl;
        CheckBox serverEnabledControl;
        #endregion

        public ConfigControl()
        {
            InitializeComponent();

            AutoScroll = true;

            InitConfig();
        }

        private void InitConfig()
        {
            pathLabel = new Label() { Text = "Music Path", Width = labelWidth, Height = labelHeight, Location = new Point(10, 10), ForeColor = Color.Purple };
            serverEnabledLabel = new Label() { Text = "Server Enabled", Width = labelWidth, Height = labelHeight, Location = new Point(10, 60), ForeColor = Color.Purple };

            pathControl = new Button() { Text = "Choose", Width = labelWidth, Height = labelHeight, Location = new Point(20 + labelWidth, 10), ForeColor = Color.Purple };
            serverEnabledControl = new CheckBox() { Width = 10, Height = 10, Location = new Point(20 + labelWidth, 20 + labelHeight), ForeColor = Color.Purple, Checked = Configuration.ServerEnabled };
            
            pathControl.Click += PathControl_Click;
            serverEnabledControl.CheckStateChanged += ServerEnabledControl_CheckStateChanged;

            Controls.Add(pathLabel);
            Controls.Add(serverEnabledLabel);
            Controls.Add(pathControl);
            Controls.Add(serverEnabledControl);
        }

        private void ServerEnabledControl_CheckStateChanged(object sender, EventArgs e)
        {
            Configuration.ServerEnabled = serverEnabledControl.Checked;
        }

        private void PathControl_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.ShowDialog();
            string path = folderBrowser.SelectedPath;


            if (!string.IsNullOrEmpty(path))
                Configuration.LibraryPath = path;
        }
    }
}
