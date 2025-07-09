using SIGEBI.Application.DTOs.PublishersDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace SIGEBI.Application.Validations
{
    public class PublishersValidation
    {
        public static AbstractValidator<CreationPublisherDto> CreationValidator => new InlineValidator<CreationPublisherDto>
        {
            v => v.RuleFor(x => x.PublisherName)
                .NotEmpty().WithMessage("El nombre de la editorial es obligatorio.")
                .MaximumLength(255).WithMessage("El nombre de la editorial no puede exceder los 255 caracteres."),
            v => v.RuleFor(x => x.Email)
                .EmailAddress().WithMessage("El correo electrónico no es válido.")
                .MaximumLength(255).WithMessage("El correo electrónico no puede exceder los 255 caracteres."),
            v => v.RuleFor(x => x.PhoneNumber)
                .MaximumLength(20).WithMessage("El número de teléfono no puede exceder los 20 caracteres.")
        };

        public static AbstractValidator<UpdatePublisherDto> UpdateValidator => new InlineValidator<UpdatePublisherDto>
        {
            v => v.RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID debe ser mayor a 0."),
            v => v.RuleFor(x => x.PublisherName)
                .NotEmpty().WithMessage("El nombre de la editorial es obligatorio.")
                .MaximumLength(255).WithMessage("El nombre de la editorial no puede exceder los 255 caracteres."),
            v => v.RuleFor(x => x.Email)
                .EmailAddress().WithMessage("El correo electrónico no es válido.")
                .MaximumLength(255).WithMessage("El correo electrónico no puede exceder los 255 caracteres.")
        };

        public static AbstractValidator<RemovePublisherDto> RemoveValidator => new InlineValidator<RemovePublisherDto>
        {
            v => v.RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID debe ser mayor a 0."),
            v => v.RuleFor(x => x.Reason)
                .MaximumLength(500).WithMessage("El motivo de la eliminación no puede exceder los 500 caracteres.")
        };
    }
}
