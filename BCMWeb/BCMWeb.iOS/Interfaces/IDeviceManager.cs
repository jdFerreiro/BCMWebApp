using BCMWeb.Interfaces;
using BCMWeb.Models;
using Plugin.Battery;
using Plugin.DeviceInfo;
using Plugin.DeviceInfo.Abstractions;
using Xamarin.Forms;

[assembly: Dependency(typeof(BCMWeb.Droid.IDeviceManager))]
namespace BCMWeb.Droid
{
    public class IDeviceManager : IIDeviceManager
    {
        public int GetBatteryPercentage()
        {
            int _percentageBattery = CrossBattery.Current.RemainingChargePercent;
            return _percentageBattery;
        }

        public DispositivoModel GetDeviceData()
        {
            IDeviceInfo _device = CrossDeviceInfo.Current;

            DispositivoModel _dispositivo = new DispositivoModel
            {
                Fabricante = _device.Manufacturer,
                Id = 0,
                IdUnicoDispositivo = _device.Id,
                Modelo = _device.Model,
                Nombre = _device.DeviceName,
                Plataforma = _device.Platform.ToString(),
                Tipo = _device.Idiom.ToString(),
                Version = _device.Version,
            };

            return _dispositivo;
        }
    }
}