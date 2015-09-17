using System;
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
    }
}
