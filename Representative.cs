using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowABillBecomesALaw
{
    public class Representative : Politician
    {
        public Representative(string name) : base("Representative " + name) { }
    }
}
