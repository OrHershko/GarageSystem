using System;
using System.Collections.Generic;
using System.Reflection;
using Ex03.GarageLogic;
using static Ex03.GarageLogic.Garage;

namespace Ex03.ConsoleUI
{
    internal class UserInterface
    {
        private Garage m_Garage = new Garage();
        private VehicleFactory m_VehicleFactory = new VehicleFactory();
        private Dictionary<Type, Action<Vehicle>> m_VehicleDataInputMethods = new Dictionary<Type, Action<Vehicle>>();
        private bool m_IsContinueProgram = true;

        private enum eMenuOptions
        {
            AddNewVehicle = 1,
            DisplayVehiclesList,
            ChangeVehicleStatus,
            InflateVehicleTiresToMaximum,
            RefuelVehicle,
            ChargeElectricVehicle,
            DisplayFullVehicleDetails,
            ExitProgram
        }

        public UserInterface()
        {
            initializeVehicleDataInputMethods();
        }

        private void initializeVehicleDataInputMethods()
        {
            foreach(Type type in m_VehicleFactory.TypesOfVehicle)
            {
                m_VehicleDataInputMethods.Add(type, delegate(Vehicle vehicle)
                {
                    PropertyInfo[] properties = type.GetProperties();

                    foreach(PropertyInfo property in properties)
                    {
                        if(property.CanWrite && property.SetMethod.IsPublic)
                        {
                            object value = property.GetValue(vehicle);

                            if (!isDefaultValue(value))
                            {
                                continue;
                            }

                            while (true)
                            {
                                try
                                {
                                    string addedTextForBool = "";
                                    string addedTextForEnum = "";

                                    if(property.PropertyType == typeof(bool))
                                    {
                                        addedTextForBool = " true/false, does the vehicle";
                                    }

                                    if(property.PropertyType.IsEnum)
                                    {
                                        addedTextForEnum = $" ({string.Join(", ", Enum.GetNames(property.PropertyType))})";
                                    }

                                    Console.WriteLine($"Please enter{addedTextForBool} {SplitCamelCase(property.Name).ToLower()}{addedTextForEnum}:");
                                    string userInput = Console.ReadLine();
                                    value = convertToPropertyType(userInput, property.PropertyType);
                                    property.SetValue(vehicle, value);

                                    break;
                                }
                                catch(TargetInvocationException i_InvocationException) when (i_InvocationException.InnerException != null)
                                {
                                    Console.WriteLine($"Invalid input: {i_InvocationException.InnerException.Message}");
                                }
                                catch(FormatException i_FormatException)
                                {
                                    Console.WriteLine($"Invalid input format: {i_FormatException.Message}");
                                }
                                catch (Exception i_GeneralException)
                                {
                                    Console.WriteLine($"Invalid input: {i_GeneralException.Message}");
                                }
                            }
                        }
                    }
                });
            }
        }

        private object convertToPropertyType(string i_UserInput, Type i_PropertyType)
        {
            object result = null;

            try
            {
                if(i_PropertyType == typeof(int))
                {
                    result = int.Parse(i_UserInput);
                }

                if(i_PropertyType == typeof(float))
                {
                    result = float.Parse(i_UserInput);
                }

                if(i_PropertyType == typeof(double))
                {
                    result = double.Parse(i_UserInput);
                }

                if(i_PropertyType == typeof(bool))
                {
                    result = bool.Parse(i_UserInput);
                }

                if(i_PropertyType.IsEnum)
                {
                    if(int.TryParse(i_UserInput, out int number))
                    {
                        throw new FormatException();
                    }

                    result = Enum.Parse(i_PropertyType, i_UserInput, true);
                }

                if(i_PropertyType == typeof(string))
                {
                    result = i_UserInput;
                }
            }
            catch(Exception i_GeneralException)
            {
                throw new FormatException($"{i_GeneralException.Message} Please enter a valid input.", i_GeneralException);
            }

