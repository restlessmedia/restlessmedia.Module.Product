using SqlBuilder;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restlessmedia.Module.Product.Data.DataModel
{
  [Table("_VProduct", Schema = "dbo")]
  internal class VProduct : DataModel<VProduct, ProductEntity>
  {
    [Key]
    public int ProductId { get; set; }

    public string Title { get; set; }

    public string ProductCode { get; set; }

    public ProductFlagTypes ProductFlags { get; set; }

    [ReadOnly(true)]
    public int? EntityGuid { get; set; }

    [ReadOnly(true)]
    public int? LicenseId { get; set; }

    public EntityType EntityType { get; set; }

    public int? ProductOptionId { get; set; }

    public string Description { get; set; }

    public int? ProductDetailId { get; set; }

    public decimal Net { get; set; }

    public decimal Tax { get; set; }

    public ProductDetailFlags ProductDetailFlags { get; set; }

    public int CategoryId { get; set; }

    public string Category { get; set; }

    public int FileId { get; set; }

    public string SystemFileName { get; set; }

    public string File { get; set; }

    public string FileName { get; set; }

    public string MimeType { get; set; }
  }
}