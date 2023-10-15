namespace RomanNumeralsBusinessLogic.Contract.Interfaces
{
    public interface IRomanNumeralsEngine
    {
        int ConvertToInt(string romanNumeral);
        string ConvertToRomanNumeral(int decimalNumber);
    }
}