            return result;
        }

        private bool isDefaultValue(object i_ValueToCheck)
        {
            bool isDefaultValue = false;

            if (i_ValueToCheck == null)
            {
                isDefaultValue = true;
            }
            else if(i_ValueToCheck.GetType().IsValueType)
            {
                isDefaultValue = Activator.CreateInstance(i_ValueToCheck.GetType()).Equals(i_ValueToCheck);
            }

            return isDefaultValue;
        }

        public void RunGarage()
        {
            while(m_IsContinueProgram)
            {
                Console.Clear();
                PrintMenu();
                eMenuOptions userMenuInput = getValidMenuInput();
                preformSelectedMenuOption(userMenuInput);
            }
        }

        public void PrintMenu()
        {
            Console.WriteLine("Welcome to the garage!");
            Console.WriteLine("Please enter a number between 1 to 7 to choose from the options, or Q if you wish to close the program:");
            Console.WriteLine("1. Add New Vehicle");
            Console.WriteLine("2. Display Vehicles List");
            Console.WriteLine("3. Change Vehicle Status");
            Console.WriteLine("4. Inflate Vehicle Tires To Maximum");
            Console.WriteLine("5. Refuel Vehicle");
            Console.WriteLine("6. Charge Electric Vehicle");
            Console.WriteLine("7. Display Full Vehicle Details");
        }

        private eMenuOptions getValidMenuInput()
        {
            eMenuOptions userChoice;

            while (true)
            {
                string userInput = Console.ReadLine();

                if(userInput.ToLower() == "q")
                {
                    m_IsContinueProgram = false;
                    userChoice = eMenuOptions.ExitProgram;
                    break;
                }

                if(int.TryParse(userInput, out int numberInput))
                {
                    if (numberInput >= 1 && numberInput <= 7)
                    {
                        userChoice = (eMenuOptions)numberInput;
                        break;
                    }
                }

                Console.WriteLine("Invalid input, must be a number between 1 to 7 or Q to exit.");
            }
            
            return userChoice;
        }

        private void preformSelectedMenuOption(eMenuOptions i_UserMenuInput)
        {
            switch(i_UserMenuInput)
            {
                case eMenuOptions.AddNewVehicle:
                    addNewVehicle();
                    break;
                case eMenuOptions.DisplayVehiclesList:
                    displayVehiclesList();
                    break;
                case eMenuOptions.ChangeVehicleStatus:
                    changeVehicleStatus();
                    break;
                case eMenuOptions.InflateVehicleTiresToMaximum:
                    inflateVehicleWheelsToMaximum();
                    break;
                case eMenuOptions.RefuelVehicle:
                    refuelVehicle();
                    break;
                case eMenuOptions.ChargeElectricVehicle:
                    chargeElectricVehicle();
                    break;
                case eMenuOptions.DisplayFullVehicleDetails:
                    displayFullVehicleDetails();
                    break;
                case eMenuOptions.ExitProgram:
                    break;
            }
        }

        private void addNewVehicle()
        {
            Console.WriteLine("Please enter a license number:");
            string licenceNumber = Console.ReadLine();

            if (m_Garage.IsVehicleInGarage(licenceNumber))
            {
                vehicleExistInGarage(licenceNumber);
            }
            else
            {
                addVehicleToGarage(licenceNumber);
                Console.WriteLine("Vehicle added to the garage successfully.");
            }

            printEndOfActionMessage();
        }

        private void vehicleExistInGarage(string i_LicenceNumber)
        {
            Console.WriteLine("Vehicle already exist in the garage,");
            Console.WriteLine("Transfer to In-repair state.");

            m_Garage.ChangeStateToExistingVehicle(Garage.eStatesInGarage.InRepair, i_LicenceNumber);
        }

