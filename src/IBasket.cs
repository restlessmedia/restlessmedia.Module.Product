using System.Collections.Generic;

namespace restlessmedia.Module.Product
{
  public interface IBasket : IList<IBasketProduct>
  {
    bool IsEmpty { get; }

    void Add(IEnumerable<IBasketProduct> items);

    void Add(IBasketProduct item, int qty);

    void SetQty(int productDetailId, int qty);

    int IndexOf(int productDetailId);

    IBasketProduct Find(int productDetailId);

    IBasketProduct Find(IBasketProduct item);

    void Remove(int productDetailId);

    IDelivery Delivery { get; }

    /// <summary>
    /// Product total excluding tax
    /// </summary>
    decimal ProductTotal { get; }

    /// <summary>
    /// Total amount of tax payable on the products
    /// </summary>
    /// <param name="tax"></param>
    /// <returns></returns>
    decimal GetTaxTotal(decimal tax);

    /// <summary>
    /// Invoice total = Product total + Total payable tax + delivery cost
    /// </summary>
    /// <param name="tax"></param>
    /// <returns></returns>
    decimal GetTotal(decimal tax);

    /// <summary>
    /// Returns total count including quantities
    /// </summary>
    int TotalCount { get; }
  }
}