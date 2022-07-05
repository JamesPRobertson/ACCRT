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

            Console.WriteLine($"Current Packet: {physics_info.packetId}\n");
            Console.WriteLine("Temperatures:");
            Console.WriteLine($"   Air:     {physics_info.airTemp}");
            Console.WriteLine($"   Track:   {physics_info.roadTemp}\n");
            
            Console.WriteLine($"gas:   {physics_info.gas}");
            Console.WriteLine($"brake: {physics_info.brake}\n");
            Console.WriteLine($"RPM:   {physics_info.rpms}");
            Console.WriteLine($"gear:  {physics_info.gear}");
            Console.WriteLine($"fuel:  {physics_info.fuel}\n");

            Console.WriteLine( "Velocities: ");
            Console.WriteLine($"   Speed: {physics_info.speedKmh}");
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
            UpdatePhysics(this.ptr_physics);
            this.physics_info = Marshal.PtrToStructure<SPageFilePhysics>(ptr_physics);

            UpdateGraphics(this.ptr_graphics);
            this.graphics_info = Marshal.PtrToStructure<SPageFileGraphics>(ptr_graphics);

            UpdateStatic(this.ptr_static);
            this.static_info = Marshal.PtrToStructure<SPageFileStatic>(ptr_static);
      }
      
      void CustomPrintError(string message) {
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

   class CLI {
      public static void Main(string[] args) {
         TelemetryParser driver_telemetry = new TelemetryParser();
         driver_telemetry.main();
      }
   }
}
