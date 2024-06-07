using FluentValidation;
using URM.Core.Models;

namespace URM.Service.ValidationRules.FluentValidation
{
	public class RoleValidator : AbstractValidator<AppRole>
	{
		public RoleValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName} gereklidir");
		}
	}
}
