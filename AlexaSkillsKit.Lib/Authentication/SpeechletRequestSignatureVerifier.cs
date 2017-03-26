// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="SpeechletRequestSignatureVerifier.cs">
//
// </copyright>
// <summary>
//   The speechlet request signature verifier.
// </summary>
//
// --------------------------------------------------------------------------------------------------------------------
namespace AlexaSkillsKit.Authentication
{
    /// <summary>
    ///     The speechlet request signature verifier.
    /// </summary>
    public class SpeechletRequestSignatureVerifier
    {
        /// <summary>
        ///     The _get cert cache key.
        /// </summary>
        private static System.Func<string, string> _getCertCacheKey =
            (string url) => string.Format("{0}_{1}", Sdk.SIGNATURE_CERT_URL_REQUEST_HEADER, url);

        /// <summary>
        ///     The _policy.
        /// </summary>
        private static System.Runtime.Caching.CacheItemPolicy _policy =
            new System.Runtime.Caching.CacheItemPolicy
            {
                Priority = System.Runtime.Caching.CacheItemPriority.Default,
                AbsoluteExpiration = System.DateTimeOffset.UtcNow.AddHours(24)
            };

        /// <summary>
        /// </summary>
        /// <param name="serializedSpeechletRequest">
        /// The serialized Speechlet Request.
        /// </param>
        /// <param name="expectedSignature">
        /// The expected Signature.
        /// </param>
        /// <param name="cert">
        /// The cert.
        /// </param>
        /// <returns>
        /// The <see cref="System.Boolean"/> .
        /// </returns>
        public static bool CheckRequestSignature(
            byte[] serializedSpeechletRequest,
            string expectedSignature,
            Org.BouncyCastle.X509.X509Certificate cert)
        {
            byte[] expectedSig = null;
            try
            {
                expectedSig = System.Convert.FromBase64String(expectedSignature);
            }
            catch (System.FormatException)
            {
                return false;
            }

            var publicKey =
                (Org.BouncyCastle.Crypto.Parameters.RsaKeyParameters)cert.GetPublicKey();
            var signer =
                Org.BouncyCastle.Security.SignerUtilities.GetSigner(Sdk.SIGNATURE_ALGORITHM);
            signer.Init(false, publicKey);
            signer.BlockUpdate(serializedSpeechletRequest, 0, serializedSpeechletRequest.Length);

            return signer.VerifySignature(expectedSig);
        }

        /// <summary>
        /// </summary>
        /// <param name="certChainUrl">
        /// The cert Chain Url.
        /// </param>
        /// <returns>
        /// The <see cref="X509Certificate"/> .
        /// </returns>
        public static Org.BouncyCastle.X509.X509Certificate RetrieveAndVerifyCertificate(
            string certChainUrl)
        {
            // making requests to externally-supplied URLs is an open invitation to DoS
            // so restrict host to an Alexa controlled subdomain/path
            if (!VerifyCertificateUrl(certChainUrl)) return null;

            var webClient = new System.Net.WebClient();
            var content = webClient.DownloadString(certChainUrl);

            var pemReader =
                new Org.BouncyCastle.OpenSsl.PemReader(new System.IO.StringReader(content));
            var cert = (Org.BouncyCastle.X509.X509Certificate)pemReader.ReadObject();
            try
            {
                cert.CheckValidity();
                if (!CheckCertSubjectNames(cert)) return null;
            }
            catch (Org.BouncyCastle.Security.Certificates.CertificateExpiredException)
            {
                return null;
            }
            catch (Org.BouncyCastle.Security.Certificates.CertificateNotYetValidException)
            {
                return null;
            }

            return cert;
        }

        /// <summary>
        /// </summary>
        /// <param name="certChainUrl">
        /// The cert Chain Url.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> .
        /// </returns>
        public static async System.Threading.Tasks.Task<Org.BouncyCastle.X509.X509Certificate> RetrieveAndVerifyCertificateAsync(string certChainUrl)
        {
            // making requests to externally-supplied URLs is an open invitation to DoS
            // so restrict host to an Alexa controlled subdomain/path
            if (!VerifyCertificateUrl(certChainUrl)) return null;

            var httpClient = new System.Net.Http.HttpClient();
            var httpResponse = await httpClient.GetAsync(certChainUrl);
            var content = await httpResponse.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content)) return null;

            var pemReader =
                new Org.BouncyCastle.OpenSsl.PemReader(new System.IO.StringReader(content));
            var cert = (Org.BouncyCastle.X509.X509Certificate)pemReader.ReadObject();
            try
            {
                cert.CheckValidity();
                if (!CheckCertSubjectNames(cert)) return null;
            }
            catch (Org.BouncyCastle.Security.Certificates.CertificateExpiredException)
            {
                return null;
            }
            catch (Org.BouncyCastle.Security.Certificates.CertificateNotYetValidException)
            {
                return null;
            }

