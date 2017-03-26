// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="SpeechletResponseEnvelope.cs">
//
// </copyright>
// <summary>
//   The speechlet response envelope.
// </summary>
//
// --------------------------------------------------------------------------------------------------------------------
namespace AlexaSkillsKit.Json
{
    /// <summary>
    ///     The speechlet response envelope.
    /// </summary>
    public class SpeechletResponseEnvelope
    {
        /// <summary>
        ///     The _serializer settings.
        /// </summary>
        private static Newtonsoft.Json.JsonSerializerSettings _serializerSettings =
            new Newtonsoft.Json.JsonSerializerSettings()
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                ContractResolver =
                        new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                Converters =
                        new System.Collections.Generic.List<Newtonsoft.Json.JsonConverter>
                            {
                                        new Newtonsoft . Json . Converters . StringEnumConverter()
                            }
            };

        /// <summary>
        ///     Gets or sets the response.
        /// </summary>
        public virtual Speechlet.SpeechletResponse Response { get; set; }

        /// <summary>
        ///     Gets or sets the session attributes.
        /// </summary>
        public virtual System.Collections.Generic.Dictionary<string, string> SessionAttributes
        {
            get;
            set;
        }

        /// <summary>
        ///     Gets or sets the version.
        /// </summary>
        public virtual string Version { get; set; }

        /// <summary>
        /// </summary>
        /// <returns>
        ///     The <see cref="System.String" /> .
        /// </returns>
        public virtual string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, _serializerSettings);
        }
    }
}