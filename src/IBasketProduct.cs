using System;

namespace restlessmedia.Module.Product
{
  public interface IBasketProduct
  {
    int ProductId { get; }

    int ProductDetailId { get; }

    decimal Net { get; }

    decimal Tax { get; }

    decimal Total { get; }

    int Qty { get; set; }

    string Title { get; }

    DateTime ValidFrom { get; }

    string Description { get; }
  }
}