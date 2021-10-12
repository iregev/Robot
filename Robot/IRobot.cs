using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot
{
    public interface IRobot
    {
        //public Task<bool> Move(int pivotDegrees, double meters);
        public Task<string> MoveStr(int pivotDegrees, double meters);
    }
}
