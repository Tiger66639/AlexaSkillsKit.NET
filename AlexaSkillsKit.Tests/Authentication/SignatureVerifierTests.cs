﻿using AlexaSkillsKit.Authentication;
using Xunit;

namespace AlexaSkillsKit.Tests
{
    public class SignatureVerifierTests
    {
        [Fact]
        public void VerifyCertificateUrlTest()
        {
            // samples from
            // https://developer.amazon.com/public/solutions/alexa/alexa-skills-kit/docs/developing-an-alexa-skill-as-a-web-service

            var validUrls = new string[] {
                "https://s3.amazonaws.com/echo.api/echo-api-cert.pem",
                "https://s3.amazonaws.com:443/echo.api/echo-api-cert.pem",
                "https://s3.amazonaws.com/echo.api/../echo.api/echo-api-cert.pem"
            };

            var invalidUrls = new string[] {
                "http://s3.amazonaws.com/echo.api/echo-api-cert.pem",
                "https://notamazon.com/echo.api/echo-api-cert.pem",
                "https://s3.amazonaws.com/EcHo.aPi/echo-api-cert.pem",
                "https://s3.amazonaws.com/invalid.path/echo-api-cert.pem",
                "https://s3.amazonaws.com:563/echo.api/echo-api-cert.pem"
             };

            foreach (var validUrl in validUrls)
            {
                Assert.True(SpeechletRequestSignatureVerifier.VerifyCertificateUrl(validUrl));
            }

            foreach (var invalidUrl in invalidUrls)
            {
                Assert.False(SpeechletRequestSignatureVerifier.VerifyCertificateUrl(invalidUrl));
            }
        }
    }
}