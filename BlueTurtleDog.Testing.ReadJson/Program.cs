using FinerWorks.API.List_Images;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueTurtleDog.Testing.ReadJson
{
    class Program
    {
        static void Main(string[] args)
        {

            // Read Alice's inventory from the CSV import file she tried creating from the template
            var aliceInventory = ReadCsv();

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
            // handleId,fieldType,name,description,productImageUrl,collection,sku,ribbon,price,surcharge,visible,discountMode,discountValue,inventory,weight,productOptionName1,productOptionType1,productOptionDescription1,productOptionName2,productOptionType2,productOptionDescription2,productOptionName3,productOptionType3,productOptionDescription3,productOptionName4,productOptionType4,productOptionDescription4,productOptionName5,productOptionType5,productOptionDescription5,productOptionName6,productOptionType6,productOptionDescription6,additionalInfoTitle1,additionalInfoDescription1,additionalInfoTitle2,additionalInfoDescription2,additionalInfoTitle3,additionalInfoDescription3,additionalInfoTitle4,additionalInfoDescription4,additionalInfoTitle5,additionalInfoDescription5,additionalInfoTitle6,additionalInfoDescription6,customTextField1,customTextCharLimit1,customTextMandatory1
            var buffer = new StringBuilder("handleId,fieldType,name,description,productImageUrl,collection,sku,ribbon,price,surcharge,visible,discountMode,discountValue,inventory,weight,productOptionName1,productOptionType1,productOptionDescription1,productOptionName2,productOptionType2,productOptionDescription2,productOptionName3,productOptionType3,productOptionDescription3,productOptionName4,productOptionType4,productOptionDescription4,productOptionName5,productOptionType5,productOptionDescription5,productOptionName6,productOptionType6,productOptionDescription6,additionalInfoTitle1,additionalInfoDescription1,additionalInfoTitle2,additionalInfoDescription2,additionalInfoTitle3,additionalInfoDescription3,additionalInfoTitle4,additionalInfoDescription4,additionalInfoTitle5,additionalInfoDescription5,additionalInfoTitle6,additionalInfoDescription6,customTextField1,customTextCharLimit1,customTextMandatory1\n");

            foreach (var item in aliceInventory)
            {
                var product = finerworksInventory
                    .Where(i => i.sku == item.SKU)
                    .FirstOrDefault();

                if (product == null)
                {
                    System.Diagnostics.Debug.Print($"oshit, didn't find product for item, {item.HandleId} - {item.SKU}");
                }
                else
                {
                    //handleId
                    buffer.Append($"{item.HandleId},");

                    //fieldType
                    buffer.Append($"{item.FieldType},");

                    //name
                    buffer.Append($"\"{item.Title}\",");

                    //description
                    buffer.Append($"\"{item.Description}\",");

                    //productImageUrl
                    buffer.Append($"{product.File.file_preview_uri},");

                    //collection
                    buffer.Append(",");

                    //sku
                    buffer.Append($"{product.sku},");

                    //ribbon
                    buffer.Append(",");

                    //price
                    buffer.Append($"{item.Price},");

                    //surcharge
                    buffer.Append(",");

                    //visible
                    buffer.Append($"{product.File.active.ToString().ToUpper()},");

                    //discountMode
                    buffer.Append(",");

                    //discountValue
                    buffer.Append(",");

                    //inventory
                    buffer.Append("InStock,");

                    //weight
                    buffer.Append(",");

                    //productOptionName1,productOptionType1,productOptionDescription1,
                    buffer.Append(",,,");

                    //productOptionName2,productOptionType2,productOptionDescription2,
                    buffer.Append(",,,");

                    //productOptionName3,productOptionType3,productOptionDescription3,
                    buffer.Append(",,,");

                    //productOptionName4,productOptionType4,productOptionDescription4,
                    buffer.Append(",,,");

                    //productOptionName5,productOptionType5,productOptionDescription5,
                    buffer.Append(",,,");

                    //productOptionName6,productOptionType6,productOptionDescription6,
                    buffer.Append(",,,");

                    //additionalInfoTitle1,additionalInfoDescription1,
                    buffer.Append($"Material,{item.Material},");

                    //additionalInfoTitle2,additionalInfoDescription2,
                    buffer.Append($"Size,{item.Size},");

                    //additionalInfoTitle3,additionalInfoDescription3,
                    buffer.Append($"Measurement,{item.Measurement},");

                    //additionalInfoTitle4,additionalInfoDescription4,
                    buffer.Append($"Product Code,{item.ProductCode},");

                    //additionalInfoTitle5,additionalInfoDescription5,
                    buffer.Append($"Other Product Name,{product.name},");

                    //additionalInfoTitle6,additionalInfoDescription6,
                    buffer.Append(",,");

                    //customTextField1,customTextCharLimit1,customTextMandatory1
                    buffer.Append(",,\n");
                }
            }

            System.Diagnostics.Debug.Print(buffer.ToString());
        }

        /// <summary>
        /// Read Alice's Inventory CSV to pull things not available from Finerworks API, like handleId, description, material, and size
        /// </summary>
        /// <returns></returns>
        public static List<AliceInventoryRowModel> ReadCsv()
        {
            var results = new List<AliceInventoryRowModel>();
            var CsvFactory = new CsvHelper.Factory();
            var configuration = new CsvHelper.Configuration.CsvConfiguration(System.Globalization.CultureInfo.CurrentCulture);

            // Configure CSV reader
            configuration.DetectColumnCountChanges = true;
            configuration.HasHeaderRecord = true;
            configuration.IgnoreBlankLines = true;
            configuration.TrimOptions = CsvHelper.Configuration.TrimOptions.Trim;
            configuration.Delimiter = ",";

            // Map properties of RegisterBulkCsvRowModel to field headers in CSV file
            configuration.RegisterClassMap<AliceInventoryCsvRowMap>();

            using (var txtreader = new StreamReader("Inventory3.csv"))
            {
                using (var csv = CsvFactory.CreateReader(txtreader, configuration))
                {
                    // For each row in CSV file
                    results.AddRange(csv.GetRecords<AliceInventoryRowModel>());
                }

                txtreader.Close();
            }

            return results;
        }
    }
}
