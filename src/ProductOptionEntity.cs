using System.Text.RegularExpressions;

namespace restlessmedia.Module.Product
{
  public class ProductOptionEntity : Entity
  {
    public ProductOptionEntity() { }

    public override EntityType EntityType
    {
      get
      {
        return EntityType.ProductOption;
      }
    }

    public override int? EntityId
    {
      get
      {
        return ProductOptionId;
      }
    }

    public int ProductOptionId { get; set; }

    public string Description { get; set; }

    public string DescriptionSample
    {
      get
      {
        if (string.IsNullOrEmpty(Description))
        {
          return Description;
        }

        int maxLength = 200;
        const string _matchTagPattern = @"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[\^'"">\s]+))?)+\s*|\s*)/?>";
        string description = Regex.Replace(Description, _matchTagPattern, string.Empty).Trim();

        return description.Length > maxLength ? string.Concat(description.Substring(0, maxLength), "...") : description;
      }
    }

    public byte Qty { get; set; }

    public ProductDetailEntity Detail
    {
      get
      {
        if (_detail == null)
        {
          _detail = new ProductDetailEntity();
        }

        return _detail;
      }
      set
      {
        _detail = value;
      }
    }

    private ProductDetailEntity _detail = null;
  }
}