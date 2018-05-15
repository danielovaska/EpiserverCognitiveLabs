using Episerver.Labs.Cognitive.Attributes;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Framework.Blobs;
using EPiServer.Framework.DataAnnotations;
using EPiServer.SpecializedProperties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EpiserverSite4.Models.Media
{
    [ContentType(GUID = "0A89E464-56D4-449F-AEA8-2BF774AB8730")]
    [MediaDescriptor(ExtensionString = "jpg,jpeg,jpe,ico,gif,bmp,png")]
    public class ImageFile : ImageData 
    {
        /// <summary>
        /// Gets or sets the copyright.
        /// </summary>
        /// <value>
        /// The copyright.
        /// </value>
        public virtual string Copyright { get; set; }
        [Vision(VisionType = VisionTypes.Description)]
        public virtual string Description { get; set; }
        //Alternatively you can of course also use a property list.
        [Vision(VisionType = VisionTypes.Tags)]
        [Display(Order = 305)]
        [UIHint(Global.SiteUIHints.StringsCollection)]
        public virtual IList<string> TagList { get; set; }

        //Assigns image categories to a string array
        //[Vision(VisionType = VisionTypes.Categories)]
        //[BackingType(typeof(PropertyStringList))]
        //[Display(Order = 305)]
        //[UIHint(Global.SiteUIHints.Strings)]
        //public virtual string[] ImageCategories { get; set; }

        //True if the image contains adult content. Useful for moderation
        //[Vision(VisionType = VisionTypes.Adult)]
        public virtual bool IsAdultContent { get; set; }

        //True if the image contains racy content. USeful for moderation
        //[Vision(VisionType = VisionTypes.Racy)]
        public virtual bool IsRacyContent { get; set; }

        //True if the image is clipart
        //[Vision(VisionType = VisionTypes.ClipArt)]
        public virtual bool IsClipArt { get; set; }

        //True if the image is a line drawing
        //[Vision(VisionType = VisionTypes.LineDrawing)]
        public virtual bool IsLineDrawing { get; set; }

        //True if the image is black and white
        //[Vision(VisionType = VisionTypes.BlackAndWhite)]
        public virtual bool IsBlackAndWhite { get; set; }
        //Text recognized in the image
        //[Vision(VisionType = VisionTypes.Text)]
        public virtual string TextRecognized { get; set; }

        [Vision(VisionType = VisionTypes.Text)]
        public virtual XhtmlString TextInPicture { get; set; }

        
    }
}
