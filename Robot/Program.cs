using System;
using System.Threading.Tasks;

namespace Robot
{
    class Program
    {
        private static robotClient robot = new();

        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
//            var urlTask = Task.Run(() => robot.MoveStr(1, 2));
            var urlTask = Task.Run(() => robot.MoveJson(1, 2));
            var urlResult = urlTask.Result;
            Console.WriteLine("Hello {0}", urlResult);
        }

    }
}
