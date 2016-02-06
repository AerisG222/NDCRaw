namespace NDCRaw
{
    public class CustomWhiteBalance
    {
        public float R { get; set; }
        public float G1 { get; set; }
        public float B { get; set; }
        public float G2 { get; set; }
        
        
        public CustomWhiteBalance(float r, float g1, float b, float g2)
        {
            R = r;
            G1 = g1;
            B = b;
            G2 = g2;
        }
    }
}
