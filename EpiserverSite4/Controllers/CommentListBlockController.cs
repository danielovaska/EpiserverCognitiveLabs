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
        private readonly IContentRepository _contentRepository;
        private readonly IPageRouteHelper _pageRouteHelper;

        public CommentListBlockController(IContentRepository contentRepository, IPageRouteHelper pageRouteHelper)
        {
            _contentRepository = contentRepository;
            _pageRouteHelper = pageRouteHelper;
        }

        public override ActionResult Index(CommentListBlock currentBlock)
        {
            var viewModel = new CommentListBlockViewModel();
            var currentPage = _pageRouteHelper.Page;
            var commentsFolder = GetCommentsFolder(currentPage.ContentLink);
            var comments = _contentRepository.GetChildren<CommentPage>(commentsFolder.ContentLink).FilterForDisplay();
            if (comments != null && comments.Any())
            {
                viewModel.Comments = from comment in comments
                                     select new CommentViewModel { Text = comment.MainBody.ToString() };
            }
            viewModel.PostbackData = new CommentPostbackData { CurrentPageLink = currentPage.ContentLink, CurrentLanguage = currentPage.Language.ToString() };

            return PartialView(viewModel);
        }
        [HttpPost]
        public ActionResult Save(CommentPostbackData PostbackData)
        {
            //Never trust user input!
            var commentsFolder = GetCommentsFolder(PostbackData.CurrentPageLink);
            var newComment = _contentRepository.GetDefault<CommentPage>(commentsFolder.ContentLink);
            newComment.Name = "Comment";
            newComment.MainBody = new XhtmlString($"<p>{PostbackData.Comment}</p>");
            _contentRepository.Save(newComment,EPiServer.DataAccess.SaveAction.RequestApproval,EPiServer.Security.AccessLevel.Read);
            var url = UrlResolver.Current.GetUrl(PostbackData.CurrentPageLink);
            return Redirect(url);
        }
        private CommentsContainerPage GetCommentsFolder(ContentReference currentPageLink)
        {
            var commentsFolders = _contentRepository.GetChildren<CommentsContainerPage>(currentPageLink).FilterForDisplay();
            if (commentsFolders != null && commentsFolders.Any())
            {
                var commentsFolder = commentsFolders.First();
                return commentsFolder;
            }
            return null;
        }
    }
}
