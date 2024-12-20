using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// <para>Class for logging.</para>
/// </summary>
public static class CustomLogger
{
    /// <summary>
    /// <para>Log error message.</para>
    /// </summary>
    /// <param name="sender">class that sends the message</param>
    /// <param name="message">the message</param>
    public static void Error(string message, [CallerFilePath] string sender = "")
    {
        Debug.LogError($"[{ClassName(sender)}]: {message}");
    }

    /// <summary>
    /// <para>Log debug message.</para>
    /// </summary>
    /// <param name="sender">class that sends the message</param>
    /// <param name="message">the message</param>
    public static void Log(string message, [CallerFilePath] string sender = "")
    {
        Debug.Log($"[{ClassName(sender)}]: {message}");
    }

    /// <summary>
    /// <para>Log warning message.</para>
    /// </summary>
    /// <param name="sender">class that sends the message</param>
    /// <param name="message">the message</param>
    public static void Warning(string message, [CallerFilePath] string sender = "")
    {
        Debug.LogWarning($"[{ClassName(sender)}]: {message}");
    }

    private static string ClassName(string absoluteClassPath)
    {
        return Path.GetFileNameWithoutExtension(absoluteClassPath);
    }
}