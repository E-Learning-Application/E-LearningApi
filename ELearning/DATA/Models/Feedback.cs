using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public int UserId { get; set; }//the user who gave the feedback
        public AppUser User { get; set; }
        public double Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        //the feedback is given on the call, so i should wait for zahran's to develop the call model and then i add the call id here
        //zahran took so long i think we are cooked
        //zahran:now i can rest in peace...... or not
    }
}
