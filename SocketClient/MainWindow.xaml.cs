using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SocketClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
       public MainWindow()
        {
            InitializeComponent();
        }

        private Socket clientSocket;
        private void connectBtn_Click(object sender, RoutedEventArgs e)
        {
            this.clientSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);

            IPAddress address = IPAddress.Parse("192.168.3.37");
            this.clientSocket.Connect(address, 48889);
            this.ShowMsg("connect ok");
            Thread thread = new Thread(RecviceData);
            thread.IsBackground = true;
            thread.Start(clientSocket);
        }
       
        private void RecviceData(object cientobj)
        {
            Socket cient = cientobj as Socket;
            while (true)
            {
                byte[] buffer = new byte[1024 * 1024 * 2];
                int acdata = cient.Receive(buffer);
                // if (acdata < 1) { break; }
                if (acdata > 0)
                {

                    string sdata = Encoding.UTF8.GetString(buffer, 0, acdata);
                    Console.WriteLine($"receive len={acdata},lenstr={sdata.Length}");                    
                    ShowMsg($"{cient.RemoteEndPoint.ToString()},recivedata={sdata.Trim()}");
                }
                else { break; }
            }
        }
        private void ShowMsg(string log)
        {

            // Console.WriteLine("#87##"+log+", len="+log.Length);           
            this.Dispatcher.Invoke(() => {
                this.infoTxt.AppendText(log + "\r\n");
            });

        }



        private void sendbtn_Click(object sender, RoutedEventArgs e)
        {
            string str = this.msgTxt.Text.Trim();
            byte[] bdata = Encoding.UTF8.GetBytes(str);
            this.clientSocket.Send(bdata);
        }
    }
}
