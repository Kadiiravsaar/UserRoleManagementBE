using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using URM.Core.Services;
using URM.Service.Rules;
using URM.Service.Services;
namespace URM.Service.DependencyResolvers
{

	public class AutoFacBusinessModule : Module
	{
     
        protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
			
			builder.RegisterType<RoleBusinessRules>().AsSelf().InstancePerLifetimeScope();

			builder.RegisterType<AuthService>().As<IAuthService>().InstancePerLifetimeScope();
			builder.RegisterType<TokenService>().As<ITokenService>().InstancePerLifetimeScope();
			builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
			builder.RegisterType<RoleService>().As<IRoleService>().InstancePerLifetimeScope();

		}

		//builder.RegisterType<AdminRoleRequirementInterceptor>().InstancePerLifetimeScope();
		//builder.RegisterType<RoleService>().As<IRoleService>()
		//	.EnableInterfaceInterceptors()
		//	.InterceptedBy(typeof(AdminRoleRequirementInterceptor))
		//	.InstancePerLifetimeScope();
	}

}
