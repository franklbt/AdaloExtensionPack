using System;
using System.Threading.Tasks;

namespace AdaloExtensionPack.Core.Helpers;

public static class TasksExtensions
{
    public static void Ignore(this Task task) => ArgumentNullException.ThrowIfNull(task);

    public static void Ignore<T>(this Task<T> task) => ArgumentNullException.ThrowIfNull(task);
}
