
namespace Ex03.GarageLogic
{
    public sealed class FuelMotorcycle : FuelVehicle
    {
        private eLicenseType m_LicenseType;
        private int m_EngineVolumeInCc;
        private const int k_NumberOfWheels = 2;
        private const int k_MaxAirPressure = 33;

        public FuelMotorcycle()
        {
            MaxAmountOfFuelInLitters = 5.5f;
            FuelType = eFuelTypes.Octan98;
            InitWheelsList(k_NumberOfWheels, k_MaxAirPressure);
        }

        public eLicenseType LicenseType
        {
            get { return m_LicenseType; }
            set { m_LicenseType = value; }
        }

        public int EngineVolumeInCc
        {
            get { return m_EngineVolumeInCc; }
            set
            {
                if(value < 0)
                {
                    throw new ValueOutOfRangeException();
                }

                m_EngineVolumeInCc = value;
            }
        }
    }
}