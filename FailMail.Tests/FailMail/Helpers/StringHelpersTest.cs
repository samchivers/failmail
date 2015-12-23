using System;
using FailMail.FailMail.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FailMail.Tests.FailMail.Helpers
{
    [TestClass]
    public class StringHelpersTest
    {
        [TestMethod]
        public void TestContainsAny()
        {
            // Arrange
            const string testString1 = "this is a string that contains RE: and FW:";

            // Act
            var result1 = testString1.ContainsAny("RE:", "FW:", "Test");
            var result2 = testString1.ContainsAny("Stuff", "not", "present");

            // Assert
            Assert.AreEqual(true, result1);
            Assert.AreEqual(false, result2);
        }
    }
}
