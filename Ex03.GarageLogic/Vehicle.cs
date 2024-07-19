
using System.Collections.Generic;
using System.Text;

namespace Ex03.GarageLogic
{
    public abstract class Vehicle
    {
        private string m_ModelName;
        private string m_LicenseNumber;
        private List<Wheel> m_Wheels = new List<Wheel>();
        protected float m_CurrentEnergyPercentage;

        public enum eColors
        {
            Yellow = 1,
            White,
            Red,
            Black
        }

        public enum eLicenseType
        {
            A = 1,
            A1,
            AA,
            B1
        }

        public string ModelName
        {
            get { return m_ModelName; }
            set { m_ModelName = value; }
        }

        public string LicenseNumber
        {
            get { return m_LicenseNumber; }
            set { m_LicenseNumber = value; }
        }

        public float CurrentEnergyPercentage
        {
            get { return m_CurrentEnergyPercentage; }
        }

        protected void InitWheelsList(int i_NumOfWheels, float i_MaxAirPressure)
        {
            for (int i = 0; i < i_NumOfWheels; i++)
            {
                m_Wheels.Add(new Wheel());
                m_Wheels[i].MaxAirPressure = i_MaxAirPressure;
            }
        }

        public abstract void CalculateCurrentEnergyPercentage();

        public void InitWheels(float i_CurrentWheelsAirPressure, string i_WheelsManufacturerName)
        {
            foreach (Wheel wheel in m_Wheels)
            {
                wheel.CurrentAirPressure = i_CurrentWheelsAirPressure;
                wheel.ManufacturerName = i_WheelsManufacturerName;
            }
        }

        public void FillWheelsAirPressureToMaximum()
        {
            foreach (Wheel wheel in m_Wheels)
            {
                wheel.Inflate(wheel.MaxAirPressure - wheel.CurrentAirPressure);
            }
        }

        public void AddWheelsInfoToVehicleInfo(ref StringBuilder io_VehicleInfo)
        {
            int indexWheels = 1;

            foreach (Wheel wheel in m_Wheels)
            {
                io_VehicleInfo.AppendLine($"Wheel number {indexWheels}: Manufacturer: {wheel.ManufacturerName}, Current Air Pressure: {wheel.CurrentAirPressure}, Max Air Pressure: {wheel.MaxAirPressure}");
                indexWheels++;
            }
        }
    }
}
