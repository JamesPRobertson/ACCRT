using AcpmfData;
using SharedMemory;
using System;

namespace Driver {
   class Thing {
      const string FILE_NAME_GRAPHICS = "Local\\acpmf_graphics";
      const string FILE_NAME_PHYSICS  = "Local\\acpmf_physics";
      const string FILE_NAME_STATIC   = "Local\\acpmf_static";

      public static int Main(string[] args) {
         BufferReadWrite data_buffer;
         string file_name = FILE_NAME_STATIC;

         try {
            Console.WriteLine("Start to read file '{0}'", file_name);
            data_buffer = new BufferReadWrite(name: file_name);
         }
         catch(System.UnauthorizedAccessException) {
            CustomPrintError(string.Format("Unable to open '{0}', access denied.", file_name));
            return 0xFF;
         }
         catch(System.IO.FileNotFoundException) {
            CustomPrintError(string.Format("Couldn't find file '{0}', is ACC running?", file_name));
            return 0xFF;
         }

         while(true){
            SPageFileStatic read_data;
            data_buffer.Read<SPageFileStatic>(out read_data);
            Console.Write(read_data);
            Thread.Sleep(250);
         }
         
         /*
         while(true){
            int read_data;
            data_buffer.Read<int>(out read_data);
            Console.Write("{0}\r", read_data);
            Thread.Sleep(250);
         }
         */
      }

      public static void CustomPrintError(string message) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
      }
   }
}
