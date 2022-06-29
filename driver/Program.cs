// Suppresses unused variables warning for the struct that follows.
#pragma warning disable CS0414

using SharedMemory;
using System;


struct SPageFileStatic {
   string smVersion = "";
   string acVersion = "";

   // session static info
   int numberOfSessions = 0;
   int numCars = 0;
   string carModel = "";
   string track = "";
   string playerName = "";
   string playerSurname = "";
   string playerNick = "";
   int sectorCount = 0;

   // car static info
   float maxTorque = 0;
   float maxPower = 0;
   int	maxRpm = 0;
   float maxFuel = 0f;
   float suspensionMaxTravel = 0f;
   float tyreRadius = 0f;
   float maxTurboBoost = 0f;

   float deprecated_1 = -273f;
   float deprecated_2 = -273f;

   int penaltiesEnabled = 0;

   float aidFuelRate = 0f;
   float aidTireRate = 0f;
   float aidMechanicalDamage = 0f;
   int aidAllowTyreBlankets = 0;
   float aidStability = 0f;
   int aidAutoClutch = 0;
   int aidAutoBlip = 0;

   int hasDRS = 0;
   int hasERS = 0;
   int hasKERS = 0;
   float kersMaxJ = 0f;
   int engineBrakeSettingsCount = 0;
   int ersPowerControllerCount = 0;
   float trackSPlineLength = 0f;
   string trackConfiguration = "";
   float ersMaxJ = 0;

   int isTimedRace = 0;
   int hasExtraLap = 0;

   string carSkin = "";
   int reversedGridPositions = 0;
   int PitWindowStart = 0;
   int PitWindowEnd = 0;
   int isOnline = 0;
   public SPageFileStatic() {}
};

/*
struct SPageFileGraphic {
   int packetId = 0;
   int status = 0;
   int session = 0;
   string currentTime = "";
   string lastTime = "";
   string bestTime = "";
   string split = "";
   int completedLaps = 0;
   int position = 0;
   int iCurrentTime = 0;
   int iLastTime = 0;
   int iBestTime = 0;
   float sessionTimeLeft = 0;
   float distanceTraveled = 0;
   int isInPit = 0;
   int currentSectorIndex = 0;
   int lastSectorTime = 0;
   int numberOfLaps = 0;
   string tyreCompound = "";
   float replayTimeMultiplier = 0;
   float normalizedCarPosition = 0;

   int activeCars = 0;
   //float carCoordinates[60][3];
   //int carID[60];
   int playerCarID = 0;
   float penaltyTime = 0;
   int flag = 0;
   //PenaltyShortcut penalty = PenaltyShortcut::None;
   int idealLineOn = 0;
   int isInPitLane = 0;

   float surfaceGrip = 0;
   int mandatoryPitDone = 0;

   float windSpeed = 0;
   float windDirection = 0;

   int isSetupMenuVisible = 0;

   int mainDisplayIndex = 0;
   int secondaryDisplayIndex = 0;
   int TC = 0;
   int TCCut = 0;
   int EngineMap = 0;
   int ABS = 0;
   int fuelXLap = 0;
   int rainLights = 0;
   int flashingLights = 0;
   int lightsStage = 0;
   float exhaustTemperature = 0.0f;
   int wiperLV = 0;
   int DriverStintTotalTimeLeft = 0;
   int DriverStintTimeLeft = 0;
   int rainTyres = 0;
   public SPageFileGraphic() {}
};
*/

// Restore the warning for unassigned variables.
#pragma warning restore CS0414


namespace Driver {
   class Thing {
      const string FILE_NAME_GRAPHICS = "Local\\acpmf_graphics";
      const string FILE_NAME_PHYSICS  = "Local\\acpmf_physics";
      const string FILE_NAME_STATIC   = "Local\\acpmf_static";

      public static void Main(string[] args) {
         BufferReadWrite data_buffer;
         string file_name = FILE_NAME_STATIC;

         try {
            Console.WriteLine("Start to read file '{0}'", file_name);
            data_buffer = new BufferReadWrite(name: file_name);
         }
         catch(System.UnauthorizedAccessException) {
            CustomPrintError(string.Format("Unable to open '{0}', access denied.", file_name));
            return;
         }
         catch(System.IO.FileNotFoundException) {
            CustomPrintError(string.Format("Couldn't find file {0}, is ACC running?", file_name));
            return;
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
            static_buffer.Read<int>(out read_data);
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
