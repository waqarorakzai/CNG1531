// **************************************************************************************************
//		CTrafficLightHlaObject
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
/// This is a wrapper class for local data structures. This class is extended from the object model of RACoN API
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
  public class CTrafficLightHlaObject : HlaObject
  {
        #region Declarations
        public string Sign;
        #endregion //Declarations

        #region Constructor
        public CTrafficLightHlaObject(HlaObjectClass _type) : base(_type)
    {
            Sign = "";
        }
    // Copy constructor - used in callbacks
    public CTrafficLightHlaObject(HlaObject _obj) : base(_obj)
    {
            Sign = "";
        }
    #endregion //Constructor
  }
}
