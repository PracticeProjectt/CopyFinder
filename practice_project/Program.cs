using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace practice_project
{
    public class CompareFiles
    {
        const int BYTES_TO_READ = sizeof(Int64);

        public static bool FilesAreEqual(FileInfo first, FileInfo second)
        {
            if (first.Length != second.Length)
                return false;

            //if (string.Equals(first.FullName, second.FullName, StringComparison.OrdinalIgnoreCase))
            //    return true;

            int iterations = (int)Math.Ceiling((double)first.Length / BYTES_TO_READ);

            using (FileStream fs1 = first.OpenRead())
            using (FileStream fs2 = second.OpenRead())
            {
                byte[] one = new byte[BYTES_TO_READ];
                byte[] two = new byte[BYTES_TO_READ];

                for (int i = 0; i < iterations; i++)
                {
                    fs1.Read(one, 0, BYTES_TO_READ);
                    fs2.Read(two, 0, BYTES_TO_READ);

                    if (BitConverter.ToInt64(one, 0) != BitConverter.ToInt64(two, 0))
                        return false;
                }
            }
            Console.WriteLine("Copy: " + second.Directory + '\\' + second.Name);
            return true;
        }



    }
    public class RecursiveFileProcessor
    {

        // Process all files in the directory passed in, recurse on any directories 
        // that are found, and process the files they contain.
        public static void ProcessDirectory(string targetDirectory, FileInfo fileToFind)
        {
            try
            {
                // Process the list of files found in the directory.
                string[] fileEntries = Directory.GetFiles(targetDirectory);
                using (fileToFind.OpenRead())
                {
                    foreach (string fileName in fileEntries)
                    {
                        FileInfo file = new FileInfo(fileName);
                        CompareFiles.FilesAreEqual(fileToFind, file);
                    }
                }
                // Recurse into subdirectories of this directory.
                string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
                foreach (string subdirectory in subdirectoryEntries)
                    ProcessDirectory(subdirectory, fileToFind);
            }
            catch (System.UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        // Insert logic for processing found files here.
        public static void CompareFile(string path)
        {
            Console.WriteLine($"Processed file '{path}'.");
        }
        public static void CheckAllFiles(string directoryName, FileInfo file_to_find)
        {
            //foreach (string path in directoryName)
            //{
            //    //if (File.Exists(path))
            //    //{
            //    //    // This path is a file
            //    //    ProcessFile(path);
            //    //}
            //    //else 
            if (Directory.Exists(directoryName))
            {
                // This path is a directory
                ProcessDirectory(directoryName, file_to_find);
            }
            else
            {
                Console.WriteLine("{0} is not a valid file or directory.", directoryName);
            }
        }


    }
    class Program
    {

        static void Main(string[] args)
        {
            string path1 = @"D:\Programming\C++\Project1\Project1\Project1\Source.cpp";
            FileInfo fileInfo1 = new FileInfo(path1);
            string path2 = @"D:\";
            RecursiveFileProcessor.CheckAllFiles(path2, fileInfo1);
            Console.WriteLine("Finished");
            Console.ReadKey();
        }
    }
}
