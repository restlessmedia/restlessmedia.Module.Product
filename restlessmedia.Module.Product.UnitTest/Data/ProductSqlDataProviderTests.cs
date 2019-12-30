using FakeItEasy;
using restlessmedia.Module.Data;
using restlessmedia.Module.Product.Data;
using SqlBuilder.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace restlessmedia.Module.Product.UnitTest.Data
{
  public class ProductSqlDataProviderTests
  {
    [Fact(Skip = "Beta")]
    public void test()
    {
      ProductSqlDataProvider productSqlDataProvider = CreateInstance();
      productSqlDataProvider.ListProducts(1, ProductFlagTypes.Active, ProductOrder.CostAsc);
    }

    private ProductSqlDataProvider CreateInstance()
    {
      IDataContext dataContext = A.Fake<IDataContext>();
      IModelDataService<Product.Data.DataModel.VProduct> modelDataService = A.Fake<IModelDataService<Product.Data.DataModel.VProduct>>();
      ProductSqlDataProvider productSqlDataProvider = new ProductSqlDataProvider(dataContext, modelDataService);
      return productSqlDataProvider;
    }
  }
}
