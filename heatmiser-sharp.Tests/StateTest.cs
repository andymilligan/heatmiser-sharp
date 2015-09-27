// <copyright file="StateTest.cs">Copyright ©  2015</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using heatmiser_sharp;

namespace heatmiser_sharp.Tests
{
    /// <summary>This class contains parameterized unit tests for State</summary>
    [PexClass(typeof(State))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class StateTest
    {
        /// <summary>Test stub for SetHeating(Int32, Nullable`1&lt;Int32&gt;, Nullable`1&lt;Int32&gt;)</summary>
        [PexMethod]
        public void SetHeatingTest(
            [PexAssumeUnderTest]State target,
            int temp,
            int? hours,
            int? minutes
        )
        {
            target.SetHeating(temp, hours, minutes);
            // TODO: add assertions to method StateTest.SetHeatingTest(State, Int32, Nullable`1<Int32>, Nullable`1<Int32>)
        }

        /// <summary>Test stub for SetHotWater(Boolean)</summary>
        [PexMethod]
        public void SetHotWaterTest([PexAssumeUnderTest]State target, bool waterState)
        {
            target.SetHotWater(waterState);
            // TODO: add assertions to method StateTest.SetHotWaterTest(State, Boolean)
        }
    }
}
