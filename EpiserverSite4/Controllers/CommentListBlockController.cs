using System.Web;
using System.Web.Mvc;
using EPiServer.Core;
using EpiserverSite4.Helpers;
using EpiserverSite4.Models.Blocks;
using EpiserverSite4.Models.Pages;
using EpiserverSite4.Models.ViewModels;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EPiServer;
using EPiServer.Web.Routing;
using System.Linq;
using EPiServer.Filters;
using EpiserverSite4.Business;

namespace EpiserverSite4.Controllers
{
    public class CommentListBlockController : BlockController<CommentListBlock>
    {
        private readonly IContentLoader _contentLoader;
        private readonly IPageRouteHelper _pageRouteHelper;

        public CommentListBlockController(IContentLoader contentLoader, IPageRouteHelper pageRouteHelper)
        {
            _contentLoader = contentLoader;
            _pageRouteHelper = pageRouteHelper;
        }

        public override ActionResult Index(CommentListBlock currentBlock)
        {
            var viewModel = new CommentListBlockViewModel();
            var currentPage = _pageRouteHelper.Page;
            
            var commentsFolder = _contentLoader.GetChildren<CommentsContainerPage>(currentPage.ContentLink).FilterForDisplay();
            if(commentsFolder!=null && commentsFolder.Any())
            {
                var comments = _contentLoader.GetChildren<CommentPage>(commentsFolder.First().ContentLink).FilterForDisplay();
                if(comments!=null && comments.Any())
                {
                    viewModel.Comments = from comment in comments
                                         select new CommentViewModel { Text = comment.MainBody.ToString() };
                }

            }

            return PartialView(viewModel);
        }
        

    }
}
