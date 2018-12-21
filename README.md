Disposable FileSystem
=====================
A C# library to help creating temporary files and directories in tests projects

# Installation
The library is available as a [NuGet package](https://www.nuget.org/packages/DisposableFileSystem/), which can be installed from the command line with:


```bash
dotnet add package DisposableFileSystem
```
or with:

```
Install-Package DisposableFileSystem
```

in the Package Manager console.


# Usage
## Create a temporary, disposable directory

The following creates a temporary directory, located in [current user's temporary folder](https://docs.microsoft.com/en-us/dotnet/api/system.io.path.gettemppath?view=netframework-4.7.2):

```csharp
var directory = new DisposableDirectory();
```

The directory and all its content can be recursively deleted invoking `Dispose()`, therefore a common pattern is using a disposable directory with a `using` statement:

```csharp
using(var directory = new DisposableDirectory())
{
    var fullPath = directory.Path;
    
    // operate with the disposable directory
}
```
The `Path` property returns the full path of the disposable directory.

## Create subdirectories
Use the method `CreateDirectory()` to create a subdirectory inside the temporary directory:

```csharp
using(var directory = new DisposableDirectory())
{
    directory.CreateDirectory("sub-dir");
}
```

The subdirectory and its content will be deleted when `Dispose()` is invoked on the root directory.

It's possible to provide a list of directory names to create nested directories. For example, to create the following structure:

```bash
disp
└── dir1
    └── mdir2
        └── dir3
```

the following can be used:

```csharp
using(var directory = new DisposableDirectory())
{
    directory.CreateDirectory("dir1", "dir2", "dir3");
}
```

All the directories and their content will be recursively deleted at the disposable directory `Dispose()`.


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
