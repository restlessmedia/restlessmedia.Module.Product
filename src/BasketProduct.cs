using System;

namespace restlessmedia.Module.Product
{
  [Serializable]
  public class BasketProduct : IBasketProduct
  {
    public BasketProduct(int qty)
    {
        Qty = qty;
    }

    public BasketProduct() { }

    public int ProductId { get; set; }

    public int ProductDetailId { get; set; }

    public decimal Net { get; set; }

    public decimal Tax { get; set; }

    public decimal Total
    {
      get
      {
        return (Net + Tax) * Qty;
      }
    }

    public int Qty { get; set; }

    public string Title { get; set; }

    public DateTime ValidFrom { get; set; }

    public string Description { get; set; }
  }
}