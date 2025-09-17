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
            int vehicleAge = 5;
            string vehicleType = "car";

            // Act
            var premium = _calculator.Calculate(vehicleType, vehicleAge, vehicleValue, out breakdown);

            // Assert
            decimal expectedBase = vehicleValue * 0.02m; // 2000
            decimal expectedPremium = expectedBase * 1.2m * 1.0m; // vehicle factor 1.2, age factor 1.0

            Assert.That(premium, Is.EqualTo(expectedPremium));
            Assert.That(breakdown, Does.Contain("Base"));
            Assert.That(breakdown, Does.Contain("VehicleFactor"));
            Assert.That(breakdown, Does.Contain("AgeFactor"));
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

            // Assert
            decimal expectedBase = vehicleValue * 0.02m; // 10000
            decimal expectedPremium = expectedBase * 1.5m * 1.3m; // truck factor + old age factor

            Assert.That(premium, Is.EqualTo(expectedPremium));
        }

        [Test]
        public void Calculate_ShouldReturnBasePremium_ForUnknownVehicleType()
        {
            // Arrange
            string breakdown;
            decimal vehicleValue = 200000;
            int vehicleAge = 3;
            string vehicleType = "scooter"; // unknown → default factor 1.0

            // Act
            var premium = _calculator.Calculate(vehicleType, vehicleAge, vehicleValue, out breakdown);

            // Assert
            decimal expectedBase = vehicleValue * 0.02m; // 4000
            decimal expectedPremium = expectedBase * 1.0m * 1.0m;

            Assert.That(premium, Is.EqualTo(expectedPremium));
        }
    }
}
