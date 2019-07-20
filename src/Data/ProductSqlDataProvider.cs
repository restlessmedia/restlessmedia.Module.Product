using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using SqlBuilder.DataServices;
using restlessmedia.Module.Data;
using restlessmedia.Module.Data.Sql;
using restlessmedia.Module.File;
using restlessmedia.Module.Address;
using restlessmedia.Module.Meta;
using restlessmedia.Module.Category;

namespace restlessmedia.Module.Product.Data
{
  internal class ProductSqlDataProvider : SqlDataProviderBase
  {
    internal ProductSqlDataProvider(IDataContext context)
      : base(context) { }

    public void UpdateSaleOrderNumber(int saleId, string orderNumber)
    {
      Execute("dbo.SPUpdateSaleOrderNumber", new { saleId = saleId, orderNumber = orderNumber });
    }

    public decimal ChargeAmount(SaleEntity sale, decimal amount)
    {
      return QueryWithTransaction<decimal>("dbo.SPChargeAmount", new { saleId = sale.SaleId, amount = amount }).First();
    }

    public SaleFlags GetSaleFlags(int saleId)
    {
      IEnumerable<SaleFlags> result = Query<SaleFlags>("dbo.SPGetSaleFlags", new { saleId = saleId });
      return result.Any() ? result.First() : SaleFlags.None;
    }

    public SaleFlags SetSaleFlags(int saleId, SaleFlags flags)
    {
      IEnumerable<SaleFlags> result = QueryWithTransaction<SaleFlags>("dbo.SPSetSaleFlags", new { saleId = saleId, flags = (int)flags });
      return result.Any() ? result.First() : SaleFlags.None;
    }

    public void UpdateSaleFlags(int saleId, SaleFlags flags)
    {
      Execute("dbo.SPUpdateSaleFlags", new { saleId = saleId, flags = (int)flags });
    }

    public void Update(ProductOptionEntity option)
    {
      Execute("dbo.SPUpdateProductOption", new { productOptionId = option.ProductOptionId, description = option.Description, qty = option.Qty, net = option.Detail.Net, productDetailFlags = (int?)option.Detail.Flags });
    }

    public void CreateOptions(IDbTransaction transaction, ProductEntity product, ModelCollection<ProductOptionEntity> options)
    {
      foreach (ProductOptionEntity productOption in options)
      {
        // create it then set back the id
        productOption.ProductOptionId = transaction.Connection.Execute("dbo.SPCreateProductOption", new { productId = product.ProductId, description = productOption.Description, net = productOption.Detail.Net, tax = productOption.Detail.Tax, productDetailFlags = (int?)productOption.Detail.Flags, validFrom = productOption.Detail.ValidFrom }, transaction: transaction, commandType: CommandType.StoredProcedure);
      }
    }

    public void CreateOptions(ProductEntity product, ModelCollection<ProductOptionEntity> options)
    {
      ExecuteWithTransaction((transaction) =>
      {
        CreateOptions(transaction, product, options);
      });
    }

    public void Delete(int productId)
    {
      Execute("dbo.SPDeleteProduct", new { productId = productId });
    }

    public ProductEntity Read(int productId)
    {
      using (IGridReader reader = QueryMultiple("dbo.SPReadProduct", new { productId = productId }))
      {
        ProductEntity product = reader.Read<ProductEntity, ProductOptionEntity, ProductDetailEntity, CategoryEntity, FileEntity, ProductEntity>((p, o, d, c, f) => { p.MinOption = o; p.MinOption.Detail = d; p.Category = c; p.Thumb = f; return p; }, splitOn: "ProductOptionId,ProductDetailId,CategoryId,FileId").SingleOrDefault();
        product.Options = new ModelCollection<ProductOptionEntity>(reader.Read<ProductOptionEntity, ProductDetailEntity, ProductOptionEntity>((o, d) => { o.Detail = d; return o; }, splitOn: "ProductDetailId"));
        product.MetaData = new MetaCollection(reader.Read<MetaEntity>());
        product.Files = new ModelCollection<FileEntity>(reader.Read<FileEntity>());
        return product;
      }
    }

    public void ClearBasket(Guid userKey)
    {
      ExecuteWithTransaction((transaction) =>
      {
        ClearBasket(transaction, userKey);
      });
    }

    public void Update(ProductEntity product)
    {
      ExecuteWithTransaction((transaction) =>
      {
        // this uses context to determine the basket to clear
        transaction.Connection.Execute("dbo.SPUpdateProduct", new { productId = product.ProductId, title = product.Title, productFlags = (short)product.ProductFlags, productCode = product.ProductCode, categoryId = product.Category.CategoryId }, transaction: transaction, commandType: CommandType.StoredProcedure);

        // save options
        foreach (ProductOptionEntity option in product.Options)
        {
          transaction.Connection.Execute("dbo.SPUpdateProductOption", new { productOptionId = option.ProductOptionId, description = option.Description, qty = option.Qty, net = option.Detail.Net, tax = option.Detail.Tax, productDetailFlags = (int?)option.Detail.Flags }, transaction: transaction, commandType: CommandType.StoredProcedure);
        }
      });
    }

    public void Create(ProductEntity product)
    {
      ExecuteWithTransaction((transaction) =>
      {
        product.ProductId = Execute(transaction, "dbo.SPCreateProduct", new { title = product.Title, flags = (short)product.ProductFlags, productCode = product.ProductCode, categoryId = product.Category.CategoryId, net = (decimal?)null, tax = (decimal?)null, productDetailFlags = (int?)null });
        CreateOptions(transaction, product, product.Options);
      });
    }

