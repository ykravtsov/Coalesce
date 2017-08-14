using System.Collections.Generic;
using Microsoft.Extensions.CommandLineUtils;
using NuGet.Frameworks;

namespace IntelliTect.Coalesce.DotnetCliTool
{
    public class DotnetBaseParams
    {
        private readonly CommandLineApplication _App;
        private CommandOption _buildBasePath;
        private CommandArgument _command;
        private CommandOption _frameworkOption;

        private CommandOption _outputOption;
        private CommandOption _projectPath;
        private CommandOption _runtimeOption;

        public DotnetBaseParams(string name, string fullName, string description)
        {
            _App = new CommandLineApplication(false)
            {
                Name = name,
                FullName = fullName,
                Description = description
            };

            AddDotnetBaseParameters();
        }


        public NuGetFramework Framework { get; set; }

        public string Command { get; set; }

        public List<string> RemainingArguments { get; set; }

        public void Parse(string[] args)
        {
            _App.OnExecute(() =>
            {
                if (_frameworkOption.HasValue())
                    Framework = NuGetFramework.Parse(_frameworkOption.Value());
                Command = _command.Value;
                RemainingArguments = _App.RemainingArguments;


                return 0;
            });

            _App.Execute(args);
        }

        private void AddDotnetBaseParameters()
        {
            _App.HelpOption("-?|-h|--help");

            _command = _App.Argument(
                "<COMMAND>",
                "The command to execute.");
        }
    }
}