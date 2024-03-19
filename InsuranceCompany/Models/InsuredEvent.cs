using System.ComponentModel.DataAnnotations;

namespace InsuranceCompany.Models
{
    public class InsuredEvent
    {
        public enum EventState
        {
            [Display(Name = "Nahlášeno")]
            Registered,

            [Display(Name = "V šetření")]
            InInvestigation,

            [Display(Name = "Schváleno")]
            Accepted,

            [Display(Name = "Zamítnuto")]
            Rejected
        }

        public int Id { get; set; }

        [Display(Name = "Pojistka")]
        public Insurance Insurance { get; set; }
        public int InsuranceId { get; set; }

        [Display(Name = "Klient")]
        public Client Client { get; set; }
        public int ClientId { get; set; }

        [Display(Name = "Popis")]
        [Required(ErrorMessage = "Zadejte popis pojistné události!")]
        public string Description { get; set; } = "";

        [Display(Name = "Stav")]
        public EventState State { get; set; }
    }
}
