namespace NDCRaw
{
    public class ChromaticAbberationCorrection
    {
        public float RedMagnification { get; set; }
        public float BlueMagnification { get; set; }
        
        
        public ChromaticAbberationCorrection(float redMagnification, float blueMagnification)
        {
            RedMagnification = redMagnification;
            BlueMagnification = blueMagnification;
        }
    }
}
