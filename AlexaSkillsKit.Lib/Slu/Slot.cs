// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Slot.cs">
//
// </copyright>
// <summary>
//   The slot.
// </summary>
//
// --------------------------------------------------------------------------------------------------------------------
namespace AlexaSkillsKit.Slu
{
    /// <summary>
    ///     The slot.
    /// </summary>
    public class Slot
    {
        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        public virtual string Value { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="json">
        /// </param>
        /// <returns>
        /// The <see cref="Slot"/> .
        /// </returns>
        public static Slot FromJson(Newtonsoft.Json.Linq.JObject json)
        {
            return new Slot { Name = json.Value<string>("name"), Value = json.Value<string>("value") };
        }
    }
}