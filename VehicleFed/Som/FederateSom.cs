// **************************************************************************************************
//		FederateSom
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
  public class FederateSom : Racon.ObjectModel.CObjectModel
  {
    #region Declarations
    #region SOM Declaration
    public tms.Som.CVehicleOC VehicleOC;
    public tms.Som.CTrafficLightOC TrafficLightOC;
    public tms.Som.CVehicleMessageIC VehicleMessageIC;
    public tms.Som.CLightMessageIC LightMessageIC;
    public HlaDimension AreaOfResponsibility;
    #endregion
    #endregion //Declarations
    
    #region Constructor
    public FederateSom() : base()
    {
      // Construct SOM
      VehicleOC = new tms.Som.CVehicleOC();
      AddToObjectModel(VehicleOC);
      TrafficLightOC = new tms.Som.CTrafficLightOC();
      AddToObjectModel(TrafficLightOC);
      VehicleMessageIC = new tms.Som.CVehicleMessageIC();
      AddToObjectModel(VehicleMessageIC);
      LightMessageIC = new tms.Som.CLightMessageIC();
      AddToObjectModel(LightMessageIC);
      AreaOfResponsibility = new HlaDimension("AreaOfResponsibility");
      AddToObjectModel(AreaOfResponsibility);
    }
    #endregion //Constructor
  }
}
