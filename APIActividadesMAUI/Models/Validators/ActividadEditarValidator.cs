﻿using APIActividadesMAUI.Models.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIActividadesMAUI.Models.Validators
{
    public class ActividadEditarValidator:AbstractValidator<Actividad>
    {
        public ActividadEditarValidator()
        {
            RuleFor(x => x.Titulo).MaximumLength(100).WithMessage("El titulo debe tener un tamaño menor a 100 caracteres.");
            RuleFor(x => x.Descripcion).MaximumLength(250).WithMessage("Ingrese una descripción menor a 250 caracteres.");
            RuleFor(x => x.FechaRealizacion).Must(x =>
            {
                var fechaActual = DateTime.Now.Date;
                return x.Value.Year <= fechaActual.Year &&
                       x.Value.Month <= fechaActual.Month &&
                       x.Value.Day <= fechaActual.Day;
            }).WithMessage("La fecha de la actividad debe ser menor a la actual.")
        .When(x => x.FechaRealizacion.HasValue);
        }
    }
}
