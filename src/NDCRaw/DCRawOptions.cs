using System.Collections.Generic;


namespace NDCRaw
{
    public class DCRawOptions
    {
        public string DCRawPath { get; set; }
        public bool UseCameraWhiteBalance { get; set; }
        public bool AverageWholeImageForWhiteBalance { get; set; }
        public GrayBox AverageGrayBoxForWhiteBalance { get; set; }
        public CustomWhiteBalance WhiteBalance { get; set; }
        public bool? UseEmbeddedColorMatrix { get; set; }
        public string DeadPixelFile { get; set; }
        public string DarkFrameFile { get; set; }
        public float? DarknessLevel { get; set; }
        public float? SaturationLevel { get; set; }
        public int? WaveletDenoisingThreshold { get; set; }
        public HighlightMode? HighlightMode { get; set; }
        public FlipImage? Flip { get; set; }
        public Colorspace? Colorspace { get; set; }
        public string OutputIccProfileFile { get; set; }
        public string CameraIccProfileFile { get; set; }
        public bool DocumentMode { get; set; }
        public bool DocumentModeNoScaling { get; set; }
        public bool DontAutomaticallyBrighten { get; set; }
        public float? AdjustBrightness { get; set; }
        public CustomGammaCurve GammaCurve { get; set; }
        public InterpolationQuality? Quality { get; set; }
        public bool HalfSizeColorImage { get; set; }
        public bool InterpolateRggbAsFourColors { get; set; }
        public int? AppliedMedianFilterNumberPasses { get; set; }
        public bool Write16Bits { get; set; }
        public Format Format { get; set; }

        
        public DCRawOptions()
        {
            DCRawPath = "dcraw";
            Format = Format.Ppm;
        }
        
        
        public string[] GetArguments(string rawFile)
        {
            var args = new List<string>();
            
            if(UseCameraWhiteBalance)
            {
                args.Add("-w");
            }
            
            if(AverageWholeImageForWhiteBalance)
            {
                args.Add("-a");
            }
            
            if(AverageGrayBoxForWhiteBalance != null)
            {
                args.Add("-A");
                args.Add($"{AverageGrayBoxForWhiteBalance.X}");
                args.Add($"{AverageGrayBoxForWhiteBalance.Y}");
                args.Add($"{AverageGrayBoxForWhiteBalance.Width}");
                args.Add($"{AverageGrayBoxForWhiteBalance.Height}");
            }
            
            if(WhiteBalance != null)
            {
                args.Add("-r");
                args.Add($"{WhiteBalance.R}");
                args.Add($"{WhiteBalance.G1}");
                args.Add($"{WhiteBalance.B}");
                args.Add($"{WhiteBalance.G2}");
            }
            
            if(UseEmbeddedColorMatrix != null)
            {
                var arg = UseEmbeddedColorMatrix == true ? "+M" : "-M";
                args.Add(arg);
            }
            
            if(!string.IsNullOrEmpty(DeadPixelFile))
            {
                args.Add("-P");
                args.Add($"{DeadPixelFile}");
            }
            
            if(!string.IsNullOrEmpty(DarkFrameFile))
            {
                args.Add("-K");
                args.Add($"{DarkFrameFile}");
            }
            
            if(DarknessLevel != null)
            {
                args.Add("-k");
                args.Add($"{DarknessLevel}");
            }
            
            if(SaturationLevel != null)
            {
                args.Add("-S");
                args.Add($"{SaturationLevel}");
            }
            
            if(WaveletDenoisingThreshold != null)
            {
                args.Add("-n");
                args.Add($"{WaveletDenoisingThreshold}");
            }
            
            if(HighlightMode != null)
            {
                args.Add("-H");
                args.Add($"{(int)HighlightMode}");
            }
            
            if(Flip != null)
            {
                args.Add("-t");
                args.Add($"{(int)Flip}");
            }
            
            if(Colorspace != null)
            {
                args.Add("-o");
                args.Add($"{(int)Colorspace}");
            }
            
            if(!string.IsNullOrEmpty(OutputIccProfileFile))
            {
                args.Add("-o");
                args.Add($"{OutputIccProfileFile}");
            }
            
            if(!string.IsNullOrEmpty(CameraIccProfileFile))
            {
                args.Add("-p");
                args.Add($"{CameraIccProfileFile}");
            }
            
            if(DocumentMode)
            {
                args.Add("-d");
            }
            
            if(DocumentModeNoScaling)
            {
                args.Add("-D");
            }
            
            if(DontAutomaticallyBrighten)
            {
                args.Add("-W");
            }
            
            if(AdjustBrightness != null)
            {
                args.Add("-b");
                args.Add($"{AdjustBrightness}");
            }
            
            if(GammaCurve != null)
            {
                args.Add("-g");
                args.Add($"{GammaCurve.Power}");
                args.Add($"{GammaCurve.ToeSlope}");
            }
            
            if(Quality != null)
            {
                args.Add("-q");
                args.Add($"{(int)Quality}");
            }
            
            if(HalfSizeColorImage)
            {
                args.Add("-h");
            }
            
            if(InterpolateRggbAsFourColors)
            {
                args.Add("-f");
            }
            
            if(AppliedMedianFilterNumberPasses != null)
            {
                args.Add("-m");
                args.Add($"{AppliedMedianFilterNumberPasses}");
            }
            
            if(Write16Bits)
            {
                args.Add("-6");
            }
            
            if(Format == Format.Tiff)
            {
                args.Add("-T");
            }
            
            args.Add(rawFile);
            
            return args.ToArray();
        }
    }
}
