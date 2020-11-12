using FinerWorks.API.List_Images;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueTurtleDog.Testing.CreateCsvForPortfolio
{
    class Program
    {
        static void Main(string[] args)
        {

            // Parse the JSON response from an offline call to the list_images API endpoint made in PostMan
            var json = File.ReadAllText("finerworks list_images response.json");
            var resp = new Response();
            resp = (Response)JsonConvert.DeserializeObject(json, resp.GetType());

            // Invert the files collection to make it a products collection
            var finerworksInventory = resp.files
                .SelectMany(f => f.products, (f, p) => new { f, p })
                .Select(fileAndProduct => new Product
                {
                    asking_price = fileAndProduct.p.asking_price,
                    description_long = fileAndProduct.p.description_long,
                    description_short = fileAndProduct.p.description_short,
                    File = fileAndProduct.f,
                    image_guid = fileAndProduct.p.image_guid,
                    image_url_1 = fileAndProduct.p.image_url_1,
                    image_url_2 = fileAndProduct.p.image_url_2,
                    image_url_3 = fileAndProduct.p.image_url_3,
                    image_url_4 = fileAndProduct.p.image_url_4,
                    image_url_5 = fileAndProduct.p.image_url_5,
                    monetary_format = fileAndProduct.p.monetary_format,
                    name = fileAndProduct.p.name,
                    per_item_price = fileAndProduct.p.per_item_price,
                    product_code = fileAndProduct.p.product_code,
                    quantity = fileAndProduct.p.quantity,
                    sku = fileAndProduct.p.sku,
                    total_price = fileAndProduct.p.total_price
                })
                .ToList();

            // Init with header row
            var buffer = new StringBuilder("Title,SKU,Image,Description,Year");

            foreach (var item in finerworksInventory)
            {
                // Title
                buffer.Append($"{item.name},");

                // SKU
                buffer.Append($"{item.sku},");

                // Image
                buffer.Append($"{item.image_url_1},");

                // Description
                // TODO: Replace with description from the Products collection in Wix
                buffer.Append($"{item.name},");

                // Year
                buffer.Append($"{item.File.date_added.Year}");

                buffer.Append("\n");
            }

            System.Diagnostics.Debug.Print(buffer.ToString());
        }
    }
}
