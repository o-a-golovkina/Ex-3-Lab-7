using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Lab_7
{
    public class Patient
    {
        private string name = "Bob";
        private string surname = "Blake";
        private short age = 1;
        private int number = 123456789;
        private PatientType type;
        private int code;

        private short control = 25;

        private static int counter = 0;
        private static short minAge;

        Regex regex1 = new Regex(@"^[a-zA-Z]{3,}$");

        public string Name
        {
            private get => name;
            set
            {
                if (!regex1.IsMatch(value))
                    throw new FormatException("The name must contain only Latin letters and be more than two characters!");
                name = value;
            }
        }

        public string Surname
        {
            private get => surname;
            set
            {
                if (!regex1.IsMatch(value))
                    throw new FormatException("The surname must contain only Latin letters and be more than two characters!");
                surname = value;
            }
        }

        [JsonProperty(PropertyName = "Full name")]
        public string FullName
        {
            get { return $"{Name} {Surname}"; }
        }
        public short Age
        {
            get => age;
            set
            {
                if (!(value >= minAge && value <= 90))
                    throw new FormatException($"Incorrect input! The age can be from {minAge} to 90.");
                age = value;
            }
        }

        [JsonProperty(PropertyName = "Phone number")]
        public int Number
        {
            get => number;
            set
            {
                if (value < 100000000 || value > 999999999)
                    throw new FormatException("Invalid phone number! Try again!");
                number = value;
            }
        }

        [JsonProperty(PropertyName = "Patient type")]
        public PatientType Type
        {
            get => type;
            set => type = value;
        }

        [JsonProperty(PropertyName = "Patient code")]
        public int Code
        {
            get => code;
            set => code = value;
        }

        [JsonIgnore]
        public short Healthlevel { get; set; } = -1;

        [JsonIgnore]
        public static int Counter => counter;

        [JsonIgnore]
        public static short MinAge
        {
            get => minAge;
            set
            {
                if (value < 1 || value >= 90) throw new FormatException("The min age must be above 1 and below 90!");
                minAge = value;
            }
        }

        public Patient()
        {
            counter++;
        }

        public Patient(string name, string surname) : this()
        {
            Name = name;
            Surname = surname;
        }

        public Patient(string name, string surname, short age) : this(name, surname)
        {
            Age = age;
        }

        public Patient(string name, string surname, short age, int number) : this(name, surname, age)
        {
            Number = number;
        }

        public Patient(string name, string surname, short age, int number, PatientType type, int code) : this(name, surname, age, number)
        {
            Type = type;
            Code = code;
        }

        public bool PatientWait(short time)
        {
            if (time < 0)
                return false;
            Healthlevel -= (short)((control + time) / 5);
            if (Healthlevel < 0) Healthlevel = 0;
            return true;
        }

        public bool PatientTakeMedicine(short time)
        {
            if (time < 0)
                return false;
            Healthlevel += (short)((control + time) / 2);
            if (Healthlevel > 100) Healthlevel = 100;
            return true;
        }

        public bool PatientTakeMedicine(short time, int dosage)
        {
            if (time < 0)
                return false;
            if (dosage < 0)
                return false;
            Healthlevel += (short)((control + time + (dosage / 2)) / 2);
            if (Healthlevel > 100) Healthlevel = 100;
            return true;
        }

        public void PatientVisitedDoctor()
        {
            Healthlevel += (short)(control * 2);
            if (Healthlevel > 100) Healthlevel = 100;
        }

        public static void ResetCounter()
        {
            counter = 0;
        }

        public static void CounterMin()
        {
            counter--;
        }

        public static string IsNormalBMI(short weight, double height)
        {
            string res = "";
            double bmi;
            bmi = weight / (height * height);
            if (bmi > 18.5 && bmi < 30)
                res = $"\nBMI = {bmi:f2} - within normal range.";
            else res = $"\nBMI = {bmi:f2} - outside the normal range.";
            return res;
        }

        public override string ToString()
        {
            return $"{Name};{Surname};{Age};{Number};{Type};{Code}";
        }

        public static Patient Parse(string s, List<Patient> patients)
        {
            PatientType t = 0;
            if (s == null) throw new NullReferenceException("The string is empty!");
            string[] parts = s.Split(';');
            if (parts.Length != 6) throw new FormatException("The string in incorrect format!");
            Regex regex1 = new Regex(@"^[a-zA-Z]{3,}$");
            if (parts[0] == "" || (!regex1.IsMatch(parts[0]))) throw new FormatException("The name in incorrect format!");
            if (parts[1] == "" || (!regex1.IsMatch(parts[1]))) throw new FormatException("The surname in incorrect format!");
            if (short.Parse(parts[2]) < minAge) throw new FormatException("The age in incorrect format!");
            if (int.Parse(parts[3]) < 100000000 || int.Parse(parts[3]) > 999999999) throw new FormatException("The number in incorrect format!");
            if (!TypeIsValid(parts[4], ref t)) throw new FormatException("The type in incorrect format!");
            if (!CheckCode(patients, int.Parse(parts[5]))) throw new FormatException("The code in incorrect format!");
            return new Patient(parts[0], parts[1], short.Parse(parts[2]), int.Parse(parts[3]), t, int.Parse(parts[5]));
        }

        public static bool TryParse(string s, out Patient patient, List<Patient> patients)
        {
            bool valid = false;
            patient = null!;

            try
            {
                patient = Parse(s, patients);
                valid = true;
            }
            catch (FormatException ex) { Console.WriteLine(ex.Message); }
            catch (NullReferenceException ex) { Console.WriteLine(ex.Message); }
            catch (Exception ex) { Console.WriteLine($"TryParse: {ex.Message}"); }
            return valid;
        }

        private static bool TypeIsValid(string c, ref PatientType t)
        {
            try
            {
                c = c.ToUpper();
                t = (PatientType)Enum.Parse(typeof(PatientType), c);
                if (!Enum.IsDefined(t)) return false;
            }
            catch (ArgumentException) { return false; }
            return true;
        }

        public static bool CheckCode(List<Patient> p, int c)
        {
            if (c < 100 || c > 999) return false;
            foreach (var el in p)
            {
                if (el == null) return true;
                if (el.Code == c) return false;
            }
            return true;
        }
    }
}
