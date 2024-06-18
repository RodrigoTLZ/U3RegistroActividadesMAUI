using APIActividadesITESRC.Models.DTOs;
using APIActividadesITESRC.Models.Entities;
using APIActividadesITESRC.Repositories;
using FluentValidation;

namespace APIActividadesITESRC.Models.Validators
{
    public class DepartamentoValidator:AbstractValidator<DepartamentoDTO>
    {
        private readonly ItesrcneActividadesContext context;

        public DepartamentoValidator(ItesrcneActividadesContext context)
        {
            this.context = context;
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("- El nombre del departamento no debe estar vacío.");
            RuleFor(x => x.Nombre).Must((Departamento, Nombre, cancellationToken) => ValidarNombreExistente(Nombre, Departamento.Id)).WithMessage("- Ya se encuentra registrado un departamento con ese nombre.");


            RuleFor(x => x.Username).NotEmpty().WithMessage("- Ingrese el nombre de usuario.");
            RuleFor(x => x.Username).EmailAddress().WithMessage("- Ingrese un correo válido como nombre de usuario.");
            RuleFor(x => x.Username).Must((Departamento, Username, cancellationToken) => ValidarCorreoExistente(Username, Departamento.Id)).WithMessage("El nombre de usuario ya se encuentra registrado.");

            RuleFor(x => x.Password).NotEmpty().WithMessage("- Ingrese una contraseña.");
            RuleFor(x => x.Password).MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");
        }

        private bool ValidarCorreoExistente(string correo, int id)
        {
            if (correo != null)
            {
                var respuesta =context.Departamentos.Any(x => x.Username.ToUpper() == correo.ToUpper() && x.Id == id);
                if (respuesta)
                {
                    return false;
                }
            }
            return true;
        }


        private bool ValidarNombreExistente(string nombre, int id)
        {
            if (nombre != null)
            {
                var respuesta = context.Departamentos.Any(x => x.Nombre.ToUpper() == nombre.ToUpper() && x.Id == id);
                if (respuesta)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
