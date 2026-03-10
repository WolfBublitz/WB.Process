#:package System.CommandLine@2.0.3

using System.CommandLine;

Command buildNugetCommand = new("nuget", "Builds a NuGet package from a .csproj file")
{
    new Argument<string>("project-file")
};

RootCommand rootCommand = new("Ascii Art file-based app sample");

rootCommand.Subcommands.Add(buildNugetCommand);

ParseResult parseResult = rootCommand.Parse(args);

return await parseResult.InvokeAsync().ConfigureAwait(false);