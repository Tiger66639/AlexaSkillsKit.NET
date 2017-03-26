// Session.cs

namespace AlexaSkillsKit.Speechlet
{
    public class Session
    {
        public const string INTENT_SEQUENCE = "intentSequence";

        public const string SEPARATOR = ";";

        public virtual Application Application { get; set; }

        public virtual System.Collections.Generic.Dictionary<string, string> Attributes { get; set; }

        public virtual string[] IntentSequence
        {
            get
            {
                return string.IsNullOrEmpty(this.Attributes[INTENT_SEQUENCE])
                           ? new string[0]
                           : this.Attributes[INTENT_SEQUENCE].Split(
                               new string[1] { SEPARATOR },
                               System.StringSplitOptions.RemoveEmptyEntries);
            }
        }

        public virtual bool IsNew { get; set; }

        public virtual string SessionId { get; set; }

        public virtual User User { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="json"></param>
        /// <returns>
        /// </returns>
        public static Session FromJson(Newtonsoft.Json.Linq.JObject json)
        {
            var attributes = new System.Collections.Generic.Dictionary<string, string>();
            var jsonAttributes = json.Value<Newtonsoft.Json.Linq.JObject>("attributes");
            if (jsonAttributes != null)
            {
                foreach (var attrib in jsonAttributes.Children())
                {
                    attributes.Add(
                        Newtonsoft.Json.Linq.Extensions.Value<Newtonsoft.Json.Linq.JProperty>(
                            attrib).Name,
                        Newtonsoft.Json.Linq.Extensions.Value<Newtonsoft.Json.Linq.JProperty>(
                            attrib).Value.ToString());
                }
            }

            return new Session
            {
                SessionId = json.Value<string>("sessionId"),
                IsNew = json.Value<bool>("new"),
                User = User.FromJson(json.Value<Newtonsoft.Json.Linq.JObject>("user")),
                Application =
                               Application.FromJson(
                                   json.Value<Newtonsoft.Json.Linq.JObject>("application")),
                Attributes = attributes
            };
        }
    }
}