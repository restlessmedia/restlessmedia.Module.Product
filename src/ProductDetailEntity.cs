using System;

namespace restlessmedia.Module.Product
{
  public class ProductDetailEntity : Entity
  {
    public ProductDetailEntity()
    {
      ValidFrom = DateTime.Now;
      Tax = 0;
    }

    public override EntityType EntityType
    {
      get
      {
        return EntityType.ProductDetail;
      }
    }

    public override int? EntityId
    {
      get
      {
        return ProductDetailId;
      }
    }

    public int ProductDetailId { get; set; }

    public DateTime ValidFrom { get; set; }

    public decimal Net { get; set; }

    /// <summary>
    /// The percentage of the net value which is tax
    /// </summary>
    public decimal Tax { get; set; }

    public bool IsDiscounted
    {
      get
      {
        return Flags.HasFlag(ProductDetailFlags.Discount);
      }
      set
      {
        Flags = value ? Flags |= ProductDetailFlags.Discount : Flags &= ~ProductDetailFlags.Discount;
      }
    }

    public ProductDetailFlags Flags { get; set; }

    /// <summary>
    /// Mapping for dapper when this appears in a result with flags in another type
    /// </summary>
    private ProductDetailFlags ProductDetailFlags
    {
      set
      {
        Flags = value;
      }
    }
  }
}