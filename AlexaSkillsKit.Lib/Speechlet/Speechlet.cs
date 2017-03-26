﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Speechlet.cs">
//
// </copyright>
// <summary>
//   The speechlet.
// </summary>
//
// --------------------------------------------------------------------------------------------------------------------
namespace AlexaSkillsKit.Speechlet
{
    /// <summary>
    ///     The speechlet.
    /// </summary>
    public abstract class Speechlet : ISpeechlet
    {
        /// <summary>
        /// Processes Alexa request AND validates request signature
        /// </summary>
        /// <param name="httpRequest">
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> .
        /// </returns>
        public virtual System.Net.Http.HttpResponseMessage GetResponse(
            System.Net.Http.HttpRequestMessage httpRequest)
        {
            Authentication.SpeechletRequestValidationResult validationResult =
                Authentication.SpeechletRequestValidationResult.OK;
            System.DateTime now = System.DateTime.UtcNow; // reference time for this request

            string chainUrl = null;
            if (!httpRequest.Headers.Contains(Sdk.SIGNATURE_CERT_URL_REQUEST_HEADER)
                || string.IsNullOrEmpty(
                    chainUrl =
                        System.Linq.Enumerable.First(
                            httpRequest.Headers.GetValues(Sdk.SIGNATURE_CERT_URL_REQUEST_HEADER))))
            {
                validationResult = validationResult
                                   | Authentication.SpeechletRequestValidationResult.NoCertHeader;
            }

            string signature = null;
            if (!httpRequest.Headers.Contains(Sdk.SIGNATURE_REQUEST_HEADER)
                || string.IsNullOrEmpty(
                    signature =
                        System.Linq.Enumerable.First(
                            httpRequest.Headers.GetValues(Sdk.SIGNATURE_REQUEST_HEADER))))
            {
                validationResult = validationResult
                                   | Authentication.SpeechletRequestValidationResult.NoSignatureHeader;
            }

            var alexaBytes =
                AsyncHelpers.RunSync<byte[]>(() => httpRequest.Content.ReadAsByteArrayAsync());
            System.Diagnostics.Debug.WriteLine(httpRequest.ToLogString());

            // attempt to verify signature only if we were able to locate certificate and signature headers
            if (validationResult == Authentication.SpeechletRequestValidationResult.OK)
            {
                if (
                    !Authentication.SpeechletRequestSignatureVerifier.VerifyRequestSignature(
                        alexaBytes,
                        signature,
                        chainUrl))
                {
                    validationResult = validationResult
                                       | Authentication.SpeechletRequestValidationResult
                                           .InvalidSignature;
                }
            }

            Json.SpeechletRequestEnvelope alexaRequest = null;
            try
            {
                var alexaContent = System.Text.Encoding.UTF8.GetString(alexaBytes);
                alexaRequest = Json.SpeechletRequestEnvelope.FromJson(alexaContent);
            }
            catch (Newtonsoft.Json.JsonReaderException)
            {
                validationResult = validationResult
                                   | Authentication.SpeechletRequestValidationResult.InvalidJson;
            }
            catch (System.InvalidCastException)
            {
                validationResult = validationResult
                                   | Authentication.SpeechletRequestValidationResult.InvalidJson;
            }

            // attempt to verify timestamp only if we were able to parse request body
            if (alexaRequest != null)
            {
                if (
                    !Authentication.SpeechletRequestTimestampVerifier.VerifyRequestTimestamp(
                        alexaRequest,
                        now))
                {
                    validationResult = validationResult
                                       | Authentication.SpeechletRequestValidationResult
                                           .InvalidTimestamp;
                }
            }

            if (alexaRequest == null || !this.OnRequestValidation(validationResult, now, alexaRequest))
            {
                return
                    new System.Net.Http.HttpResponseMessage(
                            System.Net.HttpStatusCode.BadRequest)
                    {
                        ReasonPhrase = validationResult.ToString()
                    };
            }

            string alexaResponse = this.DoProcessRequest(alexaRequest);

            System.Net.Http.HttpResponseMessage httpResponse;
            if (alexaResponse == null)
            {
                httpResponse =
                    new System.Net.Http.HttpResponseMessage(
                        System.Net.HttpStatusCode.InternalServerError);
            }
            else
            {
                httpResponse =
                    new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.OK);
                httpResponse.Content = new System.Net.Http.StringContent(
                                             alexaResponse,
                                             System.Text.Encoding.UTF8,
                                             "application/json");
                System.Diagnostics.Debug.WriteLine(httpResponse.ToLogString());
            }

            return httpResponse;
        }

        /// <summary>
        /// The on intent.
        /// </summary>
        /// <param name="intentRequest">
        /// The intent request.
        /// </param>
        /// <param name="session">
        /// The session.
        /// </param>
        /// <returns>
        /// The <see cref="SpeechletResponse"/> .
        /// </returns>
        public abstract SpeechletResponse OnIntent(IntentRequest intentRequest, Session session);

