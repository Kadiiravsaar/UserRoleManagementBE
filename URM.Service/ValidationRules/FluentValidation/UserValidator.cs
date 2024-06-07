using FluentValidation;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URM.Core.Models;

namespace URM.Service.ValidationRules.FluentValidation
{
	public class UserValidator : AbstractValidator<AppUser>
	{
		public UserValidator()
		{
			RuleFor(x => x.Email).NotEmpty().WithMessage("{PropertyName} gereklidir");
			RuleFor(x => x.UserName).NotEmpty().WithMessage("{PropertyName} gereklidir");
		}
	}
}
