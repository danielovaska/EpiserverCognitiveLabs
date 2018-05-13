using System.Collections.Generic;

namespace EpiserverSite4.Models.ViewModels
{
    public class CommentListBlockViewModel
    {
        public IEnumerable<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
    }
}
