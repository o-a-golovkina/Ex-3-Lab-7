﻿namespace Lab_7
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
            Functions function = new();
            int i = 0;
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
                    " 1 - add object\n 2 – output objects\n 3 – find object\n 4 – delete object\n 5 – demonstrate behavior\n 6 – demonstrate static method\n 0 – exit programs\n" +
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
                            function.AddPatient(i, ref patients, N);
                            i++;
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
                            for (int y = 0; y < i; y++)
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
                                    i--;
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
                                function.CaseFive(i, ref patients);
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
                                function.CaseSix(i, ref patients);
                            }
                            catch (FormatException ex) { Console.WriteLine(ex.Message); repeat = true; }
                            catch (Exception) { Console.WriteLine("Something went wrong... Try again!"); repeat = true; }
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