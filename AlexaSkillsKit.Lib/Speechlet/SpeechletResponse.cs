// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="SpeechletResponse.cs">
//
// </copyright>
// <summary>
//   The speechlet response.
// </summary>
//
// --------------------------------------------------------------------------------------------------------------------
namespace AlexaSkillsKit.Speechlet
{
    /// <summary>
    ///     The speechlet response.
    /// </summary>
    public class SpeechletResponse
    {
        /// <summary>
        ///     Gets or sets the card.
        /// </summary>
        public virtual UI.Card Card { get; set; }

        /// <summary>
        ///     Gets or sets the output speech.
        /// </summary>
        public virtual UI.OutputSpeech OutputSpeech { get; set; }

        /// <summary>
        ///     Gets or sets the reprompt.
        /// </summary>
        public virtual UI.Reprompt Reprompt { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether should end session.
        /// </summary>
        public virtual bool ShouldEndSession { get; set; }
    }
}