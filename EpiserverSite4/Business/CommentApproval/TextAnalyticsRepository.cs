using System;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using System.Collections.Generic;
using Microsoft.Rest;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

public class TextAnalyticsRepository : ITextAnalyticsRepository
{
    private ITextAnalyticsAPI GetClient()
    {
        ITextAnalyticsAPI client = new TextAnalyticsAPI();
        client.AzureRegion = AzureRegions.Westus;
        return client;

    }
    public string GetLanguage(string text)
    {
        using (var client = GetClient())
        {
            var result = client.DetectLanguageAsync(new BatchInput(
                       new List<Input>()
                           {
                          new Input("1", text)
                       })).Result;
            return result.Documents.First().DetectedLanguages.First().Iso6391Name;
        }
           
       
    }
    public double? GetSentiment(string text)
    {
        using (var client = GetClient())
        {
            SentimentBatchResult result = client.SentimentAsync(
                    new MultiLanguageBatchInput(
                        new List<MultiLanguageInput>()
                        {
                          new MultiLanguageInput(GetLanguage(text), "0", text)

                        })).Result;
            return result.Documents.First().Score;
        }
    }
}