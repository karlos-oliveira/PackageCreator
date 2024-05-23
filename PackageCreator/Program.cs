using System.Diagnostics;

namespace PackageCreator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Inform All In One solution path: ");
                string? solutionPath = Console.ReadLine();

                if (string.IsNullOrEmpty(solutionPath))
                    Console.WriteLine("A path must be informed");
                else
                {
                    CreateNupkg(solutionPath);
                    PublishNupkg(solutionPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        protected static void CreateNupkg(string solutionPath)
        {
            string packageDirectory = Path.Combine(solutionPath, "NugetPackages");

            if (!Directory.Exists(packageDirectory))
                Directory.CreateDirectory(packageDirectory);

            Process.Start("CMD.exe"
                , @$"/C dotnet pack {Path.Combine(solutionPath, "AllInOne.sln")} --output {packageDirectory}")
                    .WaitForExit();

            Console.WriteLine($"Nuget packages was created in {packageDirectory} successfully");

        }

        protected static void PublishNupkg(string solutionPath)
        {
            Console.WriteLine("Inform nuget feed url: ");
            string? nugetFeed = Console.ReadLine();
            string[] packages = Directory.GetFiles(Path.Combine(solutionPath, "NugetPackages"), "*.nupkg");
            int count = 0;

            foreach (string package in packages)
            {
                Process.Start("CMD.exe"
                    , @$"/C dotnet nuget push --source {nugetFeed} --api-key Dummy {package} ")
                        .WaitForExit();

                count++;
            }

            Console.WriteLine($"{count}/{packages.Length} nuget packages was published successfully");
        }
    }
}
