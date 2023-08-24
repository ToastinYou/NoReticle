using CitizenFX.Core;
using System.Runtime.CompilerServices;
using static NoReticle.Client.Client;

namespace NoReticle.Client
{
    public static class ClientLog
    {
        public static readonly string MessagePrefix = "[NoReticle]:";

        /// <summary>
        /// Writes a debug message to the client's console.
        /// </summary>
        /// <param name="message">Message to display.</param>
        public static void Log(string message)
        {
            Debug.WriteLine($"{MessagePrefix} ({Handle}) {message}");
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
