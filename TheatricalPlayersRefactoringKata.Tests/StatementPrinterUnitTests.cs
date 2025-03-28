using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
            var play = new Play("Hamlet", 2000, "tragedy");
            var performance = new Performance("hamlet", 30);
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
            var play = new Play("Hamlet", 2000, "tragedy");
            var performance = new Performance("hamlet", 35);
            var printer = new StatementPrinter();

            // Act: base amount 2000*10 = 20000, mais extra 1000 por audiência acima de 30 (5*1000=5000) = 25000.
            int amount = printer.CalculateAmount(performance, play);

            Assert.AreEqual(25000, amount);
        }

        [TestMethod]
        public void CalculateAmount_Comedy_WithAudienceUnderOrEqual20_ReturnsCorrectAmount()
        {
            // Arrange
            var play = new Play("As You Like It", 1500, "comedy");
            var performance = new Performance("asyoulikeit", 20);
            var printer = new StatementPrinter();

            // Act: base amount = 1500*10 = 15000, mais 300 por audiência = 300*20 = 6000; total = 21000.
            int amount = printer.CalculateAmount(performance, play);

            Assert.AreEqual(21000, amount);
        }

        [TestMethod]
        public void CalculateAmount_Comedy_WithAudienceAbove20_AddsExtraAmount()
        {
            // Arrange
            var play = new Play("As You Like It", 1500, "comedy");
            var performance = new Performance("asyoulikeit", 25);
            var printer = new StatementPrinter();

            // Act: base amount = 1500*10 = 15000.
            // Extra: se audiência > 20, adiciona 10000 + 500*(audiência-20) = 10000 + 500*5 = 12500.
            // Mais adicional de comédia: 300 por audiência = 300*25 = 7500.
            // Total = 15000 + 12500 + 7500 = 35000.
            int amount = printer.CalculateAmount(performance, play);

            Assert.AreEqual(35000, amount);
        }

        [TestMethod]
        public void CalculateAmount_ClampsLinesToMinimumAndMaximum()
        {
            // Arrange
            var playLow = new Play("TestLow", 500, "tragedy");
            var playHigh = new Play("TestHigh", 5000, "tragedy");
            var performance = new Performance("test", 30);
            var printer = new StatementPrinter();

            // Act:
            int amountLow = printer.CalculateAmount(performance, playLow);
            int amountHigh = printer.CalculateAmount(performance, playHigh);

            // Para playLow, lines é limitado a 1000: base = 1000 * 10 = 10000.
            // Para playHigh, lines é limitado a 4000: base = 4000 * 10 = 40000.
            Assert.AreEqual(10000, amountLow);
            Assert.AreEqual(40000, amountHigh);
        }

        [TestMethod]
        public void CalculateVolumeCredits_Tragedy_WithAudienceUnderOrEqual30_ReturnsZero()
        {
            // Arrange
            var play = new Play("Hamlet", 2000, "tragedy");
            var performance = new Performance("hamlet", 30);
            var printer = new StatementPrinter();

            // Act
            int credits = printer.CalculateVolumeCredits(performance, play);

            Assert.AreEqual(0, credits);
        }

        [TestMethod]
        public void CalculateVolumeCredits_Tragedy_WithAudienceAbove30_ReturnsCorrectCredits()
        {
            // Arrange
            var play = new Play("Hamlet", 2000, "tragedy");
            var performance = new Performance("hamlet", 35);
            var printer = new StatementPrinter();

            // Act: créditos = max(35 - 30, 0) = 5.
            int credits = printer.CalculateVolumeCredits(performance, play);

            Assert.AreEqual(5, credits);
        }

        [TestMethod]
        public void CalculateVolumeCredits_Comedy_WithAudienceAbove30_ReturnsCorrectCredits()
        {
            // Arrange
            var play = new Play("As You Like It", 1500, "comedy");
            var performance = new Performance("asyoulikeit", 35);
            var printer = new StatementPrinter();

            // Act: créditos = max(35 - 30, 0) = 5, mais floor(35/5)=7, total = 12.
            int credits = printer.CalculateVolumeCredits(performance, play);

            Assert.AreEqual(12, credits);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CalculateAmount_UnknownPlayType_ThrowsException()
        {
            // Arrange
            var play = new Play("Unknown", 1500, "mystery");
            var performance = new Performance("unknown", 25);
            var printer = new StatementPrinter();

            // Act
            printer.CalculateAmount(performance, play);
        }
    }
}
