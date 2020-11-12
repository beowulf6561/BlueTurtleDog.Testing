using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinerWorks.API.List_Images
{
    public class Status
    {
        public bool success { get; set; }
        public int status_code { get; set; }
        public string message { get; set; }
        public object debug { get; set; }
    }

    public class Product
    {
        public string monetary_format { get; set; }
        public int quantity { get; set; }
        public string sku { get; set; }
        public object product_code { get; set; }
        public double per_item_price { get; set; }
        public double total_price { get; set; }
        public double asking_price { get; set; }
        public string name { get; set; }
        public object description_short { get; set; }
        public object description_long { get; set; }
        public string image_url_1 { get; set; }
        public object image_url_2 { get; set; }
        public object image_url_3 { get; set; }
        public object image_url_4 { get; set; }
        public object image_url_5 { get; set; }
        public string image_guid { get; set; }

        public ImageFile File { get; set; }

        public override string ToString()
        {
            return $"{monetary_format},{quantity},{sku},{product_code},{per_item_price},{total_price},{asking_price},{name},{description_short},{description_long},{image_url_1},{image_url_2},{image_url_3},{image_url_4},{image_url_5},{image_guid}";
        }
    }

    public class ImageFile
    {
        public string guid { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string file_name { get; set; }
        public string file_thumbnail_uri { get; set; }
        public string file_preview_uri { get; set; }
        public string personal_gallery_title { get; set; }
        public string members_gallery_category { get; set; }
        public object file_hires_uri { get; set; }
        public int file_size { get; set; }
        public int pix_w { get; set; }
        public int pix_h { get; set; }
        public DateTime date_added { get; set; }
        public bool active { get; set; }
        public IList<Product> products { get; set; }

        public override string ToString()
        {
            return $"{guid},{title},{description},{file_name},{file_thumbnail_uri},{file_preview_uri},{personal_gallery_title},{members_gallery_category},{file_hires_uri},{file_size},{pix_w},{pix_h},{date_added},{active},{products.Count}";
        }
    }

    public class Response
    {
        public Status status { get; set; }
        public IList<ImageFile> files { get; set; }
    }
}
