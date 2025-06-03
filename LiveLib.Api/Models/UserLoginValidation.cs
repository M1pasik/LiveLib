\using FluentValidation;
using LiveLib.Api.Models;

namespace LiveLib.Api.Validators
{

    public class UserLoginValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginValidator()
        {

            RuleFor(u => u.Username)
                .NotEmpty()
                    .WithMessage("Имя пользователя обязательно")
                .MinimumLength(3)
                    .WithMessage("Длина имени пользователя должна быть от 3 символов")
                .MaximumLength(50)
                    .WithMessage("Длина имени пользователя должна быть до 50 символов")

 
            RuleFor(u => u.Password)
                .NotEmpty()
                    .WithMessage("Пароль обязателен")
                .MinimumLength(8)
                    .WithMessage("Длина пароля должна быть от 8 символов")
                .MaximumLength(100)
                    .WithMessage("Длина пароля должна быть до 100 символов")
                .Matches("[A-Z]")
                    .WithMessage("Пароль должен содержать хотя бы одну заглавную букву")
                .Matches("[a-z]")
                    .WithMessage("Пароль должен содержать хотя бы одну строчную букву")
                .Matches("[0-9]")
                    .WithMessage("Пароль должен содержать хотя бы одну цифру")

        }
    }
}