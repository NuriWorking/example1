using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question1
{
    class ValidatorLauncher
    {
        static void Main(string[] args)
        {
            SetUserData("../CLIENT/INSPECTOR.TXT");
            SetTestQueueInput("../TestData/Question1/LOGIN_TEST.TXT");
            while (LogOut)
            {
                //string input = Console.ReadLine();
                string input = TestQue.Dequeue();
                if (SignIn(input))
                {
                    Console.WriteLine("LOGIN SUCCESS");
                    LogOut = false;
                }
                else
                {
                    Console.WriteLine("LOGIN FAIL");
                }
            }
            Console.ReadKey();
        }


        static Dictionary<string, string> UserDict = new Dictionary<string, string>();
        static bool LogOut = true;
        static void SetUserData(string filename)
        {
            string line;
            StreamReader file = new StreamReader(filename);
            while ((line = file.ReadLine()) != null)
            {
                string[] idPw = line.Split(' ');
                //System.Console.WriteLine(pwDe);
                UserDict.Add(idPw[0], idPw[1]);
            }
            file.Close();
        }
        static bool SignIn(string input)
        {
            string[] inputIdPw = input.Split(' ');
            string inputId = inputIdPw[0];
            string inputPw = inputIdPw[1];
            if (UserDict.ContainsKey(inputId))
            {
                string encryptInputPw = CardUtility.passwordEncryption_SHA256(inputPw);
                if (encryptInputPw.Equals(UserDict[inputId]))
                    return true;
                else
                    return false;
            }
            else
            {
                Console.WriteLine("ID not Exist");
                return false;
            }
        }

        #region for test
        private static void SetTestQueueInput(string filename)
        {
            string line;
            StreamReader file = new StreamReader(filename);
            while ((line = file.ReadLine()) != null)
            {
                string[] idPw = line.Split(' ');;
                TestQue.Enqueue(line);
            }
            file.Close();
        }
        static Queue<string> TestQue = new Queue<string>();
        #endregion
    }
}
