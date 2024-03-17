using UnityEngine;

/// <summary>
/// <para>Class for logging.</para>
/// </summary>
public static class CustomLogger
{
    /// <summary>
    /// Log error message.
    /// </summary>
    /// <param name="obj">the object that sends messages</param>
    /// <param name="message">the message</param>
    public static void Error(Object obj, string message)
    {
        Debug.LogError($"[{obj.name}]: {message}");
    }

    /// <summary>
    /// Log debug message.
    /// </summary>
    /// <param name="obj">the object that sends messages</param>
    /// <param name="message">the message</param>
    public static void Log(Object obj, string message)
    {
        Debug.Log($"[{obj.name}]: {message}");
    }
}