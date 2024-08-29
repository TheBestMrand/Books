using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary
{
    public class ISBNGenerator
    {
        private static readonly Random _random = new Random();

        public static string GenerateISBN13()
        {
            // ISBN-13 format: 978-XXXX-XXXX-X
            string isbn = "978";

            for (int i = 0; i < 9; i++)
            {
                isbn += _random.Next(0, 10).ToString();
            }

            int checkDigit = CalculateCheckDigit(isbn);

            isbn += checkDigit.ToString();

            return FormatISBN(isbn);
        }

        private static int CalculateCheckDigit(string isbn)
        {
            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                int digit = int.Parse(isbn[i].ToString());
                sum += (i % 2 == 0) ? digit : digit * 3;
            }

            int checkDigit = (10 - (sum % 10)) % 10;
            return checkDigit;
        }

        private static string FormatISBN(string isbn)
        {
            return $"{isbn.Substring(0, 3)}-{isbn.Substring(3, 1)}-{isbn.Substring(4, 3)}-{isbn.Substring(7, 5)}-{isbn[12]}";
        }

        public static bool ValidateISBN13(string isbn)
        {
            isbn = new string(isbn.Where(c => !char.IsWhiteSpace(c) && c != '-').ToArray());

            if (isbn.Length != 13 || !isbn.All(char.IsDigit))
            {
                return false;
            }

            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                int digit = int.Parse(isbn[i].ToString());
                sum += (i % 2 == 0) ? digit : digit * 3;
            }

            int checkDigit = (10 - (sum % 10)) % 10;
            return checkDigit == int.Parse(isbn[12].ToString());
        }
    }
}
