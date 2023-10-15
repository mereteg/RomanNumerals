namespace RomanNumeralsTest
{
    public class ParsingRomanNumeralTest
    {
        [Theory]
        [MemberData(nameof(DecimalNumberToRomanNumeralPatterns))]
        public void ParsingOk(string romanNumeral, int expectedDecimalNumber)
        {
            //Arrange 
            IRomanNumeralsEngine sut = new RomanNumeralsEngine();

            //Act
            int decimalNumber = sut.ConvertToInt(romanNumeral);

            //Assert
            Assert.Equal(expectedDecimalNumber, decimalNumber);
        }

        [Theory]
        [MemberData(nameof(NoneValidRomanNumerals))]
        public void ParsingNotOkInvalidRomanNumeral(string romanNumeral)
        {
            //Arrange 
            IRomanNumeralsEngine sut = new RomanNumeralsEngine();

            //Act
            var ex = Assert.Throws<ApplicationException>(() => sut.ConvertToInt(romanNumeral));

            //Assert
            Assert.Contains($"The input: '{romanNumeral}' is not a valid roman numeral", ex.Message);
        }

        public static List<object[]> DecimalNumberToRomanNumeralPatterns =>
            new List<object[]> {
                new object[] {"MCMXCIX", 1999 },
                new object[] {"MMCDXLIV", 2444 },
                new object[] {"mMcdXLiV", 2444 },
                new object[] {"XC", 90 },
                new object[] {"MMM", 3000},
                new object[] {"I", 1},
                new object[] {"II", 2},
                new object[] {"IV", 4},
                new object[] {"V", 5},
                new object[] {"Vi", 6},
                new object[] {"VI", 6},
                new object[] {"XI", 11},
                new object[] {"XV", 15},
                new object[] {"XXX", 30},
                new object[] {"LXXIV", 74},
                new object[] {"C", 100},
                new object[] {"CL", 150},
                new object[] {"CC", 200},
                new object[] {"DC", 600},
                new object[] {"CV", 105},
                new object[] {"CX", 110},
                new object[] {"CM", 900},
                new object[] {"MMMCMXCIX", 3999},
            };

        public static List<object[]> NoneValidRomanNumerals =>
        new List<object[]> {
            new object[] {"MCMHXCIX" },
            new object[] {"MMCDXLIVK" },
            new object[] {"" },
            new object[] {"1999"},
            new object[] {"MMMMCMHXCIX" },
            new object[] {"VV" },
            new object[] {"VV" },
            new object[] {"LL" },
            new object[] {"DD" },
            new object[] {"IIV" },
            new object[] {"IIX" },
            new object[] {"XXL" },
            new object[] {"XXC" },
            new object[] {"CCD" },
            new object[] {"CCM" },
            new object[] {"IL" },
            new object[] {"IC" },
            new object[] {"ID" },
            new object[] {"IM" },
            new object[] {"XD" },
            new object[] {"XM" },
            new object[] {"IIII" },
            new object[] {"XXXX" },
            new object[] {"CCCC" },
            new object[] {"MMMM" },
            new object[] {"MCMDIX" },
            new object[] {"MCMM" },
            new object[] {"VIX" },
        };
    }
}