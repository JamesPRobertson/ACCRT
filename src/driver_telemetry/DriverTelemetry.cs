using AcpmfData;
using System.Runtime.InteropServices;

namespace Driver {
   class TelemetryParser {
      SPageFilePhysics  physics_info;
      SPageFileGraphics graphics_info;
      SPageFileStatic   static_info;

      IntPtr ptr_physics;
      IntPtr ptr_graphics;
      IntPtr ptr_static;

      public TelemetryParser() {
         InitializeTelemetryStructs();
      }

      ~TelemetryParser() {
         Marshal.FreeHGlobal(ptr_physics);
         Marshal.FreeHGlobal(ptr_graphics);
         Marshal.FreeHGlobal(ptr_static);
         ptr_physics  = IntPtr.Zero;
         ptr_graphics = IntPtr.Zero;
         ptr_static   = IntPtr.Zero;
      }

      public void main() {
         while(true){
            UpdateAllTelemetryStructs();
            Console.Clear();

            Console.WriteLine($"Current Packet: {this.physics_info.packetId}\n");
            Console.WriteLine("Temperatures:");
            Console.WriteLine($"   Air:     {this.physics_info.airTemp}");
            Console.WriteLine($"   Track:   {this.physics_info.roadTemp}\n");
            
            Console.WriteLine($"Speed:      {this.physics_info.speedKmh:000.0} km/h");
            Console.WriteLine($"throttle:   {this.physics_info.gas:0.00%}");
            Console.WriteLine($"brake:      {this.physics_info.brake:0.00%}\n");
            Console.WriteLine($"RPM:        {this.physics_info.rpms}");
            Console.WriteLine($"gear:       {this.physics_info.gear}");
            Console.WriteLine($"fuel:       {this.physics_info.fuel:0.00} L\n");

            Console.WriteLine("---------------------------------------------\n");
            Console.WriteLine($"Lap Time:    {this.graphics_info.currentTime}");
            Console.WriteLine($"Last Lap:    {this.graphics_info.lastTime}");
            Console.WriteLine($"Best Lap:    {this.graphics_info.bestTime}\n");

            Console.WriteLine($"Current Track:  {this.static_info.track}");

            Thread.Sleep(16);
         }

         return;
      }

      void InitializeTelemetryStructs() {
         this.physics_info = new SPageFilePhysics();
         this.ptr_physics = Marshal.AllocHGlobal(Marshal.SizeOf(physics_info));

         this.graphics_info = new SPageFileGraphics();
         this.ptr_graphics = Marshal.AllocHGlobal(Marshal.SizeOf(graphics_info));

         this.static_info = new SPageFileStatic();
         this.ptr_static = Marshal.AllocHGlobal(Marshal.SizeOf(static_info));
      }

      void UpdateAllTelemetryStructs() {
            UpdatePhysics(this.ptr_physics, Marshal.SizeOf(physics_info));
            this.physics_info = Marshal.PtrToStructure<SPageFilePhysics>(this.ptr_physics);

            UpdateGraphics(this.ptr_graphics, Marshal.SizeOf(graphics_info));
            this.graphics_info = Marshal.PtrToStructure<SPageFileGraphics>(ptr_graphics);

            UpdateStatic(this.ptr_static, Marshal.SizeOf(static_info));
            this.static_info = Marshal.PtrToStructure<SPageFileStatic>(this.ptr_static);
      }
      
      void CustomPrintError(string message) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
      }

#region DllImports
      [DllImport(".\\acc_telemetry.dll", EntryPoint="update_physics_struct")]
      static extern void UpdatePhysics(IntPtr PhysicsData, int struct_size);

      [DllImport(".\\acc_telemetry.dll", EntryPoint="update_graphics_struct")]
      static extern void UpdateGraphics(IntPtr GraphicsData, int struct_size);

      [DllImport(".\\acc_telemetry.dll", EntryPoint="update_static_struct")]
      static extern void UpdateStatic(IntPtr StaticData, int struct_size);
#endregion
   }

   class CLI {
      public static void Main(string[] args) {
         Console.WriteLine("Initializing...");
         TelemetryParser driver_telemetry = new TelemetryParser();
         driver_telemetry.main();
      }
   }
}
