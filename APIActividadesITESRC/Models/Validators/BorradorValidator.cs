using APIActividadesITESRC.Models.DTOs;
using FluentValidation;

namespace APIActividadesITESRC.Models.Validators
{
    public class BorradorValidator:AbstractValidator<ActividadDTO>
    {
        public BorradorValidator()
        {
            RuleFor(x => x.Titulo).NotEmpty().WithMessage("- Debe ingresar como mínimo el título antes de guardar como borrador.");
        }
    }
}
