
using System;

namespace Ex03.GarageLogic
{
    public abstract class FuelVehicle : Vehicle
    {
        private eFuelTypes m_FuelType;
        private float m_CurrentAmountOfFuelInLitters;
        private float m_MaxAmountOfFuelInLitters;

        public enum eFuelTypes
        {
            Soler = 1,
            Octan95,
            Octan96,
            Octan98
        }

        public eFuelTypes FuelType
        {
            get { return m_FuelType; }
            set { m_FuelType = value; }
        }

        public float CurrentAmountOfFuelInLitters
        {
            get { return m_CurrentAmountOfFuelInLitters; }
            set 
            {
                if (value >= 0 && value <= m_MaxAmountOfFuelInLitters)
                {
                    m_CurrentAmountOfFuelInLitters = value;
                }
                else
                {
                    throw new ValueOutOfRangeException(0, m_MaxAmountOfFuelInLitters);
                }
            }
        }

        public float MaxAmountOfFuelInLitters
        {
            get { return m_MaxAmountOfFuelInLitters; }
            set { m_MaxAmountOfFuelInLitters = value; }
        }

        public void FillTankWithFuel(float i_FuelToAddInLitters, eFuelTypes i_FuelType)
        {
            if (i_FuelType != m_FuelType)
            {
                throw new ArgumentException($"Wrong fuel type, must be {m_FuelType}");
            }

            if (i_FuelToAddInLitters + m_CurrentAmountOfFuelInLitters > m_MaxAmountOfFuelInLitters)
            {
                throw new ValueOutOfRangeException(0, m_MaxAmountOfFuelInLitters - m_CurrentAmountOfFuelInLitters);
            }

            m_CurrentAmountOfFuelInLitters += i_FuelToAddInLitters;
        }

        public override void CalculateCurrentEnergyPercentage()
        {
            m_CurrentEnergyPercentage = (m_CurrentAmountOfFuelInLitters / m_MaxAmountOfFuelInLitters) * 100;
        }
    }
}
