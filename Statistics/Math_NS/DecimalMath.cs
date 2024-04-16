namespace QuickStatistics.Net.Math_NS;

public class DecimalMath
{
    /// <summary>
    /// Calculates the square root of a specified decimal number using the Babylonian method.
    /// </summary>
    /// <param name="number">The decimal number for which the square root is to be calculated.</param>
    /// <param name="accuracy">the targeted accuracy</param>
    /// <returns>The square root of the specified decimal number.</returns>
    public static decimal Sqrt(decimal number, decimal accuracy = 1e-28m)
    {
        if (number < 0) throw new ArgumentOutOfRangeException(nameof(number), "Square root for negative numbers is not defined.");

        if (number == 0) return 0;
        
        decimal guess = number / 2m;
        decimal result = number;

        // Use a better initial guess
        if (number >= 1m)
            while (guess * guess > number)
                guess /= 2m;
        else
            while (guess * guess > number)
                guess *= 2m;

        while (Math.Abs(result - guess) > accuracy)
        {
            result = guess;
            guess = (result + number / result) / 2m;
        }

        return guess;
    }
}