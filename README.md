# Surveying3D
Calculate the volume, height, width, and depth of the 3d object.
This repository include NuGet package and cli.
Currently only the wavefront obj format is supported, but glTF will also be supported.

# Install
- [Class library(.NET Standard2.0)](https://www.nuget.org/packages/Surveying3D/)
```
dotnet add package Surveying3D --version 1.0.0
```
- [CLI(.NET Core Global Tools)](https://www.nuget.org/packages/Surveying3D-Cli/)
```
dotnet tool install --global Surveying3D-Cli --version 1.0.1
```
If you have never used .NET Global Tools, set the path. 
- Linux/Mac
  - $HOME/.dotnet/tools
- Windows
  - %USERPROFILE%\\.dotnet\tools

Details are [here](https://docs.microsoft.com/ja-jp/dotnet/core/tools/global-tools).
# How to use

## Class library
The simplest usage is to give a path.
```
var results = Surveyor.Survey(pathToModel);
```
result includes volume, height, width and depth.

## CLI
I prepared three commands.
- test
  - This command takes the path of one object and outputs result to console or file.
- list
  - This command takes a file containing the model path as input and outputs the result. A file with the model path can be created with "find \` pwd\` -name * .obj ".
- search
  - Search for models with the specified extension (default is obj) under the specified directory and get the results for those models.

If you start without giving command line arguments, you can see the following details.
```
$surveying3d

search: Search and measure models with the specified extension under the specified directory.
-d, -rootDirectory: search under root dir
-o, -outputDir: [default=null]output dir, The default output is not a file, but the console
-e, -extension: [default=obj]extension to search
-n, -outputFileName: [default=result.json]String

list: The path of the file describing the list of obj (for example, created with [find `pwd` -name *.obj])
[0]: Text path that describes the model file path.
-o, -outputDir: [default=null]output dir, The default output is not a file, but the console
-n, -outputFileName: [default=result.json]String

test: check one object
[0]: 3d object path
-o, -outputDir: [default=null]output dir, The default output is not a file, but the console
-n, -outputFileName: [default=result.json]output file name
```

# ⚠️Note
- In wavefront obj, the surface can be defined with 3 or more points (triangle, square ...), but this algorithm assumes that the surface definition is 3 points (triangle).

- If the face is not closed, the exact volume cannot be calculated.
