using System;
using System.Text;
namespace HelloWorldApplication
{
   class HelloWorld
   {
      static void Main(string[] args)
      {
byte[] buf = new byte[760] {0xfc,0x48};
		  byte[] encoded = new byte[buf.Length];
for (int i = 0; i < buf.Length; i++)
{
    encoded[i] = (byte)(((uint)buf[i] + 5) & 0xFF);
}

StringBuilder hex = new StringBuilder(encoded.Length * 2);
foreach (byte b in encoded)
{
    hex.AppendFormat("0x{0:x2}, ", b);
}
Console.WriteLine("The payload length: " + hex.Length);
Console.WriteLine("The payload is: " + hex.ToString());
      }
   }
}