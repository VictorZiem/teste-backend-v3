using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TheatricalPlayersRefactoringKata;

namespace TheatricalPlayersRefactoringKata.Tests
{
    [TestClass]
    public class StatementPrinterUnitTests
    {
        [TestMethod]
        public void CalculateAmount_Tragedy_WithAudienceUnderOrEqual30_ReturnsBaseAmount()
        {
            // Arrange
            var play = new Play { Name = "Hamlet", Type = "tragedy", Lines = 2000 };
            var performance = new Performance { PlayId = "hamlet", Audience = 30 };
            var printer = new StatementPrinter();

            // Act
            int amount = printer.CalculateAmount(performance, play);

            // Base amount: lines * 10 = 2000 * 10 = 20000.
            Assert.AreEqual(20000, amount);
        }

        [TestMethod]
        public void CalculateAmount_Tragedy_WithAudienceAbove30_AddsExtraAmount()
        {
            // Arrange
            var play = new Play { Name = "Hamlet", Type = "tragedy", Lines = 2000 };
            var performance = new Performance { PlayId = "hamlet", Audience = 35 };
            var printer = new StatementPrinter();

            // Act: base amount 2000*10 = 20000, plus extra 1000 per audience above 30 (5*1000=5000) = 25000.
            int amount = printer.CalculateAmount(performance, play);

            Assert.AreEqual(25000, amount);
        }

        [TestMethod]
        public void CalculateAmount_Comedy_WithAudienceUnderOrEqual20_ReturnsCorrectAmount()
        {
            // Arrange
            var play = new Play { Name = "As You Like It", Type = "comedy", Lines = 1500 };
            var performance = new Performance { PlayId = "asyoulikeit", Audience = 20 };
            var printer = new StatementPrinter();

            // Act: base amount = 1500*10 = 15000, plus 300 per audience = 300*20 = 6000; total = 21000.
            int amount = printer.CalculateAmount(performance, play);

            Assert.AreEqual(21000, amount);
        }

        [TestMethod]
        public void CalculateAmount_Comedy_WithAudienceAbove20_AddsExtraAmount()
        {
            // Arrange
            var play = new Play { Name = "As You Like It", Type = "comedy", Lines = 1500 };
            var performance = new Performance { PlayId = "asyoulikeit", Audience = 25 };
            var printer = new StatementPrinter();

            // Act: base amount = 1500*10 = 15000.
            // Extra: if audience >20, add 10000 + 500*(audience-20) = 10000 + 500*5 = 12500.
            // Plus additional comedy amount: 300 per audience = 300*25 = 7500.
            // Total = 15000 + 12500 + 7500 = 35000.
            int amount = printer.CalculateAmount(performance, play);

            Assert.AreEqual(35000, amount);
        }

        [TestMethod]
        public void CalculateAmount_ClampsLinesToMinimumAndMaximum()
        {
            // Arrange
            var playLow = new Play { Name = "TestLow", Type = "tragedy", Lines = 500 };
            var playHigh = new Play { Name = "TestHigh", Type = "tragedy", Lines = 5000 };
            var performance = new Performance { PlayId = "test", Audience = 30 };
            var printer = new StatementPrinter();

            // Act:
            int amountLow = printer.CalculateAmount(performance, playLow);
            int amountHigh = printer.CalculateAmount(performance, playHigh);

            // For playLow, lines clamped to 1000 => base = 1000 * 10 = 10000.
            // For playHigh, lines clamped to 4000 => base = 4000 * 10 = 40000.
            Assert.AreEqual(10000, amountLow);
            Assert.AreEqual(40000, amountHigh);
        }

        [TestMethod]
        public void CalculateVolumeCredits_Tragedy_WithAudienceUnderOrEqual30_ReturnsZero()
        {
            // Arrange
            var play = new Play { Name = "Hamlet", Type = "tragedy", Lines = 2000 };
            var performance = new Performance { PlayId = "hamlet", Audience = 30 };
            var printer = new StatementPrinter();

            // Act
            int credits = printer.CalculateVolumeCredits(performance, play);

            Assert.AreEqual(0, credits);
        }

        [TestMethod]
        public void CalculateVolumeCredits_Tragedy_WithAudienceAbove30_ReturnsCorrectCredits()
        {
            // Arrange
            var play = new Play { Name = "Hamlet", Type = "tragedy", Lines = 2000 };
            var performance = new Performance { PlayId = "hamlet", Audience = 35 };
            var printer = new StatementPrinter();

            // Act: credits = max(35 - 30, 0) = 5.
            int credits = printer.CalculateVolumeCredits(performance, play);

            Assert.AreEqual(5, credits);
        }

        [TestMethod]
        public void CalculateVolumeCredits_Comedy_WithAudienceAbove30_ReturnsCorrectCredits()
        {
            // Arrange
            var play = new Play { Name = "As You Like It", Type = "comedy", Lines = 1500 };
            var performance = new Performance { PlayId = "asyoulikeit", Audience = 35 };
            var printer = new StatementPrinter();

            // Act: credits = max(35 - 30, 0) = 5, plus floor(35/5)=7, total = 12.
            int credits = printer.CalculateVolumeCredits(performance, play);

            Assert.AreEqual(12, credits);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CalculateAmount_UnknownPlayType_ThrowsException()
        {
            // Arrange
            var play = new Play { Name = "Unknown", Type = "mystery", Lines = 1500 };
            var performance = new Performance { PlayId = "unknown", Audience = 25 };
            var printer = new StatementPrinter();

            // Act
            printer.CalculateAmount(performance, play);
        }
    }
}
