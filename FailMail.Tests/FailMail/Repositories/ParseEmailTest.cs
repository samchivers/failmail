using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using FailMail.FailMail.Helpers;

namespace FailMail.Tests.FailMail.Repositories
{
    [TestClass]
    public class ParseEmailTest
    {
        [TestMethod]
        public void ParseLabelsWithNoLabels()
        {
            // Arrange
            const string message = "This is a test string that has no Labels in it";

            // Act
            var result = ParseEmail.ParseLabels(message);

            // Assert
            CollectionAssert.AreEqual(new List<string>(), result);
        }

        [TestMethod]
        public void ParseLabelsWithOneLabel()
        {
            // Arrange
            const string message = "This is a test string with one Label in [Labels:bug]";
            var correctResult = new List<string>() { "bug" };

            // Act
            var result = ParseEmail.ParseLabels(message);

            // Assert
            CollectionAssert.AreEqual(correctResult, result);
        }
        
        [TestMethod]
        public void ParseLabelWithMultipleLabels()
        {
            // Arrange
            const string message = "This is a test string with multiple labels [Labels:bug,bugs,buggier]";
            var correctResult = new List<string>() { "bug", "bugs", "buggier" };

            // Act
            var result = ParseEmail.ParseLabels(message);

            // Assert
            CollectionAssert.AreEqual(correctResult, result);
        }

        [TestMethod]
        public void ParseLabelWithLabelsIncludingSpaces()
        {
            // Arrange
            const string message = "This is a test string with multiple labels [Labels:bug,bugs,buggier bugs]";
            var correctResult = new List<string>() { "bug", "bugs", "buggier bugs" };

            // Act
            var result = ParseEmail.ParseLabels(message);

            // Assert
            CollectionAssert.AreEqual(correctResult, result);
        }

        [TestMethod]
        public void ParseLabelWithExtraSquareBrackets()
        {
            // Arrange
            const string message = "This is a test string [] with multiple labels [Labels:bug,bugs,buggier bugs]";
            var correctResult = new List<string>() { "bug", "bugs", "buggier bugs" };

            // Act
            var result = ParseEmail.ParseLabels(message);

            // Assert
            CollectionAssert.AreEqual(correctResult, result);
        }

        [TestMethod]
        public void ParseLabelWithSpacesAfterTheComma()
        {
            // Arrange
            const string message = "This is a test string [] with multiple labels [Labels:bug, bugs, buggier bugs]";
            var correctResult = new List<string>() { "bug", "bugs", "buggier bugs" };

            // Act
            var result = ParseEmail.ParseLabels(message);

            // Assert
            CollectionAssert.AreEqual(correctResult, result);
        }

        [TestMethod]
        public void ParseOwnerWithNoOwner()
        {
            // Arrange
            const string message = "This is a test string that has no Labels in it";

            // Act
            var result = ParseEmail.ParseOwner(message);

            // Assert
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void ParseOwnerWithOneOwner()
        {
            // Arrange
            const string message = "This is a test string with one Label in [Owner:githubusername]";
            const string correctResult = "githubusername";

            // Act
            var result = ParseEmail.ParseOwner(message);

            // Assert
            Assert.AreEqual(correctResult, result);
        }

        [TestMethod]
        public void ParseLabelsAndOwner()
        {
            // Arrange
            const string message = "This is a string with an [Owner:githubusername] and several [Labels:bug, buggy, buggier bug] that should be parsed correctly";
            const string correctOwner = "githubusername";
            var correctLabels = new List<string>() { "bug", "buggy", "buggier bug" };

            // Act
            var ownerResult = ParseEmail.ParseOwner(message);
            var labelsResult = ParseEmail.ParseLabels(message);

            // Assert
            Assert.AreEqual(correctOwner, ownerResult);
            CollectionAssert.AreEqual(correctLabels, labelsResult);
        }

        [TestMethod]
        public void ConvertHtmlToMarkdown()
        {
            // Arrange
            const string message = "This a sample <strong>paragraph</strong> from <a href=\"http://test.com\">my site</a>";
            const string correctMarkdown = "This a sample **paragraph** from [my site](http://test.com)";

            // Act
            var result = ParseEmail.ConvertHtmlToMarkdown(message);

            // Assert
            Assert.AreEqual(correctMarkdown, result);
        }
    }
}
