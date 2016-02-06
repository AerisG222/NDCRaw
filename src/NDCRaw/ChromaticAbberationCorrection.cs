namespace NDCRaw
{
    public class ChromaticAbberationCorrection
    {
        public float R { get; set; }
        public float B { get; set; }
        
        
        public ChromaticAbberationCorrection(float r, float b)
        {
            R = r;
            B = b;
        }
    }
}
