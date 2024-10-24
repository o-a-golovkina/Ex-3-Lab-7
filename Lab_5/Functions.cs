using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;

namespace Lab_7
{
    public class Functions
    {
        private Random r = new();
        public bool repeat = false;
        public short healthlevel;

        public void AddPatient(int i, ref List<Patient> patients, int n)
        {
            if (i >= n) throw new IndexOutOfRangeException("\nError! You have reached your patient limit.");

            do
            {
                repeat = false;
                Console.Write("\nChoose the mode to add object:\n 1 - Old\n 2 - New\nAnswer --> ");
                try
                {
                    short ans = short.Parse(Console.ReadLine()!);
                    if (ans == 1) OldMode(i, ref patients);
                    else if (ans == 2) NewMode(i, ref patients);
                    else
                    {
                        Console.WriteLine("\nIncorrect input. Try again!");
                        repeat = true;
                    }
                }
                catch (Exception) { Console.WriteLine("\nIncorrect input. Try again!"); repeat = true; }
            } while (repeat);
            Console.WriteLine("\nThe profile has been successfully added!");
        }

        private void OldMode(int i, ref List<Patient> patients)
        {
            int mode = r.Next(1, 5);
            Console.WriteLine("\nPlease wait, mode selection...");
            switch (mode)
            {
                case 1:
                    Console.WriteLine("The mode has been selected! Filling was done through initializers (1).");
                    patients.Add(new Patient() { Name = "Marry", Surname = "Brown", Age = 36, Number = 123456789 });
                    break;
                case 2:
                    Console.WriteLine("The mode has been selected! Name and surname are filled in automatically (2).");
                    patients.Add(new Patient("Anne", "Hathaway"));
                    do
                    {
                        repeat = false;
                        try
                        {
                            Console.Write("\nInput the age of patient --> ");
                            short age = short.Parse(Console.ReadLine()!);
                            patients[i].Age = age;
                        }
                        catch (FormatException ex) { Console.WriteLine(ex.Message); repeat = true; }
                    } while (repeat);
                    do
                    {
                        repeat = false;
                        try
                        {
                            Console.Write("\nInput a phone number (9 digits) --> (+380)");
                            int number = int.Parse(Console.ReadLine()!);
                            patients[i].Number = number;
                        }
                        catch (FormatException ex) { Console.WriteLine(ex.Message); repeat = true; }
                    } while (repeat);
                    break;
                case 3:
                    Console.WriteLine("The mode has been selected! Name, surname and age are filled in automatically (3).");
                    patients.Add(new Patient("Nicholas", "Golitsyn", 29));
                    do
                    {
                        repeat = false;
                        try
                        {
                            Console.Write("\nInput a phone number (9 digits) --> (+380)");
                            int number = int.Parse(Console.ReadLine()!);
                            patients[i].Number = number;
                        }
                        catch (FormatException ex) { Console.WriteLine(ex.Message); repeat = true; }
                    } while (repeat);
                    break;
                case 4:
                    Console.WriteLine("The mode has been selected! Name, surname, age and phone number are filled in automatically (4).");
                    patients.Add(new Patient("Reed", "Scott", 46, 333333333));
                    break;
            }

            do
            {
                repeat = false;
                try
                {
                    Console.WriteLine("\nType of the patient:");
                    foreach (var type in Enum.GetValues(typeof(PatientType)))
                    {
                        int index = (int)type;
                        Console.WriteLine($" {index}) " + type);
                    }
                    PatientType t;
                    do
                    {
                        Console.Write("\nChoose a category --> ");
                        string? c = Console.ReadLine();
                        c = c!.ToUpper();
                        t = (PatientType)Enum.Parse(typeof(PatientType), c);
                        if (!Enum.IsDefined(t)) Console.WriteLine("Incorrect input! Try again!");
                    }
                    while (!Enum.IsDefined(t));
                    patients[i].Type = t;
                }
                catch (FormatException ex) { Console.WriteLine(ex.Message); repeat = true; }
                catch (Exception) { Console.WriteLine("Something went wrong... Try again!"); repeat = true; }
            } while (repeat);

            patients[i].Code = Code(patients);
            Console.WriteLine("\nThe patient's code: " + patients[i].Code);
        }

