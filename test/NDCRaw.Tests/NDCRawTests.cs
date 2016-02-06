using System;
using System.IO;
using System.Runtime.InteropServices;
using Xunit;
using NDCRaw;


namespace NDCRaw.Tests
{
    public class NDCRawTests
    {
        [Fact]
        public void NoCustomizationTest()
        {
            var dcraw = new DCRaw(new DCRawOptions());
            var outfile = dcraw.Convert("DSC_3982.NEF");
            
            Assert.True(File.Exists(outfile));
            
            //File.Delete(outfile);
        }
        
        
        [Fact]
        public void SomeCustomizationsTest()
        {
            var opts = new DCRawOptions();
            
            opts.UseCameraWhiteBalance = true;
            opts.AdjustBrightness = 1.5f;
            opts.Write16Bits = true;
            opts.Quality = InterpolationQuality.Quality3;
            opts.DontAutomaticallyBrighten = true;
            opts.Format = OutputFormat.Tiff;
            
            var dcraw = new DCRaw(opts);
            var outfile = dcraw.Convert("DSC_3982.NEF");
            
            Assert.True(File.Exists(outfile));
            
            //File.Delete(outfile);
        }
    }
}