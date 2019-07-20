namespace restlessmedia.Module.Product
{
  public interface ICheckoutProvider : IProvider
  {
    /// <summary>
    /// Creates a sale using the provider and returns a redirect url if the user needs to be redirected to provider url to continue the process
    /// </summary>
    /// <param name="sale"></param>
    /// <returns></returns>
    Purchase CreateSale(SaleEntity sale);
  }
}