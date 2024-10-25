using Lab_7;

namespace TestProject
{
    [TestClass]
    public class PatientTest
    {
        [TestMethod]
        [DataRow((short)-25, (short)50, false, (short)50)] //Time < 0
        [DataRow((short)10, (short)50, true, (short)43)] //Time > 0
        [DataRow((short)10, (short)1, true, (short)0)] //Healthlevel under 0
        public void PatientWaitTest(short time, short initialHealthLevel, bool expectedResult, short expectedHealthLevel)
        {
            //Arrange
            Patient patient = new Patient();
            patient.Healthlevel = initialHealthLevel;

            //Act
            bool actualResult = patient.PatientWait(time);
            short actualHealthLevel = patient.Healthlevel;

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedHealthLevel, actualHealthLevel);
        }

        [TestMethod]
        [DataRow((short)-25, (short)50, false, (short)50)] //Time < 0
        [DataRow((short)10, (short)50, true, (short)67)] //Time > 0
        [DataRow((short)10, (short)99, true, (short)100)] // Healthlevel exceeds 100
        public void PatientTakeMedicineTest(short time, short initialHealthLevel, bool expectedResult, short expectedHealthLevel)
        {
            //Arrange
            Patient patient = new Patient();
            patient.Healthlevel = initialHealthLevel;

            //Act
            bool actualResult = patient.PatientTakeMedicine(time);
            short actualHealthLevel = patient.Healthlevel;

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedHealthLevel, actualHealthLevel);
        }

        [TestMethod]
        [DataRow((short)-25, (short)10, (short)50, false, (short)50)] // Time < 0
        [DataRow((short)10, (short)10, (short)50, true, (short)70)]   // Time > 0 and Dosage > 0
        [DataRow((short)25, (short)-10, (short)50, false, (short)50)] // Dosage < 0
        [DataRow((short)10, (short)10, (short)99, true, (short)100)]  // Healthlevel exceeds 100
        public void PatientTakeMedicineTest(short time, short dosage, short initialHealthLevel, bool expectedResult, short expectedHealthLevel)
        {
            //Arrange
            Patient patient = new Patient();
            patient.Healthlevel = initialHealthLevel;

            //Act
            bool actualResult = patient.PatientTakeMedicine(time, dosage);
            short actualHealthLevel = patient.Healthlevel;

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedHealthLevel, actualHealthLevel);
        }


        [TestMethod]
        public void PatientVisitedDoctorTest_Healthlevel_More_100()
        {
            //Arrange
            Patient patient = new Patient();
            patient.Healthlevel = 99;

            short expected = 100;

            //Act
            patient.PatientVisitedDoctor();
            short actual = patient.Healthlevel;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CounterMinTest_DecrementsCounter_AfterDeletePatient()
        {
            //Arrange
            Patient.ResetCounter();
            Patient patient = new Patient();
            short expected = 0;

            //Act
            Patient.CounterMin();
            int actual = Patient.Counter;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow((short)60, 1.58, "\nBMI = 24,03 - within normal range.")]  //BMI within normal range
        [DataRow((short)40, 1.7, "\nBMI = 13,84 - outside the normal range.")] //BMI outside normal range
        public void IsNormalBMITest(short weight, double height, string expected)
        {
            //Arrange

            //Act
            string actual = Patient.IsNormalBMI(weight, height);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void ParseTest_StringIsEmpty()
        {
            //Arrange
            List<Patient> patients = new List<Patient>();
            string? s = null;

            //Act

            //Assert
            Assert.ThrowsException<NullReferenceException>(() => Patient.Parse(s!, patients));
        }

        [TestMethod]
        [DataRow("Marry;Brown;18")] //Incorrect number of parts
        [DataRow(";Brown;25;123456789;PREGNANT;123")] //Incorrect part 0
        [DataRow("M;Brown;25;123456789;PREGNANT;123")] //Incorrect part 0
        [DataRow("Marry;;25;123456789;PREGNANT;123")] //Incorrect part 1
        [DataRow("Marry;B;25;123456789;PREGNANT;123")] //Incorrect part 1
        [DataRow("Marry;Brown;-9;123456789;PREGNANT;123")] //Incorrect part 2 (age < 0)
        [DataRow("Marry;Brown;160;123456789;PREGNANT;123")] //Incorrect part 2 (age > 150)
        [DataRow("Marry;Brown;25;12;PREGNANT;123")] //Incorrect part 3 (ID too short)
        [DataRow("Marry;Brown;25;1000000000;PREGNANT;123")] //Incorrect part 3 (ID too large)
        [DataRow("Marry;Brown;25;123456789;P;123")] //Incorrect part 4 (invalid status)
        [DataRow("Marry;Brown;25;123456789;6;123")] //Incorrect part 4 (invalid status number)
        [DataRow("Marry;Brown;25;123456789;PREGNANT;1")] //Incorrect part 5 (Code too small)
        [DataRow("Marry;Brown;25;123456789;PREGNANT;1000")] //Incorrect part 5 (Code too large)
        [DataRow("Marry;Brown;25;123456789;PREGNANT;521")] //Incorrect part 5 (duplicate Code)
        public void ParseTest_IncorrectParts(string input)
        {
            //Arrange
            List<Patient> patients = new List<Patient> { new Patient() { Code = 521 } };

            //Act + Assert
            Assert.ThrowsException<FormatException>(() => Patient.Parse(input, patients));
        }

        [TestMethod]
        public void TryParseTest_ValidString()
        {
            //Arrange
            List<Patient> patients = new List<Patient>();
            string s = "Marry;Brown;25;123456789;PREGNANT;521";
            Patient result = new Patient() { Name="Marry", Surname="Brown", Age=25, Number=123456789, Type=0, Code=521 };

            //Act
            bool isSuccess = Patient.TryParse(s, out Patient patient, patients);

            //Assert
            Assert.IsTrue(isSuccess);
            Assert.IsNotNull(result);
            Assert.AreEqual("Marry Brown", result.FullName);
            Assert.AreEqual(25, result.Age);
            Assert.AreEqual(123456789, result.Number);
            Assert.AreEqual(PatientType.PREGNANT, result.Type);
            Assert.AreEqual(521, result.Code);
        }

        [TestMethod]
        public void ToStringTest_AllFieldsSet()
        {
            //Arrange
            Patient patient = new Patient()
            {
                Name = "Marry",
                Surname = "Brown",
                Age = 35,
                Number = 123456789,
                Type = PatientType.PREGNANT,
                Code = 123
            };
            string expected = "Marry;Brown;35;123456789;PREGNANT;123";

            //Act
            string actual = patient.ToString();

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(12, false)]       //Code less than 100
        [DataRow(10000, false)]    //Code more than 999
        [DataRow(123, true)]       //Null element in array, valid code
        [DataRow(123, false, 123)] //Code already exists in patients
        [DataRow(245, true, 123)]  //Correct input, code does not exist
        public void CheckCodeTest(int code, bool expected, int? existingCode = null)
        {
            //Arrange
            List<Patient> patients;

            if (existingCode.HasValue)
            {
                patients = [ new Patient() { Code = existingCode.Value } ];
            }
            else
            {
                patients = [null!];
            }

            //Act
            bool result = Patient.CheckCode(patients, code);

            //Assert
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void NameTest_Incorrect_Name()
        {
            //Arrange
            Patient patient = new Patient();

            //Act + Assert
            Assert.ThrowsException<FormatException>(() => patient.Name = "");
        }

        [TestMethod]
        public void NameTest_Correct_Name()
        {
            //Arrange
            Patient patient = new Patient() { Name = "David" };

            //Act
            string result = patient.FullName;

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [DataRow((short)0)] // Age less than 1
        [DataRow((short)120)] // Age more than 99
        public void MinAgeTest_InvalidAge(short age)
        {
            //Act + Assert
            Assert.ThrowsException<FormatException>(() => Patient.MinAge = age);
        }


        [TestMethod]
        [DataRow(1)] //Number less than 100000000
        [DataRow(1000000000)] //Number more than 999999999
        public void NumberTest_InvalidNumber(int number)
        {
            //Arrange
            Patient patient = new Patient();

            //Act + Assert
            Assert.ThrowsException<FormatException>(() => patient.Number = number);
        }
    }
}