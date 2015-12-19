using System.ComponentModel.DataAnnotations;

namespace Wisher.UserManagment.Models
{
    public class RegisterExternalBindingModel
    {

        [Required]
        public string UserName { get; set; }
        [Required]
        public string Provider { get; set; }
        [Required]
        public string ExternalAccessToken { get; set; }

    }
}