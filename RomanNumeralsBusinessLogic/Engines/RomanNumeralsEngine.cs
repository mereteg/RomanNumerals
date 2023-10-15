using RomanNumeralsBusinessLogic.Contract.Interfaces;
using RomanNumeralsBusinessLogic.Model;

namespace RomanNumeralsBusinessLogic.Engines
{
    /// <summary>
    /// Engine that handles converting roman numeral string to and from decimal number
    /// </summary>
    public class RomanNumeralsEngine : IRomanNumeralsEngine
    {
        private const int MAX_NUMBER_OF_ONES = 3;
        private const int MAX_NUMBER_OF_THOUSAND = 3;
        private const int MAX_INT_TO_CONVERT = 3999;
        private const int MIN_INT_TO_CONVERT = 1;


        #region Roman numeral to int
        /// <summary>
        /// Parses a string representing a roman numeral,  
        /// </summary>
        /// <param name="romanNumeral">The string representing the numeral numeral</param>
        /// <returns>The value in the decimal number, if the input can parse as a roman numeral</returns>
        /// <exception cref="ApplicationException"></exception>
        public int ConvertToInt(string romanNumeral)
        {
            if (string.IsNullOrEmpty(romanNumeral))
                throw new ApplicationException($"The input: '{romanNumeral}' is not a valid roman numeral");

            // Allow both upper and lowercase roman numeral input
            romanNumeral = romanNumeral.ToUpper();

            int currentPositionInNumeral = 0;
            int decimalNumber = ParseThousands(romanNumeral, ref currentPositionInNumeral);
            decimalNumber += ParseHundres(romanNumeral, decimalNumber == 0, ref currentPositionInNumeral);
            decimalNumber += ParseTens(romanNumeral, decimalNumber == 0, ref currentPositionInNumeral);
            decimalNumber += ParseOnes(romanNumeral, decimalNumber == 0, ref currentPositionInNumeral);
            if (romanNumeral.Length > currentPositionInNumeral + 1)
                //TODO Create specific Exception
                throw new ApplicationException($"The input: '{romanNumeral}' is not a valid roman numeral. Unexpected char {romanNumeral[currentPositionInNumeral]} at position {currentPositionInNumeral + 1}");
            return decimalNumber;
        }


        private static int ParseThousands(string romanNumeral, ref int currentPositionInNumeral)
        {
            int numberOfThousandsRead = 0;
            if (romanNumeral[currentPositionInNumeral] == RomanNumeral.Thousand)
            {
                numberOfThousandsRead++;
                while (CanReadAnotherNumeral(romanNumeral, RomanNumeral.Thousand, currentPositionInNumeral, numberOfThousandsRead, MAX_NUMBER_OF_THOUSAND))
                {
                    numberOfThousandsRead++;
                    MovePositionToNextCharIfLegal(romanNumeral, RomanNumeral.PositionLevel.Thousand, ref currentPositionInNumeral);
                }
            }
            return numberOfThousandsRead * 1000;

        }

        /// <summary>
        /// Parses  the roman numeral for hundres
        /// </summary>
        /// <param name="romanNumeral">the roman numeral to parse</param>
        /// <param name="currentPositionInNumeral">The position to start the parsing from</param>
        /// <returns>the value of the hundred part of the roman numeral</returns>
        private static int ParseHundres(string romanNumeral, bool firstCharToParse, ref int currentPositionInNumeral)
        {
            int numberOfHundresRead = ParseHundresAndTensAndOnes(romanNumeral, firstCharToParse, RomanNumeralToNumber.RelationForHundres(), RomanNumeral.PositionLevel.Hundred, ref currentPositionInNumeral);
            return numberOfHundresRead * 100;
        }

        /// <summary>
        /// Parses  the roman numeral for tens
        /// </summary>
        /// <param name="romanNumeral">the roman numeral to parse</param>
        /// <param name="currentPositionInNumeral">The position to start the parsing from</param>
        /// <returns>the value of the ten part of the roman numeral</returns>
        private static int ParseTens(string romanNumeral, bool firstCharToParse, ref int currentPositionInNumeral)
        {
            int numberOfTensRead = ParseHundresAndTensAndOnes(romanNumeral, firstCharToParse, RomanNumeralToNumber.RelationForTens(), RomanNumeral.PositionLevel.Ten, ref currentPositionInNumeral);
            return numberOfTensRead * 10;
        }

