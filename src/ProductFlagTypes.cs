using System;

namespace restlessmedia.Module.Product
{
  [Flags]
  public enum ProductFlagTypes : byte
  {
    /// <summary>
    /// Product is available to purchase
    /// </summary>
    Active = 1
  }
}