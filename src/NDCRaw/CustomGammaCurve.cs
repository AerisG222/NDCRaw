namespace NDCRaw
{
    public class CustomGammaCurve
    {
        public float P { get; set; }
        public float Ts { get; set; }
        
        public CustomGammaCurve(float p, float ts)
        {
            P = p;
            Ts = ts;
        }
    }
}
