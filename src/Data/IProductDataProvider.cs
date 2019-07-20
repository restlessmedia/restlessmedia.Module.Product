using restlessmedia.Module.Data;
using System;
using System.Collections.Generic;

namespace restlessmedia.Module.Product.Data
{
  public interface IProductDataProvider : IDataProvider
  {
    void UpdateSaleOrderNumber(int saleId, string orderNumber);

    decimal ChargeAmount(SaleEntity sale, decimal amount);

    SaleFlags GetSaleFlags(int saleId);

    SaleFlags SetSaleFlags(int saleId, SaleFlags flags);

    void UpdateSaleFlags(int saleId, SaleFlags flags);

    void Update(ProductOptionEntity option);

    void CreateOptions(ProductEntity product, ModelCollection<ProductOptionEntity> options);

    void Delete(int productId);

    ProductEntity Read(int productId);

    void ClearBasket(Guid userKey);

    void Update(ProductEntity product);

    void Create(ProductEntity product);

    SaleEntity ReadSale(int saleId);

    SaleEntity ReadSale(string orderNumber);

    SaleEntity FindSale(string query);

    ModelCollection<SaleEntity> ListSales(int page, int maxPerPage);

    ModelCollection<SaleEntity> ListUserSales(Guid userKey);

    ModelCollection<ProductEntityBase> ListProducts(int categoryId, ProductFlagTypes flags, ProductOrder order);

    ModelCollection<ProductEntityBase> ListProducts(ProductFlagTypes flags);

    ModelCollection<ShippingEntity> ListShipping();

    BasketProduct ReadBasketProduct(int productDetailId);

    int CreateSale(Guid userKey, SaleFlags flags, decimal taxRate, string saleCode, int? shippingId);

    SaleEntity CreateAndReturnSale(Guid userKey, SaleFlags flags, decimal taxRate, string saleCode, int? shippingId);

    void AddFlag(int saleId, SaleFlags flag);

    void SaveBasket(Guid userKey, IEnumerable<IBasketProduct> items);

    ModelCollection<IBasketProduct> ListBasket();
  }
}
