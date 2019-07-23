using restlessmedia.Test;
using Xunit;

namespace restlessmedia.Module.Product.UnitTest
{
    public class ProductDetailEntityTests
    {
        [Fact]
        public void IsDiscounted_uses_flags()
        {
            new ProductDetailEntity
            {
                IsDiscounted = false
            }.Flags.HasFlag(ProductDetailFlags.Discount).MustBeFalse();

            new ProductDetailEntity
            {
                IsDiscounted = true
            }.Flags.HasFlag(ProductDetailFlags.Discount).MustBeTrue();
        }
    }
}
