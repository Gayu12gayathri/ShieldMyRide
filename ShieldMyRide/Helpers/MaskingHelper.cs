namespace ShieldMyRide.Helpers
{
    public static class MaskingHelper
    {
        public static string MaskAadhaar(string aadhaar)
        {
            if (string.IsNullOrEmpty(aadhaar) || aadhaar.Length < 4) return "XXXX";
            return new string('X', aadhaar.Length - 4) + aadhaar.Substring(aadhaar.Length - 4);
        }

        public static string MaskPan(string pan)
        {
            if (string.IsNullOrEmpty(pan) || pan.Length < 4) return "XXXX";
            return new string('X', pan.Length - 4) + pan.Substring(pan.Length - 4);
        }
    }
}
