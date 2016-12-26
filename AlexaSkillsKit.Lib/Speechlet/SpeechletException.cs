// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="SpeechletException.cs">
//   
// </copyright>
// <summary>
//   The speechlet exception.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace AlexaSkillsKit .Speechlet
    {
        /// <summary>
        ///     The speechlet exception.
        /// </summary>
        public class SpeechletException : System . Exception
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="SpeechletException"/> class. 
                ///     Initializes a new instance of the <see cref="SpeechletException"/>
                ///     class.
                /// </summary>
                public SpeechletException( )
                    : base()
                    {}

                /// <summary>
                /// Initializes a new instance of the <see cref="SpeechletException"/> class. 
                /// Initializes a new instance of the <see cref="SpeechletException"/>
                ///     class.
                /// </summary>
                /// <param name="message">
                /// The message.
                /// </param>
                public SpeechletException( string message )
                    : base(message)
                    {}

                /// <summary>
                /// Initializes a new instance of the <see cref="SpeechletException"/> class. 
                /// Initializes a new instance of the <see cref="SpeechletException"/>
                ///     class.
                /// </summary>
                /// <param name="message">
                /// The message.
                /// </param>
                /// <param name="cause">
                /// The cause.
                /// </param>
                public SpeechletException( string message, System . Exception cause )
                    : base(message, cause)
                    {}
            }
    }
