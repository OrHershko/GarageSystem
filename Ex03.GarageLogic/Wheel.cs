
namespace Ex03.GarageLogic
{
    public class Wheel
    {
        private string m_ManufacturerName;
        private float m_CurrentAirPressure;
        private float m_MaxAirPressure;

        public string ManufacturerName
        {
            get { return m_ManufacturerName;}
            set { m_ManufacturerName = value; }
        }

        public float CurrentAirPressure
        {
            get { return m_CurrentAirPressure; }
            set
            {
                if (value >= 0 && value <= MaxAirPressure)
                {
                    m_CurrentAirPressure = value;
                }
                else
                {
                    throw new ValueOutOfRangeException(0, MaxAirPressure);
                }
            }
        }

        public float MaxAirPressure
        {
            get { return m_MaxAirPressure; }
            set { m_MaxAirPressure = value; }
        }
        public void Inflate(float i_AirToAdd)
        {
            while(true)
            {
                if (i_AirToAdd + m_CurrentAirPressure > m_MaxAirPressure)
                {
                    throw new ValueOutOfRangeException(0, m_MaxAirPressure - m_CurrentAirPressure);
                }

                m_CurrentAirPressure += i_AirToAdd;
                break;
            }
        }
    }
}
