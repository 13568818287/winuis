using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Richbox
{
   public class MyTaskTest
    {
        public Task<int> Add(int a ,int b)
        {
            return Task<int>.Run(()=> {
                int c = a + b;
                Task.Delay(100);
                Console.WriteLine("###currentId="+Task.CurrentId+" ,  c="+c+" , a="+a);
                return c;
            });            
        }


        int cc = 0;
        public Task<int> Add2(int a, int b)
        {
            return Task<int>.Run(() => {
                int c = a + b;
                Task.Delay(100);
                cc = c;
                Console.WriteLine("###currentId=" + Task.CurrentId + " ,  c=" + c + " , a=" + a+", cc="+cc);

                return c;
            });
        }
    }
}
