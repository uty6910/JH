using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

public static class IDisposableExtensions
{
    public static void DisposeAll(this IEnumerable<IDisposable> disposables)
    { disposables.ToList().ForEach(_ => _.Dispose()); }

    public static IDisposable AddTo(this IDisposable currentDisposable, ICollection<IDisposable> disposables)
    {
        disposables.Add(currentDisposable);
        return currentDisposable;
    }
}