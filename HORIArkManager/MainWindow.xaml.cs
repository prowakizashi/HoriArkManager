using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HORIArkManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("HoriArkManager");
            if (key != null)
            {
                ProfileDir.Text = (string)key.GetValue("ProfilePath");
                ExecDir.Text = (string)key.GetValue("ExecPath");
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var files = Directory.GetFiles(ProfileDir.Text);
                var targetDir = System.IO.Path.GetDirectoryName(ExecDir.Text);
                targetDir = System.IO.Path.GetFullPath(System.IO.Path.Combine(targetDir,  "../../Saved/Config/WindowsServer/"));

                var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("HoriArkManager");
                key.SetValue("ProfilePath", ProfileDir.Text);
                key.SetValue("ExecPath", ExecDir.Text);
                key.Close();


                foreach (var file in files)
                {
                    file.Contains(".ini");
                    string fileName = System.IO.Path.GetFileName(file);
                    var destPath = System.IO.Path.Combine(targetDir, fileName);
                    var sourcePath = System.IO.Path.Combine(ProfileDir.Text, file);
                    var text = File.ReadAllBytes(sourcePath);
                    File.WriteAllBytes(destPath, text);
                }

                WriteConsole(ExecDir.Text);
                Process.Start(ExecDir.Text, "TheCenter?listen?MultiHome=192.168.0.3?Port=7777?QueryPort=27015?MaxPlayers=70 -nosteamclient -game -server -log");
            }
            catch (Exception ex)
            {
                WriteConsole(ex.Message);
                WriteConsole(ex.StackTrace);
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("mcrcon.exe -c -H 192.168.0.3 -P 32330 -p snubble2017 \"broadcast Hello World !\"");
        }

        private void ProfileButton_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ProfileDir.Text = dialog.SelectedPath;
                dialog.Dispose();
            }
        }

        private void ExecDirButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ExecDir.Text = dialog.FileName;
                dialog.Dispose();
            }
        }


        void WriteConsole(string text)
        {
            if (Console.Text.Length > 0)
                Console.Text += "\n";
            Console.Text += "\n" + text;
        }
    }
}
