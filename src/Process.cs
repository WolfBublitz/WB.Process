using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using R3;

namespace WB.Process;

public sealed class Process(string command, string[] arguments) : IDisposable
{
    private readonly Subject<string> standardOutput = new();

    private readonly Subject<string> standardError = new();

    private readonly ProcessStartInfo processStartInfo = new(command, arguments)
    {
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true,
    };

    public string Command => processStartInfo.FileName;

    public IReadOnlyCollection<string> Arguments => processStartInfo.ArgumentList;

    public Observable<string> StandardError { get; }

    public Observable<string> StandardOutput { get; }

    /// <inheritdoc/>
    public void Dispose()
    {
        standardError.Dispose();
        standardOutput.Dispose();
    }

    public async Task<int> ExecutAsync()
    {
        using System.Diagnostics.Process process = new()
        {
            StartInfo = processStartInfo,
            EnableRaisingEvents = true,
        };

        process.OutputDataReceived += (sender, args) =>
        {
            if (args.Data is not null)
            {
                standardOutput.OnNext(args.Data);
            }
        };

        process.ErrorDataReceived += (sender, args) =>
        {
            if (args.Data is not null)
            {
                standardError.OnNext(args.Data);
            }
        };

        await process.WaitForExitAsync().ConfigureAwait(false);

        return process.ExitCode;
    }
}
