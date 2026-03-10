using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using R3;

namespace WB.Process;

/// <summary>
/// A Process.
/// </summary>
/// <param name="command">The command to execute.</param>
/// <param name="arguments">A list of parameters.</param>
public sealed class Process(string command, params string[] arguments) : IDisposable
{
    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Private Fields                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘

    private readonly Subject<string> standardOutput = new();

    private readonly Subject<string> standardError = new();

    private readonly ProcessStartInfo processStartInfo = new(command, arguments)
    {
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true,
    };

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Properties                                                           │
    // └─────────────────────────────────────────────────────────────────────────────┘

    /// <summary>
    /// Gets the command that is executed by this <see cref="Process"/>.
    /// </summary>
    public string Command => processStartInfo.FileName;

    /// <summary>
    /// Gets the list of arguments.
    /// </summary>
    public IReadOnlyCollection<string> Arguments => processStartInfo.ArgumentList;

    /// <summary>
    /// Gets an observable stream standard error messages.
    /// </summary>
    public Observable<string> StandardError => standardError.AsObservable();

    /// <summary>
    /// Gets an observable stream of standard output messages.
    /// </summary>
    public Observable<string> StandardOutput => standardOutput.AsObservable();

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘

    /// <inheritdoc/>
    public void Dispose()
    {
        standardError.Dispose();
        standardOutput.Dispose();
    }

    /// <summary>
    /// Executes the <see cref="Command"/> asynchronous
    /// </summary>
    /// <returns>A <see cref="Task"/> that delivers the exit code of the <see cref="Process"/>.</returns>
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
