
using System;

namespace Ex03.GarageLogic
{
    public class VehicleFactory
    {
        private Type[] m_TypesOfVehicle = new Type[] { typeof(ElectricMotorcycle), typeof(ElectricCar), typeof(FuelMotorcycle), typeof(FuelCar), typeof(FuelTruck) };

        public Type[] TypesOfVehicle
        {
            get { return m_TypesOfVehicle; }
        }

        public Vehicle CreateVehicle(int i_UserInput)
        {
            Type selectedType = m_TypesOfVehicle[i_UserInput - 1];
            Vehicle userVehicle = (Vehicle)Activator.CreateInstance(selectedType);

            return userVehicle;
        }
    }
}
