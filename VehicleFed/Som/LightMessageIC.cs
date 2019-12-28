// **************************************************************************************************
//		CLightMessageIC
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
  public class CLightMessageIC : HlaInteractionClass
  {
    #region Declarations
    public HlaParameter Duration;
    public HlaParameter Sign;
    public HlaParameter DateTime;
    #endregion //Declarations
    
    #region Constructor
    public CLightMessageIC() : base()
    {
      // Initialize Class Properties
      Name = "HLAinteractionRoot.LightMessage";
      ClassPS = PSKind.Subscribe;
      
      // Create Parameters
      // Duration
      Duration = new HlaParameter("Duration");
      Parameters.Add(Duration);
      // Sign
      Sign = new HlaParameter("Sign");
      Parameters.Add(Sign);
      // DateTime
      DateTime = new HlaParameter("DateTime");
      Parameters.Add(DateTime);
    }
    #endregion //Constructor
  }
}
