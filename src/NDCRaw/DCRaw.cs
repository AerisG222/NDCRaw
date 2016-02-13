using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;


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
        
        
        // http://stackoverflow.com/questions/10788982/is-there-any-async-equivalent-of-process-start
        Task<DCRawResult> RunProcessAsync(string fileName)
        {
            var tcs = new TaskCompletionSource<DCRawResult>();
            var ext = Options.Format == Format.Ppm ? ".ppm" : ".tiff";
            var output = fileName.Replace(Path.GetExtension(fileName), ext);
            
            var process = new Process
            {
                StartInfo = Options.GetStartInfo(fileName),
                EnableRaisingEvents = true
            };

            process.Exited += (sender, args) =>
            {
                var result = new DCRawResult {
                    ExitCode = process.ExitCode,
                    StandardOutput = process.StandardOutput.ReadToEnd(),
                    StandardError = process.StandardError.ReadToEnd(),
                    OutputFilename = output
                };
                
                tcs.SetResult(result);
                process.Dispose();
            };

            process.Start();

            return tcs.Task;
        }
    }
}
