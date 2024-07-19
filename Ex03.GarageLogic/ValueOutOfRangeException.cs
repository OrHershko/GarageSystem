
using System;

namespace Ex03.GarageLogic
{
    public class ValueOutOfRangeException : Exception
    {
        private float m_MaxValue;
        private float m_MinValue;

        public ValueOutOfRangeException(float i_MinValue, float i_MaxValue) : 
            base($"Value is out of range. Should be between {i_MinValue} and {i_MaxValue}.")
        {
            m_MaxValue = i_MaxValue;
            m_MinValue = i_MinValue;
        }

        public ValueOutOfRangeException() :
            base($"Value is out of range. Should be a positive number.")
        { }
    }
}
