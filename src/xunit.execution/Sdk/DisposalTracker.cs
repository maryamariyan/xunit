using System;
using System.Collections.Concurrent;

namespace Xunit.Sdk
{
    /// <summary>
    /// Tracks disposable objects, and disposes them in the reverse order they were added to
    /// the tracker.
    /// </summary>
    public class DisposalTracker : IDisposable
    {
        readonly ConcurrentStack<IDisposable> toDispose = new ConcurrentStack<IDisposable>();

        /// <summary>
        /// Add an object to be disposed.
        /// </summary>
        /// <param name="disposable">The object to be disposed.</param>
        public void Add(IDisposable disposable)
        {
            toDispose.Push(disposable);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            for (int i = toDispose.Count; i > 0; --i)
            {
                IDisposable obj;
                // keep trying until TryPop succeeds
                while (!toDispose.TryPop(out obj));

                obj.Dispose();
            }

            toDispose.Clear();
        }
    }
}
