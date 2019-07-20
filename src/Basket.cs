using System.Collections.Generic;
using System.Linq;

namespace restlessmedia.Module.Product
{
  public class Basket : List<IBasketProduct>, IBasket
  {
    public Basket()
    {
      Delivery = new Delivery();
    }

    public bool IsEmpty
    {
      get
      {
        return Count == 0;
      }
    }

    public void Add(IEnumerable<IBasketProduct> items)
    {
      AddRange(items);
    }

    public void Add(IBasketProduct item, int qty = 1)
    {
      item.Qty = qty;

      IBasketProduct product = Find(item);
      if (product != null)
      {
        product.Qty++;
      }
      else
      {
        base.Add(item);
      }
    }

    public void SetQty(int productDetailId, int qty)
    {
      IBasketProduct product = Find(productDetailId);
      product.Qty = qty;
    }

    public int IndexOf(int productDetailId)
    {
      for (int i = 0; i < Count; i++)
      {
        if (int.Equals(this[i].ProductDetailId, productDetailId))
        {
          return i;
        }
      }
      return -1;
    }

    public IBasketProduct Find(int productDetailId)
    {
      int index = IndexOf(productDetailId);
      return index > -1 ? this[index] : null;
    }

    public IBasketProduct Find(IBasketProduct item)
    {
      return Find(item.ProductDetailId);
    }

    public void Remove(int productDetailId)
    {
      IBasketProduct product = Find(productDetailId);
      Remove(product);
    }

    public IDelivery Delivery { get; private set; }

    public decimal ProductTotal
    {
      get
      {
        return Count > 0 ? this.Sum(b => b.Total) : 0;
      }
    }

    public decimal GetTaxTotal(decimal tax)
    {
      return ProductTotal * tax;
    }

    public decimal GetTotal(decimal tax)
    {
      return ProductTotal + GetTaxTotal(tax) + GetShippingTotal();
    }

    public int TotalCount
    {
      get
      {
        return this.Sum(b => b.Qty);
      }
    }

    private decimal GetShippingTotal()
    {
      return Delivery != null && Delivery.Shipping != null ? Delivery.Shipping.Cost : 0;
    }
  }
}