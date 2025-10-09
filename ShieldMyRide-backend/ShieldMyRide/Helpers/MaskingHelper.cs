namespace ShieldMyRide.Helpers
{
    public static class MaskingHelper
    {
        public static string MaskAadhaar(string aadhaar)
        {
            if (string.IsNullOrEmpty(aadhaar) || aadhaar.Length < 4)
                return "****";

            // Mask everything except last 4 digits
            return new string('*', aadhaar.Length - 4) + aadhaar[^4..];
        }

        public static string MaskPan(string pan)
        {
            if (string.IsNullOrEmpty(pan) || pan.Length < 4)
                return "****";

            // Mask everything except last 4 characters
            return new string('*', pan.Length - 4) + pan[^4..];
        }
    }
}
