using System;
using System.Collections.Generic;
using System.Linq;

namespace restlessmedia.Module.Product
{
  public class SaleEntity : Entity
  {
    public SaleEntity() { }

    public override EntityType EntityType
    {
      get
      {
        return EntityType.Sale;
      }
    }

    public override int? EntityId
    {
      get
      {
        return SaleId;
      }
    }

    public int SaleId { get; set; }

    /// <summary>
    /// The public sale code used to identify a sale between customer and supplier
    /// </summary>
    public string SaleCode { get; set; }

    public Guid UserKey { get; set; }

    public DateTime CheckoutDate { get; set; }

    /// <summary>
    /// Some flags once set are disabled  e.g. when a sale is cancelled, it cannot be 'uncancelled'.
    /// </summary>
    /// <param name="flag"></param>
    /// <returns></returns>
    public bool IsFlagDisabled(SaleFlags flag)
    {
      SaleFlags checkFlags = SaleFlags.Cancelled | SaleFlags.Refunded | SaleFlags.Created;

      if (!checkFlags.HasFlag(flag))
      {
        return false;
      }

      return SaleFlags.HasFlag(flag);
    }

    public SaleFlags SaleFlags { get; set; }

    public int SaleFlagsValue
    {
      get
      {
        return (int)SaleFlags;
      }
    }

    /// <summary>
    /// Order number held by merchant when using a provider like google checkout.
    /// </summary>
    public string OrderNumber { get; set; }

    /// <summary>
    /// Returns the current relevant sales status flag
    /// </summary>
    public SaleFlags CurrentStatus
    {
      get
      {
        // first set-up the flags to check (remove non status flags)
        SaleFlags checkFlags = SaleFlags & ~(SaleFlags.Delivery | SaleFlags.CollectionOnly);

        // get the values remaining
        IEnumerable<SaleFlags> setValues = System.Enum.GetValues(SaleFlags.GetType()).Cast<SaleFlags>().Where(f => (f & checkFlags) == f);

        // find the max and cast to find the highest relevant
        return setValues.Any() ? setValues.Max() : SaleFlags.None;
      }
    }

    /// <summary>
    /// Total of products including qty excluding tax
    /// </summary>
    public decimal ProductTotal { get; set; }

    /// <summary>
    /// Tax rate applied to the sale.  It seems displaying it, we leave as 0.1 but when using it for calculations, we need to multiple by 100
    /// </summary>
    public decimal TaxRate { get; set; }

    /// <summary>
    /// Used for google interaction, converts and returns the tax rate as a double for google checkout
    /// </summary>
    public double GoogleTaxRate
    {
      get
      {
        return (double)TaxRate;
      }
    }

    /// <summary>
    /// Calculated tax total
    /// </summary>
    public decimal TaxTotal
    {
      get
      {
        return TaxRate * ProductTotal;
      }
    }

    /// <summary>
    /// Count of items in the sale.  This is not a read of the items collection in case the items aren't available i.e in sale list view
    /// </summary>
    public int ItemCount { get; set; }

    /// <summary>
    /// Totals including tax
    /// </summary>
    public decimal InvoiceTotal
    {
      get
      {
        return ProductTotal + TaxTotal + Delivery.Total;
      }
    }

    public IEnumerable<SaleItemEntity> Items
    {
      get
      {
        if (_items == null)
        {
          _items = new List<SaleItemEntity>();
        }

        return _items;
      }
      set
      {
        _items = value;
      }
    }

    /// <summary>
    /// Delivery information
    /// </summary>
    public IDelivery Delivery
    {
      get
      {
        return _delivery = _delivery ?? new Delivery();
      }
      set
      {
        _delivery = value;
      }
    }

    /// <summary>
    /// Username of the user that placed the order
    /// </summary>
    public string UserName { get; set; }

    public string UserEmail { get; set; }

    public Currency Currency { get { return Currency.GBP; } }

    public decimal Paid { get; set; }

    private IDelivery _delivery;

    private IEnumerable<SaleItemEntity> _items;
  }
}