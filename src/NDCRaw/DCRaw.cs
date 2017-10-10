using System;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using Medallion.Shell;


namespace NDCRaw
{
    public class DCRaw
    {
        public DCRawOptions Options { get; private set; }
        
        
        public DCRaw(DCRawOptions options)
        {
            Options = options;
        }
        
        
        public DCRawResult Convert(string srcPath)
        {
            return ConvertAsync(srcPath).Result;
        }
        
        
        public Task<DCRawResult> ConvertAsync(string srcPath)
        {
            if(!File.Exists(srcPath))
            {
                throw new FileNotFoundException("Please make sure the raw image exists.", srcPath);
            }
            
            return RunProcessAsync(srcPath);
        }
        
        
        async Task<DCRawResult> RunProcessAsync(string fileName)
        {
            var ext = Options.Format == Format.Ppm ? ".ppm" : ".tiff";
            var output = fileName.Replace(Path.GetExtension(fileName), ext);

            try
            {
                var cmd = Command.Run(Options.DCRawPath, Options.GetArguments(fileName));

                await cmd.Task;

                return new DCRawResult {
                    ExitCode = cmd.Result.ExitCode,
                    StandardOutput = cmd.StandardOutput.ReadToEnd(),
                    StandardError = cmd.StandardError.ReadToEnd(),
                    OutputFilename = output
                };
            }
            catch (Win32Exception ex)
            {
                throw new Exception("Error when trying to start the dcraw process.  Please make sure dcraw is installed, and its path is properly specified in the options.", ex);
            }
        }
    }
}
