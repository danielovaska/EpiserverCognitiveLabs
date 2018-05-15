using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using System.Linq;

namespace EpiserverSite4.Controllers
{
    public class TextAnalyticsRepository : ITextAnalyticsRepository
    {
        private ITextAnalyticsAPI GetClient()
        {
            ITextAnalyticsAPI client = new TextAnalyticsAPI();
            client.AzureRegion = AzureRegions.Westus;
            client.SubscriptionKey = "07dc127ac4d14c9a9a4760cdf0562ebe";
            return client;
        }
        public string GetLanguage(string text)
        {
            string language;
            using (var client = GetClient())
            {
                var languageResult = client.DetectLanguage(
                   new BatchInput(
                       new List<Input>()
                       {
                          new Input("1", text)
                       }));
                language = languageResult
                    .Documents.First()
                    .DetectedLanguages
                    .OrderBy(l => l.Score)
                    .First()
                    .Iso6391Name;
            }
            return language;
        }
        public double? GetSentiment(string text)
        {
            double? sentimentScore = null;
            var language = GetLanguage(text);
            using (var client = GetClient())
            {
                var sentimentResult = client.Sentiment(
               new MultiLanguageBatchInput(
                   new List<MultiLanguageInput>()
                   {
                                new MultiLanguageInput(language, "0", text)
                   }));
                sentimentScore = sentimentResult.Documents.First().Score;
            }
            return sentimentScore;
        }

    }
}

