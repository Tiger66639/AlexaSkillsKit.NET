// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="PlainTextOutputSpeech.cs">
//   
// </copyright>
// <summary>
//   The plain text output speech.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace AlexaSkillsKit .UI
    {
        /// <summary>
        ///     The plain text output speech.
        /// </summary>
        public class PlainTextOutputSpeech : OutputSpeech
            {
                /// <summary>
                ///     Gets or sets the text.
                /// </summary>
                public virtual string Text { get ; set ; }

                /// <summary>
                ///     Gets the type.
                /// </summary>
                public override string Type
                    {
                        get
                            {
                                return "PlainText" ;
                            }
                    }
            }
    }
