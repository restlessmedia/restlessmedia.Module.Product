using restlessmedia.Module.Data;
using SqlBuilder.DataServices;

namespace restlessmedia.Module.Product.Data
{
  internal class ProductDataProvider : ProductSqlDataProvider, IProductDataProvider
  {
    public ProductDataProvider(IDataContext context, IModelDataService<DataModel.VProduct> modelDataService)
      : base(context, modelDataService) { }
  }
}