
namespace RomanNumeralsTest
{
    public class ConstructingRomanNumeralTest
    {
        [Theory]
        [MemberData(nameof(RomanNumeralToDecimalNumber))]
        public void ConstructingOk(int decimalNumber, string expectedRomanNumeral)
        {
            //Arrange 
            IRomanNumeralsEngine sut = new RomanNumeralsEngine();

            //Act
            string romanNumeral = sut.ConvertToRomanNumeral(decimalNumber);

            //Assert
            Assert.Equal(expectedRomanNumeral, romanNumeral);

        }


        [Theory]
        [MemberData(nameof(NoneValidDecimalNumber))]
        public void ConstructingNotOkInvalidDecimal(int decimalNumber)
        {
            //Arrange 
            IRomanNumeralsEngine sut = new RomanNumeralsEngine();

            //Act
            var ex = Assert.Throws<ApplicationException>(() => sut.ConvertToRomanNumeral(decimalNumber));

            //Assert
            Assert.Contains($"Unable to convert {decimalNumber} to roman numeral.", ex.Message);

        }



        public static List<object[]> RomanNumeralToDecimalNumber =>
                new List<object[]> {
                    new object[] { 1999 ,"MCMXCIX" },
                    new object[] { 2444 ,"MMCDXLIV"},
                    new object[] { 90,"XC" },
                    new object[] { 1000,"M" },
                    new object[] { 2,"II" },
                    new object[] { 50,"L" },
                    new object[] { 312,"CCCXII" },
                    new object[] { 75,"LXXV" },
                    new object[] { 18,"XVIII" },
                    new object[] { 233,"CCXXXIII" },
                    new object[] { 61,"LXI" },
                };

        public static List<object[]> NoneValidDecimalNumber =>
           new List<object[]> {
                    new object[] { -100 },
                    new object[] { 0},
                    new object[] {4000 }
           };
    }
}