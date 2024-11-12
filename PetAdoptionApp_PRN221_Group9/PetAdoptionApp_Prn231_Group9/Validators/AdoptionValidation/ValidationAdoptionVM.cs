using BusinessLogicLayer.ViewModels.AdoptionsDTOs;
using FluentValidation;

namespace PetAdoptionApp_Prn231_Group9.Validators.AdoptionValidation
{
    public class ValidationAdoptionVM : AbstractValidator<AdoptionDTOs>
    {
        public ValidationAdoptionVM()
        {
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.UserEmail).NotEmpty();
            RuleFor(x => x.ContactNumber).NotEmpty().Length(10).WithMessage("This field should not be nullable");
            RuleFor(x => x.AdoptionReason).NotEmpty();
            RuleFor(x => x.AdoptionStatus).NotEmpty();
        }
    }
}