        private void NewMode(int i, ref List<Patient> patients)
        {
            do
            {
                repeat = false;
                try
                {
                    Console.WriteLine("\n-----------------------------------------------");
                    Console.WriteLine("Type of the patient:");
                    foreach (var type in Enum.GetValues(typeof(PatientType)))
                    {
                        int index = (int)type;
                        Console.WriteLine($" {index}) " + type);
                    }
                    Console.WriteLine("\nCode of the patient must consist of 3 digits.");
                    Console.WriteLine("-----------------------------------------------");
                    Console.Write("\nInput a string with data of patient (name;surname;age;number;type;code) --> ");
                    string? str = Console.ReadLine();
                    bool result = Patient.TryParse(str!, out Patient patient,patients);
                    if (patient == null) throw new ArgumentException();
                    if (result)
                    {
                        patients.Add(patient);
                        Console.WriteLine($"\nResult of adding - {result}: " + patients[i].ToString());
                    }
                }
                catch (ArgumentException) { Console.WriteLine("The patient wasn't added!"); repeat = true; }
            } while (repeat);
        }

        private int Code(List<Patient> p)
        {
            int code;
            do
            {
                code = r.Next(100, 1000);
            } while (!Patient.CheckCode(p, code));
            return code;
        }

        public void OutputPatient(int i, List<Patient> p)
        {
            Console.WriteLine($"\nPatient №{i + 1} \n-----------------------------------");
            Console.WriteLine("Full name: " + p[i].FullName);
            Console.WriteLine("Age: " + p[i].Age);
            Console.WriteLine("Phone number: (+380)" + p[i].Number);
            Console.WriteLine("Type of the patient: " + p[i].Type);
            if (p[i].Healthlevel != -1) Console.WriteLine("The health level: " + p[i].Healthlevel);
            Console.WriteLine("The code: " + p[i].Code);
            Console.WriteLine();
        }

        public void DeletePatient(int i, ref List<Patient> p)
        {
            p.RemoveAt(i);
            Patient.CounterMin();
        }

        public int FindCode(int code, List<Patient> p)
        {
            return p.FindIndex(patient => patient != null && patient.Code == code);
        }

        public int FindPhoneNumber(int number, List<Patient> p)
        {
            return p.FindIndex(patient => patient != null && patient.Number == number);
        }

        public int FindNum(int num, List<Patient> p)
        {
            int index = 0;
            return p.FindIndex(patient => patient != null && ++index == num);
        }

