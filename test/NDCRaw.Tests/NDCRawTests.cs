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
            _files.Add("DSC_0041.NEF");
            _files.Add("DSC_3982.NEF");
            _files.Add("DSC_9762.NEF");
            _files.Add("space test.NEF");
        }
        
        
        [Fact]
        public void NoCustomizationTest()
        {
            ExecuteTest(new DCRawOptions(), "noargs");
            ExecuteTest(GetPreferredDefaultOptions(), "pref");
        }
        
        
        [Fact]
        public void CurrentConversionTest()
        {
            var opts = GetPreferredDefaultOptions();
            
            opts.AdjustBrightness = 1.5f;
            opts.Write16Bits = true;

            ExecuteTest(opts, "curr");
        }
        
        
        [Fact]
        public void ColorspaceTests()
        {
            var opts = GetPreferredDefaultOptions();
            
            opts.Colorspace = Colorspace.Adobe;
            ExecuteTest(opts, "adobe");
            
            opts.Colorspace = Colorspace.sRGB;
            ExecuteTest(opts, "srgb");
        }
        
        
        [Fact]
        public void UseIccProfileTest()
        {
            var opts = GetPreferredDefaultOptions();
            
            // special key to use the embedded profile if exists
            opts.CameraIccProfileFile = "embed";
            
            ExecuteTest(opts, "icc");
        }
        
        
        [Fact]
        public void QualityTests()
        {
            var opts = GetPreferredDefaultOptions();
            
            opts.Quality = InterpolationQuality.Quality0;
            ExecuteTest(opts, "q0");
            
            opts.Quality = InterpolationQuality.Quality1;
            ExecuteTest(opts, "q1");
            
            opts.Quality = InterpolationQuality.Quality2;
            ExecuteTest(opts, "q2");
            
            opts.Quality = InterpolationQuality.Quality3;
            ExecuteTest(opts, "q3");
        }
        
        
        [Fact]
        public void Quality3CorrectionTests()
        {
            var opts = GetPreferredDefaultOptions();
            
            opts.AppliedMedianFilterNumberPasses = 1;
            ExecuteTest(opts, "q3c1");
            
            opts.AppliedMedianFilterNumberPasses = 2;
            ExecuteTest(opts, "q3c2");
        }
        
        
        [Fact]
        public void BitTests()
        {
            var opts = GetPreferredDefaultOptions();
            
            opts.Write16Bits = true;
            ExecuteTest(opts, "bit16");
            
            opts.Write16Bits = false;
            ExecuteTest(opts, "bit8");
        }
        
        
        [Fact]
        public void AverageWhiteBalanceTest()
        {
            var opts = GetPreferredDefaultOptions();
            
            opts.UseCameraWhiteBalance = false;
            opts.AverageWholeImageForWhiteBalance = true;

            ExecuteTest(opts, "avgwb");
        }
        
        
        [Fact]
        public void AutoBrightenTest()
        {
            var opts = new DCRawOptions();
            
            opts.DontAutomaticallyBrighten = false;
            
            ExecuteTest(opts, "brighten");
        }
        
        
        [Fact]
        public void FormatTest()
        {
            var opts = new DCRawOptions();
            
            opts.Format = Format.Tiff;
            
            ExecuteTest(opts, "tiff");
        }
        
        
        [Fact]
        public void HighlightTests()
        {
            var opts = new DCRawOptions();
            
            opts.HighlightMode = HighlightMode.Blend;
            ExecuteTest(opts, "hblend");
            
            opts.HighlightMode = HighlightMode.Clip;
            ExecuteTest(opts, "hclip");
            
            opts.HighlightMode = HighlightMode.Unclip;
            ExecuteTest(opts, "hunclip");
            
            opts.HighlightMode = HighlightMode.Rebuild3;
            ExecuteTest(opts, "h3");
            
            opts.HighlightMode = HighlightMode.Rebuild5;
            ExecuteTest(opts, "h5");
            
            opts.HighlightMode = HighlightMode.Rebuild8;
            ExecuteTest(opts, "h8");
            
            opts.HighlightMode = HighlightMode.Rebuild9;
            ExecuteTest(opts, "h9");
        }
        
        
        [Fact]
        public void MyOptimalDaytimePhotoTest()
        {
            var opts = GetPreferredDefaultOptions();
            opts.HighlightMode = HighlightMode.Blend;
            opts.Colorspace = Colorspace.sRGB;
            
            ExecuteTest(opts, "dayopt");
        }
        
        
        [Fact]
        public void MyOptimalNighttimePhotoTest()
        {
            var opts = GetPreferredDefaultOptions();
            opts.DontAutomaticallyBrighten = true;
            opts.HighlightMode = HighlightMode.Blend;
            opts.Colorspace = Colorspace.sRGB;
            
            ExecuteTest(opts, "nightopt");
        }
        
        
        DCRawOptions GetPreferredDefaultOptions()
        {
            var opts = new DCRawOptions();
            
            opts.UseCameraWhiteBalance = true;
            opts.Quality = InterpolationQuality.Quality3;
            
            return opts;
        }
        
        
        void ExecuteTest(DCRawOptions opts, string renameSuffix)
        {
            foreach(var sourceFile in _files)
            {
                var dcraw = new DCRaw(opts);
                
                if(SHOW_COMMAND_LINES)
                {
                    Console.WriteLine($"prefix: {renameSuffix} cmdline: {opts.GetStartInfo(sourceFile).Arguments}");
                }
                
                var result = dcraw.Convert(sourceFile);
                
                Assert.True(File.Exists(result.OutputFilename));
                
                if(!KEEP_TEST_RESULTS)
                {
                    File.Delete(result.OutputFilename);
                }
                else
                {
                    var dir = "test_output";
                    Directory.CreateDirectory(dir);
                    var newFile = Path.Combine(Path.GetDirectoryName(sourceFile), dir, $"{Path.GetFileNameWithoutExtension(result.OutputFilename)}_{renameSuffix}{Path.GetExtension(result.OutputFilename)}");
                    File.Move(result.OutputFilename, newFile);
                }
            }
        }
    }
}
