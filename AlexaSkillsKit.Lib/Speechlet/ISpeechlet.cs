// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="ISpeechlet.cs">
//
// </copyright>
// <summary>
//   The <see cref="Speechlet" /> interface.
// </summary>
//
// --------------------------------------------------------------------------------------------------------------------
namespace AlexaSkillsKit.Speechlet
{
    /// <summary>
    ///     The <see cref="Speechlet" /> interface.
    /// </summary>
    public interface ISpeechlet
    {
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
        SpeechletResponse OnIntent(IntentRequest intentRequest, Session session);

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
        SpeechletResponse OnLaunch(LaunchRequest launchRequest, Session session);

        /// <summary>
        /// The on request validation.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <param name="referenceTimeUtc">
        /// The reference time utc.
        /// </param>
        /// <param name="requestEnvelope">
        /// The request envelope.
        /// </param>
        /// <returns>
        /// The <see cref="System.Boolean"/> .
        /// </returns>
        bool OnRequestValidation(
            Authentication.SpeechletRequestValidationResult result,
            System.DateTime referenceTimeUtc,
            Json.SpeechletRequestEnvelope requestEnvelope);

        /// <summary>
        /// The on <paramref name="session"/> ended.
        /// </summary>
        /// <param name="sessionEndedRequest">
        /// The <paramref name="session"/> ended request.
        /// </param>
        /// <param name="session">
        /// The session.
        /// </param>
        void OnSessionEnded(SessionEndedRequest sessionEndedRequest, Session session);

        /// <summary>
        /// The on <paramref name="session"/> started.
        /// </summary>
        /// <param name="sessionStartedRequest">
        /// The <paramref name="session"/> started request.
        /// </param>
        /// <param name="session">
        /// The session.
        /// </param>
        void OnSessionStarted(SessionStartedRequest sessionStartedRequest, Session session);
    }
}