using AcpmfData;
using Server;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Driver {
   class TelemetryParser {
      SPageFilePhysics  physics_info;
      SPageFileGraphics graphics_info;
      SPageFileStatic   static_info;

      IntPtr ptr_physics;
      IntPtr ptr_graphics;
      IntPtr ptr_static;

      const string DLL_DATA_FILE = @".\acc_telemetry.dll";

      public TelemetryParser() {
         InitializeTelemetry();
      }

      ~TelemetryParser() {
         Marshal.FreeHGlobal(ptr_physics);
         Marshal.FreeHGlobal(ptr_graphics);
         Marshal.FreeHGlobal(ptr_static);
         ptr_physics  = IntPtr.Zero;
         ptr_graphics = IntPtr.Zero;
         ptr_static   = IntPtr.Zero;
      }

      public void TestPrintData() {
         while(true){
            UpdateAllTelemetrySources();
            Console.Clear();

            Console.WriteLine($"Current Packet: {this.physics_info.packetId}\n");
            Console.WriteLine("Temperatures:");
            Console.WriteLine($"   Air:     {this.physics_info.airTemp:00} c");
            Console.WriteLine($"   Track:   {this.physics_info.roadTemp:00} c\n");
            
            Console.WriteLine($"Speed:      {this.physics_info.speedKmh:000.0} km/h");
            Console.WriteLine($"throttle:   {this.physics_info.gas:0.00%}");
            Console.WriteLine($"brake:      {this.physics_info.brake:0.00%}\n");
            Console.WriteLine($"RPM:        {this.physics_info.rpms}");
            Console.WriteLine($"gear:       {this.physics_info.gear}");
            Console.WriteLine($"fuel:       {this.physics_info.fuel:0.00} L\n");

            Console.WriteLine("---------------------------------------------\n");
            Console.WriteLine($"Lap Time:   {this.graphics_info.currentTime}");
            Console.WriteLine($"Last Lap:   {this.graphics_info.lastTime}");
            Console.WriteLine($"Best Lap:   {this.graphics_info.bestTime}\n");

            Console.WriteLine($"Current Track:  {this.static_info.track}");

            Thread.Sleep(16);
         }

         return;
      }

      public List<string> GetDataToSend() {
         UpdateAllTelemetrySources();

         List<string> stringy_data = new List<string>();
         stringy_data.Add($"Current Packet: {this.physics_info.packetId}\n");
         stringy_data.Add("Temperatures:\n");
         stringy_data.Add($"   Air:     {this.physics_info.airTemp:00} c\n");
         stringy_data.Add($"   Track:   {this.physics_info.roadTemp:00} c\n\n");
         
         stringy_data.Add($"speed:      {this.physics_info.speedKmh:000.0} km/h\n");
         stringy_data.Add($"throttle:   {this.physics_info.gas:0.00%}\n");
         stringy_data.Add($"brake:      {this.physics_info.brake:0.00%}\n");
         stringy_data.Add($"RPM:        {this.physics_info.rpms}\n");
         stringy_data.Add($"gear:       {this.physics_info.gear}\n");
         stringy_data.Add($"fuel:       {this.physics_info.fuel:0.00} L\n\n");

         stringy_data.Add("---------------------------------------------\n");
         stringy_data.Add($"Lap Time:   {this.graphics_info.currentTime} : ");
         stringy_data.Add($"Last Lap:   {this.graphics_info.lastTime} : ");
         stringy_data.Add($"Best Lap:   {this.graphics_info.bestTime}\n");

         stringy_data.Add($"Current Track:  {this.static_info.track}");

         return stringy_data;
      }

      void InitializeTelemetry() {
         this.physics_info = new SPageFilePhysics();
         this.ptr_physics = Marshal.AllocHGlobal(Marshal.SizeOf(physics_info));

         this.graphics_info = new SPageFileGraphics();
         this.ptr_graphics = Marshal.AllocHGlobal(Marshal.SizeOf(graphics_info));

         this.static_info = new SPageFileStatic();
         this.ptr_static = Marshal.AllocHGlobal(Marshal.SizeOf(static_info));
      }

      void UpdateAllTelemetrySources() {
            // TODO: maybe update these exception messages
            if (!UpdatePhysicsData(this.ptr_physics, Marshal.SizeOf(physics_info)))
               throw new System.Exception("Physics data corrupted");
            this.physics_info = Marshal.PtrToStructure<SPageFilePhysics>(this.ptr_physics);

            if (!UpdateGraphicsData(this.ptr_graphics, Marshal.SizeOf(graphics_info)))
               throw new System.Exception("Graphics data corrupted");
            this.graphics_info = Marshal.PtrToStructure<SPageFileGraphics>(ptr_graphics);

            if (!UpdateStaticData(this.ptr_static, Marshal.SizeOf(static_info)))
               throw new System.Exception("Static data corrupted");
            this.static_info = Marshal.PtrToStructure<SPageFileStatic>(this.ptr_static);
      }
      
      void CustomPrintError(string message) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
      }

#region DllImports
      [DllImport(DLL_DATA_FILE, EntryPoint="update_physics_data")]
      static extern bool UpdatePhysicsData(IntPtr PhysicsData, int data_size);

      [DllImport(DLL_DATA_FILE, EntryPoint="update_graphics_data")]
      static extern bool UpdateGraphicsData(IntPtr GraphicsData, int data_size);

      [DllImport(DLL_DATA_FILE, EntryPoint="update_static_data")]
      static extern bool UpdateStaticData(IntPtr StaticData, int data_size);
#endregion
   }

   class CLI {
      public static void Main(string[] args) {
         //TelemetryParser driver_telemetry = new TelemetryParser();
         //driver_telemetry.main();

         TelemetryServer output_sver = new TelemetryServer();
         output_sver.ExecuteUDPServer();
      }
   }
}
