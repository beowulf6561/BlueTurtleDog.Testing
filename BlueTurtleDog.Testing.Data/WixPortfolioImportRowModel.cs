using Newtonsoft.Json;
using System.Collections.Generic;
using Wix.Collections;

namespace BlueTurtleDog.Testing.ReadJson
{
    public class WixPortfolioImportRowModel
    {
        public string Title { get; set; }
        public string SKU { get; set; }
        public double Price { get; set; }
        public string PriceOutput 
        { 
            get { return this.Price.ToString("C2"); }
        }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Year { get; set; }
        public List<GalleryFieldImage> Gallery { get; set; } = new List<GalleryFieldImage>();
        public string GalleryOutput
        {
            get { return JsonConvert.SerializeObject(this.Gallery); }
        }
    }
}
