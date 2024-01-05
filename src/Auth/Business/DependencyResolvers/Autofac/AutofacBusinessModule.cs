using Autofac;
using Business.Abstract;
using Business.Concrete;
using Core.Extensions;
using Core.Utilities.Security;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using DataAccess.Concrete;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterDalTypes(builder);
            RegisterBusinessTypes(builder);
        }

        private void RegisterDalTypes(ContainerBuilder builder)
        {
            builder.RegisterType<UserDal, IUserDal>();
            builder.RegisterType<UserLoginDal, IUserLoginDal>();
            builder.RegisterType<UserDeviceDal, IUserDeviceDal>();
            builder.RegisterType<UserPasswordResetDal, IUserPasswordResetDal>();
        }

        private void RegisterBusinessTypes(ContainerBuilder builder)
        {
            builder.RegisterType<JwtHelper, ITokenHelper>();          
            builder.RegisterType<UserManager, IUserService>();
            builder.RegisterType<AuthManager, IAuthService>();            
            builder.RegisterType<UserLoginManager, IUserLoginService>();
            builder.RegisterType<UserDeviceManager, IUserDeviceService>();
            builder.RegisterType<UserPasswordResetManager, IUserPasswordResetService>();   
        }
    }
}