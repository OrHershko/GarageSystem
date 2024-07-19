
namespace Ex03.GarageLogic
{
    public sealed class FuelCar : FuelVehicle
    {
        private eColors m_Color;
        private int m_NumberOfDoors;
        private const int k_NumberOfWheels = 5;
        private const int k_MaxAirPressure = 31;

        public FuelCar()
        {
            MaxAmountOfFuelInLitters = 45f;
            FuelType = eFuelTypes.Octan95;
            InitWheelsList(k_NumberOfWheels, k_MaxAirPressure);
        }

        public eColors Color
        {
            get { return m_Color; }
            set { m_Color = value; }
        }

        public int NumberOfDoors
        {
            get { return m_NumberOfDoors; }
            set 
            {
                if (value >= 2 && value <= 5)
                {
                    m_NumberOfDoors = value;
                }
                else
                {
                    throw new ValueOutOfRangeException(2, 5);
                }
            }
        }
    }
}
