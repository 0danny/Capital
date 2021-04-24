using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital.Core.IO
{
    public class LoggerFactory
    {
        public static void debug(object c, object p)
        {
            Console.WriteLine("[" + DateTime.Now.ToString("hh:mm:ss tt") + "] [" + c.GetType().Name + "]: " + p);
        }
    }
}
