// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Sdk.cs">
//
// </copyright>
// <summary>
//   The sdk.
// </summary>
//
// --------------------------------------------------------------------------------------------------------------------
namespace AlexaSkillsKit
{
    /// <summary>
    ///     The sdk.
    /// </summary>
    public static class Sdk
    {
        /// <summary>
        ///     The characte r_ encoding.
        /// </summary>
        public const string CHARACTER_ENCODING = "UTF-8";

        /// <summary>
        ///     The ech o_ ap i_ domai n_ name.
        /// </summary>
        public const string ECHO_API_DOMAIN_NAME = "echo-api.amazon.com";

        /// <summary>
        ///     The signatur e_ algorithm.
        /// </summary>
        public const string SIGNATURE_ALGORITHM = "SHA1withRSA";

        /// <summary>
        ///     The signatur e_ cer t_ type.
        /// </summary>
        public const string SIGNATURE_CERT_TYPE = "X.509";

        /// <summary>
        ///     The signatur e_ cer t_ ur l_ host.
        /// </summary>
        public const string SIGNATURE_CERT_URL_HOST = "s3.amazonaws.com";

        /// <summary>
        ///     The signatur e_ cer t_ ur l_ path.
        /// </summary>
        public const string SIGNATURE_CERT_URL_PATH = "/echo.api/";

        /// <summary>
        ///     The signatur e_ cer t_ ur l_ reques t_ header.
        /// </summary>
        public const string SIGNATURE_CERT_URL_REQUEST_HEADER = "SignatureCertChainUrl";

        /// <summary>
        ///     The signatur e_ ke y_ type.
        /// </summary>
        public const string SIGNATURE_KEY_TYPE = "RSA";

        /// <summary>
        ///     The signatur e_ reques t_ header.
        /// </summary>
        public const string SIGNATURE_REQUEST_HEADER = "Signature";

        /// <summary>
        ///     The timestam p_ toleranc e_ sec.
        /// </summary>
        public const int TIMESTAMP_TOLERANCE_SEC = 150;

        /// <summary>
        ///     The version.
        /// </summary>
        public const string VERSION = "1.0";

        /// <summary>
        ///     The deserialization settings.
        /// </summary>
        public static Newtonsoft.Json.JsonSerializerSettings DeserializationSettings =
            new Newtonsoft.Json.JsonSerializerSettings
            {
                MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore
            };
    }
}