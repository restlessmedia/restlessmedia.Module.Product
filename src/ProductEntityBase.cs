using restlessmedia.Module.File;

namespace restlessmedia.Module.Product
{
  public class ProductEntityBase : Entity
  {
    public ProductEntityBase() { }

    public override EntityType EntityType
    {
      get
      {
        return EntityType.Product;
      }
    }

    public override int? EntityId
    {
      get
      {
        return ProductId;
      }
    }

    public int? ProductId { get; set; }

    public ProductOptionEntity MinOption { get; set; }

    public FileEntity Thumb { get; set; }
  }
}