public interface ITextAnalyticsRepository
{
    string GetLanguage(string text);
    double? GetSentiment(string text);
}