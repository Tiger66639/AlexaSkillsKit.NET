// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Application.cs">
//
// </copyright>
// <summary>
//   The application.
// </summary>
//
// --------------------------------------------------------------------------------------------------------------------
namespace AlexaSkillsKit.Speechlet
{
    /// <summary>
    ///     The application.
    /// </summary>
    public class Application
    {
        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="json">
        /// </param>
        /// <returns>
        /// The <see cref="Application"/> .
        /// </returns>
        public static Application FromJson(Newtonsoft.Json.Linq.JObject json)
        {
            return new Application { Id = json.Value<string>("applicationId") };
        }
    }
}