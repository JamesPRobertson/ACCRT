using AcpmfData;
using System.Runtime.InteropServices;

namespace Driver {
   class TelemetryParser {
      public static int Main(string[] args) {
         SPageFilePhysics physics_struct = new SPageFilePhysics();
         IntPtr ptr_physics_struct = Marshal.AllocHGlobal(Marshal.SizeOf(physics_struct));

         SPageFileGraphics graphics_struct = new SPageFileGraphics();
         IntPtr ptr_graphics_struct = Marshal.AllocHGlobal(Marshal.SizeOf(graphics_struct));

         SPageFilePhysics static_struct = new SPageFilePhysics();
         IntPtr ptr_static_struct = Marshal.AllocHGlobal(Marshal.SizeOf(static_struct));

         while(true){
            UpdatePhysics(ptr_physics_struct);
            physics_struct = Marshal.PtrToStructure<SPageFilePhysics>(ptr_physics_struct);

            Console.Clear();

            Console.WriteLine($"brake: {physics_struct.brake}");
            Console.WriteLine($"gear:  {physics_struct.gear}");
            Console.WriteLine($"fuel:  {physics_struct.fuel}");
            Thread.Sleep(16);
         }

         Marshal.FreeHGlobal(ptr_physics_struct);
         ptr_physics_struct = IntPtr.Zero;

         return 0;
      }
      
      static void CustomPrintError(string message) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
      }

#region DllImports
      [DllImport(".\\acc_telemetry.dll", EntryPoint="update_physics_struct")]
      static extern void UpdatePhysics(IntPtr PhysicsData);

      [DllImport(".\\acc_telemetry.dll", EntryPoint="update_graphics_struct")]
      static extern void UpdateGraphics(IntPtr GraphicsData);

      [DllImport(".\\acc_telemetry.dll", EntryPoint="update_static_struct")]
      static extern void UpdateStatic(IntPtr StaticData);
#endregion
   }
}
