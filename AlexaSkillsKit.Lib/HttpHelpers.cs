// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="HttpHelpers.cs">
//
// </copyright>
// <summary>
//   The http helpers.
// </summary>
//
// --------------------------------------------------------------------------------------------------------------------
namespace AlexaSkillsKit
{
    /// <summary>
    ///     The http helpers.
    /// </summary>
    public static class HttpHelpers
    {
        /// <summary>
        /// </summary>
        /// <param name="httpRequest">
        /// The http Request.
        /// </param>
        /// <returns>
        /// The <see cref="System.String"/> .
        /// </returns>
        public static string ToLogString(this System.Net.Http.HttpRequestMessage httpRequest)
        {
            var serializedRequest =
                AsyncHelpers.RunSync<byte[]>(
                    () =>
                        new System.Net.Http.HttpMessageContent(httpRequest).ReadAsByteArrayAsync
                            ());
            return System.Text.Encoding.UTF8.GetString(serializedRequest);
        }

        /// <summary>
        /// </summary>
        /// <param name="httpResponse">
        /// The http Response.
        /// </param>
        /// <returns>
        /// The <see cref="System.String"/> .
        /// </returns>
        public static string ToLogString(this System.Net.Http.HttpResponseMessage httpResponse)
        {
            var serializedRequest =
                AsyncHelpers.RunSync<byte[]>(
                    () =>
                        new System.Net.Http.HttpMessageContent(httpResponse)
                            .ReadAsByteArrayAsync());
            return System.Text.Encoding.UTF8.GetString(serializedRequest);
        }
    }
}