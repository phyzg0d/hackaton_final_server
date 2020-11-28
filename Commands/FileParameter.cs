namespace ServerAspNetCoreLinux.Commands
{
    public class FileParameter
    {
        public byte[] File { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public FileParameter(byte[] file, string filename, string contenttype)
        {
            File = file;
            FileName = filename;
            ContentType = contenttype;
        }
    }
}