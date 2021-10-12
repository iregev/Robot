using System;
using System.Threading.Tasks;

namespace Robot
{
    class Program
    {
        
        public static void Main(string[] args)
        {
            var x = "0";
            var y = "0";
            var webserver = "http://172.16.0.1:8001/WebService1/move_webservice";
            
            if (args.Length == 2)
            {
                x = args[0];
                y = args[1];
            }
            Console.WriteLine("X:{0} Y:{1}", x, y);
            Console.WriteLine("Webserver: {0}", webserver);
            robotClient robot = new(webserver);

            var urlTask = Task.Run(() => robot.Move(Int16.Parse(x), Int16.Parse(y)));
            var urlResult = urlTask.Result;
            Console.WriteLine("Result-- x:{0}, y:{1}", urlResult[0], urlResult[1]);
        }

    }
}
