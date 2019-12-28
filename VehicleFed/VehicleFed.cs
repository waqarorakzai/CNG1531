// **************************************************************************************************
//		CVehicleFed
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
/// The application specific federate that is extended from the Generic Federate Class of RACoN API. This file is intended for manual code operations.
/// </summary>

// System
using System;
using System.Collections.Generic; // for List
// Racon
using Racon;
using Racon.RtiLayer;
// Application
using tms.Som;

namespace tms
{
    public partial class CVehicleFed : Racon.CGenericFederate
    {

        // DDM Declarations
        public HlaRegion aor1;
        public HlaRegion aor2;
        public HlaRegion aor3;
        public HlaRegion aor4;

        // FdAmb_InteractionReceivedHandler
        public override void FdAmb_InteractionReceivedHandler(object sender, HlaInteractionEventArgs data)
        {
            string Sign = "";
            int Duration = 0;
            int DateTime = 0;
            // Call the base class handler
            base.FdAmb_InteractionReceivedHandler(sender, data);

            if (data.Interaction.ClassHandle == Som.LightMessageIC.Handle)
            {
                Console.WriteLine("Recieved: " + Som.LightMessageIC.Handle);

                if (data.IsValueUpdated(Som.LightMessageIC.Sign))
                {
                    Sign = data.GetParameterValue<string>(Som.LightMessageIC.Sign);
                    Console.WriteLine("Traffic Light Sign: " + Sign);
                    if(Sign == "Green")
                    {
                        Console.WriteLine("Green ! GO now");
                    }
                    else if(Sign == "Red")
                    {
                        Console.WriteLine("RED! Wait");
                    }
                }

                else if (data.IsValueUpdated(Som.LightMessageIC.Duration))
                {
                    Duration = data.GetParameterValue<int>(Som.LightMessageIC.Duration);
                    Console.WriteLine("Duration of Light : " + Duration);
                }

                else if (data.IsValueUpdated(Som.LightMessageIC.DateTime))
                {
                    DateTime = data.GetParameterValue<int>(Som.LightMessageIC.DateTime);
                    Console.WriteLine("DateTime recieved : " + DateTime);
                }


            }

            else if (data.Interaction.ClassHandle == Som.VehicleMessageIC.Handle)
            {
                string TypeOfVehicle = "";

                if (data.IsValueUpdated(Som.VehicleMessageIC.Type))
                {
                    TypeOfVehicle = data.GetParameterValue<string>(Som.VehicleMessageIC.Type);
                    Console.WriteLine("Vehicle Type : " + TypeOfVehicle + " arrived at the intersection\n");
                }
            }
        }

        // Start Registration
        public override void FdAmb_StartRegistrationForObjectClassAdvisedHandler(object sender, HlaDeclarationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_StartRegistrationForObjectClassAdvisedHandler(sender, data);

            #region User Code
            if(data.ObjectClassHandle == Som.TrafficLightOC.Handle)
            {
                RegisterHlaObject(Program.TrafficLights);
            }
            else if(data.ObjectClassHandle == Som.VehicleOC.Handle)
            {
                RegisterHlaObject(Program.VehicleObject);
            }
            #endregion //User Code
        }


        public override void FdAmb_ObjectDiscoveredHandler(object sender, HlaObjectEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_ObjectDiscoveredHandler(sender, data);

            #region User Code
            // Check the class type of the discovered object
            if (data.ClassHandle == Som.TrafficLightOC.Handle) // A ship
            {
                CTrafficLightHlaObject newTrafficLight = new CTrafficLightHlaObject(data.ObjectInstance);
                newTrafficLight.Type = Som.TrafficLightOC;
                RequestAttributeValueUpdate(newTrafficLight, null);
            }
            #endregion //User Code
        }

        public override void FdAmb_AttributeValueUpdateRequestedHandler(object sender, HlaObjectEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_AttributeValueUpdateRequestedHandler(sender, data);
            Console.WriteLine("Updating attribute Values Objects Now");
            // !!! If this federate is created only one object instance, then it is sufficient to check the handle of that object, otherwise we need to check all the collection
            if (data.ObjectInstance.Handle == Program.VehicleObject.Handle)
            {
                // We can update all attributes if we dont want to check every attribute.
                
                UpdateVehicleAttribute(Program.VehicleObject);
            }
        }

