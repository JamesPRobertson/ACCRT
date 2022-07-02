using AcpmfData;
using SharedMemory;
using System.Runtime.InteropServices;

namespace Driver {
   class TelemetryParser {
      const string FILE_NAME_GRAPHICS = "Local\\acpmf_graphics";
      const string FILE_NAME_PHYSICS  = "Local\\acpmf_physics";
      const string FILE_NAME_STATIC   = "Local\\acpmf_static";

      /*
      public static int Main(string[] args) {
         string file_name = FILE_NAME_PHYSICS;
         BufferReadWrite data_buffer = OpenBuffer(file_name);

         //ReadStaticTelemetry(data_buffer);
         //ReadBufferWithInt(data_buffer);
         ReadPhysicsTelemtry(data_buffer);

         return 0x0;
      }
      */

      public static int Main(string[] args) {
         InitPhysics();
         SPageFilePhysics physics_struct = FillPhysics();
         Console.WriteLine(physics_struct);
         Console.WriteLine("Did it work?");
         Console.ReadKey();

         while(true){
            Console.Clear();
            PrintPhysics();
            Thread.Sleep(16);
         }
         
         return 0;
      }



      static BufferReadWrite OpenBuffer(string file_name) {
         BufferReadWrite data_buffer;

         try {
            Console.WriteLine($"Start to read file '{file_name}'");
            data_buffer = new BufferReadWrite(name: file_name);

            return data_buffer;
         }
         catch(System.UnauthorizedAccessException) {
            CustomPrintError(string.Format($"Unable to open '{file_name}', access denied."));
            throw;
         }
         catch(System.IO.FileNotFoundException) {
            CustomPrintError(string.Format($"Couldn't find file '{file_name}', is ACC running?"));
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

      static void ReadPhysicsTelemtry(BufferReadWrite buffer) {
         while(true){
            SPageFilePhysics read_data;
            buffer.Read<SPageFilePhysics>(out read_data);

            Console.Write(read_data);
            Console.WriteLine("--------------------------------");
            Thread.Sleep(250);
         }
      }

      static void ReadBufferWithInt(BufferReadWrite buffer) {
         while(true){
            int read_data;
            long buffer_size = buffer.BufferSize;
            for (int buffer_position = 0; buffer_position < buffer_size; buffer_position++){
               buffer.Read<int>(out read_data, buffer_position);

               Console.Write($"{read_data}\r");
            }
         }
      }

#region DllImports
      [DllImport(".\\acc_telemetry.dll", EntryPoint="set_up_number")]
      static extern void SetUpNumber();

      [DllImport(".\\acc_telemetry.dll", EntryPoint="get_test_number")]
      static extern int GetTestNumber();

      [DllImport(".\\acc_telemetry.dll", EntryPoint="init_physics")]
      static extern void InitPhysics();

      [DllImport(".\\acc_telemetry.dll", EntryPoint="print_physics")]
      static extern void PrintPhysics();

      [DllImport(".\\acc_telemetry.dll", EntryPoint="fill_physics")]
      static extern SPageFilePhysics FillPhysics();

#endregion
   }
}
