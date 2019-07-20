using System;

namespace restlessmedia.Module.Product
{
  /// <summary>
  /// These flags should be in an order so whatever is the largest flag set will be the current status.
  /// This may not work with refunded and cancelled.
  /// </summary>
  [Flags]
  public enum SaleFlags
  {
    None = 0,
    /// <summary>
    /// The sale process was initiated and basket created to sale items
    /// </summary>
    Created = 1,
    /// <summary>
    /// The order is ready to be charged (i.e. payment checks have passed) and full payment has not yet been recieved
    /// </summary>
    Chargeable = 2,
    /// <summary>
    /// Full payment has been taken for the sale
    /// </summary>
    Charged = 4,
    /// <summary>
    /// The sale was cancelled before payment was made
    /// </summary>
    Cancelled = 8,
    /// <summary>
    /// The sale was paid for but refunded
    /// </summary>
    Refunded = 16,
    /// <summary>
    /// Item has been shipped to the customer
    /// </summary>
    Shipped = 32,
    /// <summary>
    /// Item has been collected in person by the customer
    /// </summary>
    Collected = 64,
    /// <summary>
    /// Sale is being delivered (was option 1)
    /// </summary>
    Delivery = 128,
    /// <summary>
    /// No delivery - item is being collected from an arranged location (was option 2)
    /// </summary>
    CollectionOnly = 256,
    /// <summary>
    /// Charge attempt failed
    /// </summary>
    PaymentDeclined = 512
  }
}