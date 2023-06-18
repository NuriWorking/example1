using System;
using System.Collections.Generic;
using System.IO;

namespace SP_Example
{
    class FileIO
    {
        static void Mainn(string[] args)
        {
            //파일 읽기
            static void PrintFile(string fileName)
            {
                //한줄씩
                string[] lines = File.ReadAllLines(fileName);
                //전체
                string all = File.ReadAllText(fileName);

                //streamreader활용
                string line;
                using (StreamReader file = new StreamReader(fileName/*, encoding:System.Text.Encoding.UTF8*/)) //streamreader에 encoding설정가능^^
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        System.Console.WriteLine(line);
                    }
                }
            }

            //string 파일로 저장
            static void WriteAllText(string path, string inputText)
            {
                File.WriteAllText(path, inputText);
            }

            //string[] 파일로 저장
            static void WriteTextByLine(string path, string[] lines)
            {
                //1.
                File.WriteAllLines(path, lines);
                
                //2.streamwriter활용
                using (StreamWriter sr = new StreamWriter(path))
                {
                    foreach (string line in lines)
                    {
                        sr.WriteLine(line);
                    }
                }
            }

            //텍스트 파일 읽어서 특정 위치에 복사
            static void CopyFile(string inputFilename, string outputFilename)
            {
                const int BUF_SIZE = 4096;
                byte[] buffer = new byte[BUF_SIZE];
                int readLength;
                FileStream fs_in =
                new FileStream(inputFilename, FileMode.Open, FileAccess.Read);
                FileStream fs_out =
                new FileStream(outputFilename, FileMode.Create, FileAccess.Write);
                while ((readLength = fs_in.Read(buffer, 0, BUF_SIZE)) > 0)
                {
                    fs_out.Write(buffer, 0, readLength);
                }
                fs_in.Close();
                fs_out.Close();
            }
            //CopyFile(".\\data\\print_sample.txt", ".\\data\\print_sample_copy.txt");

            //특정 위치에 존재하는 파일 리스트 출력
            void FileDirList(string path)
            {
                string[] subdirectoryEntries = Directory.GetDirectories(path);
                foreach (string subdirectory in subdirectoryEntries)
                    Console.WriteLine("[{0}]", subdirectory);
                string[] fileEntries = Directory.GetFiles(".");
                foreach (string fileName in fileEntries)
                    Console.WriteLine(fileName);
            }
            //FileDirList(".");

            //특정 위치 하위에 존재하는 모든 파일 리스트 출력
            void FileDirAllList(string path)
            {
                DirectoryInfo di = new DirectoryInfo(path);
                FileInfo[] fiArr = di.GetFiles("*.*", SearchOption.AllDirectories);
                foreach (var f in fiArr)
                {
                    Console.WriteLine(f.Name);
                }
            }
            //FileDirAllList(".");

            //1.INPUT 폴더 하위에 위치한 파일들의 파일명(상대경로 포함), 크기를 Console화면에
            //출력하시오. 
            //2. INPUT 폴더 하위에 위치한 파일들 중 크기가 3Kbyte가 넘는 파일들은 모두
            //OUTPUT 폴더로 복사하시오. (OUTPUT 폴더 및 서브 폴더 생성)
            //단, 파일 복사 시 바이너리 파일을 버퍼에 읽고 쓰는 방식으로 구현하고, 버퍼의 크기는
            //512Byte로 설정하시오.
            static void MyCopyFile(string path, string filename)
            {
                string inFile = ".\\INPUT\\" + path + "\\" + filename;
                string outPath = ".\\OUTPUT\\" + path;
                string outFile = outPath + "\\" + filename;

                //폴더 생성
                System.IO.Directory.CreateDirectory(outPath);

                const int BUF_SIZE = 512;
                byte[] buffer = new byte[BUF_SIZE];
                int nFReadLen;

                FileStream fs_in = new FileStream(inFile, FileMode.Open, FileAccess.Read);
                FileStream fs_out = new FileStream(outFile, FileMode.Create, FileAccess.Write);
                while ((nFReadLen = fs_in.Read(buffer, 0, BUF_SIZE)) > 0)
                {
                    fs_out.Write(buffer, 0, nFReadLen);
                }
                fs_in.Close();
                fs_out.Close();
            }
            void Run_IO()
            {
                //폴더 정보
                DirectoryInfo di = new DirectoryInfo("./INPUT");
                //파일 정보
                FileInfo[] fiArr = di.GetFiles("*.*", SearchOption.AllDirectories);
                foreach (var f in fiArr)
                {
                    long fSize = f.Length;
                    string fName = f.Name;
                    string path = f.DirectoryName.Substring(di.FullName.Length);

                    //C:\Users\nurri\source\repos\SP_Example\SP_Example\bin\Debug\net5.0\INPUT\DOC\CSS
                    //C:\Users\nurri\source\repos\SP_Example\SP_Example\bin\Debug\net5.0\INPUT <-길이만큼 substring
                    
                    Console.WriteLine(".{0}\\{1}: {2}bytes.", path, fName, fSize);

                    if (f.Length > 3 * 1024) // 3Kbyte
                    {
                        MyCopyFile(path, fName);
                    }
                }
            }
            Run_IO();

        }
    }
}
