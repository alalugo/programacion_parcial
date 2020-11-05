using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nomina.Validations
{
    public class ValidateIdentificationAttribute : ValidationAttribute
    {
        public string Identification { get; set; }

        public string GetErrorMessage() => "La cedula ingresada no es valida!";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Identification = (string)value;

            var result = ValidateIdentification(Identification);

            if (result)
                return ValidationResult.Success;

            return new ValidationResult(GetErrorMessage());
        }

        private bool ValidateIdentification(string identification)
        {
            if (identification.Length != 11)
                return false;

            var verificatorDigit = Convert.ToInt32(identification.Substring(10, 1));
            var digitsToValidate = identification.Substring(0, 10).ToString();

            int[,] arr = new int[2, 10];

            for (int i = 0; i < digitsToValidate.Length; i++)
                arr[0, i] = Convert.ToInt32(digitsToValidate[i].ToString());

            int a = 1;

            for (int i = 0; i < digitsToValidate.Length; i++)
            {
                arr[1, i] = a;

                a = (a == 1) ? 2 : 1;
            }

            string str = "";

            for (int i = 0; i < digitsToValidate.Length; i++)
            {
                var productResult = arr[0, i] * arr[1, i];
                str = string.Concat(str, productResult.ToString());
            }


            int sum = 0;

            for (int i = 0; i < str.Length; i++)
            {
                var digit = Convert.ToInt32(str[i].ToString());

                sum += digit;
            }

            var result = RoundUp(sum) - sum;

            if (result == verificatorDigit)
                return true;

            return false;
        }

        private int RoundUp(int i)
        {
            return ((int)Math.Ceiling(i / 10.0)) * 10;
        }

    }
}
