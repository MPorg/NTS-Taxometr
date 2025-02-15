using AndroidX.Lifecycle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxometrMauiMvvm.Data.CustomElements
{
    public class CustomSwitch : Switch
    {
        public CustomSwitch()
        {
            VisualStateManager.GoToState(this, this.IsToggled ? "On" : "Off");
        }
    }
}
