using restlessmedia.Module.Category;
using restlessmedia.Module.File;
using restlessmedia.Module.Meta;

namespace restlessmedia.Module.Product
{
  public class ProductEntity : ProductEntityBase
  {
    /// <summary>
    /// Returns the category object for this category id - this will make a single db call if invoked.
    /// </summary>
    public CategoryEntity Category
    {
      get
      {
        if (_category == null)
        {
          _category = new CategoryEntity();
        }

        return _category;
      }
      set
      {
        _category = value;
      }
    }

    public string ProductCode { get; set; }

    public ProductFlagTypes ProductFlags { get; set; }

    public ProductFlagTypes SetFlag(ProductFlagTypes flag, bool on = true)
    {
      ProductFlagTypes flags = ProductFlags;

      // toggle flag
      if (on)
      {
        flags |= flag;
      }
      else
      {
        flags &= flag;
      }

      // now set the main property back after the changes
      ProductFlags = flags;

      // return result
      return flags;
    }

    public ModelCollection<ProductOptionEntity> Options
    {
      get
      {
        return _options = _options ?? new ModelCollection<ProductOptionEntity>();
      }
      set
      {
        _options = value;
      }
    }

    public ModelCollection<FileEntity> Files = new ModelCollection<FileEntity>();

    public MetaCollection MetaData = new MetaCollection();

    private ModelCollection<ProductOptionEntity> _options;

    private CategoryEntity _category;
  }
}