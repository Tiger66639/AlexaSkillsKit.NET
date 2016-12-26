// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="SpeechletRequestValidationResult.cs">
//   
// </copyright>
// <summary>
//   The speechlet request validation result.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace AlexaSkillsKit .Authentication
    {
        /// <summary>
        ///     The speechlet request validation result.
        /// </summary>
        [System . Flags]
        public enum SpeechletRequestValidationResult
            {
                /// <summary>
                ///     The ok.
                /// </summary>
                OK = 0,

                /// <summary>
                ///     The no signature header.
                /// </summary>
                NoSignatureHeader = 1,

                /// <summary>
                ///     The no cert header.
                /// </summary>
                NoCertHeader = 2,

                /// <summary>
                ///     The invalid signature.
                /// </summary>
                InvalidSignature = 4,

                /// <summary>
                ///     The invalid timestamp.
                /// </summary>
                InvalidTimestamp = 8,

                /// <summary>
                ///     The invalid json.
                /// </summary>
                InvalidJson = 16
            }
    }
