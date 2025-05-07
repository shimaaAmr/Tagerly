using FluentValidation;
using Tagerly.Services.Interfaces;

namespace Tagerly.ViewModels.Configurations
{
	public class SignUpViewModelValidator : AbstractValidator<SignUpViewModel>
	{

		private readonly IUserService _userService;
		public SignUpViewModelValidator(IUserService userService)
		{
			_userService = userService;


			RuleFor(x => x.UserName)
					.NotEmpty().WithMessage("Username is required")
					.Length(3, 50).WithMessage("Username must be between 3 and 50 characters");

			RuleFor(x => x.Email)
			   .NotEmpty().WithMessage("Email is required")
			   .EmailAddress().WithMessage("Invalid email format");
			//.MustAsync(async (email, cancellation) => !await _userService.IsEmailTakenAsync(email))
			//.WithMessage("This email is already registered");

			RuleFor(x => x.Phone)
				.NotEmpty().WithMessage("Phone number is required")
				.Matches(@"^\+?\d{10,15}$").WithMessage("Phone number must be between 10 and 15 digits");

			RuleFor(x => x.Address)
				.MaximumLength(100).WithMessage("Address cannot exceed 100 characters");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Password is required")
				.MinimumLength(6).WithMessage("Password must be at least 6 characters long")
				.WithMessage("Password must contain at least one lowercase letter, one uppercase letter, and one number");

			RuleFor(x => x.ConfirmPassword)
				.NotEmpty().WithMessage("Confirm password is required")
				.Equal(x => x.Password).WithMessage("Password and confirm password do not match");

			RuleFor(x => x.Role)
				.NotEmpty().WithMessage("Role is required");
		}
	}
}

