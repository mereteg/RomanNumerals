namespace RomanNumeralsBusinessLogic.Model
{
    public static class RomanNumeral
    {
        public static char One = 'I';
        public static char Five = 'V';
        public static char Ten = 'X';
        public static char Fifty = 'L';
        public static char Hundred = 'C';
        public static char FiveHundred = 'D';
        public static char Thousand = 'M';

        public static char[] ValidRomanNumeralsLevelHundredAndThousand = { One, Five, Ten, Fifty, Hundred, FiveHundred, Thousand };
        public static char[] ValidRomanNumeralsLevelTen = { One, Five, Ten, Fifty, Hundred };
        public static char[] ValidRomanNumeralsLevelOne = { One, Five, Ten };


        public enum PositionLevel
        {
            One,
            Ten,
            Hundred,
            Thousand
        }
        public static char[] GetValidRomanNumeralsForLevel(PositionLevel level) =>
            level switch
            {
                PositionLevel.One => ValidRomanNumeralsLevelOne,
                PositionLevel.Ten => ValidRomanNumeralsLevelTen,
                _ => ValidRomanNumeralsLevelHundredAndThousand
            };

    }
}
