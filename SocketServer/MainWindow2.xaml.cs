using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace SocketServer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow2 : Window
    {
        static List<Socket> sockets = new List<Socket>();
        public MainWindow2()
        {
            InitializeComponent();
        }
        //监听客户端发来的请求 socketWatch为监听 secketSend为通信
        Socket socketSend;
        //根据IP地址寻扎Socket
        Dictionary<string, Socket> disSocket = new Dictionary<string, Socket>();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IPAddress ip = IPAddress.Any;
                //当开始监听时候，在服务器创建一个负责IP地址跟端口号的Socket
                Socket socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(txtPort.Text));
                IPEndPoint point = new IPEndPoint(ip,48889);
                //监听
                socketWatch.Bind(point);
                ShowMsg("监听成功");
                socketWatch.Listen(10);


                Thread th = new Thread(ListenClickConnect);
                th.IsBackground = true;
                th.Start(socketWatch);
            }
            catch { }
        }
        public void ListenClickConnect(object o)
        {
            Socket socketWatch = o as Socket;
            //等待用户连接创建一个负责通信的Socket
            while (true)
            {
                try
                {
                    socketSend = socketWatch.Accept();//等到客户端新的连接
                                                      //将远程客户端IP地址存储到泛型集合中
                    disSocket.Add(socketSend.RemoteEndPoint.ToString(), socketSend);
                    Thread th = new Thread(Receive);
                    th.IsBackground = true;
                    th.Start(socketSend);
                    ShowMsg(socketSend.RemoteEndPoint.ToString() + ":" + "连接成功");
                    //将远程集合放入下拉框中
                    cmb_select.Items.Add(socketSend.RemoteEndPoint.ToString());
                    //开启一个新的线程不停接受用户发来的消息

                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// 服务器端不停接受发来的消息
        /// </summary>
        /// <param name="o"></param>
        private void Receive(object o)
        {
            socketSend = o as Socket;
            while (true)
            {
                try
                {
                    //客户端连接成功后服务器接收发来消息
                    byte[] buffer = new byte[1054 * 1024 * 2];
                    //实际接收有效字节数
                    int r = socketSend.Receive(buffer);
                    if (r == 0)
                    {
                        break;
                    }
                    string str = Encoding.UTF8.GetString(buffer, 0, r);
                    ShowMsg(socketSend.RemoteEndPoint + ":" + str);
                }
                catch { }
            }
        }
        void ShowMsg(string str)
        {

            Action action1 = () => { txtLog.AppendText(str + "\r\n"); };
            Dispatcher.BeginInvoke(action1);//WPF中只有UI线程才能操作UI元素
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// 服务器给客户端发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                string str = txtMsg.Text;
                List<Byte> list = new List<byte>();
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
                list.Add(0);
                list.AddRange(buffer);
                byte[] newbyte = list.ToArray();
                // socketSend.Send(buffer);
                //获得用户在下拉款选中的IP地址
                string ip = cmb_select.SelectedItem.ToString();
                //disSocket[ip].Send(buffer);
                disSocket[ip].Send(newbyte);
            }
            catch { }
        }
        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //获得要发送文件的路径
            string path = txtbox.Text;
            using (FileStream fsRead = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[1024 * 1024 * 5];//只能发送5MB
                int r = fsRead.Read(buffer, 0, buffer.Length);
                List<byte> list = new List<byte>();
                list.Add(1);
                list.AddRange(buffer);
                byte[] newBuffer = list.ToArray();

                disSocket[cmb_select.SelectedItem.ToString()].Send(newBuffer, 0, r + 1, SocketFlags.None);

            }
        }
        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
          //  ofd.InitialDirectory = @"C:\Users\\Desktop";
            ofd.Title = "请选择你要发送的文件";
            ofd.Filter = "所有文件|*.*";
            ofd.ShowDialog();
            txtbox.Text = ofd.FileName;
        }
        /// <summary>
        /// 发送震动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            byte[] buffer = new byte[1];
            buffer[0] = 2;
            disSocket[cmb_select.SelectedItem.ToString()].Send(buffer);
        }
    }
}


