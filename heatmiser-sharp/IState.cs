using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace heatmiser_sharp
{
    public interface IState
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="waterState"></param>
        void SetWater(bool waterOn);

        /// <summary>
        /// Set the heating temperature for a fixed duration or until the next until the next programmed time.
        /// </summary>
        /// <param name="temp"></param>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        void SetHeating(int temp, int? hours = null, int? minutes = null);

        void ReadState();
    }
}