    public SaleEntity ReadSale(int saleId)
    {
      return ReadSaleInternal("dbo.SPReadSale", new { saleId = saleId });
    }

    public SaleEntity ReadSale(string orderNumber)
    {
      return ReadSaleInternal("dbo.SPReadSaleFromOrderNumber", new { orderNumber = orderNumber });
    }

    public SaleEntity FindSale(string query)
    {
      using (IGridReader reader = QueryMultiple("dbo.SPFindSale", new { query = ToSqlSafe(query) }))
      {
        SaleEntity sale = reader.Read<SaleEntity>().First();
        sale.Items = reader.Read<SaleItemEntity>();
        return sale;
      }
    }

    public ModelCollection<SaleEntity> ListSales(int page, int maxPerPage)
    {
      return Query((connection) =>
      {
        IEnumerable<SaleEntity> result = connection.Query<SaleEntity>("dbo.SPListSales", new { page = page, maxPerPage = maxPerPage }, commandType: CommandType.StoredProcedure);
        int totalCount = connection.Query<int>("dbo.SPListSales_Count").First();
        return new ModelCollection<SaleEntity>(result, totalCount);
      });
    }

    public ModelCollection<SaleEntity> ListUserSales(Guid userKey)
    {
      return ModelQuery<SaleEntity>("dbo.SPListUserSales", new { userKey = userKey });
    }

    public ModelCollection<ProductEntityBase> ListProducts(int categoryId, ProductFlagTypes flags, ProductOrder order)
    {
      return ListProductsInternal("dbo.SPListProductsByCategory", new { categoryId = categoryId, productFlags = (byte)flags, order = (byte)order });
    }

    public ModelCollection<ProductEntityBase> ListProducts(ProductFlagTypes flags)
    {
      return ListProductsInternal("dbo.SPListProducts", new { productFlags = (short)flags });
    }

    public ModelCollection<ShippingEntity> ListShipping()
    {
      return ModelQuery<ShippingEntity>("dbo.SPListShipping");
    }

    public BasketProduct ReadBasketProduct(int productDetailId)
    {
      return Query<BasketProduct>("dbo.SPBasketProduct", new { productDetailId = productDetailId }).FirstOrDefault();
    }

    public int CreateSale(Guid userKey, SaleFlags flags, decimal taxRate, string saleCode, int? shippingId)
    {
      return QueryWithTransaction<int>("dbo.SPCreateSale", new { flags = (int)flags, taxRate = taxRate, saleCode = saleCode, userKey = userKey, shippingId = shippingId }).First();
    }

    public SaleEntity CreateAndReturnSale(Guid userKey, SaleFlags flags, decimal taxRate, string saleCode, int? shippingId)
    {
      int saleId = CreateSale(userKey, flags, taxRate, saleCode, shippingId);
      return ReadSale(saleId);
    }

    public void AddFlag(int saleId, SaleFlags flag)
    {
      Execute("dbo.SPSaleAddFlag", new { saleId = saleId, flag = (int)flag });
    }

    public void SaveBasket(Guid userKey, IEnumerable<IBasketProduct> items)
    {
      ExecuteWithTransaction((transaction) =>
      {
        ClearBasket(transaction, userKey);

        foreach (BasketProduct product in items)
        {
          transaction.Connection.Execute("dbo.SPAddToBasket", new { productDetailId = product.ProductDetailId, qty = product.Qty, userKey = userKey }, transaction: transaction, commandType: CommandType.StoredProcedure);
        }
      });
    }

    public ModelCollection<IBasketProduct> ListBasket()
    {
      return ModelQuery<IBasketProduct>("dbo.SPListBasket");
    }

    /// <summary>
    /// Cleans a string of any non alphanumeric and underscore characters to protect against sql injection.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private static string ToSqlSafe(string value)
    {
      if (string.IsNullOrEmpty(value))
      {
        return value;
      }

      const string pattern = "[^A-Za-z0-9_]";
      return Regex.Replace(value, pattern, string.Empty);
    }

    private SaleEntity ReadSaleInternal(string command, object param = null)
    {
      using (IGridReader reader = QueryMultiple(command, param))
      {
        SaleEntity sale = reader.Read<SaleEntity, ShippingEntity, AddressEntity, SaleEntity>((s, sh, a) =>
        {
          s.Delivery = new Delivery();

          if (sh != null)
          {
            s.Delivery.Shipping = sh;
          }
          else
          {
            s.Delivery.DeliveryType = DeliveryType.PickUpOnly;
          }

          s.Delivery.DeliveryAddress = a;

          return s;
        }, splitOn: "ShippingId,AddressId").First();

        sale.Items = reader.Read<SaleItemEntity>().ToList();

        return sale;
      }
    }

    private ModelCollection<ProductEntityBase> ListProductsInternal(string command, object args)
    {
      return Query((connection) =>
      {
        return new ModelCollection<ProductEntityBase>(connection.Query<ProductEntity, ProductOptionEntity, ProductDetailEntity, FileEntity, ProductEntity>(command, (p, o, d, f) =>
        {
          p.MinOption = o;
          p.MinOption.Detail = d;
          p.Thumb = f;
          return p;
        }, args, splitOn: "ProductOptionId,ProductDetailId,FileId", commandType: CommandType.StoredProcedure));
      });
    }

    private void ClearBasket(IDbTransaction transaction, Guid userKey)
    {
      transaction.Connection.Execute("dbo.SPClearBasket", new { userKey = userKey }, transaction: transaction, commandType: CommandType.StoredProcedure);
    }
  }
}