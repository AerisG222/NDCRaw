using System;
using System.Collections.Generic;
using System.IO;
using Xunit;


namespace NDCRaw.Tests
{
    public class NDCRawTests
    {
        const bool KEEP_TEST_RESULTS = true;
        const bool SHOW_COMMAND_LINES = true;
        
        
        List<string> _files = new List<string>();
        
        
        public NDCRawTests()
        {
            _files.Add("DSC_3982.NEF");
        }
        
        
        [Fact]
        public void NoCustomizationTest()
        {
            ExecuteTest(new DCRawOptions(), "noargs_");
            ExecuteTest(GetPreferredDefaultOptions(), "pref_");
        }
        
        
        [Fact]
        public void CurrentConversionTest()
        {
            var opts = GetPreferredDefaultOptions();
            
            opts.AdjustBrightness = 1.5f;
            opts.Write16Bits = true;

            ExecuteTest(opts, "curr_");
        }
        
        
        [Fact]
        public void ColorspaceTests()
        {
            var opts = GetPreferredDefaultOptions();
            
            opts.Colorspace = Colorspace.Adobe;
            ExecuteTest(opts, "adobe_");
            
            opts.Colorspace = Colorspace.sRGB;
            ExecuteTest(opts, "srgb_");
        }
        
        
        [Fact]
        public void UseIccProfileTest()
        {
            var opts = GetPreferredDefaultOptions();
            
            // special key to use the embedded profile if exists
            opts.CameraIccProfileFile = "embed";
            
            ExecuteTest(opts, "icc_");
        }
        
        
        [Fact]
        public void QualityTests()
        {
            var opts = GetPreferredDefaultOptions();
            
            opts.Quality = InterpolationQuality.Quality0;
            ExecuteTest(opts, "q0_");
            
            opts.Quality = InterpolationQuality.Quality1;
            ExecuteTest(opts, "q1_");
            
            opts.Quality = InterpolationQuality.Quality2;
            ExecuteTest(opts, "q2_");
            
            opts.Quality = InterpolationQuality.Quality3;
            ExecuteTest(opts, "q3_");
        }
        
        
        [Fact]
        public void Quality3CorrectionTests()
        {
            var opts = GetPreferredDefaultOptions();
            
            opts.AppliedMedianFilterNumberPasses = 1;
            ExecuteTest(opts, "q3c1_");
            
            opts.AppliedMedianFilterNumberPasses = 2;
            ExecuteTest(opts, "q3c2_");
            
            opts.AppliedMedianFilterNumberPasses = 10;
            ExecuteTest(opts, "q3c10_");
        }
        
        
        [Fact]
        public void BitTests()
        {
            var opts = GetPreferredDefaultOptions();
            
            opts.Write16Bits = true;
            ExecuteTest(opts, "bit16_");
            
            opts.Write16Bits = false;
            ExecuteTest(opts, "bit8_");
        }
        
        
        [Fact]
        public void AverageWhiteBalanceTest()
        {
            var opts = GetPreferredDefaultOptions();
            
            opts.UseCameraWhiteBalance = false;
            opts.AverageWholeImageForWhiteBalance = true;

            ExecuteTest(opts, "avgwb_");
        }
        
        
        [Fact]
        public void AutoBrightenTest()
        {
            var opts = new DCRawOptions();
            
            opts.DontAutomaticallyBrighten = false;
            
            ExecuteTest(opts, "brighten_");
        }
        
        
        [Fact]
        public void FormatTest()
        {
            var opts = new DCRawOptions();
            
            opts.Format = Format.Tiff;
            
            ExecuteTest(opts, "tiff_");
        }
        
        
        [Fact]
        public void HighlightTests()
        {
            var opts = new DCRawOptions();
            
            opts.HighlightMode = HighlightMode.Blend;
            ExecuteTest(opts, "hblend_");
            
            opts.HighlightMode = HighlightMode.Clip;
            ExecuteTest(opts, "hclip_");
            
            opts.HighlightMode = HighlightMode.Unclip;
            ExecuteTest(opts, "hunclip_");
            
            opts.HighlightMode = HighlightMode.Rebuild3;
            ExecuteTest(opts, "h3_");
            
            opts.HighlightMode = HighlightMode.Rebuild4;
            ExecuteTest(opts, "h4_");
            
            opts.HighlightMode = HighlightMode.Rebuild5;
            ExecuteTest(opts, "h5_");
            
            opts.HighlightMode = HighlightMode.Rebuild6;
            ExecuteTest(opts, "h6_");
            
            opts.HighlightMode = HighlightMode.Rebuild7;
            ExecuteTest(opts, "h7_");
            
            opts.HighlightMode = HighlightMode.Rebuild8;
            ExecuteTest(opts, "h8_");
            
            opts.HighlightMode = HighlightMode.Rebuild9;
            ExecuteTest(opts, "h9_");
        }
        
        
        [Fact]
        public void MyOptimalTest()
        {
            var opts = GetPreferredDefaultOptions();
            opts.Colorspace = Colorspace.sRGB;
            
            // does imagemagick work better with 8b or 16b?
            // does imagemagick work better with adobe or srgb?
            
            ExecuteTest(opts, "optimal_");
        }
        
        
        DCRawOptions GetPreferredDefaultOptions()
        {
            var opts = new DCRawOptions();
            
            opts.UseCameraWhiteBalance = true;
            opts.Quality = InterpolationQuality.Quality3;
            opts.DontAutomaticallyBrighten = true;
            
            return opts;
        }
        
        
        void ExecuteTest(DCRawOptions opts, string renamePrefix)
        {
            foreach(var sourceFile in _files)
            {
                var dcraw = new DCRaw(opts);
                
                if(SHOW_COMMAND_LINES)
                {
                    Console.WriteLine($"prefix: {renamePrefix} cmdline: {opts.GetStartInfo(sourceFile).Arguments}");
                }
                
                var outfile = dcraw.Convert(sourceFile);
                
                Assert.True(File.Exists(outfile));
                
                if(!KEEP_TEST_RESULTS)
                {
                    File.Delete(outfile);
                }
                else
                {
                    var dir = "test_output";
                    Directory.CreateDirectory(dir);
                    var newFile = Path.Combine(Path.GetDirectoryName(sourceFile), dir, renamePrefix + Path.GetFileName(outfile));
                    File.Move(outfile, newFile);
                }
            }
        }
    }
}
