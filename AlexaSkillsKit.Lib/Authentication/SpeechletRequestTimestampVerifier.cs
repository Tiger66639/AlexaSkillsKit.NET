// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="SpeechletRequestTimestampVerifier.cs">
//
// </copyright>
// <summary>
//   The speechlet request timestamp verifier.
// </summary>
//
// --------------------------------------------------------------------------------------------------------------------
namespace AlexaSkillsKit.Authentication
{
    /// <summary>
    ///     The speechlet request timestamp verifier.
    /// </summary>
    public class SpeechletRequestTimestampVerifier
    {
        /// <summary>
        /// Verifies request timestamp
        /// </summary>
        /// <param name="requestEnvelope">
        /// The request Envelope.
        /// </param>
        /// <param name="referenceTimeUtc">
        /// The reference Time Utc.
        /// </param>
        /// <returns>
        /// The <see cref="System.Boolean"/> .
        /// </returns>
        public static bool VerifyRequestTimestamp(
            Json.SpeechletRequestEnvelope requestEnvelope,
            System.DateTime referenceTimeUtc)
        {
            // verify timestamp is within tolerance
            var diff = referenceTimeUtc - requestEnvelope.Request.Timestamp;
            System.Diagnostics.Debug.WriteLine(
                "Request was timestamped {0:0.00} seconds ago.",
                diff.TotalSeconds);
            return System.Math.Abs((decimal)diff.TotalSeconds) <= Sdk.TIMESTAMP_TOLERANCE_SEC;
        }
    }
}