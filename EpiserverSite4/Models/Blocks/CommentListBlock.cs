using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using EPiServer;

namespace EpiserverSite4.Models.Blocks
{
    /// <summary>
    /// Used to present contact information with a call-to-action link
    /// </summary>
    /// <remarks>Actual contact details are retrieved from a contact page specified using the ContactPageLink property</remarks>
    [SiteContentType(GUID = "7E932EAF-6BC2-4753-902A-8670EDC5FA12")]
    [SiteImageUrl]
    public class CommentListBlock : SiteBlockData
    {
        
    }
}
