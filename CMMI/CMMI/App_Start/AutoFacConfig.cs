using Autofac;
using Autofac.Integration.WebApi;
using CMMI.Interfaces.Facade;
using CMMI.Services.Facade;

namespace CMMI.App_Start
{
    public static class AutoFacConfig
    {
        public static void Initialize(ContainerBuilder builder)
        {
            builder.RegisterType<RestaurantFacade>().As<IRestaurantFacade>().InstancePerApiRequest();
            builder.RegisterType<ReviewFacade>().As<IReviewFacade>().InstancePerApiRequest();
            builder.RegisterType<UserFacade>().As<IUserFacade>().InstancePerApiRequest();
        }
    }
}