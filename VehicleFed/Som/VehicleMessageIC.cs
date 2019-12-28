// **************************************************************************************************
//		CVehicleMessageIC
//
//		generated
//			by		: 	Simulation Generator (SimGe) v.0.3.2
//			at		: 	Tuesday, December 10, 2019 12:17:47 PM
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
  public class CVehicleMessageIC : HlaInteractionClass
  {
    #region Declarations
    public HlaParameter Type;
    #endregion //Declarations
    
    #region Constructor
    public CVehicleMessageIC() : base()
    {
      // Initialize Class Properties
      Name = "HLAinteractionRoot.VehicleMessage";
      ClassPS = PSKind.PublishSubscribe;
      
      // Create Parameters
      // Type
      Type = new HlaParameter("Type");
      Parameters.Add(Type);
    }
    #endregion //Constructor
  }
}
