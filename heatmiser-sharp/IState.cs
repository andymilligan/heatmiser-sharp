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
        /// Reads the temperatures and settings
        /// </summary>
        void ReadState();
        /// <summary>
        /// Sets the hot water state
        /// </summary>
        /// <param name="waterOn"></param>
        void SetWater(bool waterOn);

        /// <summary>
        /// Sets the heating temperature for a fixed duration or until the next until the next programmed time
        /// </summary>
        /// <param name="temp"></param>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        void SetHeating(int temp, int? hours = null, int? minutes = null);

    }
}
