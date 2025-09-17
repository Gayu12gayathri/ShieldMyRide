using System;

namespace ShieldMyRide.Services
{
    public interface IPremiumCalculator
    {
        decimal Calculate(string vehicleType, int vehicleAge, decimal vehicleValue, out string breakdown);
    }

    public class PremiumCalculator : IPremiumCalculator
    {
        public decimal Calculate(string vehicleType, int vehicleAge, decimal vehicleValue, out string breakdown)
        {
            // base = 2% of vehicle value
            decimal basePremium = vehicleValue * 0.02m;

            // Vehicle risk factor
            decimal vehicleFactor = vehicleType.ToLower() switch
            {
                "car" => 1.2m,
                "bike" => 0.8m,
                "truck" => 1.5m,
                _ => 1.0m
            };

            // Age factor: older vehicles may cost more
            decimal ageFactor = vehicleAge > 10 ? 1.3m : 1.0m;

            decimal premium = basePremium * vehicleFactor * ageFactor;

            breakdown = $"Base({vehicleValue} * 0.02): {basePremium}, VehicleFactor: {vehicleFactor}, AgeFactor: {ageFactor}";
            return premium;
        }
    }
}
