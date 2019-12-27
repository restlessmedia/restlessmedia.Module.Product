using System;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using restlessmedia.Module.File;
using restlessmedia.Module.Meta;
using restlessmedia.Module.Product.Data;
using restlessmedia.Module.Security;

namespace restlessmedia.Module.Product
{
  public class ProductService : IProductService
  {
    public ProductService(IFileService fileService, IMetaService metaService, IProductDataProvider productDataProvider, ICheckoutProvider checkoutProvider, ISecurityService securityService)
    {
      _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
      _metaService = metaService ?? throw new ArgumentNullException(nameof(metaService));
      _productDataProvider = productDataProvider ?? throw new ArgumentNullException(nameof(productDataProvider));
      _checkoutProvider = checkoutProvider ?? throw new ArgumentNullException(nameof(checkoutProvider));
      _securityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
    }

    /// <summary>
    /// Updates order number to a sale (usually merchant order number when sale is initiated)
    /// </summary>
    /// <param name="saleId"></param>
    /// <param name="orderNumber"></param>
    public void UpdateSaleOrderNumber(int saleId, string orderNumber)
    {
      _productDataProvider.UpdateSaleOrderNumber(saleId, orderNumber);
    }

    /// <summary>
    /// If using a merchant, we may need to confirm the amount requested to pay was actually paid and nothing is outstanding.
    /// </summary>
    /// <param name="sale"></param>
    /// <param name="amount"></param>
    /// <returns>Outstanding amount</returns>
    public decimal ChargeAmount(SaleEntity sale, decimal amount)
    {
      return _productDataProvider.ChargeAmount(sale, amount);
    }

    /// <summary>
    /// If true, the sale exists in the system
    /// </summary>
    /// <param name="saleId"></param>
    /// <returns></returns>
    public bool SaleExists(int saleId)
    {
      // piggy back on this method which is light and should tell us if there's a sale
      return GetSaleFlags(saleId) != SaleFlags.None;
    }

    /// <summary>
    /// Simply returns the sale flags for a sale
    /// </summary>
    /// <param name="saleId"></param>
    /// <returns></returns>
    public SaleFlags GetSaleFlags(int saleId)
    {
      return _productDataProvider.GetSaleFlags(saleId);
    }

    /// <summary>
    /// Sets a particular sale flag.
    /// </summary>
    /// <param name="sale"></param>
    /// <param name="flags"></param>
    /// <returns></returns>
    public void SetSaleFlags(SaleEntity sale, SaleFlags flags)
    {
      SetSaleFlags(sale.SaleId, flags);
      sale.SaleFlags &= flags;
    }

    /// <summary>
    /// Sets a particular sale flag.
    /// </summary>
    /// <param name="saleId"></param>
    /// <param name="flags"></param>
    /// <returns></returns>
    public SaleFlags SetSaleFlags(int saleId, SaleFlags flags)
    {
      return _productDataProvider.SetSaleFlags(saleId, flags);
    }

    /// <summary>
    /// Routine to handle updating the entire value of the sale flags (mainly used for order status changes)
    /// </summary>
    /// <param name="saleId"></param>
    /// <param name="flags"></param>
    public void UpdateSaleFlags(int saleId, SaleFlags flags)
    {
      _productDataProvider.SetSaleFlags(saleId, flags);
    }

    /// <summary>
    /// Routine to handle updating the entire value of the sale flags (mainly used for order status changes)
    /// </summary>
    /// <param name="sale"></param>
    /// <param name="flags"></param>
    public void UpdateSaleFlags(SaleEntity sale, SaleFlags flags)
    {
      UpdateSaleFlags(sale.SaleId, flags);
    }

    public int Save(ProductEntity product)
    {
      if (product.ProductId.HasValue)
      {
        Update(product);
      }
      else
      {
        Create(product);
      }

      _metaService.Save(product, product.MetaData);

      return product.ProductId.Value;
    }

    public void CreateOptions(ProductEntity product, ModelCollection<ProductOptionEntity> options)
    {
      _productDataProvider.CreateOptions(product, options);
    }

    public void Delete(int productId)
    {
      _productDataProvider.Delete(productId);
    }

    public ProductEntity Read(int productId)
    {
      return _productDataProvider.Read(productId);
    }

    public SaleEntity ReadSale(int saleId)
    {
      SaleEntity sale;

      if ((sale = _productDataProvider.ReadSale(saleId)) == null)
      {
        throw new Exception($"SaleNotFound {saleId}");
      }

      return sale;
    }

    public SaleEntity ReadSale(string orderNumber)
    {
      SaleEntity sale;

      if ((sale = _productDataProvider.ReadSale(orderNumber)) == null)
      {
        throw new Exception("SaleNotFoundWithOrderNo {orderNumber}");
      }

      return sale;
    }

    public SaleEntity FindSale(string query)
    {
      return _productDataProvider.FindSale(query);
    }

    public ModelCollection<SaleEntity> ListSales(int page, int maxPerPage)
    {
      ModelCollection<SaleEntity> list = _productDataProvider.ListSales(page, maxPerPage);
      list.Paging.Page = page;
      list.Paging.MaxPerPage = maxPerPage;
      return list;
    }

