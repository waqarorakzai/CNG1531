// **************************************************************************************************
//		CVehicleHlaObject
//
//		generated
//			by		: 	Simulation Generator (SimGe) v.0.3.2
//			at		: 	Thursday, November 7, 2019 1:17:06 PM
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
    public class CVehicleHlaObject : HlaObject
    {
        #region Declarations
        public CVehicle Vehicle;
        #endregion //Declarations
        public CVehicleHlaObject(HlaObjectClass _type) : base(_type)
        {
            Vehicle = new CVehicle();
        }

        public CVehicleHlaObject(HlaObject _obj) : base(_obj)
        {
            Vehicle = new CVehicle();
        }
    }
}
