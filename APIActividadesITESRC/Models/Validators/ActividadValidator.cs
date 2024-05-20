using APIActividadesITESRC.Models.DTOs;
using FluentValidation;

namespace APIActividadesITESRC.Models.Validators
{
    public class ActividadValidator:AbstractValidator<ActividadDTO>
    {
        public ActividadValidator()
        {
            RuleFor(x => x.Titulo).NotEmpty().WithMessage("El titulo no debe estar vacio.");
            RuleFor(x => x.Titulo).MaximumLength(100).WithMessage("El titulo debe tener un tamaño menor a 100 caracteres.");
            RuleFor(x => x.IdDepartamento).NotNull().WithMessage("Debe seleccionar un departamento.");
            RuleFor(x => x.Descripcion).MaximumLength(250).WithMessage("Ingrese una descripción menor a 250 caracteres.");
            RuleFor(x => x.FechaRealizacion).Must(x =>
            {
                var fechaActual = DateTime.Now;
                return x <= fechaActual;

            }).WithMessage("La fecha de la actividad debe ser menor a la actual");
        }
    }
}
