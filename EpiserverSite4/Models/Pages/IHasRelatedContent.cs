using EPiServer.Core;

namespace EpiserverSite4.Models.Pages
{
    public interface IHasRelatedContent
    {
        ContentArea RelatedContentArea { get; }
    }
}
