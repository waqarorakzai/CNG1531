// **************************************************************************************************
//		CVehicleOC
//
//		generated
//			by		: 	Simulation Generator (SimGe) v.0.3.2
//			at		: 	Monday, December 9, 2019 9:21:08 PM
//		compatible with		: 	RACoN v.0.0.2.5
//
//		copyright		: 	(C) 
//		email			: 	
// **************************************************************************************************
/// <summary>
/// This class is extended from the object model of RACoN API
/// </summary>

// System
using System;
using System.Collections.Generic; // for List
// Racon
using Racon;
using Racon.RtiLayer;
// Application
using tms.Som;


namespace tms.Som
{
  public class CVehicleOC : HlaObjectClass
  {
    #region Declarations
    public HlaAttribute Direction;
    public HlaAttribute Status;
    public HlaAttribute VehicleName;
    #endregion //Declarations
    
    #region Constructor
    public CVehicleOC() : base()
    {
      // Initialize Class Properties
      Name = "HLAobjectRoot.Vehicle";
      ClassPS = PSKind.Subscribe;
      
      // Create Attributes
      // Direction
      Direction = new HlaAttribute("Direction", PSKind.Subscribe);
      Attributes.Add(Direction);
      // Status
      Status = new HlaAttribute("Status", PSKind.Subscribe);
      Attributes.Add(Status);
      // VehicleName
      VehicleName = new HlaAttribute("VehicleName", PSKind.Subscribe);
      Attributes.Add(VehicleName);
    }
    #endregion //Constructor
  }
}
