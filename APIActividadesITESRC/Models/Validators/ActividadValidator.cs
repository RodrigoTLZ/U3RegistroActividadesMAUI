using APIActividadesITESRC.Models.DTOs;
using FluentValidation;

namespace APIActividadesITESRC.Models.Validators
{
    public class ActividadValidator:AbstractValidator<ActividadDTO>
    {
        public ActividadValidator()
        {
            RuleFor(x => x.Titulo).NotEmpty().WithMessage("- El titulo no debe estar vacio.");
            RuleFor(x => x.Titulo).MaximumLength(100).WithMessage("- El titulo debe tener un tamaño menor a 100 caracteres.");

            RuleFor(x => x.IdDepartamento).NotNull().WithMessage("- Debe seleccionar un departamento.");

            RuleFor(x => x.Descripcion).MaximumLength(250).WithMessage("- Ingrese una descripción menor a 250 caracteres.");

            RuleFor(x => x.FechaRealizacion).NotEmpty().WithMessage("- Debe ingresar una fecha de realización para la actividad.");

            RuleFor(x => x.FechaRealizacion).Must(ValidarFecha).WithMessage("- La fecha de realización no debe ser mayor a la fecha actual.");
        }

        private bool ValidarFecha(DateTime? date)
        {
            DateTime currentDate = DateTime.Now.Date;
            if (date.Value.Date <= currentDate.Date)
            {
                return true;
            }
            return false;
        }
    }
}
