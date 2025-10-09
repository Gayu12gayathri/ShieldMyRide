using NUnit.Framework;
using ShieldMyRide.Services;

namespace ShieldMyRide.Tests
{
    [TestFixture]
    public class PremiumCalculatorTests
    {
        private PremiumCalculator _calculator;

        [SetUp]
        public void Setup()
        {
            _calculator = new PremiumCalculator();
        }

        [Test]
        public void Calculate_ShouldApplyFactorsCorrectly_ForCarAndNewVehicle()
        {
            // Arrange
            string breakdown;
            decimal vehicleValue = 100000; // 1 lakh
            int vehicleAge = 5; // <= 5 years (no age loading)
            string vehicleType = "car";

            // Act
            var premium = _calculator.Calculate(vehicleType, vehicleAge, vehicleValue, out breakdown);

            // Expected calculation
            decimal basePremium = vehicleValue * 0.03m; // 3000
            decimal addons = 0;
            decimal discount = 0;
            decimal subtotal = basePremium + addons - discount; // 3000
            decimal tax = subtotal * 0.18m; // 540
            decimal expectedPremium = subtotal + tax; // 3540

            // Assert
            Assert.That(premium, Is.EqualTo(expectedPremium));
            Assert.That(breakdown, Does.Contain("Base"));
            Assert.That(breakdown, Does.Contain("Add-ons"));
            Assert.That(breakdown, Does.Contain("Discount"));
        }

        [Test]
        public void Calculate_ShouldIncreasePremium_ForOldTruck()
        {
            // Arrange
            string breakdown;
            decimal vehicleValue = 500000; // 5 lakh
            int vehicleAge = 15; // old vehicle
            string vehicleType = "truck";

            // Act
            var premium = _calculator.Calculate(vehicleType, vehicleAge, vehicleValue, out breakdown);

            // Expected
            decimal basePremium = vehicleValue * 0.04m; // 20,000
            basePremium += basePremium * 0.10m; // 22,000
            decimal subtotal = basePremium;
            decimal tax = subtotal * 0.18m; // 3960
            decimal expectedPremium = subtotal + tax; // 25,960

            // Assert
            Assert.That(premium, Is.EqualTo(expectedPremium));
            Assert.That(breakdown, Does.Contain("Base"));
            Assert.That(breakdown, Does.Contain("Tax"));
        }

        [Test]
        public void Calculate_ShouldReturnBasePremium_ForUnknownVehicleType()
        {
            // Arrange
            string breakdown;
            decimal vehicleValue = 200000;
            int vehicleAge = 3;
            string vehicleType = "scooter"; // falls back to default 2%

            // Act
            var premium = _calculator.Calculate(vehicleType, vehicleAge, vehicleValue, out breakdown);

            // Expected
            decimal basePremium = vehicleValue * 0.02m; // 4000
            decimal subtotal = basePremium;
            decimal tax = subtotal * 0.18m; // 720
            decimal expectedPremium = subtotal + tax; // 4720

            // Assert
            Assert.That(premium, Is.EqualTo(expectedPremium));
            Assert.That(breakdown, Does.Contain("Base"));
            Assert.That(breakdown, Does.Contain("Total"));
        }

        [Test]
        public void Calculate_ShouldIncludeAddonsAndDiscounts_Correctly()
        {
            // Arrange
            string breakdown;
            decimal vehicleValue = 300000; // 3 lakh
            int vehicleAge = 2;
            string vehicleType = "car";
            bool zeroDep = true;
            bool roadsideAssist = true;
            int ncbPercent = 20; // 20% discount

            // Act
            var premium = _calculator.Calculate(
                vehicleType, vehicleAge, vehicleValue,
                out breakdown, zeroDep, roadsideAssist, ncbPercent);

            // Expected
            decimal basePremium = vehicleValue * 0.03m; // 9000
            decimal addons = (basePremium * 0.15m) + 500; // 1850
            decimal discount = basePremium * 0.20m; // 1800
            decimal subtotal = basePremium + addons - discount; // 9050
            decimal tax = subtotal * 0.18m; // 1629
            decimal expectedPremium = subtotal + tax; // 10679

            // Assert
            Assert.That(premium, Is.EqualTo(expectedPremium));
            Assert.That(breakdown, Does.Contain("Base"));
            Assert.That(breakdown, Does.Contain("Add-ons"));
            Assert.That(breakdown, Does.Contain("Discount"));
            Assert.That(breakdown, Does.Contain("Tax"));
            Assert.That(breakdown, Does.Contain("Total"));
        }

        [Test]
        public void Calculate_ShouldWorkForBrandNewVehicle_AgeZero()
        {
            // Arrange
            string breakdown;
            decimal vehicleValue = 150000;
            int vehicleAge = 0; // brand new
            string vehicleType = "bike";

            // Act
            var premium = _calculator.Calculate(vehicleType, vehicleAge, vehicleValue, out breakdown);

            // Expected
            decimal basePremium = vehicleValue * 0.015m; // 2250
            decimal subtotal = basePremium;
            decimal tax = subtotal * 0.18m; // 405
            decimal expectedPremium = subtotal + tax; // 2655

            // Assert
            Assert.That(premium, Is.EqualTo(expectedPremium));
            Assert.That(breakdown, Does.Contain("Base"));
            Assert.That(breakdown, Does.Contain("Total"));
        }

        [Test]
        public void Calculate_ShouldHandleFullNCB_Discount()
        {
            // Arrange
            string breakdown;
            decimal vehicleValue = 100000;
            int vehicleAge = 3;
            string vehicleType = "car";
            int ncbPercent = 100; // full discount

            // Act
            var premium = _calculator.Calculate(
                vehicleType, vehicleAge, vehicleValue,
                out breakdown, false, false, ncbPercent);

            // Expected
            decimal basePremium = vehicleValue * 0.03m; // 3000
            decimal discount = basePremium * 1.0m; // 3000
            decimal subtotal = basePremium - discount; // 0
            decimal tax = subtotal * 0.18m; // 0
            decimal expectedPremium = subtotal + tax; // 0

            // Assert
            Assert.That(premium, Is.EqualTo(expectedPremium));
            Assert.That(breakdown, Does.Contain("Discount"));
            Assert.That(breakdown, Does.Contain("Total"));
        }
    }
}
