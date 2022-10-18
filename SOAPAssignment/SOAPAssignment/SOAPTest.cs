using ServiceReference1;
using System.Diagnostics;
using System.Reflection;

[assembly: Parallelize(Workers = 10, Scope = ExecutionScope.MethodLevel)]
namespace SOAPAssignment
{
    [TestClass]
    public class SOAPTest
    {

        private static CountryInfoServiceSoapTypeClient infoServiceSoapClient = null;

        [TestInitialize]
        public void TestInit()
        {
            infoServiceSoapClient = new CountryInfoServiceSoapTypeClient(CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap);
        }

        [TestMethod]
        public void ValidateCountryCodeIfAscendingOrder()
        {
            var countryList = infoServiceSoapClient.ListOfCountryNamesByCode();
            var sortedCountryListAsc = infoServiceSoapClient.ListOfCountryNamesByCode().OrderBy(x => x.sISOCode).ToList();

            Assert.IsTrue(validateIfDataIsEqual(sortedCountryListAsc, countryList));
        }

        [TestMethod]
        public void ValidatePassingInvalidCountryCode()
        {
            var testCountryCode = infoServiceSoapClient.CountryName("XXH");

            Assert.AreEqual(testCountryCode, "Country not found in the database", "Country Code data is available in database");
        }

        [TestMethod]
        public void ValidateLastCountryCodeEntry()
        {
            var countryList = infoServiceSoapClient.ListOfCountryNamesByCode();
            int lastEntryCount = countryList.Count() - 1;
            var countryListLastEntry = countryList[lastEntryCount].sName;
            var getCountryNameOfLastDataEntry = infoServiceSoapClient.CountryName(countryList[lastEntryCount].sISOCode).ToString();

            Assert.AreEqual(getCountryNameOfLastDataEntry, countryListLastEntry, "Data were not the same");

        }

        private bool validateIfDataIsEqual(List<tCountryCodeAndName> expectedArray, List<tCountryCodeAndName> actualArray)
        {
            if (expectedArray.Count != actualArray.Count) {
                return false;
            }

            for (int i = 0; i < expectedArray.Count; i++)
            {
                foreach (PropertyInfo prop in expectedArray[i].GetType().GetProperties())
                {
                    var expectedArrayData = prop.GetValue(expectedArray[i]).ToString();
                    var actualArrayData = prop.GetValue(actualArray[i]).ToString();

                    if (expectedArrayData != actualArrayData)
                    {
                        return false;
                    }
                }
            }
            
            return true;
        }
    }
}