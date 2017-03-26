// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Card.cs">
//
// </copyright>
// <summary>
//   The card.
// </summary>
//
// --------------------------------------------------------------------------------------------------------------------
namespace AlexaSkillsKit.UI
{
    /// <summary>
    ///     The card.
    /// </summary>
    public abstract class Card
    {
        /// <summary>
        ///     Gets or sets the subtitle.
        /// </summary>
        [System.Obsolete(
             "field has been deprecated from ASK and will be removed in a future version of this library")]
        public virtual string Subtitle { get; set; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        ///     Gets the type.
        /// </summary>
        public abstract string Type { get; }
    }
}