        public override void FdAmb_ObjectRemovedHandler(object sender, HlaObjectEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_ObjectRemovedHandler(sender, data);

            #region User Code
            Console.WriteLine("Leaving the intersecton");
            #endregion //User Code
        }


        // Update attribute values
        public bool UpdateVehicleAttribute(CVehicleHlaObject vehicle)
        {
            // Add Values
            vehicle.AddAttributeValue(Som.VehicleOC.VehicleName, vehicle.Vehicle.VehicleName);
            vehicle.AddAttributeValue(Som.VehicleOC.Direction, vehicle.Vehicle.Direction);
            return (UpdateAttributeValues(vehicle, "update/reflect"));
        }

        // Reflect Object Attributes
        public override void FdAmb_ObjectAttributesReflectedHandler(object sender, HlaObjectEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_ObjectAttributesReflectedHandler(sender, data);
            Console.WriteLine("waiting reflect attribute Values Objects Now");
            // Update track
            // Get parameter values - 2nd method
            foreach (var pair in data.ObjectInstance.Attributes)
            {
                if (pair.Handle == Som.TrafficLightOC.Sign.Handle)
                {
                    Program.TrafficLights.Sign = pair.GetValue<string>();
                    Console.WriteLine(Program.VehicleObject.Vehicle.VehicleName + " recieved : " + Program.TrafficLights.Sign);
                }
            }
        }
        public bool SendMessage(string type)
        {
            HlaInteraction interaction = new Racon.RtiLayer.HlaInteraction(Som.VehicleMessageIC, "VehicleMessage");

            // Add Values
            interaction.AddParameterValue(Som.VehicleMessageIC.Type, type);

            return (SendInteraction(interaction, ""));
        }


        // Update attribute values
        private void UpdateTrafficAttributes(CTrafficLightHlaObject light, double timestamp)
        {
            // Add Values
            light.AddAttributeValue(Som.TrafficLightOC.Sign, light.Sign);
            UpdateAttributeValues(light, timestamp);
        }



        // Create AOR-1 (West)
        public void CreateWestRegion()
        {
            GetAllDimensionHandles();
            aor1 = new HlaRegion("aor1");
            List<HlaDimension> dimensions = new List<HlaDimension>();
            dimensions.Add(Som.AreaOfResponsibility);
            CreateRegion(aor1, dimensions);
            SetRangeBounds(aor1.Handle, Som.AreaOfResponsibility.Handle, 0, 1);
            List<HlaRegion> regions = new List<HlaRegion>();
            regions.Add(aor1);
            CommitRegionModifications(regions);
        }
        // Create AOR-2 (East)
        public void CreateEastRegion()
        {
            GetAllDimensionHandles();
            aor2 = new HlaRegion("aor2");
            List<HlaDimension> dimensions = new List<HlaDimension>();
            dimensions.Add(Som.AreaOfResponsibility);
            CreateRegion(aor2, dimensions);
            SetRangeBounds(aor2.Handle, Som.AreaOfResponsibility.Handle, 1, 2);
            List<HlaRegion> regions = new List<HlaRegion>();
            regions.Add(aor2);
            CommitRegionModifications(regions);
        }
        // Create AOR-3 (North)
        public void CreateNorthRegion()
        {
            GetAllDimensionHandles();
            aor2 = new HlaRegion("aor3");
            List<HlaDimension> dimensions = new List<HlaDimension>();
            dimensions.Add(Som.AreaOfResponsibility);
            CreateRegion(aor2, dimensions);
            SetRangeBounds(aor2.Handle, Som.AreaOfResponsibility.Handle, 2, 3);
            List<HlaRegion> regions = new List<HlaRegion>();
            regions.Add(aor2);
            CommitRegionModifications(regions);
        }
        // Create AOR-4 (South)
        public void CreateSouthRegion()
        {
            GetAllDimensionHandles();
            aor2 = new HlaRegion("aor4");
            List<HlaDimension> dimensions = new List<HlaDimension>();
            dimensions.Add(Som.AreaOfResponsibility);
            CreateRegion(aor2, dimensions);
            SetRangeBounds(aor2.Handle, Som.AreaOfResponsibility.Handle, 3, 4);
            List<HlaRegion> regions = new List<HlaRegion>();
            regions.Add(aor2);
            CommitRegionModifications(regions);
        }



    }
}
