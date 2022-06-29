using AcpmfData;
using SharedMemory;
using System;

namespace Driver {
   class TelemetryParser {
      const string FILE_NAME_GRAPHICS = "Local\\acpmf_graphics";
      const string FILE_NAME_PHYSICS  = "Local\\acpmf_physics";
      const string FILE_NAME_STATIC   = "Local\\acpmf_static";

      public static int Main(string[] args) {
         string file_name = FILE_NAME_STATIC;
         BufferReadWrite data_buffer = OpenBuffer(file_name);

         ReadStaticTelemetry(data_buffer);

         return 0;
      }

      static BufferReadWrite OpenBuffer(string file_name) {
         BufferReadWrite data_buffer;

         try {
            Console.WriteLine("Start to read file '{0}'", file_name);
            data_buffer = new BufferReadWrite(name: file_name);

            return data_buffer;
         }
         catch(System.UnauthorizedAccessException) {
            CustomPrintError(string.Format("Unable to open '{0}', access denied.", file_name));
            throw;
         }
         catch(System.IO.FileNotFoundException) {
            CustomPrintError(string.Format("Couldn't find file '{0}', is ACC running?", file_name));
            throw;
         }
      }

      static void CustomPrintError(string message) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
      }

      static void ReadStaticTelemetry(BufferReadWrite buffer) {
         while(true){
            SPageFileStatic read_data;
            buffer.Read<SPageFileStatic>(out read_data);

            Console.Write(read_data);
            Thread.Sleep(250);
         }
      }

      static void ReadBufferWithInt(BufferReadWrite buffer) {
         while(true){
            int read_data;
            buffer.Read<int>(out read_data);

            Console.Write("{0}\r", read_data);
            Thread.Sleep(250);
         }
      }
   }
}
