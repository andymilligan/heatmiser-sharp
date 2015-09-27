using System;
using System.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using heatmiser_sharp;

namespace heatmiser_sharp.UnitTests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void GetState()
        {
            heatmiser_sharp.State state = new State();

            state.ReadState();
            Assert.IsNotNull(state.TempActual);
            Assert.IsNotNull(state.TempSetting);
        }

        [TestMethod]
        public void SetHotWater()
        {
            heatmiser_sharp.State state = new State();
            state.SetWater(false);
            
        }

        [TestMethod]
        public void SetHeating()
        {
            heatmiser_sharp.State state = new State();
            state.SetHeating(18);

        }


        [TestMethod]
        public void SetHeatingDuration()
        {
            heatmiser_sharp.State state = new State();
            state.SetHeating(16,2,22);
            
        }

    }
}
