﻿using System;
using System.IO;
using System.Security.Cryptography;
namespace prjAES {
    class ManagedAesSample
    {
        public static void Main()
        {
            Console.WriteLine("Enter text that needs to be encrypted..");
            string noidung = "hoangthai";
            File.WriteAllText("out.txt", noidung);
            string data = File.ReadAllText("out.txt");
            Console.WriteLine("Text: "+data);
            EncryptAesManaged(data);
        }
        static void EncryptAesManaged(string raw)
        {
            try
            {
                Console.WriteLine("------------ENCODE-----------");
                double firstTime = Convert.ToDouble(DateTime.Now.ToString("ss.ffff"));
                // Create Aes that generates a new key and initialization vector (IV).    
                // Same key must be used in encryption and decryption    
                using (AesManaged aes = new AesManaged())
                {
                    // Encrypt string    
                    byte[] encrypted = Encrypt(raw, aes.Key, aes.IV);
                    // Print encrypted string
                    double lastTime = Convert.ToDouble(DateTime.Now.ToString("ss.ffff"));
                    Console.WriteLine($"Encrypted data: {System.Text.Encoding.UTF7.GetString(encrypted)}");
                    File.WriteAllText("file.txt", System.Text.Encoding.UTF7.GetString(encrypted));
                    Console.Write("Time encode:");
                    Console.WriteLine(lastTime - firstTime + "s");
                    // Decrypt the bytes to a string.    
                    Console.WriteLine("------------DECODE-----------");
                    firstTime = Convert.ToDouble(DateTime.Now.ToString("ss.ffff"));
                    string decrypted = Decrypt(encrypted, aes.Key, aes.IV);
                    lastTime = Convert.ToDouble(DateTime.Now.ToString("ss.ffff"));
                    Console.Write("Time decode:");
                    Console.WriteLine(lastTime - firstTime + "s");
                    Console.WriteLine($"Encrypted data: {System.Text.Encoding.UTF7.GetString(encrypted)}");
                    // Print decrypted string. It should be same as raw data    
                    Console.WriteLine($"Decrypted data: {decrypted}");
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
            //Console.ReadKey();
        }
        static byte[] Encrypt(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            // Create a new AesManaged.    
            using (AesManaged aes = new AesManaged())
            {
                // Create encryptor    
                ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);
                // Create MemoryStream    
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create crypto stream using the CryptoStream class. This class is the key to encryption    
                    // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream    
                    // to encrypt    
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        // Create StreamWriter and write data to a stream    
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = ms.ToArray();
                    }
                }
            }
            // Return encrypted data    
            return encrypted;
        }
        static string Decrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            string plaintext = null;
            // Create AesManaged    
            using (AesManaged aes = new AesManaged())
            {
                // Create a decryptor    
                ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
                // Create the streams used for decryption.    
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    // Create crypto stream    
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        // Read crypto stream    
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }
    }
}