        /// <summary>
        /// Parses  the roman numeral for ones
        /// </summary>
        /// <param name="romanNumeral">the roman numeral to parse</param>
        /// <param name="currentPositionInNumeral">The position to start the parsing from</param>
        /// <returns>the value of the ten part of the roman numeral</returns>
        private static int ParseOnes(string romanNumeral, bool firstCharToParse, ref int currentPositionInNumeral)
        {
            return ParseHundresAndTensAndOnes(romanNumeral, firstCharToParse, RomanNumeralToNumber.RelationForOnes(), RomanNumeral.PositionLevel.One, ref currentPositionInNumeral);
        }

        /// <summary>
        /// Parses a roman numeral from a specific position to find a part matching the rules for ones, tens and hundreds 
        /// </summary>
        /// <param name="romanNumeral">The roman numeral to find a matching part in</param>
        /// <param name="numeralToNumberRelation">Holds which char a specific roman numeral has - set to be able to parse ones as well as tens ad hunreds </param>
        /// <param name="currentPositionInNumeral">The cirret position in the roman numeral to start at</param>
        /// <returns>The number of either ones, tens or hunrdres in the current string</returns>
        private static int ParseHundresAndTensAndOnes(string romanNumeral, bool firstCharToParse, RomanNumeralToNumber.Relation numeralToNumberRelation, RomanNumeral.PositionLevel level, ref int currentPositionInNumeral)
        {
            // To have a match one of the following should be present,beginning from next position in numeral, unless nothing has been parsed
            // O, OO, OOO, OF, F, FO, FOO, FOOO, OT - 
            // where O is the Roman Numeral given in the structs NumeralForOnes  
            // where F is the Roman Numeral given in the structs NumeralForFives
            // where T is the Roman Numeral given in the structs NumeralForTens              
            if (!firstCharToParse)
                MovePositionToNextCharIfLegal(romanNumeral, level, ref currentPositionInNumeral);
            if (!MoreChars(romanNumeral, currentPositionInNumeral))
                return 0;
            if (romanNumeral[currentPositionInNumeral] == numeralToNumberRelation.NumeralForOnes)
            {
                return ParseWhenStartingWithAOne(romanNumeral, numeralToNumberRelation, level, ref currentPositionInNumeral);
            }
            else if (romanNumeral[currentPositionInNumeral] == numeralToNumberRelation.NumeralForFives)
            {
                return ParseWhenStartingWithAFive(romanNumeral, numeralToNumberRelation, level, ref currentPositionInNumeral);
            }
            else
            // This part of the roman nummeral must start with either a CharForOnes or a CharForFives
            // so if neither is present return 0 as this part isn't present in the roman numeral, and set the position back
            {
                if (!firstCharToParse)
                    currentPositionInNumeral--;
                return 0;
            }
        }
        private static int ParseWhenStartingWithAFive(string romanNumeral, RomanNumeralToNumber.Relation numeralToNumberRelation, RomanNumeral.PositionLevel level, ref int currentPositionInNumeral)
        {
            // To have a match one of the following should be present,beginning from next position in numeral, unless nothing has been parsed
            // F, FO, FOO, FOOO - 
            // where O is the Roman Numeral given in the structs NumeralForOnes  
            // where F is the Roman Numeral given in the structs NumeralForFives
            int numberOfOnes = 0;
            int totalValueOfOnesAndFives = 5;
            while (CanReadAnotherNumeral(romanNumeral, numeralToNumberRelation.NumeralForOnes, currentPositionInNumeral, numberOfOnes, MAX_NUMBER_OF_ONES))
            {
                numberOfOnes++;
                totalValueOfOnesAndFives++;
                MovePositionToNextCharIfLegal(romanNumeral, level, ref currentPositionInNumeral);
            }
            return totalValueOfOnesAndFives;
        }

