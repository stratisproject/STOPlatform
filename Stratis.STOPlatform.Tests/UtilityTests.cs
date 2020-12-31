using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stratis.STOPlatform.Controllers;
using Stratis.STOPlatform.Core;
using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using System.Linq;

namespace Stratis.STOPlatform.Tests
{
    [TestClass]
    public class UtilityTests
    {
        [TestMethod]
        public void Get_Controller_Name_From_Controller()
        {
            Utility.ControllerName<SetupController>().Should().Be("Setup");
        }
    }
}
