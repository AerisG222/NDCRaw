[![GitHub license](https://img.shields.io/github/license/mashape/apistatus.svg)](https://github.com/AerisG222/NDCRaw/blob/master/LICENSE.md)
[![NuGet](https://img.shields.io/nuget/dt/NDCRaw.svg)](https://www.nuget.org/packages/NDCRaw/)
[![NuGet](https://img.shields.io/nuget/v/NDCRaw.svg)](https://www.nuget.org/packages/NDCRaw/)

#NDCRaw

A .Net library to wrap the functionality of DCRaw.

## Motivation
Create a simple wrapper around this excellent program to allow
.Net programs to easily convert RAW image files (like Nikon NEF).

## Using
- Install DCRaw
- Add a reference to NDCRaw in your project.json
- Bring down the packages for your project via `dnu restore`

```csharp
using NDCRaw;

namespace Test
{
    public class Example
    {
        public void Convert(string file)
        {
            var dcraw = new DCRaw(new DCRawOptions());
            var outfile = dcraw.Convert(file);
        }
    }
}
```

- View the tests for more examples
- You also might want to check out NMagickWand which can then help
  working with the generated file from NDCRaw!

## Contributing
I'm happy to accept pull requests.  By submitting a pull request, you
must be the original author of code, and must not be breaking
any laws or contracts.

Otherwise, if you have comments, questions, or complaints, please file
issues to this project on the github repo.

## Todo
I hope to make many improvements to the library as time permits.
- Add tests
- Investigate options to bundle dcraw
  
## License
NDCRaw is licensed under the MIT license.  See LICENSE.md for more
information.

## Reference
- DCRaw: https://www.cybercom.net/~dcoffin/dcraw/
