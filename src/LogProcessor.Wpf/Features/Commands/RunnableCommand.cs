namespace LogProcessor.Wpf.Commands;

/// <summary>
/// Purpose: Prevent Multiple Executions. (E.g. Prevent double clicks on buttons)
/// Created by: tseb
/// </summary>
public class RunnableCommand
{
    private static readonly SemaphoreSlim _semaphore = new(1, 1);

    /// <summary>
    ///     
    /// Thread-Safe: It's a built-in .NET mechanism to ensure that only one thread can enter the critical section at a time.
    /// 
    /// await ExecuteAsync(
    ///     () => _isLoading,  // Get the current flag state
    ///     value => _isLoading = value, // Set the flag state
    ///     async() =>
    ///     {
    ///        await Task.Delay(2000); // Simulate work
    ///     });
    /// </summary>
    public static async Task ExecuteAsync(Func<bool> getFlag, Action<bool> setFlag, Func<CancellationToken, Task> action, CancellationToken cancellationToken)
    {
        if (getFlag()) return; // Prevent duplicate execution

        setFlag(true);

        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            // Execute with cancellation support
            await action(cancellationToken);
        }
        finally
        {
            setFlag(false); // Reset flag after execution
            _semaphore.Release();
        }
    }
}
