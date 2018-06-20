using System;
using EpiserverSite4.Business.CommentApproval;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alloy.Tests
{
    [TestClass]
    public class TextAnalyticsRepositoryTests
    {
        [TestMethod]
        public void CanDetectLanguage()
        {
            var textAnalyticsRepository = new TextAnalyticsRepository();
            var language = textAnalyticsRepository.GetLanguage("What language is this?");
            Assert.IsTrue(language == "en");
        }
        [TestMethod]
        public void CanClassifyTextBad()
        {
            var textAnalyticsRepository = new TextAnalyticsRepository();
            var sentiment = textAnalyticsRepository.GetSentiment("You bastard. I hate you!!!");
            Assert.IsTrue(sentiment <0.3);
        }
        [TestMethod]
        public void CanClassifyTextGood()
        {
            var textAnalyticsRepository = new TextAnalyticsRepository();
            var sentiment = textAnalyticsRepository.GetSentiment("This is such a wonderful comment!");
            Assert.IsTrue(sentiment > 0.7);
        }
    }
}
