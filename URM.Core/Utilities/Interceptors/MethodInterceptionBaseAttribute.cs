﻿using Castle.DynamicProxy;
using Microsoft.EntityFrameworkCore.Diagnostics;
using IInterceptor = Castle.DynamicProxy.IInterceptor;

namespace URM.Core.Ultities.Interceptors
{
	//[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public abstract class MethodInterceptionBaseAttribute //: Attribute, IInterceptor
	{
		//public int Priority { get; set; }

		//public virtual void Intercept(IInvocation invocation)
		//{

		//}
	}

}
