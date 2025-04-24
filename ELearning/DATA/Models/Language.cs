using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.Models
{
    public class Language
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public ICollection<AppUser>? LanguagePreferences { get; set; }
        /*
         many to many with user and the common table is language preference
         */
    }
}
