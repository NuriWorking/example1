using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SP_Example
{
    class Encryption
    {
        static void Mainn(string[] args)
        {
            void Base64Sample(string str)
            {
                byte[] byteStr = System.Text.Encoding.UTF8.GetBytes(str);
                string encodedStr;
                byte[] decodedBytes;
                encodedStr = Convert.ToBase64String(byteStr);
                Console.WriteLine(encodedStr);
                decodedBytes = Convert.FromBase64String(encodedStr); //복호화
                Console.WriteLine(Encoding.Default.GetString(decodedBytes));
            }
            void SHA256Sample(string strInput) //얘는 복호화 안됨..암호화해서 같은지 비교해야 함
            {
                byte[] hashValue;
                byte[] byteInput = System.Text.Encoding.UTF8.GetBytes(strInput); //인코딩
                SHA256 mySHA256 = SHA256Managed.Create();
                hashValue = mySHA256.ComputeHash(byteInput);
                for (int i = 0; i < hashValue.Length; i++)
                    Console.Write(String.Format("{0:X2}", hashValue[i]));
                Console.WriteLine();
            }

            void Run_Encryption()
            {
                //1.문자열을 입력 받아 Base64로 Encoding한 값을 출력하고, 그 값을 다시 Decoding하여 입력한 값과
                //동일한지 확인해 보시오.
                //Ex) ‘This is a Base64 test.’ → ‘VGhpcyBpcyBhIEJhc2U2NCB0ZXN0Lg ==‘ → ‘This is a Base64 test.’
                Base64Sample("this is a Base64 test.");

                //2. 1번에서 입력 받은 값을 SHA - 256으로 Encryption 해서 결과를 출력해 보시오.
                //Ex) ‘1234’ -> ‘03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4’
                SHA256Sample("1234");
            }
            Run_Encryption();
        }
    }
}
