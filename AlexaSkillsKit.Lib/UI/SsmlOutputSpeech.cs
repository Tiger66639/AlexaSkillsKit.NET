// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="SsmlOutputSpeech.cs">
//   
// </copyright>
// <summary>
//   The ssml output speech.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace AlexaSkillsKit .UI
    {
        /// <summary>
        ///     The ssml output speech.
        /// </summary>
        public class SsmlOutputSpeech : OutputSpeech
            {
                /// <summary>
                ///     Gets or sets the ssml.
                /// </summary>
                public virtual string Ssml { get ; set ; }

                /// <summary>
                ///     Gets the type.
                /// </summary>
                public override string Type
                    {
                        get
                            {
                                return "SSML" ;
                            }
                    }
            }
    }
