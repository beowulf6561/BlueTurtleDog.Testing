namespace BlueTurtleDog.Testing.ReadJson
{
    public sealed class WixPortfolioImportCsvRowMap : CsvHelper.Configuration.ClassMap<WixPortfolioImportRowModel>
    {
        public WixPortfolioImportCsvRowMap()
        {
            Map(r => r.Title).Name("Title");
            Map(r => r.SKU).Name("SKU");
            Map(r => r.PriceOutput).Name("Price");
            Map(r => r.Image).Name("Image");
            Map(r => r.Description).Name("Description");
            Map(r => r.Year).Name("Year");
            Map(r => r.GalleryOutput).Name("Gallery");
        }
    }
}
