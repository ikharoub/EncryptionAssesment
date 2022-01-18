using System;
using System.Text;

namespace Encryption
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Write("Input: ");
            string input = Console.ReadLine();

            if (input == null)
                return;


            ITextEncryptor encryptor = new CaesarCipherTextEncryptor();
            var output = encryptor.EncryptText(input);

            if (output.Success)
                Console.WriteLine("Output: " + output.Result);
            else
                Console.WriteLine("Output: " + output.ErrorMessage);
        }

    }

    // creating an interface for text encryption for breaking dependency between the underlaying details for encryption method.
    // to make it easy to maintain or replace without having side effects on external code.
    interface ITextEncryptor
    {
        StringEncryptionResult EncryptText(string text);
    }

    // abstract the implementation of encryption in a class to be able to reuse it in different places.
    class CaesarCipherTextEncryptor : ITextEncryptor
    {

        // can be loaded from any certain type of configuration.
        const int shift = 5;
        StringEncryptionResult ITextEncryptor.EncryptText(string text)
        {
            // creating a string building instead of string concatenation for better memory optimization as the string builder is created only once
            // but string concatenation creates per each concatenation a new object in heap.
            var output = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                // retunring validation result instead of exception for performance
                if (text[i] < 65 || text[i] > 90)
                    return new StringEncryptionResult { ErrorMessage = "Only A-Z supported." };

                // reducing the number of operations in one mathematical equation for performance;
                int shifted = (text[i] + shift - 65) % (91 - 65) + 65;
                output.Append((char)shifted);
            }

            return new StringEncryptionResult { Success = true, Result = output.ToString() };
        }
    }


    public class StringEncryptionResult
    {
        public bool Success { get; set; }
        public string Result { get; set; }
        public string ErrorMessage { get; set; }
    }
}