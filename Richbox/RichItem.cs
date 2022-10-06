using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

using System.Windows.Media;
using System.Windows.Controls;

namespace WpfApp1.Richbox
{
    public class RichItem:Section
    {
        private static int idx=0;
        public RichItem() {
            this.ID = idx++;
            this.BorderThickness = new System.Windows.Thickness(1, 1, 1, 1);
            this.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF00FF"));
            //new SolidColorBrush(Colors.Red);
          //  this.LineHeight = 20;
        }
        public int ID { set; get; }
        public  bool GetMe(int id)
        {
            return ID == id;
        }
        public void AddImage(string path) 
        {
            Paragraph p = new Paragraph();
            Image img = new Image();
            img.Source = new BitmapImage(new Uri(path));
            p.Inlines.Add(img);
            this.Blocks.Add(p);
        }
        public void AddText(string txt="Hello")
        {
            Span sp = new Span();            
            Run a1 = new Run(txt);
            sp.Inlines.Add(a1);
            Paragraph p = new Paragraph();
            p.Inlines.Add(sp);
            this.Blocks.Add(p);
        }
    }
}

