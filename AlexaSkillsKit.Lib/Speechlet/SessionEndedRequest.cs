// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="SessionEndedRequest.cs">
//
// </copyright>
// <summary>
//   The session ended request.
// </summary>
//
// --------------------------------------------------------------------------------------------------------------------
namespace AlexaSkillsKit.Speechlet
{
    /// <summary>
    ///     The session ended request.
    /// </summary>
    public class SessionEndedRequest : SpeechletRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionEndedRequest"/> class.
        /// Initializes a new instance of the <see cref="SessionEndedRequest"/>
        ///     class.
        /// </summary>
        /// <param name="requestId">
        /// The request id.
        /// </param>
        /// <param name="timestamp">
        /// The timestamp.
        /// </param>
        /// <param name="reason">
        /// The reason.
        /// </param>
        public SessionEndedRequest(string requestId, System.DateTime timestamp, ReasonEnum reason)
            : base(requestId, timestamp)
        {
            this.Reason = reason;
        }

        /// <summary>
        ///     The reason enum.
        /// </summary>
        public enum ReasonEnum
        {
            /// <summary>
            ///     The none.
            /// </summary>
            NONE = 0, // default in case parsing fails

            /// <summary>
            ///     The error.
            /// </summary>
            ERROR,

            /// <summary>
            ///     The use r_ initiated.
            /// </summary>
            USER_INITIATED,

            /// <summary>
            ///     The exceede d_ ma x_ reprompts.
            /// </summary>
            EXCEEDED_MAX_REPROMPTS,
        }

        /// <summary>
        ///     Gets the reason.
        /// </summary>
        public virtual ReasonEnum Reason { get; private set; }
    }
}