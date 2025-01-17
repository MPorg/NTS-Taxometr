using System;
using System.Collections.Generic;
using System.Text;

namespace Taxometr.Interfaces
{
    internal interface ICheckedTransition
    {
        event Action<Type> TryTransit;
    }
}