        private static int ParseWhenStartingWithAOne(string romanNumeral, RomanNumeralToNumber.Relation numeralToNumberRelation, RomanNumeral.PositionLevel level, ref int currentPositionInNumeral)
        {
            // To have a match one of the following should be present,beginning from next position in numeral, unless nothing has been parsed
            // O, OO, OOO, OF, OT - 
            // where O is the Roman Numeral given in the structs NumeralForOnes  
            // where F is the Roman Numeral given in the structs NumeralForFives
            // where T is the Roman Numeral given in the structs NumeralForTens              
            int numberOfOnes = 1;
            if (!MoreChars(romanNumeral, currentPositionInNumeral + 1))
                return numberOfOnes;
            MovePositionToNextCharIfLegal(romanNumeral, level, ref currentPositionInNumeral);
            if (romanNumeral[currentPositionInNumeral] == numeralToNumberRelation.NumeralForFives)
                return 4;
            else if (romanNumeral[currentPositionInNumeral] == numeralToNumberRelation.NumeralForTens)
                return 9;
            else
            {
                // Not a Five or Ten so set position back and try parsing One's
                currentPositionInNumeral--;
                while (CanReadAnotherNumeral(romanNumeral, numeralToNumberRelation.NumeralForOnes, currentPositionInNumeral, numberOfOnes, MAX_NUMBER_OF_ONES))
                {
                    numberOfOnes++;
                    MovePositionToNextCharIfLegal(romanNumeral, level, ref currentPositionInNumeral);
                }
                return numberOfOnes;
            }
        }


        private static void MovePositionToNextCharIfLegal(string romanNumeral, RomanNumeral.PositionLevel level, ref int currentPositionInNumeral)
        {
            if (MoreChars(romanNumeral, currentPositionInNumeral + 1))
                CheckLegalRomanNumeralChar(romanNumeral, currentPositionInNumeral + 1, level);
            currentPositionInNumeral++;
        }

        private static void CheckLegalRomanNumeralChar(string romanNumeral, int currentPositionInNumeral, RomanNumeral.PositionLevel level)
        {
            if (!RomanNumeral.GetValidRomanNumeralsForLevel(level).Contains(romanNumeral[currentPositionInNumeral]))
                //TODO Create specific Exception
                throw new ApplicationException($"The input: '{romanNumeral}' is not a valid roman numeral. Unexpected char {romanNumeral[currentPositionInNumeral]} at position {currentPositionInNumeral + 1}");
        }


        private static bool CanReadAnotherNumeral(string romanNumeral, char charForOnes, int currentPositionInNumeral, int numberOfChars, int maxNumberOfChars)
        {
            if (!MoreChars(romanNumeral, currentPositionInNumeral + 1))
                return false;
            bool nextIsTheSpecifiedChar = romanNumeral[currentPositionInNumeral + 1] == charForOnes;
            if (!nextIsTheSpecifiedChar)
                return false;
            if (numberOfChars >= maxNumberOfChars)
                //TODO Create specific Exception
                throw new ApplicationException($"The input: '{romanNumeral}' is not a valid roman numeral. Unexpected char {romanNumeral[currentPositionInNumeral]} at position {currentPositionInNumeral + 1}");
            return nextIsTheSpecifiedChar;
        }

        private static bool MoreChars(string romanNumeral, int currentPositionInNumeral)
        {
            return currentPositionInNumeral < romanNumeral.Length;
        }
        #endregion

        #region Int to roman numeral
        /// <summary>
        /// Convert to roman numeral string from integer
        /// </summary>
        /// <param name="decimalNumber"></param>
        /// <returns>Roman numeral string representing the decimal number, if decimal number is within the supported range</returns>
        /// <exception cref="ApplicationException"></exception>
        public string ConvertToRomanNumeral(int decimalNumber)
        {
            if (decimalNumber < MIN_INT_TO_CONVERT || decimalNumber > MAX_INT_TO_CONVERT)
                //TODO Create specific Exception
                throw new ApplicationException($"Unable to convert {decimalNumber} to roman numeral.Only numbers with a decimal value between {MIN_INT_TO_CONVERT} and {MAX_INT_TO_CONVERT} ca be converted");
            string romanNumeral = CreateThousandPart(ref decimalNumber);
            romanNumeral += CreateHundredsPart(ref decimalNumber);
            romanNumeral += CreateTensPart(ref decimalNumber);
            romanNumeral += CreateOnesPart(ref decimalNumber);
            return romanNumeral;
        }

