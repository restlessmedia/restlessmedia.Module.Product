namespace restlessmedia.Module.Product
{
  public class GoogleCheckoutProvider : ICheckoutProvider
  {
    public GoogleCheckoutProvider()
      : base() { }

    public Purchase CreateSale(SaleEntity sale)
    {
      string url = CreateResponse(sale);
      return new Purchase(sale, url);
    }

    private string CreateResponse(SaleEntity sale)
    {
      return null;
    }
  }
}