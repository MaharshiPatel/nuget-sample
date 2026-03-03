// Very basic sample: add the package with:
//   dotnet add package NuGet.CommandLine --version 5.7.0
// Then run this program to invoke nuget.exe (e.g. show help).

using System.Diagnostics;

string packagesDir = Environment.GetEnvironmentVariable("NUGET_PACKAGES")
    ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nuget", "packages");

string nugetExe = Path.Combine(packagesDir, "nuget.commandline", "5.7.0", "tools", "nuget.exe");

if (!File.Exists(nugetExe))
{
    Console.WriteLine("Run: dotnet restore");
    Console.WriteLine("Then run this again. nuget.exe expected at: " + nugetExe);
    return 1;
}

// Run nuget.exe with "help" (basic usage)
var psi = new ProcessStartInfo(nugetExe)
{
    Arguments = "help",
    UseShellExecute = false,
    RedirectStandardOutput = true
};
using var p = Process.Start(psi);
if (p is null) return 1;
Console.WriteLine(p.StandardOutput.ReadToEnd());
p.WaitForExit();
return p.ExitCode;
