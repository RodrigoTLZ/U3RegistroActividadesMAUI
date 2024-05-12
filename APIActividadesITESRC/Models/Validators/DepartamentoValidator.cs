using APIActividadesITESRC.Models.DTOs;
using APIActividadesITESRC.Models.Entities;
using APIActividadesITESRC.Repositories;
using FluentValidation;

namespace APIActividadesITESRC.Models.Validators
{
    public class DepartamentoValidator:AbstractValidator<DepartamentoDTO>
    {
        DepartamentosRepository repo { get;}

        public DepartamentoValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre del departamento no debe estar vacío.");
        //    RuleFor(x => x.Nombre).Must(CompararNombre).WithMessage("Ese nombre ya se encuentra registrado.");

            RuleFor(x => x.Username).NotEmpty().WithMessage("Ingrese el nombre de usuario.");
            RuleFor(x => x.Username).EmailAddress().WithMessage("Ingrese un correo válido.");
       //     RuleFor(x => x.Username).Must(CompararUser).WithMessage("Ese usuario ya se encuentra registrado");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Ingrese una contraseña.");
            RuleFor(x => x.Password).MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");

            //Validar ID SUPERIOR


        }

        private bool CompararNombre(string texto)
        {
            return repo.CompararNombre(texto);
        }

        private bool CompararUser(string texto)
        {
            return repo.CompararUser(texto);
        }
    }
}
