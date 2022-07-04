// Disable warning about unused variables for development
#pragma warning disable CS0414

using System.Runtime.InteropServices;

namespace AcpmfData {
   public struct SPageFileGraphics {
      public int packetId;
      public int status;
      public int session;
      public IntPtr currentTime;
      public IntPtr lastTime;
      public IntPtr bestTime;
      public IntPtr split;
      public int completedLaps;
      public int position;
      public int iCurrentTime;
      public int iLastTime;
      public int iBestTime;
      public float sessionTimeLeft;
      public float distanceTraveled;
      public int isInPit;
      public int currentSectorIndex;
      public int lastSectorTime;
      public int numberOfLaps;
      public IntPtr tyreCompound;
      public float replayTimeMultiplier;
      public float normalizedCarPosition;

      public int activeCars;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60 * 3)]
      public float[] carCoordinates;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
      public int[] carID;
      public int playerCarID;
      public float penaltyTime;
      public int flag;
      //PenaltyShortcut penalty = PenaltyShortcut::None;
      public int idealLineOn;
      public int isInPitLane;

      public float surfaceGrip;
      public int mandatoryPitDone;

      public float windSpeed;
      public float windDirection;

      public int isSetupMenuVisible;

      public int mainDisplayIndex;
      public int secondaryDisplayIndex;
      public int TC;
      public int TCCut;
      public int EngineMap;
      public int ABS;
      public int fuelXLap;
      public int rainLights;
      public int flashingLights;
      public int lightsStage;
      public float exhaustTemperature;
      public int wiperLV;
      public int DriverStintTotalTimeLeft;
      public int DriverStintTimeLeft;
      public int rainTyres;
   };

   //[StructLayout(LayoutKind.Sequential)]
   public struct SPageFilePhysics {
      public int packetId;
      public float gas;
      public float brake;
      public float fuel;
      public int gear;
      public int rpms;
      public float steerAngle;
      public float speedKmh;    
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
      public float[] velocity;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
      public float[] accG;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      public float[] wheelSlip;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      public float[] wheelLoad;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      public float[] wheelsPressure;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      public float[] wheelAngularSpeed;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      public float[] tyreWear;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      public float[] tyreDirtyLevel;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      public float[] tyreCoreTemperature;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      public float[] camberRAD;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      public float[] suspensionTravel;
      public float drs;
      public float tc;
      public float heading;
      public float pitch;
      public float roll;
      public float cgHeight;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
      public float[] carDamage;
      public int numberOfTyresOut;
      public int pitLimiterOn;
      public float abs;
      public float kersCharge;
      public float kersInput;
      public int autoShifterOn;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
      public float[] rideHeight;
      public float turboBoost;
      public float ballast;
      public float airDensity;
      public float airTemp;
      public float roadTemp;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
      public float[] localAngularVel;
      public float finalFF;
      public float performanceMeter;

      public int engineBrake;
      public int ersRecoveryLevel;
      public int ersPowerLevel;
      public int ersHeatCharging;
      public int ersIsCharging;
      public float kersCurrentKJ;

      public int drsAvailable;
      public int drsEnabled;

      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      public float[] brakeTemp;
      public float clutch;

      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      public float[] tyreTempI;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      public float[] tyreTempM;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      public float[] tyreTempO;

      public int isAIControlled;

      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4 * 3)]
      public float[] tyreContactPoint;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4 * 3)]
      public float[] tyreContactNormal;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4 * 3)]
      public float[] tyreContactHeading;

      public float brakeBias;

      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
      public float[] localVelocity;

      public int P2PActivations;
      public int P2PStatus;

      public int currentMaxRpm;

      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      public float[] mz;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      public float[] fx;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      public float[] fy;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      public float[] slipRatio;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      public float[] slipAngle;


      public int tcinAction;
      public int absInAction;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      public float[] suspensionDamage;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      public float[] tyreTemp;
   };

   public struct SPageFileStatic {
      public IntPtr smVersion;
      public IntPtr acVersion;

      // session static info
      public int numberOfSessions;
      public int numCars;
      public IntPtr carModel;
      public IntPtr track;
      public IntPtr playerName;
      public IntPtr playerSurname;
      public IntPtr playerNick;
      public int sectorCount;

      // car static info
      public float maxTorque;
      public float maxPower;
      public int	maxRpm;
      public float maxFuelf;
      public float suspensionMaxTravelf;
      public float tyreRadiusf;
      public float maxTurboBoostf;

      public float deprecated_1;
      public float deprecated_2;

      public int penaltiesEnabled;

      public float aidFuelRatef;
      public float aidTireRatef;
      public float aidMechanicalDamagef;
      public int aidAllowTyreBlankets;
      public float aidStabilityf;
      public int aidAutoClutch;
      public int aidAutoBlip;

      public int hasDRS;
      public int hasERS;
      public int hasKERS;
      public float kersMaxJf;
      public int engineBrakeSettingsCount;
      public int ersPowerControllerCount;
      public float trackSPlineLengthf;
      public IntPtr trackConfiguration;
      public float ersMaxJ;

      public int isTimedRace;
      public int hasExtraLap;

      public IntPtr carSkin;
      public int reversedGridPositions;
      public int PitWindowStart;
      public int PitWindowEnd;
      public int isOnline;
   };
}

// Reenable the warning for unused variables
#pragma warning restore CS0414