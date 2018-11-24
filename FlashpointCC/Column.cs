using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashpointCurator
{
    class Column : Attribute
    {
        public bool HideByDefault { get; }

        public Column(bool hideByDefault)
        {
            HideByDefault = hideByDefault;
        }
    }
}
