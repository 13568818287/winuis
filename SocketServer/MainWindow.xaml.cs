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
using System.Collections.ObjectModel;
namespace SocketServer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
       // private ObservableCollection<string> cmdDatas = new ObservableCollection<string>();
        public MainWindow()
        {
            InitializeComponent();
          //  this.clientsCmb.ItemsSource = cmdDatas;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
         //   serverSocket?.Close();
        }
        private Socket socketSend;
        private void connectBtn_Click(object sender, RoutedEventArgs e)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,ProtocolType.Tcp);
            //serverSocket = socket;
            IPAddress localep = IPAddress.Any;

            IPEndPoint end = new IPEndPoint(localep, 48889);
            socket.Bind(end);
            this.ShowMsg("#监听成功#");
            socket.Listen(10);

            Thread thread = new Thread(Recive);
            thread.IsBackground = true;
            thread.Start(socket);
        }
        Dictionary<string, Socket> dicclientsData = new Dictionary<string, Socket>();
        private void Recive(object sk)
        {
            Socket watch = sk as Socket;
            while (true)
            {              
                try {
                    socketSend = watch.Accept();
                    string rp = socketSend.RemoteEndPoint.ToString();
                    ShowMsg(rp + ": connected.");
                    if (this.dicclientsData.ContainsKey(rp))
                    {
                        dicclientsData[rp] = socketSend;
                    }
                    else
                    {
                        dicclientsData.Add(rp, socketSend);
                       // cmdDatas.Add(rp);


                    }
                    Thread thread = new Thread(RecviceData);
                    thread.IsBackground = true;
                    thread.Start(socketSend);

                    this.Dispatcher.Invoke(() => {
                        //将远程集合放入下拉框中
                        this.clientsCmb.Items.Add(socketSend.RemoteEndPoint.ToString());
                    });
                    
                } 
                catch (Exception ex)
                {
                    break;
                }
            }
        }
        private void RecviceData(object cientobj)
        {
            socketSend = cientobj as Socket;
            while (true)
            {
                //客户端连接成功后服务器接收发来消息
                try
                {

                
                byte[] buffer = new byte[1054 * 1024 * 2];
                //实际接收有效字节数
                int r = socketSend.Receive(buffer);
                if (r == 0)
                {
                    break;
                }
                string str = Encoding.UTF8.GetString(buffer, 0, r);
                // string sdata = Encoding.UTF8.GetString(buffer);
                ShowMsg($"{socketSend.RemoteEndPoint.ToString()},recivedata={str.Trim()}");
                }catch(Exception ex)
                {

                }


            }
        }
        
        private void ShowMsg(string log) 
        { 
        
           // Console.WriteLine("#87##"+log+", len="+log.Length);           
            //this.Dispatcher.Invoke(() => {
            //    this.infoTxt.AppendText(log+"\r\n");
            //}); 
            Action action1 = () => { infoTxt.AppendText(log + "\r\n"); };
            Dispatcher.BeginInvoke(action1);//WPF中只有UI线程才能操作UI元素

        }

        private void sendbtn_Click(object sender, RoutedEventArgs e)
        {
            if(null == this.clientsCmb.SelectedItem) { return; }
            string str = this.msgTxt.Text;
            byte[] bdata = Encoding.UTF8.GetBytes(str);

            //  string ip = this.cmdDatas.FirstOrDefault();
            string ip = this.clientsCmb.SelectedItem.ToString();
            Socket sobj = this.dicclientsData[ip];
            sobj.Send(bdata);
          
        }
    }
}

