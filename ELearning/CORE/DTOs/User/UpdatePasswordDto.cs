using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.DTOs.User
{
    public class UpdatePasswordDto
    {
        [Required, DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [Required, DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required, DataType(DataType.Password), Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
    }
}
