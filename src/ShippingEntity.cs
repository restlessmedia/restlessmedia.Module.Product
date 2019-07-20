namespace restlessmedia.Module.Product
{
  public class ShippingEntity : Entity, IShipping
  {
    public ShippingEntity() { }

    public override EntityType EntityType
    {
      get
      {
        return EntityType.Shipping;
      }
    }

    public override int? EntityId
    {
      get
      {
        return ShippingId;
      }
    }

    public int ShippingId { get; set; }

    public string Name { get; set; }

    public decimal Cost { get; set; }

    public int? Options { get; set; }

    public string Pattern { get; set; }
  }
}