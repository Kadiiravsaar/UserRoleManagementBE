using Castle.DynamicProxy;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using URM.Core.Services;

namespace URM.Core.Ultities.Interceptors
{
	//[AttributeUsage(AttributeTargets.Method)]
	//public class AllowAnonymousAttribute : Attribute
	//{
	//}

	public class AdminRoleRequirementInterceptor //: MethodInterception
	{
		//private readonly IHttpContextAccessor _httpContextAccessor;

		//public AdminRoleRequirementInterceptor(IHttpContextAccessor httpContextAccessor)
		//{
		//	_httpContextAccessor = httpContextAccessor;
		//}

		//protected override void OnBefore(IInvocation invocation)
		//{
		//	var method = invocation.MethodInvocationTarget ?? invocation.Method;

		//	// AllowAnonymous attribute kontrolü
		//	if (method.GetCustomAttribute<AllowAnonymousAttribute>() != null)
		//	{
		//		return; // Yönetici kontrolü gerektirmeyen metodlar için geç
		//	}

		//	var user = _httpContextAccessor.HttpContext.User;
		//	if (!user.Identity.IsAuthenticated || !user.IsInRole("Admin"))
		//	{
		//		throw new UnauthorizedAccessException("Bu işlem için yönetici yetkisi gerekmektedir.");
		//	}
		//	// Eğer yönetici ve editör ise, method işlemine devam et
		//	base.OnBefore(invocation);
		//}
	}

}
