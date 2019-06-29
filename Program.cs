using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace Пересдача_задание_1
{
    /*
     * 1. В первом потоке происходит поиск по маске файлов в определенном каталоге. 
     * Найденные файлы передаются для обработки во второй поток, который считает 
     * занимаемое ими место на диске и выводит на экран статистику. 
     * Реализация предпочтительна с использованием событий.
     */

    class Program
    {
        static void Main()
        {
            Dictionary<string, string> mainqueue = new Dictionary<string, string>();
            Thread FindFilesThread = new Thread(() => FindFiles(mainqueue));
            FindFilesThread.Start();

            
        }
        static void FindFiles(Dictionary<string, string> _mainqueue)
        {
            string mask = null;
            string path = null;
            
            Console.WriteLine("Укажите полный путь к каталогу");
            path = Console.ReadLine();

            Console.WriteLine("Введите маску для поиска файлов в формате xxx.zzz");
            Console.WriteLine("Примеры написания: *a.exe *.txt  z*.z*p ");
            mask = Console.ReadLine();
            Console.WriteLine("Вы ввели маску: " +mask);

            string[] FilesPath = Directory.GetFiles(path, mask, SearchOption.AllDirectories);
            foreach (string file in FilesPath)
            {
                _mainqueue.Add(file, mask);
                Console.WriteLine("Найден файл: {0}",file);
            }

            Thread FileInfoThread = new Thread(() => CountFileInfo(_mainqueue));
            FileInfoThread.Start();
        }
        static void CountFileInfo(Dictionary<string,string> files)
        {
            long weigth = 0;
            foreach (var file in files)
            {
                FileInfo info = new FileInfo(file.Key);
                Console.WriteLine("\n" + info.Name);
                Console.WriteLine("Атрибуты файла: {0}", info.Attributes);
                if (info.Length < 1048576)
                {
                    Console.WriteLine("Размер файла: {0} Б", info.Length);
                }
                else
                {
                    Console.WriteLine("Размер файла: {0} МБ", info.Length / 1048576);
                }

                weigth += info.Length;
            }

            if (weigth < 1048576)
            {
                Console.WriteLine("Размер найденных файлов: {0} Б", weigth);
            }
            else
            {
                Console.WriteLine("Размер найденных файлов: {0} МБ", weigth / 1048576);
            }

            Console.ReadKey();
        
        }
    }
}
