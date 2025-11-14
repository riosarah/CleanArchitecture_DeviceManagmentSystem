using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Usage : BaseEntity
    {
        public Device? Device { get; set; }
        public int DeviceId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Person? Person { get; set; }
        public int PersonId { get; set; }
    }
}
