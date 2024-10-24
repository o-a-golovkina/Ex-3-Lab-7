namespace Lab_7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int N = 0;
            int ans = 0;
            bool isValid = false;

            while (!isValid)
            {
                Console.Write("Input the number of patients --> ");
                string? input0 = Console.ReadLine();

                if (int.TryParse(input0, out N) && N > 0)
                    isValid = true;
                else
                    Console.WriteLine("Incorrect input. Please enter a number greater than 0.\n");
            }

            List<Patient> patients = new List<Patient>();
            Functions function = new();;
            bool r;

            do
            {
                r = false;
                Console.Write("\nInput the min age of patients --> ");
                try
                {
                    Patient.MinAge = short.Parse(Console.ReadLine()!);
                }
                catch (FormatException ex) { Console.WriteLine(ex.Message); r = true; }
                catch (Exception ex) { Console.WriteLine(ex.Message); r = true; }
            } while (r);


            do
            {
                bool repeat;
                isValid = false;
                while (!isValid)
                {
                    Console.Write("\nWhat do you want to do?\n" +
                                  " 1 - add object\n" +
                                  " 2 – output objects\n" +
                                  " 3 – find object\n" +
                                  " 4 – delete object\n" +
                                  " 5 – demonstrate behavior\n" +
                                  " 6 – demonstrate static method\n" +
                                  " 7 – Save collection of objects to file\n" +
                                  " 8 – Read collection of objects from file\n" +
                                  " 9 – Clear collection of objects\n" +
                                  " 0 – exit programs\n" +
                                  "Answer --> ");
                    string? input1 = Console.ReadLine();

                    if (int.TryParse(input1, out ans))
                        isValid = true;
                    else
                        Console.WriteLine("\nIncorrect input. Try again!");
                }
                switch (ans)
                {
                    case 0:
                        Environment.Exit(0);
                        break;

                    case 1:
                        try
                        {
                            function.AddPatient(ref patients, N);
                        }
                        catch (IndexOutOfRangeException ex) { Console.WriteLine(ex.Message); }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        break;

                    case 2:
                        if (patients[0] == null)
                            Console.WriteLine("\nThe list is empty!");
                        else
                        {
                            Console.WriteLine($"\nYOUR PATIENTS ({Patient.Counter}):");
                            Console.WriteLine("\nThe min age of patients: " + Patient.MinAge);
                            for (int y = 0; y < Patient.Counter; y++)
                                function.OutputPatient(y, patients);
                        }
                        break;

                    case 3:
                        do
                        {
                            repeat = false;
                            Console.Write("\nChoose mode to find object:\n" + " 1 - Code\n" + " 2 - Phone number" + "\nAnswer --> ");
                            try
                            {
                                int mode = int.Parse(Console.ReadLine()!);
                                int f;
                                if (mode == 1 || mode == 2)
                                {
                                    if (mode == 1)
                                    {
                                        Console.Write("Input code to search --> ");
                                        int search_code = int.Parse(Console.ReadLine()!);
                                        f = function.FindCode(search_code, patients);
                                    }
                                    else
                                    {
                                        Console.Write("Input phone number to search --> (+380)");
                                        int search_phone = int.Parse(Console.ReadLine()!);
                                        f = function.FindPhoneNumber(search_phone, patients);
                                    };
                                    if (f == -1)
                                    {
                                        Console.WriteLine("\nPatient wasn't found...");
                                        break;
                                    }
                                    Console.WriteLine();
                                    function.OutputPatient(f, patients);
                                }
                                else throw new FormatException("Choose only 1 or 2!");
                            }
                            catch (FormatException ex) { Console.WriteLine(ex.Message); repeat = true; }
                            catch (Exception) { Console.WriteLine("Something went wrong... Try again!"); repeat = true; }
                        } while (repeat);
                        break;

                    case 4:
                        do
                        {
                            repeat = false;
                            Console.Write("\nInput the number of patient in the list --> ");
                            try
                            {
                                int num = int.Parse(Console.ReadLine()!);
                                Console.Write("Input the code of patient --> ");
                                int code = int.Parse(Console.ReadLine()!);
                                int f1 = function.FindNum(num, patients);
                                int f2 = function.FindCode(code, patients);
                                Console.WriteLine();
                                if (f1 == -1 || f2 == -1) { Console.WriteLine("Patient wasn't found..."); break; }
                                else function.OutputPatient(f1, patients);
                                Console.Write($"DELETE the patient {patients[f1].FullName} (Yes/No) --> ");
                                string? a = Console.ReadLine();
                                if (a == "Yes" || a == "yes" || a == "YES")
                                {
                                    function.DeletePatient(f1, ref patients);
                                    Console.WriteLine("Patient WAS deleted.");
                                }
                                else if (a == "No" || a == "no" || a == "NO") { Console.WriteLine("Patient WASN'T deleted..."); break; }
                                else
                                {
                                    Console.Write("\nIncorrect input! Try again!\n");
                                    repeat = true;
                                }
                            }
                            catch (FormatException ex) { Console.WriteLine(ex.Message); repeat = true; }
                            catch (Exception) { Console.WriteLine("Something went wrong... Try again!"); repeat = true; }
                        } while (repeat);
                        break;

                    case 5:
                        do
                        {
                            repeat = false;
                            try
                            {
                                function.CaseFive(Patient.Counter, ref patients);
                            }
                            catch (FormatException ex) { Console.WriteLine(ex.Message); repeat = true; }
                            catch (Exception) { Console.WriteLine("Something went wrong... Try again!"); repeat = true; }
                        } while (repeat);
                        break;

                    case 6:
                        do
                        {
                            repeat = false;
                            try
                            {
                                function.CaseSix(Patient.Counter, ref patients);
                            }
                            catch (FormatException ex) { Console.WriteLine(ex.Message); repeat = true; }
                            catch (Exception) { Console.WriteLine("Something went wrong... Try again!"); repeat = true; }
                        } while (repeat);
                        break;

                    case 7:
                        do
                        {
                            repeat = false;
                            try
                            {
                                Console.Write("\nChoose the type of file:\n" +
                                          " 1 - CSV\n" +
                                          " 2 - JSON\n" +
                                          "Answer --> ");
                                ans = int.Parse(Console.ReadLine()!);
                                if (ans == 1)
                                {
                                    Console.Write("Input the file name for saving (*.csv) --> ");
                                    string? pathcsv = Console.ReadLine();
                                    if (!string.IsNullOrEmpty(pathcsv) && pathcsv.Contains(".csv"))
                                        function.SaveToFileCSV(patients, pathcsv);
                                    else throw new Exception();
                                }
                                else if (ans == 2)
                                {
                                    Console.Write("Input the file name for saving (*.json) --> ");
                                    string? pathjson = Console.ReadLine();
                                    if (!string.IsNullOrEmpty(pathjson) && pathjson.Contains(".json"))
                                        function.SaveToFileJSON(patients, pathjson);
                                    else throw new Exception();
                                }                                
                                else
                                    throw new Exception();
                            }
                            catch (Exception) { Console.WriteLine("\nIncorrect input. Try again!"); repeat = true; }
                        } while (repeat);
                        break;

                    case 8:
                        do
                        {
                            repeat = false;
                            try
                            {
                                Console.Write("\nChoose the type of file:\n" +
                                          " 1 - CSV\n" +
                                          " 2 - JSON\n" +
                                          "Answer --> ");
                                ans = int.Parse(Console.ReadLine()!);
                                if (ans == 1)
                                {
                                    Console.Write("Input the file name for reading (*.csv) --> ");
                                    string? pathcsv = Console.ReadLine();
                                    if (!string.IsNullOrEmpty(pathcsv) && pathcsv.Contains(".csv"))
                                    {
                                        patients = function.ReadFromFileCSV(pathcsv);
                                        Console.WriteLine("\nObjects from the file were added to collection!");
                                    }
                                    else throw new Exception();
                                }
                            }
                            catch (Exception) { Console.WriteLine("\nIncorrect input. Try again!"); repeat = true; }
                        } while (repeat);
                        break;

                    default:
                        Console.Write("\nIncorrect input. Try again!\n");
                        break;
                }
            } while (true);
        }
    }
}
