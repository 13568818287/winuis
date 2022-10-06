using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using WpfApp1.Richbox;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Thread.CurrentThread.Name = "MainThread";
        }
        private Task AddItem(string txt)
        {
            //不会按照顺序0，1，2执行
            return Task.Run(() => {

                Console.WriteLine("#36AddItem#" + Thread.CurrentThread.ManagedThreadId+",name="+Thread.CurrentThread.Name);
                this.Dispatcher.Invoke(() => {
                    Console.WriteLine("#36-1#" + txt);
                    CreateItem(txt);
                });
            });             
        }

        private void AddItem2(string txt)
        {
            //会按照顺序0，1，2执行      

                Console.WriteLine("#49AddItem#" + Thread.CurrentThread.ManagedThreadId );
                this.Dispatcher.Invoke(() => {
                    Console.WriteLine("#49-1#" + txt);
                    CreateItem(txt);
                });      
                 
        }
        private RichItem CreateItem(string txt)
        {
            RichItem item = new RichItem();
            item.AddText(txt);
            this.richbox.Document.Blocks.Add(item);
            return item;
        }
        private void AddImagItem(int i)
        {
            string path;
               // System.MulticastDelegate
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            AddRichItems();
            FindById();
        }
        private void FindById() {
            List<RichItem> custs = new List<RichItem>();
            RichItem item;
            int n = 5;
            for (int i = 1; i < n; i++)
            {
                item = new RichItem();
                item.ID = i;
                item.AddText($"Hello_{i}");
                custs.Add(item);
            }
            int findidx = 5;
            Stopwatch watch=new Stopwatch();
            watch.Start();
            custs.FirstOrDefault(new Func<RichItem, bool>(delegate (RichItem x) { return x.ID == findidx; }));
            watch.Stop();
            Console.WriteLine("#1-1# ms="+watch.ElapsedMilliseconds);
            watch.Restart();
            //在1百万数据中，这种方式运行最快
            custs.FirstOrDefault(new Func<RichItem, bool>((RichItem x) => x.ID == findidx));
            watch.Stop();
            Console.WriteLine("#1-2# ms=" + watch.ElapsedMilliseconds);
            watch.Restart();
            custs.FirstOrDefault(delegate (RichItem x) { return x.ID == findidx; });
            watch.Stop();
            Console.WriteLine("#1-3# ms=" + watch.ElapsedMilliseconds);
            watch.Restart();
            custs.FirstOrDefault((RichItem x) => x.ID == findidx);
            watch.Stop();
            Console.WriteLine("#1-4# ms=" + watch.ElapsedMilliseconds);
            watch.Restart();
            //在1百万数据中，这种方式运行最慢，有4毫秒左右的差距
            custs.FirstOrDefault(x => x.ID == findidx);
            watch.Stop();
            Console.WriteLine("#1-5# ms=" + watch.ElapsedMilliseconds);
            //custs.First(RichItem);
        }
        private void AddRichItems() 
        {
            string msg = this.msg1.Text;
            Task.Factory.StartNew(() => {
              //  Thread.CurrentThread.Name = "ChildThread";
                Console.WriteLine("#56#" + Thread.CurrentThread.Name);
                var n = 3;              
                
                for (int i = 0; i < n; i++)
                {
                    AddItem2(msg + i.ToString());
                }
            });
           

        }

    }
}
