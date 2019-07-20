using restlessmedia.Module.Address;

namespace restlessmedia.Module.Product
{
  public interface IDelivery
  {
    DeliveryType DeliveryType { get; set; }

    IAddress DeliveryAddress { get; set; }

    IShipping Shipping { get; set; }

    /// <summary>
    /// If applicable, returns the shipping amount, otherwise 0
    /// </summary>
    decimal Total { get; }
  }
}