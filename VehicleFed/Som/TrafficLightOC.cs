// **************************************************************************************************
//		CTrafficLightOC
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
  public class CTrafficLightOC : HlaObjectClass
  {
    #region Declarations
    public HlaAttribute Sign;
    public HlaAttribute Duration;
    public HlaAttribute Position;
    #endregion //Declarations
    
    #region Constructor
    public CTrafficLightOC() : base()
    {
      // Initialize Class Properties
      Name = "HLAobjectRoot.TrafficLight";
      ClassPS = PSKind.Subscribe;
      
      // Create Attributes
      // Sign
      Sign = new HlaAttribute("Sign", PSKind.Subscribe);
      Attributes.Add(Sign);
      // Duration
      Duration = new HlaAttribute("Duration", PSKind.Subscribe);
      Attributes.Add(Duration);
      // Position
      Position = new HlaAttribute("Position", PSKind.Subscribe);
      Attributes.Add(Position);
    }
    #endregion //Constructor
  }
}
