using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;

namespace prid_tuto.Models
{
    public class Member : IValidatableObject
    {
        [Key]
        [MinLength(3, ErrorMessage = "Minimum 3 characters")]
        public string Pseudo { get; set; }
        
        [Required(ErrorMessage = "Required")]
        [MinLength(3, ErrorMessage = "Minimum 3 characters")]
        public string Password { get; set; }
        
        [MinLength(3, ErrorMessage = "Minimum 3 characters")]
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


        public bool CheckPseudoUnicity(MsnContext context)
        {
            /*
            Vérifie si, en cas d'ajout d'un membre, il existe déjà un membre ayant le même pseudo que celui qu'on veut créer.

            L'appel à AsNoTracking() permet d'indiquer à EF qu'on veut contourner le contexte en allant lire directement en BD.
            Ceci est nécessaire car, quand la méthode Validate est appelée, l'objet courant se trouve dans le contexte ; il
            permet de lire les données sans utiliser le contexte pour éviter de prendre en compte l'objet courant.
            */
            return context.Entry(this).State == EntityState.Modified || context.Members.AsNoTracking().Count(m => m.Pseudo == Pseudo) == 0;
        }

        public bool CheckFullNameUnicity(MsnContext context)
        {
            if (string.IsNullOrEmpty(FullName))
                return true;
            // Vérifie s'il existe un autre membre ayant le même FullName que celui qu'on veut créer ou mettre à jour.
            return context.Members.Count(m => m.Pseudo != Pseudo && m.FullName == FullName) == 0;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var currContext = validationContext.GetService(typeof(MsnContext)) as MsnContext;
            Debug.Assert(currContext != null);
            if (!CheckPseudoUnicity(currContext))
                yield return new ValidationResult("The Pseudo of a member must be unique", new[] { nameof(Pseudo) });
            if (!CheckFullNameUnicity(currContext))
                yield return new ValidationResult("The FullName of a member must be unique", new[] { nameof(FullName) });
            if (Password.Contains("$"))
                yield return new ValidationResult("The password may not be equal to 'abc'", new[] { nameof(Password) });
            if (BirthDate.HasValue && BirthDate.Value.Date > DateTime.Today)
                yield return new ValidationResult("Can't be born in the future in this reality", new[] { nameof(BirthDate) });
            else if (Age.HasValue && Age < 18)
                yield return new ValidationResult("Must be 18 years old", new[] { nameof(BirthDate) });
        }
    }
}