        private void addVehicleToGarage(string i_LicenceNumber)
        {
            Type[] typesOfVehicles = m_VehicleFactory.TypesOfVehicle;
            int optionIndex = 1;

            Console.WriteLine("Please choose a vehicle type:");

            foreach (Type type in typesOfVehicles)
            {
                Console.WriteLine($"{optionIndex}. {SplitCamelCase(type.Name)}");
                optionIndex++;
            }

            int userInput = getValidUserVehicleInput(typesOfVehicles.Length);
            Vehicle vehicle = createVehicleWithDataFromUser(userInput, i_LicenceNumber);
            getAdministrativeInfo(out string o_OwnerName, out string o_PhoneNumber);
            m_Garage.AddVehicleToGarage(vehicle, o_OwnerName, o_PhoneNumber);
        }

        private void getAdministrativeInfo(out string o_OwnerName, out string o_PhoneNumber)
        {
            Console.WriteLine("Please enter the owner name: ");
            o_OwnerName = Console.ReadLine();
            Console.WriteLine("Please enter a phone number: ");
            o_PhoneNumber = Console.ReadLine();
        }

        private int getValidUserVehicleInput(int i_NumberOfOptions)
        {
            int numberInput;

            while (true)
            {
                string userInput = Console.ReadLine();

                if (int.TryParse(userInput, out numberInput))
                {
                    if (numberInput >= 1 && numberInput <= i_NumberOfOptions)
                    {
                        break;
                    }
                }

                Console.WriteLine($"Invalid input, must be a number between 1 and {i_NumberOfOptions}");
            }

            return numberInput;
        }

        private Vehicle createVehicleWithDataFromUser(int i_UserInput, string i_LicenceNumber)
        {
            Vehicle vehicle = m_VehicleFactory.CreateVehicle(i_UserInput);

            if (m_VehicleDataInputMethods.ContainsKey(vehicle.GetType()))
            {
                vehicle.LicenseNumber = i_LicenceNumber;
                m_VehicleDataInputMethods[vehicle.GetType()](vehicle);
                setVehicleWheelsData(ref vehicle);
                vehicle.CalculateCurrentEnergyPercentage();
            }

            return vehicle;
        }

        private void setVehicleWheelsData(ref Vehicle io_Vehicle)
        {
            while(true)
            {
                try
                {
                    Console.WriteLine("Please enter current wheels air pressure:");
                    string currentWheelsAirPressureInput = Console.ReadLine();
                    Console.WriteLine("Please enter wheels manufacturer name:");
                    string wheelsManufacturerNameInput = Console.ReadLine();
                    io_Vehicle.InitWheels(float.Parse(currentWheelsAirPressureInput), wheelsManufacturerNameInput);

                    break;
                }
                catch(FormatException i_FormatException)
                {
                    Console.WriteLine($"Invalid input. {i_FormatException.Message}");
                }
                catch(ValueOutOfRangeException i_outOfRangeException)
                {
                    Console.WriteLine($"Invalid wheels air pressure input. {i_outOfRangeException.Message}");
                }
                catch (Exception i_GeneralException)
                {
                    Console.WriteLine($"Invalid input. {i_GeneralException.Message}");
                }
            }
        }

        private void displayVehiclesList()
        {
            printVehiclesStatesOptions();
            eStatesInGarage userInputState = getValidEnumFromUser<eStatesInGarage>();
            preformSelectedDisplayOption(userInputState);
            printEndOfActionMessage();
        }

