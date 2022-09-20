using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prid_tuto.Models
{
    public class User : IValidatableObject
    {

        [Key]
        public int? UserId { get; set; }
        
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"[A-Z][a-zA-Z '_' \d ]{1,10}" , ErrorMessage = "Pseudo must begin by UpperCase Letter + alphanumeric and underscore '_' ")]
        [MinLength(3, ErrorMessage = "Minimum 3 characters") , MaxLength(10 , ErrorMessage = "Max 10 characters")]     
        public string Pseudo { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"[^$+=]{1,10}", ErrorMessage = "Don't use these special caracter ' $+= ' ")]
        [MinLength(3, ErrorMessage = "Minimum 3 characters") , MaxLength(10, ErrorMessage = "Max 10 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Required")]
        [EmailAddress]
        [MinLength(3, ErrorMessage = "Minimum 3 characters")]
        public string Email { get; set; }

        //[MinLength(3, ErrorMessage = "Minimum 3 characters")]
        
        public string FisrtName { get; set; }

        //[MinLength(3, ErrorMessage = "Minimum 3 characters")]
        public string FullName { get; set; }

        public DateTime? BirthDate { get; set; }

        public int? Age
        {
            get
            {
                if (!BirthDate.HasValue)
                    return null;
                var today = DateTime.Today;
                var age = today.Year - BirthDate.Value.Year;
                if (BirthDate.Value.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var currContext = validationContext.GetService(typeof(MsnContext)) as MsnContext;

            if (FisrtName != null && FullName.Length == 0)
                yield return new ValidationResult("The FullName of a user must be not empty", new[] { nameof(FullName) });
            else if (FullName != null && FisrtName.Length == 0)
                yield return new ValidationResult("The FisrtName of a user must be not empty", new[] { nameof(FisrtName) });
            else if (FullName.Length < 2 || FisrtName.Length < 2)
                yield return new ValidationResult("Minimum 3 characters", new[] { nameof(FullName) });


        }
    }
}