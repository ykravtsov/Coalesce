using System;
using IntelliTect.Coalesce.CodeGeneration.Common;
using IntelliTect.Coalesce.CodeGeneration.Scripts;
using Microsoft.Extensions.CommandLineUtils;

namespace IntelliTect.Coalesce.Cli
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = new CommandLineApplication(false)
            {
                Name = "Coalesce"
            };

            app.HelpOption("-h|--help");
            CommandOption dataContextClass = app.Option("-dc|--dataContext",
                "Data Context containing the classes to scaffold", CommandOptionType.SingleValue);
            CommandOption force = app.Option("-f|--force", "Use this option to overwrite existing files",
                CommandOptionType.SingleValue);
            CommandOption relativeFolderPath = app.Option("-outDir|--relativeFolderPath",
                "Specify the relative output folder path from project where the file needs to be generated, if not specified, file will be generated in the project folder",
                CommandOptionType.SingleValue);
            CommandOption onlyGenerateFiles = app.Option("-filesOnly|--onlyGenerateFiles",
                "Will only generate the file output and will not restore any of the packages",
                CommandOptionType.SingleValue);
            CommandOption validateOnly = app.Option("-vo|--validateOnly",
                "Validates the model but does not generate the models", CommandOptionType.SingleValue);
            CommandOption area = app.Option("-a|--area",
                "The area where the generated/scaffolded code should be placed", CommandOptionType.SingleValue);
            CommandOption module = app.Option("-m|--module",
                "The prefix to apply to the module name of the generated typescript files",
                CommandOptionType.SingleValue);
            CommandOption webProject = app.Option("-wp|--webproject",
                "Relative path to the web project; if empty will search up from current folder for first project.json",
                CommandOptionType.SingleValue);
            CommandOption dataProject = app.Option("-dp|--dataproject", "Relative path to the data project",
                CommandOptionType.SingleValue);
            CommandOption targetNamespace = app.Option("-ns|--namespace", "Target Namespace for the generated code",
                CommandOptionType.SingleValue);

            app.OnExecute(async () =>
            {
                Console.WriteLine("Staring Coalesce");
                Console.WriteLine("https://github.com/IntelliTect/Coalesce");
                Console.WriteLine();


                // TODO: Remove this once MSBuild issue 2369 is fixed: https://github.com/Microsoft/msbuild/issues/2369#issuecomment-323139518
                // MSBuildWorkspace is currently broken and can't find the location of the MSBuild tools, so it errors, trying to find the Sdk.props
                // file in the current working directory (wherever IntelliTect.Coalesce.Cli.exe is) and barfs.
                // CLUMSY WORKAROUND:  Add an environment variable called VSINSTALLDIR pointed to 
                // C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe
                // or wherever you have installed Visual Studio

                var model = new CommandLineGeneratorModel
                {
                    DataContextClass = dataContextClass.Value() ?? "",
                    Force = force.Value() != null && force.Value().ToLower() == "true",
                    RelativeFolderPath = relativeFolderPath.Value() ?? "",
                    OnlyGenerateFiles = onlyGenerateFiles.Value() != null &&
                                        onlyGenerateFiles.Value().ToLower() == "true",
                    ValidateOnly = validateOnly.Value() != null && validateOnly.Value().ToLower() == "true",
                    AreaLocation = area.Value() ?? "",
                    TypescriptModulePrefix = module.Value() ?? "",
                    TargetNamespace = targetNamespace.Value() ?? ""
                };

                Console.WriteLine("Loading Projects");
                // Find the web project
                ProjectContext webContext = MsBuildProjectContextBuilder.CreateContext(webProject.Value());
                if (webContext == null) throw new ArgumentException("Web project or target namespace was not found.");

                // Find the data project
                ProjectContext dataContext = MsBuildProjectContextBuilder.CreateContext(dataProject.Value());
                if (dataContext == null) throw new ArgumentException("Data project was not found.");

                Console.WriteLine("");

                var generator = new CommandLineGenerator(webContext, dataContext);
                await generator.GenerateCode(model);

                return 0;
            });

            try
            {
                app.Execute(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                    Console.WriteLine(ex.InnerException.StackTrace);
                }
            }
        }
    }
}