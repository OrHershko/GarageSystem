using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Ex03.GarageLogic
{
    public class Garage
    {
        private Dictionary<string, VehicleInformation> m_VehiclesInGarage = new Dictionary<string, VehicleInformation>();

        public enum eStatesInGarage
        {
            InRepair = 1,
            Repaired,
            Paid
        }

        private class VehicleInformation
        {
            private Vehicle m_Vehicle;
            private string m_OwnerName;
            private string m_OwnerPhoneNumber;
            private eStatesInGarage m_StateInGarage;

            public VehicleInformation(Vehicle i_Vehicle, string i_OwnerName, string i_OwnerPhoneNumber)
            {
                m_Vehicle = i_Vehicle;
                m_OwnerName = i_OwnerName;
                m_OwnerPhoneNumber = i_OwnerPhoneNumber;
                m_StateInGarage = eStatesInGarage.InRepair;
            }

            public eStatesInGarage StateInGarage
            {
                get { return m_StateInGarage; }
                set { m_StateInGarage = value; }
            }

            public Vehicle Vehicle
            {
                get { return m_Vehicle; }
            }

            private void appendPropertiesToStringBuilder(object i_ObjectToPrint, ref StringBuilder io_StringBuilder)
            {
                Type type = i_ObjectToPrint.GetType();
                PropertyInfo[] properties = type.GetProperties();

                foreach(PropertyInfo property in properties)
                {
                    object value = property.GetValue(i_ObjectToPrint);
                    io_StringBuilder.AppendLine($"{SplitCamelCase(property.Name)}: {value}");
                }
            }

            public override string ToString()
            {
                StringBuilder vehicleInfo = new StringBuilder();

                vehicleInfo.AppendLine($"Owner Name: {m_OwnerName}");
                vehicleInfo.AppendLine($"Owner Phone Number: {m_OwnerPhoneNumber}");
                vehicleInfo.AppendLine($"State in Garage: {m_StateInGarage}");
                appendPropertiesToStringBuilder(m_Vehicle, ref vehicleInfo);
                m_Vehicle.AddWheelsInfoToVehicleInfo(ref vehicleInfo);

                return vehicleInfo.ToString();
            }
        }

        public static string SplitCamelCase(string i_InputStr)
        {
            StringBuilder newText = new StringBuilder(i_InputStr.Length * 2);
            newText.Append(i_InputStr[0]);

            for (int i = 1; i < i_InputStr.Length; i++)
            {
                if (char.IsUpper(i_InputStr[i]) && i_InputStr[i - 1] != ' ')
                {
                    newText.Append(' ');
                }

                newText.Append(i_InputStr[i]);
            }

            return newText.ToString();
        }

        public void ChangeStateToExistingVehicle(eStatesInGarage i_NewState, string i_LicenseNumber)
        {
            m_VehiclesInGarage[i_LicenseNumber].StateInGarage = i_NewState;
        }
       
        public bool IsVehicleInGarage(string i_Key)
        {
            return m_VehiclesInGarage.ContainsKey(i_Key);
        }

        public void AddVehicleToGarage(Vehicle i_Vehicle, string i_OwnerName, string i_OwnerPhoneNumber)
        {
            VehicleInformation vehicleInformation = new VehicleInformation(i_Vehicle, i_OwnerName, i_OwnerPhoneNumber);
            m_VehiclesInGarage.Add(i_Vehicle.LicenseNumber, vehicleInformation);
        }

        public List<string> GetVehiclesByState(eStatesInGarage i_ChosenState)
        {
            List<string> vehiclesInState = new List<string>();

            foreach (var vehicleData in m_VehiclesInGarage)
            {
                if (vehicleData.Value.StateInGarage == i_ChosenState)
                {
                    vehiclesInState.Add(vehicleData.Key);
                }
            }

            return vehiclesInState;
        }

        public void FillWheelsAirPressureToMaximum(string i_LicenseNumber)
        {
            m_VehiclesInGarage[i_LicenseNumber].Vehicle.FillWheelsAirPressureToMaximum();
        }

        public bool IsVehicleRunOnFuel(string i_LicenseNumber)
        {
            bool isVehicleRunOnFuel = false;

            if(m_VehiclesInGarage[i_LicenseNumber].Vehicle is FuelVehicle)
            {
                isVehicleRunOnFuel = true;
            }

            return isVehicleRunOnFuel;
        }

        public void FillVehicleWithEnergy(string i_LicenseNumber, float i_EnergyToAdd, FuelVehicle.eFuelTypes i_FuelType = 0)
        {
            if (m_VehiclesInGarage[i_LicenseNumber].Vehicle is ElectricVehicle)
            {
                ElectricVehicle electricVehicle = m_VehiclesInGarage[i_LicenseNumber].Vehicle as ElectricVehicle;
                float chargingTimeInHours = i_EnergyToAdd / 60;
                electricVehicle.ChargeBattary(chargingTimeInHours);
            }
            else if(m_VehiclesInGarage[i_LicenseNumber].Vehicle is FuelVehicle)
            {
                FuelVehicle fuelVehicle = m_VehiclesInGarage[i_LicenseNumber].Vehicle as FuelVehicle;
                fuelVehicle.FillTankWithFuel(i_EnergyToAdd, i_FuelType);
            }

            m_VehiclesInGarage[i_LicenseNumber].Vehicle.CalculateCurrentEnergyPercentage();
        }

        public string GetFullDetailsOfVehicle(string i_LicenseNumber)
        {
            return m_VehiclesInGarage[i_LicenseNumber].ToString();
        }
    }
}