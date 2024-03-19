using System;
using System.ComponentModel.DataAnnotations;

namespace InsuranceCompany.Models
{
    public class Insurance
    {
        public enum InsuranceType
        {
            [Display(Name = "Životní pojištění")]
            LifeInsurance,

            [Display(Name = "Trvalé poškození")]
            PermanentDamage
        }

        public int Id { get; set; }

        [Display(Name = "Druh pojištění")]
        public InsuranceType Type { get; set; }

        [Display(Name = "Pojištěnec")]
        public Client InsuredPerson { get; set; }
        public int InsuredPersonId { get; set; }

        [Display(Name = "Pojistitel")]
        public Client Insurer { get; set; }
        public int InsurerId { get; set; }

        [Display(Name = "Pojistná částka")]
        [Required(ErrorMessage = "Zadejte, prosíme, pojistnou částku!")]
        [Range(1000, 1000000, ErrorMessage = "Pojistná částka musí být v rozmezí 1 000 - 1 000 000!")]
        public decimal InsuredAmount { get; set; }

        [Display(Name = "Měsíční splátka")]
        [Required(ErrorMessage = "Zadejte, prosíme, měsíční splátku!")]
        [Range(1000, 50000, ErrorMessage = "Splátka musí být v rozmezí 1 000 - 50 000!")]
        public decimal Instalment { get; set; }

        [Display(Name = "Počátek pojištění")]
        [Required(ErrorMessage = "Zadejte, prosíme, počáteční datum pojištění.")]
        [DataType(DataType.Date)]
        public DateOnly InsuranceStartDate { get; set; }

        [Display(Name = "Konec pojištění")]
        [Required(ErrorMessage = "Zadejte, prosíme, konečné datum pojištění.")]
        [DataType(DataType.Date)]
        public DateOnly InsuranceEndDate { get; set; }

        /// <summary>
        /// Kontrola velikosti splátky oproti celkové splatné sumě
        /// </summary>
        /// <returns></returns>
        public bool IsInstalmentSmaller() => InsuredAmount > Instalment;
    }
}
