using System.ComponentModel;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using WebSocketSharp;

namespace SimpleNoisedTest
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly WebSocket ws;

        public MainWindow()
        {
            InitializeComponent();

            ws = new WebSocket("ws://localhost:1338");
            ws.OnMessage += ws_OnMessage;
            ws.OnError += ws_OnError;

            MessageBox.Show("Please provide a file with a login command");
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            string file = ofd.FileName;
            SendCommand(File.ReadAllText(file));
        }

        private static void ws_OnError(object sender, WebSocketSharp.ErrorEventArgs e)
        {
            MessageBox.Show(e.Message);
        }

        private static void ws_OnMessage(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Data);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            string file = ofd.FileName;
            SendCommand(File.ReadAllText(file));
        }

        private void SendCommand(string command)
        {
            if (!ws.IsAlive)
                ws.Connect();
            ws.Send(command);
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (ws.IsAlive)
                ws.Close();
        }
    }
}
