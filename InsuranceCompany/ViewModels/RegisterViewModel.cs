using System.ComponentModel.DataAnnotations;

namespace InsuranceCompany.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Vyplňte emailovou adresu")]
        [EmailAddress(ErrorMessage = "Neplatná emailová adresa")]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        [Display(Name = "Rodné číslo")]
        [Required(ErrorMessage = "Zadejte, prosíme, rodné číslo!")]
        public string BirthNumber { get; set; } = "";

        [Required(ErrorMessage = "Vyplňte heslo")]
        [StringLength(100, ErrorMessage = "{0} musí mít délku alespoň {2} a nejvíc {1} znaků.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Heslo")]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Vyplňte heslo")]
        [DataType(DataType.Password)]
        [Display(Name = "Potvrzení hesla")]
        [Compare(nameof(Password), ErrorMessage = "Zadaná hesla se musí shodovat.")]
        public string ConfirmPassword { get; set; } = "";

        /// <summary>
        /// Odstranění lomítka z rodného čísla. Vždy použít před metodami k výpočtu věku a zjištění pohlaví.
        /// </summary>
        public void RepairBirthNumber()
        {
            if (BirthNumber.Contains('/'))
            {
                string repairedBirthNumber = "";
                foreach (char letter in BirthNumber)
                    repairedBirthNumber = letter != '/' ? repairedBirthNumber + letter : repairedBirthNumber;
                BirthNumber = repairedBirthNumber;
            }
        }
        /// <summary>
        /// Validace rodného čísla
        /// </summary>
        /// <returns>True v případě validního rodného čísla</returns>
        public bool CheckBirthNumber()
        {
            bool check = long.TryParse(BirthNumber, out long birthNumber);
            if (check) return birthNumber % 11 == 0;
            return false;
        }
    }
}
