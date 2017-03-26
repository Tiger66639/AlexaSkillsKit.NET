// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="SpeechletRequest.cs">
//
// </copyright>
// <summary>
//   The speechlet request.
// </summary>
//
// --------------------------------------------------------------------------------------------------------------------
namespace AlexaSkillsKit.Speechlet
{
    /// <summary>
    ///     The speechlet request.
    /// </summary>
    public abstract class SpeechletRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpeechletRequest"/> class.
        /// Initializes a new instance of the <see cref="SpeechletRequest"/>
        ///     class.
        /// </summary>
        /// <param name="requestId">
        /// The request id.
        /// </param>
        /// <param name="timestamp">
        /// The timestamp.
        /// </param>
        public SpeechletRequest(string requestId, System.DateTime timestamp)
        {
            this.RequestId = requestId;
            this.Timestamp = timestamp;
        }

        /// <summary>
        ///     Gets the request id.
        /// </summary>
        public string RequestId { get; private set; }

        /// <summary>
        ///     Gets the timestamp.
        /// </summary>
        public System.DateTime Timestamp { get; private set; }
    }
}