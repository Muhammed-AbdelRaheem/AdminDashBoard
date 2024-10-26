using System.ComponentModel.DataAnnotations;

namespace Store.DashBoard.Models
{
    public class RoleFormViewModel
    {

        [Required(ErrorMessage ="Name Is Required")]
        [MaxLength(256)]
        public string Name {  get; set; }
    }
}
