using EPiServer.Core;
using System.Collections.Generic;

namespace EpiserverSite4.Models.ViewModels
{
    public class CommentListBlockViewModel
    {
        public IEnumerable<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
        public CommentPostbackData PostbackData { get; set; }
    }
    public class CommentPostbackData
    {
        public string Comment { get; set; }
        public ContentReference CurrentPageLink { get; set; }
        public string CurrentLanguage { get; set; }

    }

}
