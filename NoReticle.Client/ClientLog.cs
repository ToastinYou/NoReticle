using CitizenFX.Core;
using NoReticle.Shared;
using System.Runtime.CompilerServices;
using static NoReticle.Client.Client;

namespace NoReticle.Client
{
    public static class ClientLog
    {
        /// <summary>
        /// Writes a debug message to the client's console.
        /// </summary>
        /// <param name="message">Message to display.</param>
        public static void Log(string message)
        {
            Debug.WriteLine($"{Constants.Prefix} ({Handle}) {message}");
        }

        /// <summary>
        /// Writes a debug message to the client's console with a reference to the calling function.
        /// </summary>
        /// <param name="message">Message to display.</param>
        /// <param name="callingMethodName"></param>
        public static void Trace(string message, [CallerMemberName] string callingMethodName = null)
        {
            Log($"({callingMethodName}) {message}");
        }
    }
}
