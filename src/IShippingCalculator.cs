namespace restlessmedia.Module.Product
{
  public interface IShippingCalculator
  {
    /// <summary>
    /// Shipping calculation routine
    /// </summary>
    /// <returns></returns>
    IShipping Calculate(IBasket basket);
  }
}