        private void preformSelectedDisplayOption(eStatesInGarage i_UserStateChoice)
        {
            List<string> vehiclesInState = new List<string>();
            string stateMessage = "";
            int printIndex = 1;

            switch (i_UserStateChoice)
            {
                case Garage.eStatesInGarage.InRepair:
                    vehiclesInState = m_Garage.GetVehiclesByState(Garage.eStatesInGarage.InRepair);
                    stateMessage = "In-repair";
                    break;
                case Garage.eStatesInGarage.Repaired:
                    vehiclesInState = m_Garage.GetVehiclesByState(Garage.eStatesInGarage.Repaired);
                    stateMessage = "Repaired";
                    break;
                case Garage.eStatesInGarage.Paid:
                    vehiclesInState = m_Garage.GetVehiclesByState(Garage.eStatesInGarage.Paid);
                    stateMessage = "Paid";
                    break;
            }

            if(vehiclesInState.Count == 0)
            {
                Console.WriteLine($"There are no vehicles in the garage in state: {stateMessage}.");
            }
            else
            {
                Console.WriteLine($"These are the license numbers of the vehicles in state: {stateMessage}");
                
                foreach (string licenseNumber in vehiclesInState)
                {
                    Console.WriteLine($"{printIndex}. {licenseNumber}");
                    printIndex++;
                }
            }
        }

        private void printVehiclesStatesOptions()
        {
            Console.WriteLine("Please enter a number between 1 and 3 to select the state in the garage:");
            Console.WriteLine("1. In-repair");
            Console.WriteLine("2. Repaired");
            Console.WriteLine("3. Paid");
        }

        private void changeVehicleStatus()
        {
            Console.WriteLine("Please enter the license number of the vehicle you want to change its status:");
            string inputLicenseNumber = Console.ReadLine();
            printVehiclesStatesOptions();
            eStatesInGarage userInputState = getValidEnumFromUser<eStatesInGarage>();

            if (m_Garage.IsVehicleInGarage(inputLicenseNumber))
            {
                m_Garage.ChangeStateToExistingVehicle(userInputState, inputLicenseNumber);
                Console.WriteLine($"The status of the vehicle with license number: {inputLicenseNumber} has been changed successfully.");
            }
            else
            {
                Console.WriteLine($"license number: {inputLicenseNumber} does not exist in the system.");
            }

            printEndOfActionMessage();
        }

        private void inflateVehicleWheelsToMaximum()
        {
            Console.WriteLine("Please enter the license number of the vehicle whose tires you want to inflate to the maximum:");
            string inputLicenseNumber = Console.ReadLine();

            if(m_Garage.IsVehicleInGarage(inputLicenseNumber))
            {
                m_Garage.FillWheelsAirPressureToMaximum(inputLicenseNumber);
                Console.WriteLine($"The wheels of the vehicle with license number: {inputLicenseNumber} has been filled to the maximum.");
            }
            else
            {
                Console.WriteLine($"license number: {inputLicenseNumber} does not exist in the system.");
            }

            printEndOfActionMessage();
        }

        private void printEndOfActionMessage()
        {
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
        }

