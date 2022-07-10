using ACCSharedMemoryDefinitions;
using Server;
using System.Runtime.InteropServices;
using System.Text.Json;

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
         Console.WriteLine(String.Join("\n", this.GetStringTelemetryData()));
      }

      public string GetJSONTelemetryData() {
         this.UpdateAllTelemetrySources();

         JsonSerializerOptions json_options = new JsonSerializerOptions();
         // For human readability in debug
         //json_options.WriteIndented = true;
         json_options.IncludeFields = true;

         ACCSharedMemoryDefinitionsPack packed_data = new ACCSharedMemoryDefinitionsPack();
         packed_data.graphics_data = this.graphics_info;
         packed_data.physics_data = this.physics_info;
         packed_data.static_data = this.static_info;

         return JsonSerializer.Serialize<ACCSharedMemoryDefinitionsPack>(packed_data, json_options);
      }

      public List<string> GetStringTelemetryData() {
         UpdateAllTelemetrySources();

         List<string> data = new List<string>();

         data.Add("----------------------------------------------\n");
         data.Add($"Current Packet: {this.physics_info.packetId}");
         data.Add("Temperatures:");
         data.Add($"   Air:      {this.physics_info.airTemp:00} c");
         data.Add($"   Track:    {this.physics_info.roadTemp:00} c");
         data.Add("\n");
         
         data.Add($"speed:       {this.physics_info.speedKmh:000.0} km/h");
         data.Add($"throttle:    {this.physics_info.gas:0.00%}");
         data.Add($"brake:       {this.physics_info.brake:0.00%}");
         data.Add($"RPM:         {this.physics_info.rpms}");
         data.Add($"gear:        {this.physics_info.gear}");
         data.Add($"fuel:        {this.physics_info.fuel:0.00} L");
         data.Add($"fuel / lap:  {this.graphics_info.fuelXLap:0.00} L");
         data.Add("\n");

         data.Add("-------------------------------\n");
         data.Add($"Lap Time:    {this.graphics_info.currentTime}");
         data.Add($"Last Lap:    {this.graphics_info.lastTime}");
         data.Add($"Best Lap:    {this.graphics_info.bestTime}");
         data.Add("\n");

         data.Add($"Current Track:  {this.static_info.track}");
         data.Add("\n");

         return data;
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
         TelemetryServer output_server = new TelemetryServer();
         output_server.ExecuteUDPServer(args);
      }

      public static void CustomPrintError(string message) {
         Console.ForegroundColor = ConsoleColor.Red;
         Console.WriteLine(message);
         Console.ForegroundColor = ConsoleColor.White;
      }
   }
}
