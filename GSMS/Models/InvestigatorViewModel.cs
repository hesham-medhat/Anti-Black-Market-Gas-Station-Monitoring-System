using System.ComponentModel.DataAnnotations;


namespace GSMS.Models
{
    public class InvestigatorViewModel
    {
        public class respondModel
        {
            [Required]
            [Display(Name = "GasStationId")]
            public string GasStationId { get; set; }

            [Required]
            [Display(Name = "Severity")]
            public int Severity { get; set; }
        }
    }
}