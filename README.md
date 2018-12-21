Disposable FileSystem
=====================
A C# library to help creating temporary files and directories in tests projects

## Usage
### Installation
The library is available as a [NuGet package](https://www.nuget.org/packages/DisposableFileSystem/), which can be installed from the command line with:


```bash
dotnet add package DisposableFileSystem
```
or with:

```
Install-Package DisposableFileSystem
```

in the Package Manager console.


# Building from source code
## Build
Run:

```
dotnet build
```

## Run tests
Run:

```bash
dotnet test DisposableFileSystemTest/DisposableFileSystemTest.csproj
```

It should be possible to run tests with a simpler `dotnet test`, but I run in the issue ['dotnet test' in solution folder fails when non-test projects are in the solution #1129](http://wiki.c2.com/?DisposableFileSystem)

### NuGet package
Create the NuGet package with:

```bash
dotnet pack -c release -o .
```

Publish it with:

```bash
dotnet nuget push DisposableFileSystem/[nupkg_file] -k [apikey] -s https://api.nuget.org/v3/index.json
```

replacing `apikey` with the API Key managed by the [NuGet account page](https://www.nuget.org/account/apikeys).
