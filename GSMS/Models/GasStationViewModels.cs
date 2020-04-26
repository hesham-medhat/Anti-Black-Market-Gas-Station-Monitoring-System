using System.ComponentModel.DataAnnotations;

namespace GSMS.Models
{
    public class GasStationViewModels
    {
        public class RefillModel
        {
            [Required]
            [Display(Name = "RefillQuantity")]
            public int RefillQuantity { get; set; }
        }

        public class ServeUserModel
        {
            [Required]
            [Display(Name = "ServeUserQuantity")]
            public int ServeUserQuantity { get; set; }

            [Required]
            [Display(Name = "UserName")]
            public string UserName { get; set; }
        }


    }
}