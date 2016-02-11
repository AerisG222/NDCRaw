using System;
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
		
		
        public string Convert(string srcPath)
        {
            return ConvertAsync(srcPath).Result;
        }
        
        
        public Task<string> ConvertAsync(string srcPath)
        {
            if(!File.Exists(srcPath))
			{
				throw new FileNotFoundException("Please make sure the raw image exists.", srcPath);
			}
            
            return RunProcessAsync(srcPath);
        }
        
        
        // TODO: what if we want to specify a timeout?
        // http://stackoverflow.com/questions/10788982/is-there-any-async-equivalent-of-process-start
        Task<string> RunProcessAsync(string fileName)
        {
            var tcs = new TaskCompletionSource<string>();
            var ext = Options.Format == Format.Ppm ? ".ppm" : ".tiff";
            var output = fileName.Replace(Path.GetExtension(fileName), ext);
            
            var process = new Process
            {
                StartInfo = Options.GetStartInfo(fileName),
                
                EnableRaisingEvents = true
            };

            process.Exited += (sender, args) =>
            {
                tcs.SetResult(output);
                process.Dispose();
            };

            process.Start();

            return tcs.Task;
        }
	}
}
