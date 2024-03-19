using System.ComponentModel.DataAnnotations;

namespace InsuranceCompany.Models
{
    public class Client
    {
        public enum Countries
        {
            [Display(Name = "Česká Republika")]
            CzechRepublic
        }

        public enum Gender
        {
            [Display(Name = "žena")]
            FEMALE,

            [Display(Name = "muž")]
            MALE
        }

        public int Id { get; set; }

        [Display(Name = "Jméno")]
        [Required(ErrorMessage = "Zadejte, prosíme, jméno!")]
        public string FirstName { get; set; } = "";

        [Display(Name = "Příjmení")]
        [Required(ErrorMessage = "Zadejte, prosíme, příjmení!")]
        public string LastName { get; set; } = "";

        [Display(Name = "Rodné číslo")]
        [Required(ErrorMessage = "Zadejte, prosíme, rodné číslo!")]
        public string BirthNumber { get; set; } = "";

        [Display(Name = "Pohlaví")]
        public Gender Sex { get; set; }

        [Display(Name = "Věk")]
        public int Age { get; set; }

        [Display(Name = "Ulice")]
        [Required(ErrorMessage = "Zadejte, prosíme, jméno ulice!")]
        public string Street { get; set; } = "";

        [Display(Name = "Číslo popisné")]
        [Required(ErrorMessage = "Zadejte, prosíme, číslo popisné!")]
        [Range(0, 10000, ErrorMessage = "Zadejte platné číslo popisné!")]
        public int HouseNumber { get; set; }

        [Display(Name = "Město")]
        [Required(ErrorMessage = "Zadejte, prosíme, jméno města!")]
        public string City { get; set; } = "";

        [Display(Name = "Poštovní směrovací číslo")]
        [Required(ErrorMessage = "Zadejte, prosíme, PSČ!")]
        [DataType(DataType.PostalCode)]
        public string PostCode { get; set; } = "";

        [Display(Name = "Země")]
        public Countries Country { get; set; }

        [Display(Name = "Telefon")]
        [Required(ErrorMessage = "Zadejte, prosíme, telefonní číslo!")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = "";

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
        /// <summary>
        /// Získání pohlaví z rodného čísla
        /// </summary>
        public void GetGender()
        {
            Sex = char.GetNumericValue(BirthNumber[2]) >= 5 ? Gender.FEMALE : Gender.MALE;
        }
        /// <summary>
        /// Výpočet věku z rodného čísla
        /// </summary>
        public void GetAge()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            int year = int.Parse(BirthNumber.Substring(0, 2));
            year = (year + 2000 >= DateTime.Now.Year) ? year += 1900 : year += 2000;
            int month = int.Parse(BirthNumber.Substring(2, 2));
            if (month >= 70) month -= 70;
            else if (month >= 50) month -= 50;
            else if (month >= 20) month -= 20;
            int day = int.Parse(BirthNumber.Substring(4, 2));
            DateOnly dayOfBirth = new DateOnly(year, month, day);
            Age = today.DayOfYear >= dayOfBirth.DayOfYear ? today.Year - dayOfBirth.Year : today.Year - dayOfBirth.Year - 1;
        }
        /// <summary>
        /// Kontrola formátu telefonního čísla + převedení do jednotného formátu 123456789
        /// </summary>
        /// <returns>True v případě, že je telefonní číslo po úpravě ve formátu 123456789</returns>
        public bool PhoneNumberCheckAndFormat()
        {
            Phone = new string(Phone.ToCharArray().Where(letter => !Char.IsWhiteSpace(letter)).ToArray());
            bool check = int.TryParse(Phone, out int phoneNumber);
            if (check && Phone.Length == 9)
                return true;
            return false;
        }
        /// <summary>
        /// Kontrola formátu PSČ + převedení do jednotného formátu 12345
        /// </summary>
        /// <returns>True v případě, že je PSČ po úpravě ve formátu 12345</returns>
        public bool PostCodeCheckAndFormat()
        {
            PostCode = new string(PostCode.ToCharArray().Where(letter => !Char.IsWhiteSpace(letter)).ToArray());
            bool check = int.TryParse(PostCode, out int postCode);
            if (check && PostCode.Length == 5)
                return true;
            return false;
        }
        /// <summary>
        /// Odstranění nechtěných mezer za jmény a městem
        /// </summary>
        public void TrimTextInputs()
        {
            FirstName.Trim();
            LastName.Trim();
            City.Trim();
        }

    }
}
