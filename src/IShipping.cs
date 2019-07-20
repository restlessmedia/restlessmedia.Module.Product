namespace restlessmedia.Module.Product
{
  public interface IShipping
  {
    int ShippingId { get; set; }

    string Name { get; set; }

    decimal Cost { get; set; }

    int? Options { get; set; }

    string Pattern { get; set; }
  }
}