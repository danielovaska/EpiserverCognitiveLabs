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
       // [Vision(VisionType = VisionTypes.Description)]
        public virtual string Description { get; set; }
        //Alternatively you can of course also use a property list.
        //[Vision(VisionType = VisionTypes.Tags)]
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

        ////Hex code of the main accent color in the image. Useful for adopting the design to match the image
        //[Vision(VisionType = VisionTypes.AccentColor)]
        //public virtual string AccentColor { get; set; }

        ////Hex code of the dominant background color
        //[Vision(VisionType = VisionTypes.DominantBackgroundColor)]
        //public virtual string DominantBackgroundColor { get; set; }

        ////Hex code of the foreground color
        //[Vision(VisionType = VisionTypes.DominantForegroundColor)]
        //public virtual string DominantForegroundColor { get; set; }

        //A list of faces identified in the image with their age and gender. It's also possible to just extract ages or gender.
        //[Vision(VisionType = VisionTypes.Faces)]
        //[BackingType(typeof(PropertyStringList))]
        //[Display(Order = 305)]
        //[UIHint(Global.SiteUIHints.Strings)]
        //public virtual string[] Faces { get; set; }

        //Text recognized in the image
        //[Vision(VisionType = VisionTypes.Text)]
        public virtual string TextRecognized { get; set; }

        //[Vision(VisionType = VisionTypes.Text)]
        public virtual XhtmlString TextInPicture { get; set; }

        //A smart thumbnail at a given size, focussing on the subject matter in the image. Most be a Blob. 
        //Just like with other Blobs stored on an image media, you can access their image by getting the url to the image and appending /[blobname] in the url
        //[ScaffoldColumn(false)]
        //[SmartThumbnail(100, 100)]
        //public virtual Blob SmartThumbnail { get; set; }
    }
}
