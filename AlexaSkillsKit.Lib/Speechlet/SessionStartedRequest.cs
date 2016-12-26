// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="SessionStartedRequest.cs">
//   
// </copyright>
// <summary>
//   The session started request.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace AlexaSkillsKit .Speechlet
    {
        /// <summary>
        ///     The session started request.
        /// </summary>
        public class SessionStartedRequest : SpeechletRequest
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="SessionStartedRequest"/> class. 
                /// Initializes a new instance of the
                ///     <see cref="SessionStartedRequest"/> class.
                /// </summary>
                /// <param name="requestId">
                /// The request id.
                /// </param>
                /// <param name="timestamp">
                /// The timestamp.
                /// </param>
                public SessionStartedRequest( string requestId, System . DateTime timestamp )
                    : base(requestId, timestamp)
                    {}
            }
    }
