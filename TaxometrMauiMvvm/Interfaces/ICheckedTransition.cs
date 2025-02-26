using System;
using System.Collections.Generic;
using System.Text;

namespace TaxometrMauiMvvm.Interfaces
{
    internal interface ICheckedTransition
    {
        event Action<Type> TryTransit;
    }
}
