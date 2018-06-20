using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpiserverSite4.Business.CommentApproval
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
            // Create a client.
            string languagePrefix = null;
            using (var client = GetClient())
            {
                var result = client.DetectLanguage(new BatchInput(
                   new List<Input>()
                       {
                          new Input("1", text)

                   }));
                languagePrefix =  result.Documents.First().DetectedLanguages.First().Iso6391Name;
            }
            return languagePrefix;
        }
        public double? GetSentiment(string text)
        {
            var language = GetLanguage(text);
            double? score = 0;
            using (var client = GetClient())
            {
                var result = client.Sentiment(
                        new MultiLanguageBatchInput(
                            new List<MultiLanguageInput>()
                            {
                          new MultiLanguageInput(language, "0", text)

                            }));
                score = result.Documents.First().Score;
            }
            return score;
                
        }
              
         
       
    }
}