using System;
using Microsoft.DotNet.Cli.Utils;
using NuGet.Frameworks;

namespace IntelliTect.Coalesce.DotnetCliTool
{
    /// <summary>
    /// This invoker is used to get around the requirement that DotNetCLI tools run against the NetCoreApp runtime.
    /// This code is adapted from https://github.com/dotnet/cli/tree/master/TestAssets/TestPackages/dotnet-dependency-tool-invoker
    /// </summary>
    public class Program
    {
        public static int Main(string[] args)
        {
            var dotnetParams = new DotnetBaseParams("dotnet-coalesce", "Coalesce Code Generation Invoker",
                "Invokes the Coalesce code generation tool which requires the .Net 4.6 framework");

            dotnetParams.Parse(args);

            if (string.IsNullOrEmpty(dotnetParams.Command))
            {
                Console.WriteLine("A command name must be provided");

                return 1;
            }

            var commandFactory =
                new ProjectDependenciesCommandFactory(NuGetFramework.Parse("net46"),
                    "Debug", null, null, null);

            int result = InvokeDependencyToolForMSBuild(commandFactory, dotnetParams);

            return result;
        }

        private static int InvokeDependencyToolForMSBuild(
            ProjectDependenciesCommandFactory commandFactory,
            DotnetBaseParams dotnetParams)
        {
            Console.WriteLine(
                $"Invoking '{dotnetParams.Command}' for '{dotnetParams.Framework.GetShortFolderName()}'.");

            return InvokeDependencyTool(commandFactory, dotnetParams, dotnetParams.Framework);
        }

        private static int InvokeDependencyTool(
            ProjectDependenciesCommandFactory commandFactory,
            DotnetBaseParams dotnetParams,
            NuGetFramework framework)
        {
            try
            {
                int exitCode = commandFactory.Create(
                        "IntelliTect.Coalesce.Cli",
                        dotnetParams.RemainingArguments,
                        framework,
                        "Debug")
                    .ForwardStdErr()
                    .ForwardStdOut()
                    .Execute()
                    .ExitCode;

                Console.WriteLine($"Command returned {exitCode}");
            }
            catch (CommandUnknownException)
            {
                Console.WriteLine($"Command {dotnetParams.Command} not found");
                return 1;
            }

            return 0;
        }
    }
}