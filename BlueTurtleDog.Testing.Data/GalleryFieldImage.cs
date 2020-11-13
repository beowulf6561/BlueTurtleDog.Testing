using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wix.Collections
{
    public class GalleryField
    {
        public List<GalleryFieldImage> Images { get; set; } = new List<GalleryFieldImage>();
    }

    public class GalleryFieldImage
    {
        public string type { get; set; } = "image";
        //public string slug { get; set; }
        public string title { get; set; }
        public string src { get; set; }
    }
}
