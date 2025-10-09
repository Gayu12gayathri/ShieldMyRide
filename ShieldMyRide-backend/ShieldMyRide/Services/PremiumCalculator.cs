using System;

namespace ShieldMyRide.Services
{
    public interface IPremiumCalculator
    {
        decimal Calculate(
            string vehicleType,
            int vehicleAge,
            decimal insuredDeclaredValue,
            out string breakdown,
            bool zeroDep = false,
            bool roadsideAssist = false,
            int ncbPercent = 0
        );
    }

    public class PremiumCalculator : IPremiumCalculator
    {
        public decimal Calculate(
            string vehicleType,
            int vehicleAge,
            decimal insuredDeclaredValue,
            out string breakdown,
            bool zeroDep = false,
            bool roadsideAssist = false,
            int ncbPercent = 0
        )
        {
            // 1. Base Premium by vehicle type
            decimal baseRate = vehicleType.ToLower() switch
            {
                "car" => 0.03m,   // 3% of IDV
                "bike" => 0.015m, // 1.5% of IDV
                "truck" => 0.04m, // 4% of IDV
                _ => 0.02m
            };

            decimal basePremium = insuredDeclaredValue * baseRate;

            // 2. Age Loading
            if (vehicleAge > 5)
                basePremium += basePremium * 0.10m; // +10% loading

            // 3. Add-ons
            decimal addons = 0;
            if (zeroDep) addons += basePremium * 0.15m; // 15% of base
            if (roadsideAssist) addons += 500;          // Flat Rs. 500

            // 4. Discounts
            decimal discount = basePremium * (ncbPercent / 100m);

            // 5. Subtotal
            decimal subtotal = basePremium + addons - discount;

            // 6. GST @18%
            decimal tax = subtotal * 0.18m;

            decimal total = subtotal + tax;

            // 7. Breakdown (for debugging/DB storage)
            breakdown = $"Base: {basePremium}, Add-ons: {addons}, Discount: {discount}, Tax: {tax}, Total: {total}";

            return total;
        }
    }
}
