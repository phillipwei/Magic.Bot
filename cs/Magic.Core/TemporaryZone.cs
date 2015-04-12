using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    public class TemporaryZone<T> : Zone<T> where T : Object, IDuplicatable
    {
    }
}
