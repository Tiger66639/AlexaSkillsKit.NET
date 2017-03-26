// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="ISpeechletAsync.cs">
//
// </copyright>
// <summary>
//   The <see cref="SpeechletAsync" /> interface.
// </summary>
//
// --------------------------------------------------------------------------------------------------------------------
namespace AlexaSkillsKit.Speechlet
{
    /// <summary>
    ///     The <see cref="SpeechletAsync" /> interface.
    /// </summary>
    public interface ISpeechletAsync
    {
        /// <summary>
        /// The on intent async.
        /// </summary>
        /// <param name="intentRequest">
        /// The intent request.
        /// </param>
        /// <param name="session">
        /// The session.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> .
        /// </returns>
        System.Threading.Tasks.Task<SpeechletResponse> OnIntentAsync(
            IntentRequest intentRequest,
            Session session);

        /// <summary>
        /// The on launch async.
        /// </summary>
        /// <param name="launchRequest">
        /// The launch request.
        /// </param>
        /// <param name="session">
        /// The session.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> .
        /// </returns>
        System.Threading.Tasks.Task<SpeechletResponse> OnLaunchAsync(
            LaunchRequest launchRequest,
            Session session);

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
        /// The on <paramref name="session"/> ended async.
        /// </summary>
        /// <param name="sessionEndedRequest">
        /// The <paramref name="session"/> ended request.
        /// </param>
        /// <param name="session">
        /// The session.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> .
        /// </returns>
        System.Threading.Tasks.Task OnSessionEndedAsync(
            SessionEndedRequest sessionEndedRequest,
            Session session);

        /// <summary>
        /// The on <paramref name="session"/> started async.
        /// </summary>
        /// <param name="sessionStartedRequest">
        /// The <paramref name="session"/> started request.
        /// </param>
        /// <param name="session">
        /// The session.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> .
        /// </returns>
        System.Threading.Tasks.Task OnSessionStartedAsync(
            SessionStartedRequest sessionStartedRequest,
            Session session);
    }
}