        private void refuelVehicle()
        {
            string inputLicenseNumber = null;

            while (true)
            {
                try
                {
                    if(inputLicenseNumber == null)
                    {
                        Console.WriteLine("Please enter the license number of the vehicle to refuel:");
                        inputLicenseNumber = Console.ReadLine();
                    }
                    
                    if(m_Garage.IsVehicleInGarage(inputLicenseNumber))
                    {
                        if(m_Garage.IsVehicleRunOnFuel(inputLicenseNumber))
                        {
                            getFuelingDetails(inputLicenseNumber);
                        }
                        else
                        {
                            Console.WriteLine($"The vehicle with license number: {inputLicenseNumber} doesn't run on fuel.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"license number: {inputLicenseNumber} does not exist in the system.");
                    }

                    break;
                }
                catch(ValueOutOfRangeException i_OutOfRangeException)
                {
                    Console.WriteLine($"Invalid fuel amount: {i_OutOfRangeException.Message}");
                }
                catch(ArgumentException i_ArgumentException)
                {
                    Console.WriteLine($"Invalid input: {i_ArgumentException.Message}");
                }
                catch(FormatException i_FormatException)
                {
                    Console.WriteLine($"Invalid input: amount of fuel should be a number.");
                }
                catch(Exception i_Exception)
                {
                    Console.WriteLine($"Invalid input: {i_Exception.Message}");
                }
            }

            printEndOfActionMessage();
        }

        private void printVehiclesFuelOptions()
        {
            Console.WriteLine("Please enter a number between 1 and 4 to select the fuel type:");
            Console.WriteLine("1. Soler");
            Console.WriteLine("2. Octan95");
            Console.WriteLine("3. Octan96");
            Console.WriteLine("4. Octan98");
        }

        private T getValidEnumFromUser<T>() where T : Enum
        {
            int numberResult;
            Array enumValues = Enum.GetValues(typeof(T));

            while (true)
            {
                string userInput = Console.ReadLine();

                if (int.TryParse(userInput, out numberResult) && Enum.IsDefined(typeof(T), numberResult))
                {
                    break;
                }
                
                Console.WriteLine("Invalid input. Please enter a valid number corresponding to one of the options.");
            }

            return (T)Enum.ToObject(typeof(T), numberResult);
        }

        private void getFuelingDetails(string i_LicenseNumber)
        {
            printVehiclesFuelOptions();
            FuelVehicle.eFuelTypes inputFuelType = getValidEnumFromUser<FuelVehicle.eFuelTypes>();
            Console.WriteLine("Please enter the amount of fuel:");
            string inputAmountOfFuel = Console.ReadLine();
            m_Garage.FillVehicleWithEnergy(i_LicenseNumber, float.Parse(inputAmountOfFuel), inputFuelType);
            Console.WriteLine($"The vehicle with license number: {i_LicenseNumber} has been refueled.");
        }

        private void chargeElectricVehicle()
        {
            string inputLicenseNumber = null;

            while (true)
            {
                try
                {
                    if (inputLicenseNumber == null)
                    {
                        Console.WriteLine("Please enter the license number of the vehicle to charge:");
                        inputLicenseNumber = Console.ReadLine();
                    }

                    if (m_Garage.IsVehicleInGarage(inputLicenseNumber))
                    {
                        if (!m_Garage.IsVehicleRunOnFuel(inputLicenseNumber))
                        {
                            getChargingDetails(inputLicenseNumber);
                        }
                        else
                        {
                            Console.WriteLine($"The vehicle with license number: {inputLicenseNumber} doesn't run on electricity.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"license number: {inputLicenseNumber} does not exist in the system.");
                    }

                    break;
                }
                catch (ValueOutOfRangeException i_OutOfRangeException)
                {
                    Console.WriteLine($"Invalid charging time: {i_OutOfRangeException.Message}");
                }
                catch (FormatException i_FormatException)
                {
                    Console.WriteLine($"Invalid input: charging time should be a number.");
                }
                catch (Exception i_Exception)
                {
                    Console.WriteLine($"Invalid input: {i_Exception.Message}");
                }
            }

            printEndOfActionMessage();
        }

        private void getChargingDetails(string i_LicenseNumber)
        {
            Console.WriteLine("Please enter the charging time in minutes:");
            string inputChargingTimeInMinutes = Console.ReadLine();
            m_Garage.FillVehicleWithEnergy(i_LicenseNumber, float.Parse(inputChargingTimeInMinutes));
            Console.WriteLine($"The vehicle with license number: {i_LicenseNumber} has been charged.");
        }

        private void displayFullVehicleDetails()
        {
            Console.WriteLine("Please enter the license number of the vehicle for which you would like to know full details:");
            string inputLicenseNumber = Console.ReadLine();

            if (m_Garage.IsVehicleInGarage(inputLicenseNumber))
            {
                string fullDetails = m_Garage.GetFullDetailsOfVehicle(inputLicenseNumber);
                Console.WriteLine(fullDetails);
            }
            else
            {
                Console.WriteLine($"license number: {inputLicenseNumber} does not exist in the system.");
            }

            printEndOfActionMessage();
        }
    }
}
