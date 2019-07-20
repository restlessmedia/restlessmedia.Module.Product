using restlessmedia.Module.Data;

namespace restlessmedia.Module.Product.Data
{
  internal class ProductDataProvider : ProductSqlDataProvider, IProductDataProvider
  {
    public ProductDataProvider(IDataContext context)
      : base(context) { }
  }
}