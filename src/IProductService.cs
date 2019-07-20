using System.Web;
using restlessmedia.Module.Security;

namespace restlessmedia.Module.Product
{
  public interface IProductService : IService
  {
    /// <summary>
    /// Updates order number to a sale (usually merchant order number when sale is initiated)
    /// </summary>
    /// <param name="saleId"></param>
    /// <param name="orderNumber"></param>
    void UpdateSaleOrderNumber(int saleId, string orderNumber);

    /// <summary>
    /// If using a merchant, we may need to confirm the amount requested to pay was actually paid and nothing is outstanding.
    /// </summary>
    /// <param name="sale"></param>
    /// <param name="amount"></param>
    /// <returns>Outstanding amount</returns>
    decimal ChargeAmount(SaleEntity sale, decimal amount);

    /// <summary>
    /// If true, the sale exists in the system
    /// </summary>
    /// <param name="saleId"></param>
    /// <returns></returns>
    bool SaleExists(int saleId);

    /// <summary>
    /// Simply returns the sale flags for a sale
    /// </summary>
    /// <param name="saleId"></param>
    /// <returns></returns>
    SaleFlags GetSaleFlags(int saleId);

    /// <summary>
    /// Sets a particular sale flag.
    /// </summary>
    /// <param name="sale"></param>
    /// <param name="flags"></param>
    /// <returns></returns>
    void SetSaleFlags(SaleEntity sale, SaleFlags flags);

    /// <summary>
    /// Sets a particular sale flag.
    /// </summary>
    /// <param name="saleId"></param>
    /// <param name="flags"></param>
    /// <returns></returns>
    SaleFlags SetSaleFlags(int saleId, SaleFlags flags);

    /// <summary>
    /// Routine to handle updating the entire value of the sale flags (mainly used for order status changes)
    /// </summary>
    /// <param name="saleId"></param>
    /// <param name="flags"></param>
    void UpdateSaleFlags(int saleId, SaleFlags flags);

    /// <summary>
    /// Routine to handle updating the entire value of the sale flags (mainly used for order status changes)
    /// </summary>
    /// <param name="sale"></param>
    /// <param name="flags"></param>
    void UpdateSaleFlags(SaleEntity sale, SaleFlags flags);

    int Save(ProductEntity product);

    void CreateOptions(ProductEntity product, ModelCollection<ProductOptionEntity> options);

    void Delete(int productId);

    ProductEntity Read(int productId);

    SaleEntity ReadSale(int saleId);

    /// <summary>
    /// Returns a sale by order number
    /// </summary>
    /// <param name="orderNumber"></param>
    /// <returns></returns>
    SaleEntity ReadSale(string orderNumber);

    /// <summary>
    /// Finds a specific sale from a keyword.  This will only return the first sale found.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    SaleEntity FindSale(string query);

    /// <summary>
    /// List all sales created by this application with pagination
    /// </summary>
    /// <param name="page"></param>
    /// <param name="maxPerPage"></param>
    /// <param name="totalCount"></param>
    /// <returns></returns>
    ModelCollection<SaleEntity> ListSales(int page, int maxPerPage);

    /// <summary>
    /// Returns a list of sales for a specific user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    ModelCollection<SaleEntity> ListUserSales(IUserInfo user);

    ModelCollection<ProductEntityBase> ListProducts(int categoryId, ProductFlagTypes flags, ProductOrder order = ProductOrder.CostDesc);

    /// <summary>
    /// Returns a list of active products in a category
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    ModelCollection<ProductEntityBase> ListProducts(int categoryId, ProductOrder order = ProductOrder.CostDesc);

    /// <summary>
    /// Returns a list of active products
    /// </summary>
    /// <param name="flags"></param>
    /// <returns></returns>
    ModelCollection<ProductEntityBase> ListProducts(ProductFlagTypes flags = ProductFlagTypes.Active);

    ModelCollection<ShippingEntity> ListShipping();

    IBasket GetBasket(HttpContextBase context, bool createIfNull = true);

    void SetBasket(HttpContextBase context, IBasket basket);

    void AddToBasket(HttpContextBase context, int productDetailId, int qty);

    /// <summary>
    /// Starts the sale process with the specified basket
    /// </summary>
    /// <param name="context"></param>
    /// <param name="user"></param>
    /// <param name="basket"></param>
    /// <param name="taxRate"></param>
    /// <param name="saleCodePrefix"></param>
    /// <returns></returns>
    Purchase ConvertBasketToSale(HttpContextBase context, IUserInfo user, IBasket basket, decimal taxRate, string saleCodePrefix);

    /// <summary>
    /// Starts the sale process using the current basket
    /// </summary>
    /// <param name="context"></param>
    /// <param name="user"></param>
    /// <param name="taxRate"></param>
    /// <param name="saleCodePrefix"></param>
    /// <returns></returns>
    Purchase ConvertBasketToSale(HttpContextBase context, IUserInfo user, decimal taxRate, string saleCodePrefix);

    /// <summary>
    /// Adds a flag to an existing sale
    /// </summary>
    /// <param name="saleId"></param>
    /// <param name="flag"></param>
    void AddFlag(int saleId, SaleFlags flag);

    /// <summary>
    /// Generates a 8 number unqiue code for a sale i.e. 12345678
    /// </summary>
    /// <returns></returns>
    string GenerateSaleCode();

    void SaveBasket(IUserInfo user, IBasket basket);

    /// <summary>
    /// Restores a saved basket from the datasource, this will overwrite any items currently set
    /// </summary>
    /// <param name="context"></param>
    /// <param name="user"></param>
    void RestoreBasket(HttpContextBase context, IUserInfo user);

    /// <summary>
    /// Clears the basket, set emptyDatasource to clear the basket saved
    /// </summary>
    /// <param name="context"></param>
    /// <param name="user"></param>
    /// <param name="emptyDatasource"></param>
    void ClearBasket(HttpContextBase context, IUserInfo user, bool emptyDatasource = false);
  }
}