using System;
using System.IO;
using System.IO.Compression;

namespace ServerAspNetCoreLinux.ServerCore.Utilities
{
    public static class FileHelper
    {
        public static void CreateFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }
        }

        public static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static void CreateDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        public static void DeleteDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }

        public static void CompressFile(string filePath, string archivedFilePath)
        {
            if (File.Exists(filePath))
            {
                using (var sourceStream = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    using (var targetStream = File.Create(archivedFilePath))
                    {
                        using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                        {
                            sourceStream.CopyTo(compressionStream);
                            Console.WriteLine("File compress succesfully");
                        }
                    }
                }
            }
        }

        public static void DecompressFile(string archivedFilePath, string unarchivedFilePath)
        {
            if (File.Exists(archivedFilePath))
            {
                using (FileStream sourceStream = new FileStream(archivedFilePath, FileMode.OpenOrCreate))
                {
                    using (FileStream targetStream = File.Create(unarchivedFilePath))
                    {
                        using (GZipStream decompressionStream =
                            new GZipStream(sourceStream, CompressionMode.Decompress))
                        {
                            decompressionStream.CopyTo(targetStream);
                            Console.WriteLine("File decompress succesfully");
                        }
                    }
                }
            }
        }

        public static void DeleteCompressedFile(string compressedFile)
        {
            if (File.Exists(compressedFile))
            {
                File.Delete(compressedFile);
            }
        }

        public static void CompressDirectory(string directoryPath, string archivedDirectoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                ZipFile.CreateFromDirectory(directoryPath, archivedDirectoryPath);
            }
        }

        public static void DecompressDirectory(string archivedDirectoryPath, string unarchivedDirectoryPath)
        {
            if (File.Exists(archivedDirectoryPath))
            {
                Directory.CreateDirectory(unarchivedDirectoryPath);
                ZipFile.ExtractToDirectory(archivedDirectoryPath, unarchivedDirectoryPath);
            }
        }

        public static void CompressFileArray(string[] files, string zipFiles)
        {
            if (File.Exists(files.ToString()))
            {
                ZipFile.CreateFromDirectory(files.ToString(), zipFiles);
            }
        }
    }
}