// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="SpeechletRequestEnvelope.cs">
//   
// </copyright>
// <summary>
//   The speechlet request envelope.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace AlexaSkillsKit .Json
    {
        /// <summary>
        ///     The speechlet request envelope.
        /// </summary>
        public class SpeechletRequestEnvelope
            {
                /// <summary>
                ///     Gets or sets the request.
                /// </summary>
                public virtual Speechlet . SpeechletRequest Request { get ; set ; }

                /// <summary>
                ///     Gets or sets the session.
                /// </summary>
                public virtual Speechlet . Session Session { get ; set ; }

                /// <summary>
                ///     Gets or sets the version.
                /// </summary>
                public virtual string Version { get ; set ; }

                /// <summary>
                /// </summary>
                /// <param name="content">
                /// </param>
                /// <returns>
                /// The <see cref="SpeechletRequestEnvelope"/> .
                /// </returns>
                public static SpeechletRequestEnvelope FromJson( string content )
                    {
                        if (string . IsNullOrEmpty(content))
                        {
                            throw new Speechlet . SpeechletException("Request content is empty") ;
                        }

                        Newtonsoft . Json . Linq . JObject json =
                            Newtonsoft . Json . JsonConvert . DeserializeObject<Newtonsoft . Json . Linq . JObject>(
                                content,
                                Sdk . DeserializationSettings) ;
                        return FromJson(json) ;
                    }

                /// <summary>
                /// </summary>
                /// <param name="json">
                /// </param>
                /// <returns>
                /// The <see cref="SpeechletRequestEnvelope"/> .
                /// </returns>
                public static SpeechletRequestEnvelope FromJson( Newtonsoft . Json . Linq . JObject json )
                    {
                        if (json["version"] != null && json . Value<string>("version") != Sdk . VERSION)
                        {
                            throw new Speechlet . SpeechletException("Request must conform to 1.0 schema.") ;
                        }

                        Speechlet . SpeechletRequest request ;
                        Newtonsoft . Json . Linq . JObject requestJson =
                            json . Value<Newtonsoft . Json . Linq . JObject>("request") ;
                        string requestType = requestJson . Value<string>("type") ;
                        string requestId = requestJson . Value<string>("requestId") ;
                        System . DateTime timestamp = requestJson . Value<System . DateTime>("timestamp") ;
                        switch (requestType)
                        {
                            case "LaunchRequest" :
                                request = new Speechlet . LaunchRequest(requestId, timestamp) ;
                                break ;

                            case "IntentRequest" :
                                request = new Speechlet . IntentRequest(
                                              requestId,
                                              timestamp,
                                              Slu . Intent . FromJson(
                                                  requestJson . Value<Newtonsoft . Json . Linq . JObject>("intent"))) ;
                                break ;

                            case "SessionStartedRequest" :
                                request = new Speechlet . SessionStartedRequest(requestId, timestamp) ;
                                break ;

                            case "SessionEndedRequest" :
                                Speechlet . SessionEndedRequest . ReasonEnum reason ;
                                System . Enum . TryParse<Speechlet . SessionEndedRequest . ReasonEnum>(
                                    requestJson . Value<string>("reason"),
                                    out reason) ;
                                request = new Speechlet . SessionEndedRequest(requestId, timestamp, reason) ;
                                break ;

                            default :
                                throw new System . ArgumentException("json") ;
                        }

                        return new SpeechletRequestEnvelope
                                   {
                                       Request = request,
                                       Session =
                                           Speechlet . Session . FromJson(
                                               json . Value<Newtonsoft . Json . Linq . JObject>("session")),
                                       Version = json . Value<string>("version")
                                   } ;
                    }
            }
    }
