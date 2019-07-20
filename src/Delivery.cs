using restlessmedia.Module.Address;

namespace restlessmedia.Module.Product
{
  public class Delivery : IDelivery
  {
    public Delivery(DeliveryType deliveryType = DeliveryType.Address)
    {
      DeliveryType = deliveryType;
      DeliveryAddress = new AddressEntity(AddressType.DeliveryAddress);
    }

    public DeliveryType DeliveryType
    {
      get
      {
        return _deliveryType;
      }
      set
      {
        _deliveryType = value;

        // if we are pick up only - reset anything delivery related
        if (_deliveryType == DeliveryType.PickUpOnly)
        {
          DeliveryAddress = null;
          Shipping = null;
        }
      }
    }

    public IAddress DeliveryAddress { get; set; }

    public IShipping Shipping { get; set; }

    public decimal Total
    {
      get
      {
        return Shipping != null ? Shipping.Cost : 0;
      }
    }

    private DeliveryType _deliveryType;
  }
}