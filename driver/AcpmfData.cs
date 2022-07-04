// Disable warning about unused variables for development
#pragma warning disable CS0414

using System.Runtime.InteropServices;

namespace AcpmfData {
   struct SPageFileGraphics {
      int packetId;
      int status;
      int session;
      IntPtr currentTime;
      IntPtr lastTime;
      IntPtr bestTime;
      IntPtr split;
      int completedLaps;
      int position;
      int iCurrentTime;
      int iLastTime;
      int iBestTime;
      float sessionTimeLeft;
      float distanceTraveled;
      int isInPit;
      int currentSectorIndex;
      int lastSectorTime;
      int numberOfLaps;
      IntPtr tyreCompound;
      float replayTimeMultiplier;
      float normalizedCarPosition;

      int activeCars;
      //float carCoordinates[60][3];
      //int carID[60];
      int playerCarID;
      float penaltyTime;
      int flag;
      //PenaltyShortcut penalty = PenaltyShortcut::None;
      int idealLineOn;
      int isInPitLane;

      float surfaceGrip;
      int mandatoryPitDone;

      float windSpeed;
      float windDirection;

      int isSetupMenuVisible;

      int mainDisplayIndex;
      int secondaryDisplayIndex;
      int TC;
      int TCCut;
      int EngineMap;
      int ABS;
      int fuelXLap;
      int rainLights;
      int flashingLights;
      int lightsStage;
      float exhaustTemperature;
      int wiperLV;
      int DriverStintTotalTimeLeft;
      int DriverStintTimeLeft;
      int rainTyres;
   };

   //[StructLayout(LayoutKind.Sequential)]
   public struct SPageFilePhysics {
      int packetId;
      float gas;
      public float brake;
      public float fuel;
      public int gear;
      int rpms;
      float steerAngle;
      float speedKmh;    
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
      public float[] velocity;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
      float[] accG;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      float[] wheelSlip;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      float[] wheelLoad;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      float[] wheelsPressure;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      float[] wheelAngularSpeed;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      float[] tyreWear;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      float[] tyreDirtyLevel;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      float[] tyreCoreTemperature;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      float[] camberRAD;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      float[] suspensionTravel;
      float drs;
      float tc;
      float heading;
      float pitch;
      float roll;
      float cgHeight;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
      float[] carDamage;
      int numberOfTyresOut;
      int pitLimiterOn;
      float abs;
      float kersCharge;
      float kersInput;
      int autoShifterOn;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
      float[] rideHeight;
      float turboBoost;
      float ballast;
      float airDensity;
      float airTemp;
      float roadTemp;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
      float[] localAngularVel;
      float finalFF;
      float performanceMeter;

      int engineBrake;
      int ersRecoveryLevel;
      int ersPowerLevel;
      int ersHeatCharging;
      int ersIsCharging;
      float kersCurrentKJ;

      int drsAvailable;
      int drsEnabled;

      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      float[] brakeTemp;
      float clutch;

      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      float[] tyreTempI;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      float[] tyreTempM;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      float[] tyreTempO;

      int isAIControlled;

      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
      float[] tyreContactPoint;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
      float[] tyreContactNormal;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
      float[] tyreContactHeading;

      float brakeBias;

      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
      float[] localVelocity;

      int P2PActivations;
      int P2PStatus;

      int currentMaxRpm;

      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      float[] mz;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      float[] fx;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      float[] fy;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      float[] slipRatio;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      float[] slipAngle;


      int tcinAction;
      int absInAction;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      float[] suspensionDamage;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      float[] tyreTemp;
   };

   struct SPageFileStatic {
      IntPtr smVersion;
      IntPtr acVersion;

      // session static info
      int numberOfSessions;
      int numCars;
      IntPtr carModel;
      IntPtr track;
      IntPtr playerName;
      IntPtr playerSurname;
      IntPtr playerNick;
      int sectorCount;

      // car static info
      float maxTorque;
      float maxPower;
      int	maxRpm;
      float maxFuelf;
      float suspensionMaxTravelf;
      float tyreRadiusf;
      float maxTurboBoostf;

      float deprecated_1;
      float deprecated_2;

      int penaltiesEnabled;

      float aidFuelRatef;
      float aidTireRatef;
      float aidMechanicalDamagef;
      int aidAllowTyreBlankets;
      float aidStabilityf;
      int aidAutoClutch;
      int aidAutoBlip;

      int hasDRS;
      int hasERS;
      int hasKERS;
      float kersMaxJf;
      int engineBrakeSettingsCount;
      int ersPowerControllerCount;
      float trackSPlineLengthf;
      IntPtr trackConfiguration;
      float ersMaxJ;

      int isTimedRace;
      int hasExtraLap;

      IntPtr carSkin;
      int reversedGridPositions;
      int PitWindowStart;
      int PitWindowEnd;
      int isOnline;
   };
}

// Reenable the warning for unused variables
#pragma warning restore CS0414