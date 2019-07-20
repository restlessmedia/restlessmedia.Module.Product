using System;

namespace restlessmedia.Module.Product
{
  [Flags]
  public enum ProductDetailFlags
  {
    None = 0,
    /// <summary>
    /// Highlights that the product detail is set at a discounted price
    /// </summary>
    Discount = 1
  }
}