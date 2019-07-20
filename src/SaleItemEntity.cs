using System;

namespace restlessmedia.Module.Product
{
  public class SaleItemEntity : Entity
  {
    public SaleItemEntity() { }

    public override EntityType EntityType
    {
      get
      {
        return EntityType.SaleItem;
      }
    }

    public override int? EntityId
    {
      get
      {
        return SaleId;
      }
    }

    public int? SaleId { get; set; }

    public int BasketId { get; set; }

    public byte Qty { get; set; }

    public Guid UserKey { get; set; }

    public int ProductDetailId { get; set; }

    public int ProductOptionId { get; set; }

    public decimal Net { get; set; }

    public decimal Tax { get; set; }

    public string Description { get; set; }

    public string ProductCode { get; set; }

    public int ProductId { get; set; }

    /// <summary>
    /// Calculated tax total
    /// </summary>
    public decimal TaxTotal(decimal tax)
    {
      return (tax / 100) * Net;
    }

    /// <summary>
    /// Total including tax
    /// </summary>
    /// <param name="tax"></param>
    /// <returns></returns>
    public decimal InvoiceTotal(decimal tax)
    {
      return TaxTotal(tax) + Net;
    }
  }
}