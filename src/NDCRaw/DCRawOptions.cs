using System.Text;
using System.Diagnostics;


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
        
        
        public ProcessStartInfo GetStartInfo(string rawFile)
        {
            var psi = new ProcessStartInfo();
            
            psi.FileName = DCRawPath;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            
            StringBuilder args = new StringBuilder();
            
            if(UseCameraWhiteBalance)
            {
                args.Append("-w ");
            }
            
            if(AverageWholeImageForWhiteBalance)
            {
                args.Append("-a ");
            }
            
            if(AverageGrayBoxForWhiteBalance != null)
            {
                args.AppendFormat("-A {0} {1} {2} {3} ", 
                    AverageGrayBoxForWhiteBalance.X,
                    AverageGrayBoxForWhiteBalance.Y,
                    AverageGrayBoxForWhiteBalance.Width,
                    AverageGrayBoxForWhiteBalance.Height);
            }
            
            if(WhiteBalance != null)
            {
                args.AppendFormat("-r {0} {1} {2} {3} ", 
                    WhiteBalance.R,
                    WhiteBalance.G1,
                    WhiteBalance.B,
                    WhiteBalance.G2);
            }
            
            if(UseEmbeddedColorMatrix != null)
            {
                var arg = UseEmbeddedColorMatrix == true ? "+M " : "-M ";
                args.Append(arg);
            }
            
            if(!string.IsNullOrEmpty(DeadPixelFile))
            {
                args.AppendFormat("-P \"{0}\" ", DeadPixelFile);
            }
            
            if(!string.IsNullOrEmpty(DarkFrameFile))
            {
                args.AppendFormat("-K \"{0}\" ", DarkFrameFile);
            }
            
            if(DarknessLevel != null)
            {
                args.AppendFormat("-k {0} ", DarknessLevel);
            }
            
            if(SaturationLevel != null)
            {
                args.AppendFormat("-S {0} ", SaturationLevel);
            }
            
            if(WaveletDenoisingThreshold != null)
            {
                args.AppendFormat("-n {0} ", WaveletDenoisingThreshold);
            }
            
            if(HighlightMode != null)
            {
                args.AppendFormat("-H {0} ", (int)HighlightMode);
            }
            
            if(Flip != null)
            {
                args.AppendFormat("-t {0} ", (int)Flip);
            }
            
            if(Colorspace != null)
            {
                args.AppendFormat("-o {0} ", (int)Colorspace);
            }
            
            if(!string.IsNullOrEmpty(OutputIccProfileFile))
            {
                args.AppendFormat("-o \"{0}\" ", OutputIccProfileFile);
            }
            
            if(!string.IsNullOrEmpty(CameraIccProfileFile))
            {
                args.AppendFormat("-p \"{0}\" ", CameraIccProfileFile);
            }
            
            if(DocumentMode)
            {
                args.Append("-d ");
            }
            
            if(DocumentModeNoScaling)
            {
                args.Append("-D ");
            }
            
            if(DontAutomaticallyBrighten)
            {
                args.Append("-W ");
            }
            
            if(AdjustBrightness != null)
            {
                args.AppendFormat("-b {0} ", AdjustBrightness);
            }
            
            if(GammaCurve != null)
            {
                args.AppendFormat("-g {0} {1} ", GammaCurve.Power, GammaCurve.ToeSlope);
            }
            
            if(Quality != null)
            {
                args.AppendFormat("-q {0} ", (int)Quality);
            }
            
            if(HalfSizeColorImage)
            {
                args.Append("-h ");
            }
            
            if(InterpolateRggbAsFourColors)
            {
                args.Append("-f ");
            }
            
            if(AppliedMedianFilterNumberPasses != null)
            {
                args.AppendFormat("-m {0} ", AppliedMedianFilterNumberPasses);
            }
            
            if(Write16Bits)
            {
                args.Append("-6 ");
            }
            
            if(Format == Format.Tiff)
            {
                args.Append("-T ");
            }
            
            args.Append(EscapeFilename(rawFile));
            
            psi.Arguments = args.ToString();
            
            return psi;
        }
        
        
        string EscapeFilename(string file)
        {
            return $"\"{file}\"";
        }
    }
}
