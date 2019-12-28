// **************************************************************************************************
//		CTrafficLightOC
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
      ClassPS = PSKind.PublishSubscribe;
      
      // Create Attributes
      // Sign
      Sign = new HlaAttribute("Sign", PSKind.PublishSubscribe);
      Attributes.Add(Sign);
      // Duration
      Duration = new HlaAttribute("Duration", PSKind.PublishSubscribe);
      Attributes.Add(Duration);
      // Position
      Position = new HlaAttribute("Position", PSKind.PublishSubscribe);
      Attributes.Add(Position);
    }
    #endregion //Constructor
  }
}
