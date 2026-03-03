# NuGet.CommandLine 5.7.0 – basic sample

Add the package (specific version **5.7.0**):

```bash
cd NuGetCommandLineSample
dotnet add package NuGet.CommandLine --version 5.7.0
```

Restore and run the sample (invokes `nuget.exe help`):

```bash
dotnet restore
dotnet run
```

The sample code in `Program.cs` locates the restored `nuget.exe` under the NuGet packages folder (`NUGET_PACKAGES` or `~/.nuget/packages`) and runs it with the `help` argument.

**Note:** `nuget.exe` is a Windows executable. On macOS/Linux the sample may only verify the package path; use Windows or the modern `dotnet nuget` CLI for cross-platform use.
