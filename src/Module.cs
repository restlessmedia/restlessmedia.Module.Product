using Autofac;
using restlessmedia.Module.Product.Data;

namespace restlessmedia.Module.Product
{
  public class Module : IModule
  {
    public void RegisterComponents(ContainerBuilder containerBuilder)
    {
      containerBuilder.RegisterType<GoogleCheckoutProvider>().As<ICheckoutProvider>().SingleInstance();
      containerBuilder.RegisterType<ProductService>().As<IProductService>().SingleInstance();
      containerBuilder.RegisterType<Delivery>().As<IDelivery>().SingleInstance();
      containerBuilder.RegisterType<ProductDataProvider>().As<IProductDataProvider>().SingleInstance();
    }
  }
}