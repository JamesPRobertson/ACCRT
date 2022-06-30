// Disable warning about unused variables for development
#pragma warning disable CS0414

namespace AcpmfData {
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


   struct SPageFilePhysics {
      int packetId = 0;
      float gas = 0;
      float brake = 0;
      float fuel = 0;
      int gear = 0;
      int rpms = 0;
      float steerAngle = 0;
      float speedKmh = 0;
      float[] velocity = new float[3];
      float[] accG = new float[3];
      float[] wheelSlip = new float[4];
      float[] wheelLoad = new float[4];
      float[] wheelsPressure = new float[4];
      float[] wheelAngularSpeed = new float[4];
      float[] tyreWear = new float[4];
      float[] tyreDirtyLevel = new float[4];
      float[] tyreCoreTemperature = new float[4];
      float[] camberRAD = new float[4];
      float[] suspensionTravel = new float[4];
      float drs = 0;
      float tc = 0;
      float heading = 0;
      float pitch = 0;
      float roll = 0;
      float cgHeight = 0;
      float[] carDamage = new float[5];
      int numberOfTyresOut = 0;
      int pitLimiterOn = 0;
      float abs = 0;
      float kersCharge = 0;
      float kersInput = 0;
      int autoShifterOn = 0;
      float[] rideHeight = new float[2];
      float turboBoost = 0;
      float ballast = 0;
      float airDensity = 0;
      float airTemp = 0;
      float roadTemp = 0;
      float[] localAngularVel = new float[3];
      float finalFF = 0;
      float performanceMeter = 0;

      int engineBrake = 0;
      int ersRecoveryLevel = 0;
      int ersPowerLevel = 0;
      int ersHeatCharging = 0;
      int ersIsCharging = 0;
      float kersCurrentKJ = 0;

      int drsAvailable = 0;
      int drsEnabled = 0;

      float[] brakeTemp = new float[4];
      float clutch = 0;

      float[] tyreTempI = new float[4];
      float[] tyreTempM = new float[4];
      float[] tyreTempO = new float[4];

      int isAIControlled = 0;

      float[,] tyreContactPoint = new float[4,3];
      float[,] tyreContactNormal = new float[4,3];
      float[,] tyreContactHeading = new float[4,3];

      float brakeBias = 0;

      float[] localVelocity = new float[3];

      int P2PActivations = 0;
      int P2PStatus = 0;

      int currentMaxRpm = 0;

      float[] mz = new float[4];
      float[] fx = new float[4];
      float[] fy = new float[4];
      float[] slipRatio = new float[4];
      float[] slipAngle = new float[4];


      int tcinAction = 0;
      int absInAction = 0;
      float[] suspensionDamage = new float[4];
      float[] tyreTemp = new float[4];

      public SPageFilePhysics() {}
   };

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
}

// Reenable the warning for unused variables
#pragma warning restore CS0414