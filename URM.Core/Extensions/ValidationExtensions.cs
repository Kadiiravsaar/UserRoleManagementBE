using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URM.Core.Exceptions;

namespace URM.Core.Extensions
{
	public static class ValidationExtensions
	{
		public static void EnsureNotNull<T>(this T obj, string message)
		{
			if (obj == null)
			{
				throw new BusinessException(message);
			}
		}
	}
}
