// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="SimpleCard.cs">
//
// </copyright>
// <summary>
//   The simple card.
// </summary>
//
// --------------------------------------------------------------------------------------------------------------------
namespace AlexaSkillsKit.UI
{
    /// <summary>
    ///     The simple card.
    /// </summary>
    public class SimpleCard : Card
    {
        /// <summary>
        ///     Gets or sets the content.
        /// </summary>
        public virtual string Content { get; set; }

        /// <summary>
        ///     Gets the type.
        /// </summary>
        public override string Type
        {
            get
            {
                return "Simple";
            }
        }
    }
}