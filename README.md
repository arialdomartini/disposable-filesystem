Disposable FileSystem
=====================
DisposableFileSystem is a C# library that helps the creation of temporary files and directories in tests projects, ensuring that any content is cleaned up when the tests end.

## Installation
The library is available as a [NuGet package](https://www.nuget.org/packages/DisposableFileSystem/), and it can be installed from the command line with:

```bash
dotnet add package DisposableFileSystem
```
or with:

```
Install-Package DisposableFileSystem
```

in the Package Manager console.

## Usage
### Create a temporary, disposable directory

The following creates a temporary directory, located in [current user's temporary folder](https://docs.microsoft.com/en-us/dotnet/api/system.io.path.gettemppath?view=netframework-4.7.2):

```csharp
var directory = DisposableDirectory.Create();
```

Since the directory and all its content can be recursively deleted invoking `Dispose()`, a common pattern is the use in combination of a `using` statement:

```csharp
using(var directory = DisposableDirectory.Create())
{
    var fullPath = directory.Path;
    
    // operate with the disposable directory
}
```

The `Path` property returns the full path of the disposable directory.

## Create subdirectories
The method `CreateDirectory()` can be used to conveniently create a sub-directory inside the temporary root directory, without having to deal with `System.IO.Path.Combine`:

```csharp
using(var directory = DisposableDirectory.Create())
{
    directory.CreateDirectory("sub-dir");
}
```

The sub-directory and its content will be deleted on `Dispose()`.

It's possible to create nested sub-directories by providing a list of their names. <br />
For example, to create the following structure:

```bash
disp
└── dir1
    └── mdir2
        └── dir3
```

the following can be used:

```csharp
using(var directory = DisposableDirectory.Create())
{
    directory.CreateDirectory("dir1", "dir2", "dir3");
}
```

In this case as well, all the directories and their content will be recursively deleted on `Dispose()`.



## Create temporary files
The method `RandomFileName()` generates a random name for a file contained in the root temporary directory:

```csharp
using (var directory = DisposableDirectory.Create())
{
    fileName = directory.RandomFileName();
    File.WriteAllText(fileName, "some text");
}
```

Files created like this will be delete on `Dispose()`.

## Path.Combine
```csharp
directory.Combine("some", "path");
```

is the short for:

```csharp
Path.Combine(directory.Path, "some", "path");
```


## Building from source code
### Build
Run:

```
dotnet build
```

### Run tests
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
