using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.ComponentModel;

namespace DigitalSignature
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("DIGITAL SIGNATURE");
            Console.Write("\nEnter data: ");
            string data = Console.ReadLine();

            byte[] dataToEncrypt = Encoding.ASCII.GetBytes(data); //dekodowanie wiadomości na bajty

            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                var encryptedData = Functions.RSAEncryption(dataToEncrypt, RSA.ExportParameters(false), false); //szyfrowanie z kluczem prywatnym
                Console.WriteLine($"HashCode: {RSA.ExportParameters(false).GetHashCode()}");


                Console.Write("Signature: ");
                foreach (var a in encryptedData)
                    Console.Write(a);
                Console.WriteLine("\n");


                var decryptedData = Functions.RSADecryption(encryptedData, RSA.ExportParameters(true), false); //deszyfrowanie z kluczem publicznym

                YesOrNo(data, Encoding.Default.GetString(decryptedData));
                Console.WriteLine(" ");
                Console.WriteLine($"Encrypted data: {Encoding.Default.GetString(decryptedData)}");
            }

            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                Console.WriteLine("==============================");
                RSAParameters rsaParameters;
                var exportedPar = RSA.ExportParameters(false);
                Console.WriteLine($"HashCode: {RSA.ExportParameters(false).GetHashCode()}");
                var signature = RSA.SignData(dataToEncrypt, new SHA1Managed());


                Console.Write("Signature: ");
                foreach (var el in signature)
                    Console.Write(el);
                Console.WriteLine("\n");


                RSA.ImportParameters(exportedPar);
                var b = RSA.VerifyData(dataToEncrypt, new SHA1Managed(), signature);
                CheckBool(b);
            }

            Console.ReadKey();
        }

        private static void YesOrNo(string encData, string decData)
        {
            if (encData.Equals(decData))
                Console.WriteLine("The signature is valid.");
            else
                Console.WriteLine("The signature is not valid");
        }

        private static void CheckBool(bool b)
        {
            if (b)
                Console.WriteLine("The signature is valid.");
            else
                Console.WriteLine("The signature is not valid");
        }
    }
}