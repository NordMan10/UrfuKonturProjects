using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.RationalNumbers
{
    public class Rational
    {
        public Rational(int numerator, int denominator = 1)
        {
            Numerator = numerator;
            Denominator = denominator;

            Reduce(ref this.numerator, ref this.denominator);
        }

        private int numerator;
        public int Numerator
        {
            get { return numerator; }
            set
            {
                if (value == 0) Denominator = 1;
                numerator = value;
            }
        }

        private int denominator;
        public int Denominator
        {
            get { return denominator; }
            set
            {
                if (Numerator == 0 && value != 0) denominator = 1;
                else denominator = value;
            }
        }

        public bool IsNan
        {
            get 
            {
                return Denominator == 0;
            }
        }

        public static Rational operator +(Rational r1, Rational r2)
        {
            if (CheckArgumetIsNaN(r1, r2)) return new Rational(1, 0);

            var commonDenominator = GetCommonDenominator(r1.Denominator, r2.Denominator);

            var changedNumeratorR1 = GetChangedNumerator(r1, commonDenominator);
            var changedNumeratorR2 = GetChangedNumerator(r2, commonDenominator);

            Rational preResult = new Rational(changedNumeratorR1 + changedNumeratorR2, commonDenominator);

            return Reduce(preResult);
        }

        public static Rational operator -(Rational r1, Rational r2)
        {
            if (CheckArgumetIsNaN(r1, r2)) return new Rational(1, 0);

            var commonDenominator = GetCommonDenominator(r1.Denominator, r2.Denominator);

            var changedNumeratorR1 = GetChangedNumerator(r1, commonDenominator);
            var changedNumeratorR2 = GetChangedNumerator(r2, commonDenominator);

            Rational preResult = new Rational(changedNumeratorR1 - changedNumeratorR2, commonDenominator);

            return Reduce(preResult);
        }

        public static Rational operator *(Rational r1, Rational r2)
        {
            if (CheckArgumetIsNaN(r1, r2)) return new Rational(1, 0);

            Rational preResult = new Rational(r1.Numerator * r2.Numerator, r1.Denominator * r2.Denominator);

            return Reduce(MoveSignToNumerator(preResult));
        }

        public static Rational operator /(Rational r1, Rational r2)
        {
            if (CheckArgumetIsNaN(r1, r2)) return new Rational(1, 0);

            if (r2.Numerator == 0) return new Rational(1, 0);

            Rational preResult = new Rational(r1.Numerator * r2.Denominator, r1.Denominator * r2.Numerator);

            return Reduce(MoveSignToNumerator(preResult));
        }

        public static implicit operator double(Rational rational)
        {
            if (rational.IsNan) return double.NaN;
            return (double)rational.Numerator / rational.Denominator;
        }

        public static explicit operator int(Rational rational)
        {
            var tempResult = (double)rational.Numerator / (double)rational.Denominator;
            if (rational.Numerator % rational.Denominator == 0) return (int)tempResult;
            else throw new Exception();
        }

        public static implicit operator Rational(int number)
        {
            return new Rational(number, 1);
        }

        private static int GetCommonDenominator(int den1, int den2)
        {
            return den1 * den2;
        }

        private static bool CheckArgumetIsNaN(Rational r1, Rational r2)
        {
            return r1.IsNan || r2.IsNan;
        }

        private static Rational Reduce(Rational rational)
        {
            var greatestCommonDevisor = (int)BigInteger.GreatestCommonDivisor(rational.Numerator, rational.Denominator);

            return new Rational(rational.Numerator / greatestCommonDevisor, 
                rational.Denominator / greatestCommonDevisor);
        }

        private void Reduce(ref int numerator, ref int denominator)
        {
            var greatestCommonDevisor = (int)BigInteger.GreatestCommonDivisor(Numerator, Denominator);

            MoveSignToNumerator(ref numerator, ref denominator);

            if (greatestCommonDevisor > 0)
            {
                numerator /= greatestCommonDevisor;
                denominator /= greatestCommonDevisor;
            }
        }

        private static int GetChangedNumerator(Rational rational, int commonDenominator)
        {
            return rational.Numerator * (commonDenominator / rational.Denominator);
        }

        private static Rational MoveSignToNumerator(Rational rational)
        {
            return new Rational(rational.Numerator * Math.Sign(rational.Denominator), 
                rational.Denominator * Math.Sign(rational.Denominator));
        }

        private void MoveSignToNumerator(ref int numerator, ref int denominator)
        {
            numerator *= Math.Sign(denominator);
            denominator *= Math.Sign(denominator);
        }
    }
}
