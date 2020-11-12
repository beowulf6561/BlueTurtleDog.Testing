using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueTurtleDog.Testing.ReadJson
{
    public sealed class AliceInventoryCsvRowMap : CsvHelper.Configuration.ClassMap<AliceInventoryRowModel>
    {

        public AliceInventoryCsvRowMap()
        {
            Map(r => r.HandleId).Name("handleid");
            Map(r => r.FieldType).Name("fieldType");
            Map(r => r.Title).Name("Title");
            Map(r => r.Material).Name("Material");
            Map(r => r.Size).Name("Size");
            Map(r => r.Measurement).Name("Measurement");
            Map(r => r.Description).Name("Description");
            Map(r => r.ProductCode).Name("Product Code");
            Map(r => r.SKU).Name("SKU");
            Map(r => r.Cost).Name("Cost");
            Map(r => r.DoubleCoust).Name("double");
            Map(r => r.Price).Name("price");
        }
    }
}
