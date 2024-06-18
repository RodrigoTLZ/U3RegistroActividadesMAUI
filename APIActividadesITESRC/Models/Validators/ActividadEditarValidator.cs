using APIActividadesITESRC.Models.Entities;
using FluentValidation;

namespace APIActividadesITESRC.Models.Validators
{
    public class ActividadEditarValidator:AbstractValidator<Actividades>
    {
        public ActividadEditarValidator()
        {
            RuleFor(x => x.Titulo).MaximumLength(100).WithMessage("- El titulo debe tener un tamaño menor a 100 caracteres.");
            RuleFor(x => x.Descripcion).MaximumLength(250).WithMessage("- Ingrese una descripción menor a 250 caracteres.");
            RuleFor(x => x.FechaRealizacion).Must(ValidarFecha).WithMessage("- La fecha de realización no debe ser mayor a la fecha actual.");
        }
        private bool ValidarFecha(DateOnly? date)
        {
            DateOnly current = DateOnly.FromDateTime(DateTime.Now);
            if (date <= current)
            {
                return true;
            }
            return false;
        }
    }
}
