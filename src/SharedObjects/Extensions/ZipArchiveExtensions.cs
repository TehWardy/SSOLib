using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace SharedObjects.Extensions
{
    public static class ZipArchiveExtensions
    {
        public static async ValueTask<T[]> DeserializeAsync<T>(this ZipArchiveEntry entry) => Data.ParseJson<T[]>(await new StreamReader(entry.Open()).ReadToEndAsync());

        public static void AddTextFile(this ZipArchive zip, string path, string text)
        {
            using StreamWriter s = new(zip.CreateEntry(path, CompressionLevel.Optimal).Open());
            s.Write(text);
            s.Flush();
        }

        public static int Depth(this ZipArchiveEntry entry) 
            => entry.FullName.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length;
    }
}