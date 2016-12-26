// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="LaunchRequest.cs">
//   
// </copyright>
// <summary>
//   The launch request.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace AlexaSkillsKit .Speechlet
    {
        /// <summary>
        ///     The launch request.
        /// </summary>
        public class LaunchRequest : SpeechletRequest
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="LaunchRequest"/> class. 
                /// Initializes a new instance of the <see cref="LaunchRequest"/>
                ///     class.
                /// </summary>
                /// <param name="requestId">
                /// The request id.
                /// </param>
                /// <param name="timestamp">
                /// The timestamp.
                /// </param>
                public LaunchRequest( string requestId, System . DateTime timestamp )
                    : base(requestId, timestamp)
                    {}
            }
    }
