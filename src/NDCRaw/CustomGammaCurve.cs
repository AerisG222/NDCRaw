namespace NDCRaw
{
    public class CustomGammaCurve
    {
        public float Power { get; set; }
        public float ToeSlope { get; set; }
        
        public CustomGammaCurve(float power, float toeSlope)
        {
            Power = power;
            ToeSlope = toeSlope;
        }
    }
}
