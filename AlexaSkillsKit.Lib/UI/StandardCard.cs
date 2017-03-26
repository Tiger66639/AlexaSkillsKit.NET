// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="StandardCard.cs">
//
// </copyright>
// <summary>
//   The standard card.
// </summary>
//
// --------------------------------------------------------------------------------------------------------------------
namespace AlexaSkillsKit.UI
{
    /// <summary>
    ///     The standard card.
    /// </summary>
    public class StandardCard : Card
    {
        /// <summary>
        ///     Gets or sets the image.
        /// </summary>
        public virtual Image Image { get; set; }

        /// <summary>
        ///     Gets or sets the text.
        /// </summary>
        public virtual string Text { get; set; }

        /// <summary>
        ///     Gets the type.
        /// </summary>
        public override string Type
        {
            get
            {
                return "Standard";
            }
        }
    }
}