    public ModelCollection<SaleEntity> ListUserSales(IUserInfo user)
    {
      return _productDataProvider.ListUserSales(user.GetUniqueId<Guid>());
    }

    public ModelCollection<ProductEntityBase> ListProducts(int categoryId, ProductFlagTypes flags, ProductOrder order = ProductOrder.CostDesc)
    {
      return _productDataProvider.ListProducts(categoryId, flags, order);
    }

    public ModelCollection<ProductEntityBase> ListProducts(int categoryId, ProductOrder order = ProductOrder.CostDesc)
    {
      return ListProducts(categoryId, ProductFlagTypes.Active, order);
    }

    public ModelCollection<ProductEntityBase> ListProducts(ProductFlagTypes flags = ProductFlagTypes.Active)
    {
      return _productDataProvider.ListProducts(flags);
    }

    public ModelCollection<ShippingEntity> ListShipping()
    {
      return _productDataProvider.ListShipping();
    }

    public IBasket GetBasket(HttpContextBase context, bool createIfNull = true)
    {
      IBasket basket = context.Session[_sessionKey] as IBasket;

      if (createIfNull && basket == null)
      {
        basket = new Basket();
        SetBasket(context, basket);
      }

      return basket;
    }

    public void SetBasket(HttpContextBase context, IBasket basket)
    {
      context.Session[_sessionKey] = basket;
    }

    public void AddToBasket(HttpContextBase context, int productDetailId, int qty)
    {
      GetBasket(context).Add(_productDataProvider.ReadBasketProduct(productDetailId), qty);
    }

    public Purchase ConvertBasketToSale(HttpContextBase context, IUserInfo user, IBasket basket, decimal taxRate, string saleCodePrefix)
    {
      UserStatus status = UserStatus.None;

      // if we have a user that has not yet been created, create them
      if (!_securityService.Exists(user.Identity.Name))
      {
        status = _securityService.CreateUser(user);

        if (status != UserStatus.Success)
        {
          throw new SecurityException($"Unable to create user for sale: {_securityService.StatusMessage(status)}");
        }
      }

      // save it to the datasource so it can be used in the sale
      SaveBasket(user, basket);

      // create sale
      SaleFlags flags = (basket.Delivery.DeliveryType == DeliveryType.Address ? SaleFlags.Delivery : SaleFlags.CollectionOnly) | SaleFlags.Created;
      SaleEntity sale = _productDataProvider.CreateAndReturnSale(user.GetUniqueId<Guid>(), flags, taxRate, string.Concat(saleCodePrefix, GenerateSaleCode()), null);

      ClearBasket(context, user, emptyDatasource: false); // clear the basket (leave the basket intact in the datasource - this binds the sale)

      Purchase purchase = _checkoutProvider.CreateSale(sale);
      purchase.UserStatus = status;

      return purchase;
    }

    public Purchase ConvertBasketToSale(HttpContextBase context, IUserInfo user, decimal taxRate, string saleCodePrefix)
    {
      return ConvertBasketToSale(context, user, GetBasket(context), taxRate, saleCodePrefix);
    }

    public void AddFlag(int saleId, SaleFlags flag)
    {
      _productDataProvider.AddFlag(saleId, flag);
    }

    public string GenerateSaleCode()
    {
      byte[] b = new byte[8];
      RNGCryptoServiceProvider rngCrypto = new RNGCryptoServiceProvider();
      rngCrypto.GetBytes(b);
      int[] numbers = b.Select(i => Convert.ToInt32((i * 100 / 255) / 10)).ToArray();
      return string.Join(string.Empty, numbers);
    }

    public void SaveBasket(IUserInfo user, IBasket basket)
    {
      _productDataProvider.SaveBasket(user.GetUniqueId<Guid>(), basket);
    }

    public void RestoreBasket(HttpContextBase context, IUserInfo user)
    {
      ModelCollection<IBasketProduct> items = _productDataProvider.ListBasket();
      ClearBasket(context, user);
      GetBasket(context).Add(items);
    }

    /// <summary>
    /// Clears the basket, set emptyDatasource to clear the basket saved
    /// </summary>
    /// <param name="context"></param>
    /// <param name="emptyDatasource"></param>
    public void ClearBasket(HttpContextBase context, IUserInfo user, bool emptyDatasource = false)
    {
      // clear internal list
      GetBasket(context).Clear();

      // empty datasource?
      if (emptyDatasource)
      {
        _productDataProvider.ClearBasket((Guid)user.UniqueId);
      }
    }

    private void Update(ProductEntity product)
    {
      _productDataProvider.Update(product);
    }

    private void Create(ProductEntity product)
    {
      _productDataProvider.Create(product);
    }

    private IFileService _fileService;

    private IMetaService _metaService;

    private IProductDataProvider _productDataProvider;

    private ICheckoutProvider _checkoutProvider;

    private ISecurityService _securityService;

    private const string _sessionKey = "basket";
  }
}