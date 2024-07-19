
namespace Ex03.GarageLogic
{
    public sealed class FuelTruck : FuelVehicle
    {
        private bool m_IsTransportHazardousMaterials;
        private float m_CargoVolume;
        private const int k_NumberOfWheels = 12;
        private const int k_MaxAirPressure = 28;

        public FuelTruck()
        {
            MaxAmountOfFuelInLitters = 120f;
            FuelType = eFuelTypes.Soler;
            InitWheelsList(k_NumberOfWheels, k_MaxAirPressure);
        }

        public bool TransportHazardousMaterials
        {
            get { return m_IsTransportHazardousMaterials; }
            set { m_IsTransportHazardousMaterials = value; }
        }

        public float CargoVolume
        {
            get { return m_CargoVolume; }
            set
            {
                if (value < 0)
                {
                    throw new ValueOutOfRangeException();
                }

                m_CargoVolume = value;
            }
        }
    }
}