        /// <summary>
        /// The on launch.
        /// </summary>
        /// <param name="launchRequest">
        /// The launch request.
        /// </param>
        /// <param name="session">
        /// The session.
        /// </param>
        /// <returns>
        /// The <see cref="SpeechletResponse"/> .
        /// </returns>
        public abstract SpeechletResponse OnLaunch(LaunchRequest launchRequest, Session session);

        /// <summary>
        /// Opportunity to set policy for handling requests with invalid
        ///     signatures and/or timestamps
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <param name="referenceTimeUtc">
        /// The reference Time Utc.
        /// </param>
        /// <param name="requestEnvelope">
        /// The request Envelope.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if request processing should continue,
        ///     otherwise <see langword="false"/>
        /// </returns>
        public virtual bool OnRequestValidation(
            Authentication.SpeechletRequestValidationResult result,
            System.DateTime referenceTimeUtc,
            Json.SpeechletRequestEnvelope requestEnvelope)
        {
            return result == Authentication.SpeechletRequestValidationResult.OK;
        }

        /// <summary>
        /// The on <paramref name="session"/> ended.
        /// </summary>
        /// <param name="sessionEndedRequest">
        /// The <paramref name="session"/> ended request.
        /// </param>
        /// <param name="session">
        /// The session.
        /// </param>
        public abstract void OnSessionEnded(SessionEndedRequest sessionEndedRequest, Session session);

        /// <summary>
        /// The on <paramref name="session"/> started.
        /// </summary>
        /// <param name="sessionStartedRequest">
        /// The <paramref name="session"/> started request.
        /// </param>
        /// <param name="session">
        /// The session.
        /// </param>
        public abstract void OnSessionStarted(SessionStartedRequest sessionStartedRequest, Session session);

        /// <summary>
        /// </summary>
        /// <param name="requestContent">
        /// </param>
        /// <returns>
        /// The <see cref="System.String"/> .
        /// </returns>
        public virtual string ProcessRequest(string requestContent)
        {
            var requestEnvelope = Json.SpeechletRequestEnvelope.FromJson(requestContent);
            return this.DoProcessRequest(requestEnvelope);
        }

        /// <summary>
        /// </summary>
        /// <param name="requestJson">
        /// </param>
        /// <returns>
        /// The <see cref="System.String"/> .
        /// </returns>
        public virtual string ProcessRequest(Newtonsoft.Json.Linq.JObject requestJson)
        {
            var requestEnvelope = Json.SpeechletRequestEnvelope.FromJson(requestJson);
            return this.DoProcessRequest(requestEnvelope);
        }

        /// <summary>
        /// </summary>
        /// <param name="requestEnvelope">
        /// </param>
        /// <returns>
        /// The <see cref="System.String"/> .
        /// </returns>
        private string DoProcessRequest(Json.SpeechletRequestEnvelope requestEnvelope)
        {
            Session session = requestEnvelope.Session;
            SpeechletResponse response = null;

            // process launch request
            if (requestEnvelope.Request is LaunchRequest)
            {
                var request = requestEnvelope.Request as LaunchRequest;
                if (requestEnvelope.Session.IsNew)
                {
                    this.OnSessionStarted(
                        new SessionStartedRequest(request.RequestId, request.Timestamp),
                        session);
                }

                response = this.OnLaunch(request, session);
            }

            // process intent request
            else if (requestEnvelope.Request is IntentRequest)
            {
                var request = requestEnvelope.Request as IntentRequest;

                // Do session management prior to calling OnSessionStarted and OnIntentAsync
                // to allow dev to change session values if behavior is not desired
                this.DoSessionManagement(request, session);

                if (requestEnvelope.Session.IsNew)
                {
                    this.OnSessionStarted(
                        new SessionStartedRequest(request.RequestId, request.Timestamp),
                        session);
                }

                response = this.OnIntent(request, session);
            }

            // process session ended request
            else if (requestEnvelope.Request is SessionEndedRequest)
            {
                var request = requestEnvelope.Request as SessionEndedRequest;
                this.OnSessionEnded(request, session);
            }

            var responseEnvelope = new Json.SpeechletResponseEnvelope
            {
                Version = requestEnvelope.Version,
                Response = response,
                SessionAttributes = requestEnvelope.Session.Attributes
            };
            return responseEnvelope.ToJson();
        }

        /// <summary>
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="session">
        /// The session.
        /// </param>
        private void DoSessionManagement(IntentRequest request, Session session)
        {
            if (session.IsNew)
            {
                session.Attributes[Session.INTENT_SEQUENCE] = request.Intent.Name;
            }
            else
            {
                // if the session was started as a result of a launch request
                // a first intent isn't yet set, so set it to the current intent
                if (!session.Attributes.ContainsKey(Session.INTENT_SEQUENCE))
                {
                    session.Attributes[Session.INTENT_SEQUENCE] = request.Intent.Name;
                }
                else
                {
                    session.Attributes[Session.INTENT_SEQUENCE] += Session.SEPARATOR
                                                                       + request.Intent.Name;
                }
            }

            // Auto-session management: copy all slot values from current intent into session
            foreach (var slot in request.Intent.Slots.Values)
            {
                session.Attributes[slot.Name] = slot.Value;
            }
        }
    }
}