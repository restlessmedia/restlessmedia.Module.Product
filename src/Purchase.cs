using restlessmedia.Module.Security;

namespace restlessmedia.Module.Product
{
  public class Purchase
  {
    public Purchase(SaleEntity sale)
    {
      Sale = sale;
    }

    public Purchase(SaleEntity sale, string url)
      : this(sale)
    {
      Url = url;
    }

    /// <summary>
    /// The original sale
    /// </summary>
    public SaleEntity Sale { get; private set; }

    /// <summary>
    /// If applicable, the url to complete the purchase process.  Used when checkout is provided by a third party process.
    /// </summary>
    public string Url { get; set; }

    public UserStatus UserStatus = UserStatus.None;
  }
}