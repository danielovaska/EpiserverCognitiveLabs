using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Approvals.ContentApprovals;
using EPiServer.Core;
using EPiServer.Approvals;
using EPiServer.Logging;
using EpiserverSite4.Controllers;
using EPiServer;
using EpiserverSite4.Models.Pages;
using EPiServer.Core.Html;
using System.Globalization;
using System.Collections.Generic;

namespace EpiserverSite4.Business.Initialization
{
    [InitializableModule]
    //Let Episerver do its initialization first to avoid problems...
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class AIApprovalModule : IInitializableModule
    {
        private IApprovalEngineEvents _approvalEngineEvents;
        private bool _eventsAreInitialized = false;
        private ILogger _log;
        public void Initialize(InitializationEngine context)
        {
            _log = LogManager.GetLogger(typeof(AIApprovalModule));
            if (!_eventsAreInitialized)
            {
                _approvalEngineEvents = ServiceLocator.Current.GetInstance<IApprovalEngineEvents>();
                _approvalEngineEvents.StepStarted += OnStepStarted;
            }
            ConfigureApprovalWorkflow();
        }
        /// <summary>
        /// Configure approval workflow for everything below the startpage
        /// </summary>
        public async void ConfigureApprovalWorkflow()
        {
            var approvalDefinitionRepository = ServiceLocator.Current.GetInstance<IApprovalDefinitionRepository>();

            var langEN = new CultureInfo[] { CultureInfo.GetCultureInfo("en") };
            var langSV = new CultureInfo[] { CultureInfo.GetCultureInfo("sv") };

            // Creates a content approval definition
            var definition = new ContentApprovalDefinition
            {
                ContentLink = ContentReference.StartPage,
                Steps = new List<ApprovalDefinitionStep>
                {
                    new ApprovalDefinitionStep("step1", new ApprovalDefinitionReviewer[]
                    {
                        new ApprovalDefinitionReviewer("Admin", langEN),
                        new ApprovalDefinitionReviewer("AI", langEN),
                    })
                },
                IsEnabled = true

            };

            // Saves the definition
            await approvalDefinitionRepository.SaveAsync(definition);
        }
        public void Uninitialize(InitializationEngine context) => _approvalEngineEvents.StepStarted -= OnStepStarted;
        /// <summary>
        /// When approval is triggered, check if the 
        /// </summary>
        /// <param name="e"></param>
        private void OnStepStarted(ApprovalEventArgs e)
        {
            var approvalEngine = ServiceLocator.Current.GetInstance<IApprovalEngine>();
            var textAnalyticsRepository = ServiceLocator.Current.GetInstance<ITextAnalyticsRepository>();
            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            var content = contentRepository.Get<IContent>(e.GetContentLink());
            var commentPage = content as CommentPage;
            if(commentPage!=null)
            {
                var text = commentPage.MainBody;
                var sentiment = textAnalyticsRepository.GetSentiment(TextIndexer.StripHtml(text.ToString(),0));
                if(sentiment!=null && sentiment > 0.3)
                {
                    var comment = $"Automatic approval with sentiment = {sentiment}";
                    approvalEngine.ApproveAsync(e.ApprovalID, "Admin", 0, ApprovalDecisionScope.Force, comment).Wait(); ;
                    var clone = commentPage.CreateWritableClone();
                    contentRepository.Publish(clone, EPiServer.Security.AccessLevel.Read);
                    _log.Information(comment);
                }
                else
                {
                    var comment = $"Failed automatic approval with sentiment = {sentiment}";
                    _log.Information(comment);
                }
            }

            
        }

    }
}
