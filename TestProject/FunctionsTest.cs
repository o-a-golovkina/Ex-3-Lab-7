using Lab_7;

namespace TestProject
{
    [TestClass]
    public class FunctionsTest
    {
        [TestMethod]
        [DataRow(2, 1)]
        [DataRow(4, -1)]
        public void FindNumTest_ValidIndex(int searchNum, int expectedIndex)
        {
            //Arrange
            Functions function = new Functions();
            List<Patient> patients = new List<Patient>
            {
                new Patient { Name = "Mary"},
                new Patient { Name = "Bob"},
                new Patient { Name = "Joy"}
            };

            //Act
            int result = function.FindNum(searchNum, patients);

            //Assert
            Assert.AreEqual(expectedIndex, result);
        }

        [TestMethod]
        [DataRow(123, 0)]
        [DataRow(326, 2)]
        [DataRow(111, -1)]
        public void FindCodeTest(int code, int expectedResult)
        {
            //Arrange
            Functions function = new Functions();
            List<Patient> patients = new List<Patient>
            {
                new Patient { Code = 123},
                new Patient { Code = 444},
                new Patient { Code = 326}
            };

            //Act
            int result = function.FindCode(code, patients);

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [DataRow(123456789, 0)]
        [DataRow(987456321, 1)]
        [DataRow(333333333, -1)]
        public void FindPhoneNumberTest(int number, int expectedResult)
        {
            //Arrange
            Functions function = new Functions();
            List<Patient> patients = new List<Patient>
            {
                new Patient { Number = 123456789},
                new Patient { Number = 987456321}
            };

            //Act
            int result = function.FindPhoneNumber(number, patients);

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void SaveToFileCSVTest()
        {
            //Arrange
            List<Patient> patients = new List<Patient>
            {
                new Patient{ Name = "Marry", Surname = "Brown", Age = 36, Number = 123456789, Type = PatientType.ALLERGIC, Code = 123 },
            };

            string tempPath = Path.GetTempFileName();

            //Act
            Functions function = new Functions();
            function.SaveToFileCSV(patients, tempPath);
            var lines = File.ReadAllLines(tempPath);

            //Assert
            Assert.AreEqual(patients.Count, lines.Length);

            if (File.Exists(tempPath))
                File.Delete(tempPath);
        }
    }
}