            return cert;
        }

        /// <summary>
        /// Verifying the Signature Certificate URL per requirements documented
        ///     at
        ///     https://developer.amazon.com/public/solutions/alexa/alexa-skills-kit/docs/developing-an-alexa-skill-as-a-web-service
        /// </summary>
        /// <param name="certChainUrl">
        /// The cert Chain Url.
        /// </param>
        /// <returns>
        /// The <see cref="System.Boolean"/> .
        /// </returns>
        public static bool VerifyCertificateUrl(string certChainUrl)
        {
            if (string.IsNullOrEmpty(certChainUrl))
            {
                return false;
            }

            System.Uri certChainUri;
            if (!System.Uri.TryCreate(certChainUrl, System.UriKind.Absolute, out certChainUri))
            {
                return false;
            }

            return certChainUri.Host.Equals(
                       Sdk.SIGNATURE_CERT_URL_HOST,
                       System.StringComparison.OrdinalIgnoreCase)
                   && certChainUri.PathAndQuery.StartsWith(Sdk.SIGNATURE_CERT_URL_PATH)
                   && certChainUri.Scheme == System.Uri.UriSchemeHttps && certChainUri.Port == 443;
        }

        /// <summary>
        /// Verifies request signature and manages the caching of the signature
        ///     certificate
        /// </summary>
        /// <param name="serializedSpeechletRequest">
        /// The serialized Speechlet Request.
        /// </param>
        /// <param name="expectedSignature">
        /// The expected Signature.
        /// </param>
        /// <param name="certChainUrl">
        /// The cert Chain Url.
        /// </param>
        /// <returns>
        /// The <see cref="System.Boolean"/> .
        /// </returns>
        public static bool VerifyRequestSignature(
            byte[] serializedSpeechletRequest,
            string expectedSignature,
            string certChainUrl)
        {
            string certCacheKey = _getCertCacheKey(certChainUrl);
            Org.BouncyCastle.X509.X509Certificate cert =
                System.Runtime.Caching.MemoryCache.Default.Get(certCacheKey) as
                    Org.BouncyCastle.X509.X509Certificate;
            if (cert == null || !CheckRequestSignature(serializedSpeechletRequest, expectedSignature, cert))
            {
                // download the cert
                // if we don't have it in cache or
                // if we have it but it's stale because the current request was signed with a newer cert
                // (signaled by signature check fail with cached cert)
                cert = RetrieveAndVerifyCertificate(certChainUrl);
                if (cert == null) return false;

                System.Runtime.Caching.MemoryCache.Default.Set(certCacheKey, cert, _policy);
            }

            return CheckRequestSignature(serializedSpeechletRequest, expectedSignature, cert);
        }

        /// <summary>
        /// Verifies request signature and manages the caching of the signature
        ///     certificate
        /// </summary>
        /// <param name="serializedSpeechletRequest">
        /// The serialized Speechlet Request.
        /// </param>
        /// <param name="expectedSignature">
        /// The expected Signature.
        /// </param>
        /// <param name="certChainUrl">
        /// The cert Chain Url.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> .
        /// </returns>
        public static async System.Threading.Tasks.Task<bool> VerifyRequestSignatureAsync(
            byte[] serializedSpeechletRequest,
            string expectedSignature,
            string certChainUrl)
        {
            string certCacheKey = _getCertCacheKey(certChainUrl);
            Org.BouncyCastle.X509.X509Certificate cert =
                System.Runtime.Caching.MemoryCache.Default.Get(certCacheKey) as
                    Org.BouncyCastle.X509.X509Certificate;
            if (cert == null || !CheckRequestSignature(serializedSpeechletRequest, expectedSignature, cert))
            {
                // download the cert
                // if we don't have it in cache or
                // if we have it but it's stale because the current request was signed with a newer cert
                // (signaled by signature check fail with cached cert)
                cert = await RetrieveAndVerifyCertificateAsync(certChainUrl);
                if (cert == null) return false;

                System.Runtime.Caching.MemoryCache.Default.Set(certCacheKey, cert, _policy);
            }

            return CheckRequestSignature(serializedSpeechletRequest, expectedSignature, cert);
        }

        /// <summary>
        /// </summary>
        /// <param name="cert">
        /// The cert.
        /// </param>
        /// <returns>
        /// The <see cref="System.Boolean"/> .
        /// </returns>
        private static bool CheckCertSubjectNames(Org.BouncyCastle.X509.X509Certificate cert)
        {
            bool found = false;
            System.Collections.ArrayList subjectNamesList =
                (System.Collections.ArrayList)cert.GetSubjectAlternativeNames();
            for (int i = 0; i < subjectNamesList.Count; i++)
            {
                System.Collections.ArrayList subjectNames =
                    (System.Collections.ArrayList)subjectNamesList[i];
                for (int j = 0; j < subjectNames.Count; j++)
                {
                    if (subjectNames[j] is string && subjectNames[j].Equals(Sdk.ECHO_API_DOMAIN_NAME))
                    {
                        found = true;
                        break;
                    }
                }
            }

            return found;
        }
    }
}