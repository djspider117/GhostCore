using System;

namespace GhostCore.Math
{
    public static class Fibbo
    {
        private const double SQRT_5 = 2.2360679775;
        private const double HALF_SQRT_5 = 1.11803398875;
        private const double ONE_OVER_SQRT_5 = 0.4472135955;

        ///<summary>
        ///Calculates the nth fibbonaci number by using Binet's Formula
        ///https://en.wikipedia.org/wiki/Fibonacci_number#Binet's_formula
        ///</summary>
        public static uint Binet(int n) => ONE_OVER_SQRT_5 * (Math.Pow(0.5 + HALF_SQRT_5, n) - Math.Pow(0.5 - HALF_SQRT_5, n));
    }
}