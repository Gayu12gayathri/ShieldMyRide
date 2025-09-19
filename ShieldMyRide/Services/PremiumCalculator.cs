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
            // 1. Base Premium (3% of vehicle value)
            decimal basePremium = vehicleValue * 0.03m;

            // 2. Risk Loading
            decimal riskLoading = 0;
            riskLoading += vehicleType.ToLower() switch
            {
                "car" => basePremium * 0.20m, // 20% extra
                "bike" => basePremium * 0.10m, // 10% extra
                "truck" => basePremium * 0.30m, // 30% extra
                _ => basePremium * 0.15m
            };

            riskLoading += vehicleAge > 10 ? basePremium * 0.15m : basePremium * 0.05m;

            // 3. Fixed Charges
            decimal fixedCharges = 500m;

            // 4. Final Premium
            decimal premium = basePremium + riskLoading + fixedCharges;

            breakdown = $"Base: {basePremium}, Risk Loading: {riskLoading}, Fixed: {fixedCharges}, Total: {premium}";
            return premium;
        }

    }
}
