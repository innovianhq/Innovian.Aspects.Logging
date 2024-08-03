<a href="https://innovian.net">
	<p align="center">
		<img src="https://innovian.net/img/bluelogo.svg" width="100px"/>
	</p>
</a>

# Innovian.Aspects.Logging
[![Build Status](https://dev.azure.com/innovian/Innovian%20Open%20Source/_apis/build/status%2FMetalama%20Aspects%2Finnovianhq.Innovian.Aspects.Logging?branchName=main)](https://dev.azure.com/innovian/Innovian%20Open%20Source/_build/latest?definitionId=334&branchName=main) [![NuGet](https://img.shields.io/nuget/v/Innovian.Aspects.Logging.svg)](https://www.nuget.org/packages/Innovian.Aspects.Logging/)

This is an aspect implemented using [Metalama](https://github.com/postsharp/Metalama) targeting .NET 8 that provides logging capabilities to projects targeting `Microsoft.Extensions.Logging` using `ILoggerFactory` and/or `ILogger`. This project 
includes a single aspect intended for direct use, `InjectLoggerAttribute` (intended to be applied to a class) and a fabric that automatically applies this attribute to every non-static class and non-static method across whatever project that 
targets it.

The attribute performs the following:
- Automatically wraps eligible methods in try/catch blocks
- Injects an optional instance of `ILoggerFactory` into all constructors of an eligible class
- Injects and instantiates an `ILogger` as a private field on the class from the `ILoggerFactory` or from a `NullLoggerFactory` instance if the injected `ILoggerFactory` is null
- Creates a stopwatch in each method that times and records, via the `ILogger` field, how long it takes a given method to execute

This project is provided to the larger open-source community by [Innovian](https://innovian.net).

## Direct usage of the attribute
The following instructions detail how to use the `InjectLoggerAttribute` directly in those project. It's intended to be applied as an attribute to any classes (not records) in your project and will automatically apply the other attributes
in the project. Direct usage of these other attributes isn't intended.

### Installation

Using the .NET CLI tools:
```sh
dotnet add package Innovian.Aspects.Logging
```

Using the Package Manager Console:
```powershell
Install-Package Innovian.Aspects.Logging
```

From within Visual Studio:

1. Open the Solution Explorer.
2. Right-click on the project within your solution you wish to add the attribute to.
3. Click on "Manage NuGet Packages...".
4. Click on the "Browse" tab and search for "Innovian.Aspects.Logging".
5. Click on the "Innovian.Aspects.Logging" package, select the appropriate version in the right-tab and click *Install*.

### Usage
Simply apply the `[InjectLogger]` attribute to any non-static class you would like the logging capabilities added to. It's recommended that the class be marked with the `partial` keyword so that you're able to access the `ILogger` field on the 
type named `_logger`, but this isn't required.

## Indirect usage via a Fabric
The following instructions detail how to use the Fabric-based approach to applying the `InjectLoggerAttribute` automatically throughout the project referencing this package. This fabric will automatically apply the aspect to
all non-static classes and non-static methods.

### Installation

Using the .NET CLI tools:
```sh
dotnet add package Innovian.Aspects.Logging.Fabric
```

Using the Package Manager Console:
```powershell
Install-Package Innovian.Aspects.Logging.Fabric
```

From within Visual Studio:

1. Open the Solution Explorer.
2. Right-click on the project within your solution you wish to add the attribute to.
3. Click on "Manage NuGet Packages...".
4. Click on the "Browse" tab and search for "Innovian.Aspects.Logging.Fabric".
5. Click on the "Innovian.Aspects.Logging.Fabric" package, select the appropriate version in the right-tab and click *Install*.

### Usage
No additional effort is necessary beyond installation of the `Innovian.Aspects.Logging.Fabric` package on the project. It will automatically identify all non-static classes to apply the `[InjectLogger]` attribute to automatically. In turn, this
will perform the code generation functionality described above.

## Notes
The fabric will only install to classes that have at least one method as there's little point to it being on the class otherwise.

Further, ecause we at Innovian also build functionality around [Dapr Workflows](https://docs.dapr.io/developing-applications/building-blocks/workflow/workflow-overview/) and the `Workflow` base type requires parameterless constructors, the fabric will
also neglect to apply to any type that implements an abstract base type called "Workflow". 