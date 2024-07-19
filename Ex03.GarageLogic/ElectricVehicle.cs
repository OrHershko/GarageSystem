
namespace Ex03.GarageLogic
{
    public abstract class ElectricVehicle : Vehicle
    {
        private float m_RemainingBatteryTimeInHours;
        private float m_MaxBatteryTimeInHours;

        public float RemainingBatteryTimeInHours
        {
            get { return m_RemainingBatteryTimeInHours; }
            set
            {
                if (value >= 0 && value <= MaxBatteryTimeInHours)
                {
                    m_RemainingBatteryTimeInHours = value;
                }
                else
                {
                    throw new ValueOutOfRangeException(0, MaxBatteryTimeInHours);
                }
            }
        }

        public float MaxBatteryTimeInHours
        {
            get { return m_MaxBatteryTimeInHours; }
            set { m_MaxBatteryTimeInHours = value; }
        }

        public void ChargeBattary(float i_HoursToCharge)
        {
            if(i_HoursToCharge + m_RemainingBatteryTimeInHours > m_MaxBatteryTimeInHours)
            {
                float possibleTimeToChargeInMinutes = (m_MaxBatteryTimeInHours - m_RemainingBatteryTimeInHours) * 60;
                throw new ValueOutOfRangeException(0, possibleTimeToChargeInMinutes);
            }

            m_RemainingBatteryTimeInHours += i_HoursToCharge;
        }

        public override void CalculateCurrentEnergyPercentage()
        {
            m_CurrentEnergyPercentage = (m_RemainingBatteryTimeInHours / m_MaxBatteryTimeInHours) * 100;
        }
    }
}
