using BlueTurtleDog.Testing.ReadJson;
using FinerWorks.API.List_Images;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wix.Collections;

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
            var buffer = new List<WixPortfolioImportRowModel>();

            foreach (var item in finerworksInventory)
            {
                var row = new WixPortfolioImportRowModel()
                {
                    Title = item.name,
                    SKU = item.sku,
                    // TODO: Add price to portfolio
                    Image = item.File.file_thumbnail_uri,
                    // TODO: Replace with description from the Products collection in Wix
                    Description = "This will be description from Wix:\nA {paper/canvas} print of my original oil painting on...mounted....",
                    Year = item.File.date_added.Year.ToString(),
                };

                // Gallery
                var galleryImage = new GalleryFieldImage()
                {
                    title = item.File.title,
                    src = $"{item.File.file_preview_uri}#originWidth={item.File.pix_w}&originHeight={item.File.pix_h}"
                };
                row.Gallery.Add(galleryImage);

                buffer.Add(row);
            }

            WriteCsv(buffer);

            System.Diagnostics.Debug.Print(buffer.ToString());
        }

        private static void WriteCsv(List<WixPortfolioImportRowModel> buffer)
        {
            var CsvFactory = new CsvHelper.Factory();
            var configuration = new CsvHelper.Configuration.CsvConfiguration(System.Globalization.CultureInfo.CurrentCulture);

            // Configure CSV reader
            configuration.DetectColumnCountChanges = true;
            configuration.HasHeaderRecord = true;
            configuration.IgnoreBlankLines = true;
            configuration.TrimOptions = CsvHelper.Configuration.TrimOptions.Trim;
            configuration.Delimiter = ",";
            configuration.IgnoreReferences = false;

            // Map properties of row model to field headers in CSV file
            configuration.RegisterClassMap<WixPortfolioImportCsvRowMap>();

            var dateStamp = System.DateTime.Now.ToString("yyyy-MM-dd HHmmss");
            var filename = $"{dateStamp} - Portfolio products import.csv";
            using (var txtreader = new StreamWriter(filename))
            {
                using (var csv = CsvFactory.CreateWriter(txtreader, configuration))
                {
                    csv.WriteRecords(buffer);
                }

                txtreader.Close();
            }
        }
    }
}
