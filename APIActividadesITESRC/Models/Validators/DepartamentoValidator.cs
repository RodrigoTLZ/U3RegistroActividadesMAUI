using APIActividadesITESRC.Models.DTOs;
using FluentValidation;

namespace APIActividadesITESRC.Models.Validators
{
    public class DepartamentoValidator:AbstractValidator<DepartamentoDTO>
    {
        public DepartamentoValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre del departamento no debe estar vacío.");
        }
    }
}
