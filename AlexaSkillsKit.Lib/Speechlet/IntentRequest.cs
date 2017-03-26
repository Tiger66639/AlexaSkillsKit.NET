// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="IntentRequest.cs">
//
// </copyright>
// <summary>
//   The intent request.
// </summary>
//
// --------------------------------------------------------------------------------------------------------------------
namespace AlexaSkillsKit.Speechlet
{
    /// <summary>
    ///     The intent request.
    /// </summary>
    public class IntentRequest : SpeechletRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntentRequest"/> class.
        /// Initializes a new instance of the <see cref="IntentRequest"/>
        ///     class.
        /// </summary>
        /// <param name="requestId">
        /// The request id.
        /// </param>
        /// <param name="timestamp">
        /// The timestamp.
        /// </param>
        /// <param name="intent">
        /// The intent.
        /// </param>
        public IntentRequest(string requestId, System.DateTime timestamp, Slu.Intent intent)
            : base(requestId, timestamp)
        {
            this.Intent = intent;
        }

        /// <summary>
        ///     Gets the intent.
        /// </summary>
        public virtual Slu.Intent Intent { get; private set; }
    }
}