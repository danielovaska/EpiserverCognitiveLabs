using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EpiserverSite4.Business.Rendering;

namespace EpiserverSite4.Models.Pages
{
    /// <summary>
    /// Used to logically group pages in the page tree
    /// </summary>
    [SiteContentType(
        GUID = "D178950C-D20E-4A46-90BD-5338C2424128",
        GroupName = Global.GroupNames.Specialized)]
    [SiteImageUrl]
    [AvailableContentTypes(
        Availability = Availability.Specific,
        IncludeOn = new[] { typeof(ProductPage) }, Include = new[] { typeof(CommentPage) })]
    public class CommentsContainerPage : SitePageData, IContainerPage
    {

    }
}
