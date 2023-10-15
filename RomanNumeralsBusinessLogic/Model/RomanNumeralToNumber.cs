namespace RomanNumeralsBusinessLogic.Model
{
    public static class RomanNumeralToNumber
    {
        public struct Relation
        {
            public char NumeralForOnes;
            public char NumeralForFives;
            public char NumeralForTens;
        }

        public static Relation RelationForHundres()
        {
            return new() { NumeralForOnes = RomanNumeral.Hundred, NumeralForFives = RomanNumeral.FiveHundred, NumeralForTens = RomanNumeral.Thousand };
        }

        public static Relation RelationForTens()
        {
            return new() { NumeralForOnes = RomanNumeral.Ten, NumeralForFives = RomanNumeral.Fifty, NumeralForTens = RomanNumeral.Hundred };
        }
        public static Relation RelationForOnes()
        {
            return new() { NumeralForOnes = RomanNumeral.One, NumeralForFives = RomanNumeral.Five, NumeralForTens = RomanNumeral.Ten };
        }
    }
}
