using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using FailMail.FailMail.Helpers;
using Xunit;

namespace FailMail.Tests.FailMail.Helpers
{
    [TestClass]
    public class ParseEmailTest
    {
        [TestMethod]
        [Fact]
        public void ParseLabelsWithNoLabels()
        {
            // Arrange
            const string message = "This is a test string that has no Labels in it";

            // Act
            var result = ParseEmailHelper.ParseLabels(message);

            // Assert
            CollectionAssert.AreEqual(new List<string>(), result);
            Xunit.Assert.Equal(new List<string>(), result);
        }

        [TestMethod]
        [Fact]
        public void ParseLabelsWithOneLabel()
        {
            // Arrange
            const string message = "This is a test string with one Label in [Labels:bug]";
            var correctResult = new List<string>() { "bug" };

            // Act
            var result = ParseEmailHelper.ParseLabels(message);

            // Assert
            CollectionAssert.AreEqual(correctResult, result);
            Xunit.Assert.Equal(correctResult, result);
        }
        
        [TestMethod]
        [Fact]
        public void ParseLabelWithMultipleLabels()
        {
            // Arrange
            const string message = "This is a test string with multiple labels [Labels:bug,bugs,buggier]";
            var correctResult = new List<string>() { "bug", "bugs", "buggier" };

            // Act
            var result = ParseEmailHelper.ParseLabels(message);

            // Assert
            CollectionAssert.AreEqual(correctResult, result);
            Xunit.Assert.Equal(correctResult, result);
        }

        [TestMethod]
        [Fact]
        public void ParseLabelWithLabelsIncludingSpaces()
        {
            // Arrange
            const string message = "This is a test string with multiple labels [Labels:bug,bugs,buggier bugs]";
            var correctResult = new List<string>() { "bug", "bugs", "buggier bugs" };

            // Act
            var result = ParseEmailHelper.ParseLabels(message);

            // Assert
            CollectionAssert.AreEqual(correctResult, result);
            Xunit.Assert.Equal(correctResult, result);
        }

        [TestMethod]
        [Fact]
        public void ParseLabelWithExtraSquareBrackets()
        {
            // Arrange
            const string message = "This is a test string [] with multiple labels [Labels:bug,bugs,buggier bugs]";
            var correctResult = new List<string>() { "bug", "bugs", "buggier bugs" };

            // Act
            var result = ParseEmailHelper.ParseLabels(message);

            // Assert
            CollectionAssert.AreEqual(correctResult, result);
            Xunit.Assert.Equal(correctResult, result);
        }

        [TestMethod]
        [Fact]
        public void ParseLabelWithSpacesAfterTheComma()
        {
            // Arrange
            const string message = "This is a test string [] with multiple labels [Labels:bug, bugs, buggier bugs]";
            var correctResult = new List<string>() { "bug", "bugs", "buggier bugs" };

            // Act
            var result = ParseEmailHelper.ParseLabels(message);

            // Assert
            CollectionAssert.AreEqual(correctResult, result);
            Xunit.Assert.Equal(correctResult, result);
        }

        [TestMethod]
        [Fact]
        public void ParseOwnerWithNoOwner()
        {
            // Arrange
            const string message = "This is a test string that has no Labels in it";

            // Act
            var result = ParseEmailHelper.ParseOwner(message);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual("", result);
            Xunit.Assert.Equal("", result);
        }

        [TestMethod]
        [Fact]
        public void ParseOwnerWithOneOwner()
        {
            // Arrange
            const string message = "This is a test string with one Label in [Owner:githubusername]";
            const string correctResult = "githubusername";

            // Act
            var result = ParseEmailHelper.ParseOwner(message);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(correctResult, result);
            Xunit.Assert.Equal(correctResult, result);
        }

        [TestMethod]
        [Fact]
        public void ParseLabelsAndOwner()
        {
            // Arrange
            const string message = "This is a string with an [Owner:githubusername] and several [Labels:bug, buggy, buggier bug] that should be parsed correctly";
            const string correctOwner = "githubusername";
            var correctLabels = new List<string>() { "bug", "buggy", "buggier bug" };

            // Act
            var ownerResult = ParseEmailHelper.ParseOwner(message);
            var labelsResult = ParseEmailHelper.ParseLabels(message);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(correctOwner, ownerResult);
            Xunit.Assert.Equal(correctOwner, ownerResult);

            CollectionAssert.AreEqual(correctLabels, labelsResult);
            Xunit.Assert.Equal(correctLabels, labelsResult);
        }

        [TestMethod]
        [Fact]
        public void ConvertHtmlToMarkdown()
        {
            // Arrange
            const string message = "This a sample <strong>paragraph</strong> from <a href=\"http://test.com\">my site</a>";
            const string correctMarkdown = "This a sample **paragraph** from [my site](http://test.com)";

            // Act
            var result = ParseEmailHelper.ConvertHtmlToMarkdown(message);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(correctMarkdown, result);
            Xunit.Assert.Equal(correctMarkdown, result);
        }

        [TestMethod]
        [Fact]
        public void ParseTargetRepo()
        {
            // Arrange
            const string message = "This is a sample message containing a target repo of [Repo:testrepo]";
            const string correctRepo = "testrepo";

            // Act
            var result = ParseEmailHelper.ParseTargetRepository(message);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(correctRepo, result);
            Xunit.Assert.Equal(correctRepo, result);
        }

        [TestMethod]
        [Fact]
        public void ParseLabelsAndOwnerAndRepo()
        {
            // Arrange
            const string message = "[Repo:sometestrepo][Owner:githubusername][Labels:bug,bug2,bug3, bug 5, bug-six] this is the rest of the email....";
            const string correctRepo = "sometestrepo";
            const string correctOwner = "githubusername";
            var correctLabels = new List<string>() { "bug", "bug2", "bug3", "bug 5", "bug-six" };

            // Act
            var repoResult = ParseEmailHelper.ParseTargetRepository(message);
            var ownerResult = ParseEmailHelper.ParseOwner(message);
            var labelsResult = ParseEmailHelper.ParseLabels(message);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(correctRepo, repoResult);
            Xunit.Assert.Equal(correctRepo, repoResult);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(correctOwner, ownerResult);
            Xunit.Assert.Equal(correctOwner, ownerResult);
            CollectionAssert.AreEqual(correctLabels, labelsResult);
            Xunit.Assert.Equal(correctLabels, labelsResult);
        }

        [TestMethod]
        [Fact]
        public void RemoveForwardOrReplyFromSubject()
        {
            // Arrange
            const string subject1 = "FW: some test subject";
            const string subject2 = "RE: some test subject";
            const string subject3 = "some test subject";
            const string correctSubject = "some test subject";

            // Act
            var result1 = ParseEmailHelper.RemoveFowardOrReplyCharactersFromSubject(subject1);
            var result2 = ParseEmailHelper.RemoveFowardOrReplyCharactersFromSubject(subject2);
            var result3 = ParseEmailHelper.RemoveFowardOrReplyCharactersFromSubject(subject3);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(correctSubject, result1);
            Xunit.Assert.Equal(correctSubject, result1);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(correctSubject, result2);
            Xunit.Assert.Equal(correctSubject, result2);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(correctSubject, result3);
            Xunit.Assert.Equal(correctSubject, result3);
        }
    }
}