        /// <summary>
        /// Create a string matching the roman numeral for the thousand part of an integer value
        /// </summary>
        /// <param name="decimalNumber">The value to convert to roman numeral</param>
        /// <returns>The thousand part of the integer as roman numeral</returns>
        private static string CreateThousandPart(ref int decimalNumber)
        {
            int numberOfThousand = decimalNumber / 1000;
            decimalNumber = decimalNumber % 1000;
            return StringWithCharRepeatedXTimes(RomanNumeral.Thousand, numberOfThousand);
        }

        /// <summary>
        /// Create a string matching the roman numeral for the 100 part of an integer value
        /// </summary>
        /// <param name="decimalNumber">The value to convert to roman numeral</param>
        /// <returns>The hhundred part of the integer as roman numeral</returns>
        private static string CreateHundredsPart(ref int decimalNumber)
        {
            int numberOfHundreds = decimalNumber / 100;
            decimalNumber = decimalNumber % 100;
            return CreateRomanForHundredsAndTensAndOnes(numberOfHundreds, RomanNumeralToNumber.RelationForHundres());
        }

        /// <summary>
        /// Create a string matching the roman numeral for the 10 part of an integer value
        /// </summary>
        /// <param name="decimalNumber">The value to convert to roman numeral</param>
        /// <returns>The tens part of the integer as roman numeral</returns>
        private static string CreateTensPart(ref int decimalNumber)
        {
            int numberOfTens = decimalNumber / 10;
            decimalNumber = decimalNumber % 10;
            return CreateRomanForHundredsAndTensAndOnes(numberOfTens, RomanNumeralToNumber.RelationForTens());
        }

        /// <summary>
        /// Create a string matching the roman numeral for the 1 part of an integer value
        /// </summary>
        /// <param name="decimalNumber">The value to convert to roman numeral</param>
        /// <returns>The ones part of the integer as roman numeral</returns>
        private static string CreateOnesPart(ref int decimalNumber)
        {
            return CreateRomanForHundredsAndTensAndOnes(decimalNumber, RomanNumeralToNumber.RelationForOnes());
        }

        private static string CreateRomanForHundredsAndTensAndOnes(int number, RomanNumeralToNumber.Relation numberRelation)
        =>
        number switch
        {
            0 => "",
            1 => numberRelation.NumeralForOnes.ToString(),
            2 => $"{StringWithCharRepeatedXTimes(numberRelation.NumeralForOnes, 2)}",
            3 => $"{StringWithCharRepeatedXTimes(numberRelation.NumeralForOnes, 3)}",
            4 => $"{numberRelation.NumeralForOnes}{numberRelation.NumeralForFives}",
            5 => numberRelation.NumeralForFives.ToString(),
            6 => $"{numberRelation.NumeralForFives}{numberRelation.NumeralForOnes}",
            7 => $"{numberRelation.NumeralForFives}{StringWithCharRepeatedXTimes(numberRelation.NumeralForOnes, 2)}",
            8 => $"{numberRelation.NumeralForFives}{StringWithCharRepeatedXTimes(numberRelation.NumeralForOnes, 3)}",
            9 => $"{numberRelation.NumeralForOnes}{numberRelation.NumeralForTens}",
            //TODO Create specific Exception
            _ => throw new ApplicationException($"CreateRomanForHundredsAndTensAndOnes can only handle decimal numbers from 0-9. Input decimal number was: {number}"),
        };


        private static string StringWithCharRepeatedXTimes(char charToRepeat, int numberOfRepetitions)
        {
            return new string(Enumerable.Repeat(charToRepeat, numberOfRepetitions).ToArray());
        }
        #endregion


    }
}
