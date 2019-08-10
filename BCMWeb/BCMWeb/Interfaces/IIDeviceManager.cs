using BCMWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMWeb.Interfaces
{
    public interface IIDeviceManager
    {
        DispositivoModel GetDeviceData();
        int GetBatteryPercentage();
    }
}
