# StarSystemSimulator


## Dependencies
### Framework: [.NET 6](https://dotnet.microsoft.com/download/dotnet/6.0)
- [OpenTK](https://github.com/opentk/opentk)
- [ImGui.NET](https://github.com/mellinoe/ImGui.NET)
- [Nlua](http://nlua.org/)
- [SixLabors.ImageSharp](https://sixlabors.com/products/imagesharp/)

## Download
Releases are available [here](https://github.com/abc013/StarSystemSimulator/releases).
The source code can either be [downloaded as zip](https://github.com/abc013/StarSystemSimulator/archive/master.zip) or cloned via `git` using:
```sh
git clone https://github.com/abc013/StarSystemSimulator.git
```

## Compiling
After installing the [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0), make a local copy of this repository.
Open a command line and navigate to the corresponding directory:
```sh
cd C:/example/path/to/downloaded/directory/StarSystemSimulator/
```
After doing so, just run the following command:
```sh
dotnet build
```
This command should fetch the dependencies via NuGet (make sure you have an internet connection!) and build the binaries for you. Done!

As text editors, Visual Studio 2022 or Visual Studio Code are recommended.

## Settings
Most options can be modified in the program. However, to get access to more settings or to have the same arguments at every startup, you can create a `settings.txt` in the same directory where the executable is. To see what can be modified, please take a look at the [Settings.cs file](https://github.com/abc013/StarSystemSimulator/blob/master/StarSystemSimulator/Settings.cs). An example of a `settings.txt` might be:
```
Scale=1.5
LocationX=-0.5
LocationY=0.55
```

## Issues
If you encounter any problems or bugs while compiling or running the program, feel free to [open an issue](https://github.com/abc013/StarSystemSimulator/issues/new)! Please don't forget to atttach the `exception.log` and `information.log` files.
