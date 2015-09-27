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

            Assert.IsNotNull(state.TempActual);
            Assert.IsNotNull(state.TempSetting);
        }


        [TestMethod]
        public void SetState()
        {
            heatmiser_sharp.State state = new State();
            state.Set(23);

            Assert.IsNotNull(state.TempActual);
            Assert.IsNotNull(state.TempSetting);
        }

        [TestMethod]
        public void SetHotWater()
        {
            heatmiser_sharp.State state = new State();
            state.SetHotWater(false);

            Assert.IsNotNull(state.TempActual);
            Assert.IsNotNull(state.TempSetting);
        }

        [TestMethod]
        public void SetHeating()
        {
            heatmiser_sharp.State state = new State();
            state.SetHeating(18);


            Assert.IsNotNull(state.TempActual);
            Assert.IsNotNull(state.TempSetting);
        }

    }
}
