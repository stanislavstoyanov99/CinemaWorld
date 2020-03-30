namespace CinemaWorld.Common
{
    using System;
    using System.Text.Json.Serialization;

    public class ReCaptchaSiteVerifyResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("score")]
        public float Score { get; set; }

        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("challenge_ts")]
        public DateTime ChallengeTime { get; set; }

        [JsonPropertyName("hostname")]
        public string Hostname { get; set; }

        [JsonPropertyName("error-codes")]
        public string[] ErrorCodes { get; set; }
    }
}