        public void CaseFive(int i, ref List<Patient> p)
        {
            Console.WriteLine("\nYou have selected the \"Patient Status\" option.");
            Console.Write("\nInput the patient's code --> ");
            int code = int.Parse(Console.ReadLine()!);
            i = FindCode(code, p);
            Console.WriteLine();
            if (i == -1) Console.WriteLine("Patient wasn't found...");
            else
            {
                OutputPatient(i, p);
                if (p[i].Healthlevel == -1)
                {
                    Console.Write("Input the health level (0-100) --> ");
                    p[i].Healthlevel = short.Parse(Console.ReadLine()!);
                    if (p[i].Healthlevel < 0 || p[i].Healthlevel > 100)
                    {
                        p[i].Healthlevel = -1;
                        throw new FormatException("Invalid input! Try again!");
                    }
                }
                repeat = true;
                do
                {
                    Console.WriteLine("\nPatient status types:\n 1) Wait\n 2) Take medicine\n 3) !NEW! Take medicine\n 4) Visited a doctor");
                    Console.WriteLine("If you want to exit, write \"Exit\"");
                    Console.Write("Input the patient's status --> ");
                    string? action = Console.ReadLine();
                    short time;
                    bool res;
                    if (action == "Wait" || action == "wait" || action == "WAIT" || action == "1")
                    {
                        Console.Write("Input waiting time in hours --> ");
                        time = short.Parse(Console.ReadLine()!);
                        res = p[i].PatientWait(time);
                        if (res) Console.WriteLine($"\n{p[i].FullName} is waiting {time} hours. The health level: {p[i].Healthlevel}.");
                        else throw new FormatException("\nInvalid time input. Try again!");
                    }
                    else if (action == "Take medicine" || action == "take medicine" || action == "TAKE MEDICINE" || action == "2")
                    {
                        Console.Write("Input time that has passed since taking the medicine in hours --> ");
                        time = short.Parse(Console.ReadLine()!);
                        res = p[i].PatientTakeMedicine(time);
                        if (res) Console.WriteLine($"\n{p[i].FullName} took the medicine. The health level: {p[i].Healthlevel}!");
                        else throw new FormatException("\nInvalid time input. Try again!");
                    }
                    else if (action == "!NEW! Take medicine" || action == "!new! take medicine" || action == "!NEW! TAKE MEDICINE" || action == "3")
                    {
                        Console.Write("Input time that has passed since taking the medicine in hours --> ");
                        time = short.Parse(Console.ReadLine()!);
                        Console.Write("Input dosage in mg --> ");
                        short dosage = short.Parse(Console.ReadLine()!);
                        res = p[i].PatientTakeMedicine(time, dosage);
                        if (res) Console.WriteLine($"\n{p[i].FullName} took the medicine with dosage:{dosage} mg. The health level: {p[i].Healthlevel}!");
                        else throw new FormatException("\nInvalid time or dosage input. Try again!");
                    }
                    else if (action == "Visited a doctor" || action == "visited a doctor" || action == "VISITED A DOCTOR" || action == "4")
                    {
                        p[i].PatientVisitedDoctor();
                        Console.WriteLine($"\n{p[i].FullName} visited a doctor. The health level: {p[i].Healthlevel}!");
                    }
                    else if (action == "Exit" || action == "exit" || action == "EXIT" || action == "0")
                        repeat = false;
                    else Console.WriteLine("\nIncorrect input! Try again!");
                } while (repeat);
                if (p[i].Healthlevel == 100)
                    Console.WriteLine($"\nYour patient {p[i].FullName} has recovered!" +
                        $"\nYou can delete the patient from the list by pressing 4 in menu.");
            }
        }

        public void CaseSix(int i, ref List<Patient> p)
        {
            short weight = 0;
            double height = 0;
            string result = "";

            Console.WriteLine("\nYou have selected the \"Is normal BMI?\" option.");
            Console.Write("\nInput the patient's code --> ");
            int code = int.Parse(Console.ReadLine()!);
            i = FindCode(code, p);
            Console.WriteLine();

            if (i == -1) Console.WriteLine("Patient wasn't found...");
            else
            {
                OutputPatient(i, p);
                do
                {
                    repeat = false;
                    try
                    {
                        Console.Write("\nInput the weight (>25 kg) --> ");
                        weight = short.Parse(Console.ReadLine()!);
                        if (weight < 25) throw new Exception("Incorrect input. Try again!");
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); repeat = true; }
                } while (repeat);
                do
                {
                    repeat = false;
                    try
                    {
                        Console.Write("\nInput the height (>1,0 m) --> ");
                        height = double.Parse(Console.ReadLine()!);
                        if (height < 1 || height > 3) throw new Exception("Incorrect input. Try again!");
                        result = Patient.IsNormalBMI(weight, height);
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); repeat = true; }
                } while (repeat);
                Console.WriteLine(result);
            }
        }

        public void SaveToFileCSV(List<Patient> patients, string path)
        {
            List<string> lines = new List<string>();
            foreach (Patient item in patients)
                lines.Add(item.ToString());
            try
            {
                File.WriteAllLines(path, lines);
                Console.WriteLine($"Check out the CSV file at: {Path.GetFullPath(path)}");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        public void SaveToFileJSON(List<Patient> patients, string path)
        {
            try
            {
                string jsonstring = JsonConvert.SerializeObject(patients);
                File.WriteAllText(path, jsonstring);
                Console.WriteLine($"Check out the JSON file at: {Path.GetFullPath(path)}");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}

