using APIActividadesMAUI.Models.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIActividadesMAUI.Models.Validators
{
    public class DepartamentoValidator:AbstractValidator<DepartamentoDTO>
    {
        public DepartamentoValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre del departamento no debe estar vacío.");
           // RuleFor(x => x.Nombre).Must(Comparar).WithMessage("Ya se encuentra registrado un departamento con ese nombre.");

            RuleFor(x => x.Username).NotEmpty().WithMessage("Ingrese el nombre de usuario.");
            RuleFor(x => x.Username).EmailAddress().WithMessage("Ingrese un correo válido.");
            //RuleFor(x => x.Username).Must(Comparar).WithMessage("El usuario ya se encuentra registrado.");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Ingrese una contraseña.");
            RuleFor(x => x.Password).MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");
        }
    }
}
