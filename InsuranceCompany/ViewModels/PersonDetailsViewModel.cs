using InsuranceCompany.Models;
using System.ComponentModel.DataAnnotations;
using System;

namespace InsuranceCompany.ViewModels
{
    public class PersonDetailsViewModel
    {
        public Client Client { get; set; }

        public List<Insurance> InsuranceContracts { get; set; }

        public List<Insurance> InsurerContracts { get; set; }

        public List<InsuredEvent> InsuredEvents { get; set; }

        [Display(Name = "Celková měsíční splátka")]
        public decimal InstalmentsSum { get; set; }
    }
}
