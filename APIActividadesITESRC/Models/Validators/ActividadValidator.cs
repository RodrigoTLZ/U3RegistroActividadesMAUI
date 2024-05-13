using APIActividadesITESRC.Models.DTOs;
using FluentValidation;

namespace APIActividadesITESRC.Models.Validators
{
    public class ActividadValidator:AbstractValidator<ActividadDTO>
    {
        public ActividadValidator()
        {
            RuleFor(x => x.Titulo).NotEmpty().WithMessage("El titulo no debe estar vacio.");
           // RuleFor(x=>x.FechaRealizacion)
        }
    }
}
