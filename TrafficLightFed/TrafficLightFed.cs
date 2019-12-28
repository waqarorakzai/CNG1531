// **************************************************************************************************
//		CTrafficLightFed
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
    public partial class CTrafficLightFed : Racon.CGenericFederate
    {
        private object thisLock = new object();

        // DDM Declarations
        public HlaRegion aor1;
        public HlaRegion aor2;
        public HlaRegion aor3;
        public HlaRegion aor4;
        // FdAmb_InteractionReceivedHandler
        public override void FdAmb_InteractionReceivedHandler(object sender, HlaInteractionEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_InteractionReceivedHandler(sender, data);

            /*if (data.Interaction.ClassHandle == Som.LightMessageIC.Handle)
            {
                Console.WriteLine("Recieved: " + Som.LightMessageIC.Handle);

                if (data.IsValueUpdated(Som.LightMessageIC.Sign))
                {
                    Sign = data.GetParameterValue<string>(Som.LightMessageIC.Sign);
                    Console.WriteLine("Sign recieved : " + Sign);
                }

                if (data.IsValueUpdated(Som.LightMessageIC.Duration))
                {
                    Duration = data.GetParameterValue<int>(Som.LightMessageIC.Duration);
                    Console.WriteLine("Duration recieved : " + Duration);
                }

                if (data.IsValueUpdated(Som.LightMessageIC.DateTime))
                {
                    DateTime = data.GetParameterValue<int>(Som.LightMessageIC.DateTime);
                    Console.WriteLine("DateTime recieved : " + DateTime);
                }


            }*/

            if (data.Interaction.ClassHandle == Som.VehicleMessageIC.Handle)
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
            // Check that this is for the StationOC
            Console.WriteLine("Registering TrafficLightOC Objects Now");
            if (data.ObjectClassHandle == Som.TrafficLightOC.Handle)
            {
                RegisterHlaObject(Program.TrafficLights[0]);
            }
            
            
            if (data.ObjectClassHandle == Som.VehicleOC.Handle)
            {
                RegisterHlaObject(Program.VehicleObject[0]);
            }
            #endregion //User 
        }


        public override void FdAmb_ObjectDiscoveredHandler(object sender, HlaObjectEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_ObjectDiscoveredHandler(sender, data);

            #region User Code
            Console.WriteLine("Discoverting Objects Now");
            // Check the class type of the discovered object
            if (data.ClassHandle == Som.TrafficLightOC.Handle) // A ship
            {
                // Create and add a new ship to the list
                CTrafficLightHlaObject newTrafficLight = new CTrafficLightHlaObject(data.ObjectInstance);
                newTrafficLight.Type = Som.TrafficLightOC;
                Program.TrafficLights.Add(newTrafficLight);
                Console.WriteLine("New Traffic Light has joined");
                Console.WriteLine("TraffiLights are : " + Program.TrafficLights.Count);
                RequestAttributeValueUpdate(newTrafficLight, null);
            }
            else if (data.ClassHandle == Som.VehicleOC.Handle) // A station
            {
                // Create and add a new ship to the list
                CVehicleHlaObject newVehicle = new CVehicleHlaObject(data.ObjectInstance);
                newVehicle.Type = Som.VehicleOC;
                Program.VehicleObject.Add(newVehicle);
                Console.WriteLine("New Vehicle Arrived at Intersection");
                RequestAttributeValueUpdate(newVehicle, null);
            }
            #endregion //User Code
        }

        public override void FdAmb_AttributeValueUpdateRequestedHandler(object sender, HlaObjectEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_AttributeValueUpdateRequestedHandler(sender, data);

            #region User Code
            Console.WriteLine("Updating attribute Values Objects Now");
            // !!! If this federate is created only one object instance, then it is sufficient to check the handle of that object, otherwise we need to check all the collection
            if (data.ObjectInstance.Handle == Program.TrafficLights[0].Handle)
            {
                // We can update all attributes if we dont want to check every attribute.
                var timestamp = Time + Lookahead;
                UpdateTrafficAttribute(Program.TrafficLights[0], timestamp);
            }
            else if(data.ObjectInstance.Handle == Program.VehicleObject[0].Handle)
            {
                Console.WriteLine("Are we suppose to do someting");
            }
            #endregion //User Code
        }

        // Reflect Object Attributes
        public override void FdAmb_ObjectAttributesReflectedHandler(object sender, HlaObjectEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_ObjectAttributesReflectedHandler(sender, data);
            Console.WriteLine("waiting reflect attribute Values Objects Now");
            foreach (var item in Program.TrafficLights)
            {
                // Update track
                if (data.ObjectInstance.Handle == item.Handle)
                {
                    // Get parameter values - 2nd method
                    foreach (var pair in data.ObjectInstance.Attributes)
                    {
                        if (pair.Handle == Som.TrafficLightOC.Sign.Handle)
                        {
                            item.TrafficLight.Sign = pair.GetValue<string>();
                            Console.WriteLine(item.Name + " has sign of " + item.TrafficLight.Sign);
                        }
                        else if (pair.Handle == Som.TrafficLightOC.Duration.Handle)
                        {
                            item.TrafficLight.Duration = pair.GetValue<int>();
                        }
                        else if (pair.Handle == Som.TrafficLightOC.Position.Handle)
                        {
                            string valueOfPos = pair.GetValue<string>();
                            Console.WriteLine(item.Name + " has position of " + valueOfPos);
                            item.TrafficLight.Position = valueOfPos;
                            if(Program.name.ToLower() == "west" && valueOfPos == "east")
                            {
                                Console.WriteLine("We are talking about "+valueOfPos);
                                Program.east++;
                            }
                            else if(Program.name.ToLower() == "east" && valueOfPos == "west")
                            {
                                Program.west++;
                                changeEastToRed();
                            }
                            else if(Program.name.ToLower() == "north" && valueOfPos == "west")
                            {
                                Program.north++;
                            }
                            else if(Program.name.ToLower() == "west" && valueOfPos == "north")
                            {
                                Program.north++;
                                changeWestToRed();
                            }
                            else if(Program.name.ToLower() == "north" && valueOfPos == "south")
                            {
                                Console.WriteLine("We are south");
                                Program.south++;
                                changeNorthToRed();
                            }
                        }
                    }
                }
            }


            foreach (var item in Program.VehicleObject)
            {
                // Update track
                if (data.ObjectInstance.Handle == item.Handle)
                {
                    foreach (var pair in data.ObjectInstance.Attributes)
                    {
                        if (pair.Handle == Som.VehicleOC.VehicleName.Handle)
                        {
                            Console.WriteLine(pair.GetValue<string>() + " is arrived at Intersection");
                        }
                        else if (pair.Handle == Som.VehicleOC.Direction.Handle)
                        {
                            //Console.WriteLine("Vehicle at Intersection " + pair.GetValue<string>());
                            if (Program.TrafficLights[0].TrafficLight.Position == pair.GetValue<string>() && Program.TrafficLights[0].TrafficLight.Sign == "Green")
                            {
                                var tm = Time + Lookahead;
                                SendMessage("Green", 30, tm);
                                Console.WriteLine("GO! Green Light");
                            }
                            else if(Program.TrafficLights[0].TrafficLight.Position == pair.GetValue<string>())
                            {
                                var tm = Time + Lookahead;
                                SendMessage("Red", 1, tm);
                                Console.WriteLine("WAIT! Red Light");
                            }
                        }
                    }
                }
            }
        }

        public bool SendMessage(string sign, int duration, double timestamp)
        {
            HlaInteraction interaction = new Racon.RtiLayer.HlaInteraction(Som.LightMessageIC, "LightMessage");

            // Add Values
            interaction.AddParameterValue(Som.LightMessageIC.Sign, sign);
            interaction.AddParameterValue(Som.LightMessageIC.Duration, duration);
            return SendInteraction(interaction, "");
        }


        // Update attribute values
        private bool UpdateTrafficAttribute(CTrafficLightHlaObject light, double timestamp)
        {
            // Add Values
            light.AddAttributeValue(Som.TrafficLightOC.Sign, light.TrafficLight.Sign);
            light.AddAttributeValue(Som.TrafficLightOC.Duration, light.TrafficLight.Duration);
            light.AddAttributeValue(Som.TrafficLightOC.Position, light.TrafficLight.Position);
            var tm = Time + Lookahead;
            foreach (var item in Program.VehicleObject)
            {
                if (item.Direction.ToLower() == Program.name.ToLower())
                {
                    SendMessage(light.TrafficLight.Sign, light.TrafficLight.Duration, tm);
                }

            }

            return (UpdateAttributeValues(light, "update/reflect"));
        }

        public bool updateSign(CTrafficLightHlaObject light, string sign)
        {
            light.AddAttributeValue(Som.TrafficLightOC.Sign, sign);
            return (UpdateAttributeValues(light, "update/reflect"));
        }

        // FdAmb_OnSynchronizationPointRegistrationConfirmedHandler
        public override void FdAmb_OnSynchronizationPointRegistrationConfirmedHandler(object sender, HlaFederationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_OnSynchronizationPointRegistrationConfirmedHandler(sender, data);

            #region User Code
            Console.WriteLine("Sync Point request ({data.Label}) is accepted by RTI.");
            #endregion //User Code
        }
        // FdAmb_OnSynchronizationPointRegistrationFailedHandler

        public override void FdAmb_OnSynchronizationPointRegistrationFailedHandler(object sender, HlaFederationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_OnSynchronizationPointRegistrationFailedHandler(sender, data);

            #region User Code
            Console.WriteLine("Pacing request ({data.Label}) is NOT accepted by RTI. Reason: {data.Reason}" + Environment.NewLine);
            #endregion //User Code
        }
        // FdAmb_SynchronizationPointAnnounced
        public override void FdAmb_SynchronizationPointAnnounced(object sender, HlaFederationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_SynchronizationPointAnnounced(sender, data);

            #region User Code
            //Report.WriteLine($"Ready for zone transfer. Label: {data.Label}" + Environment.NewLine);
            Console.WriteLine("We are changing lights to red all");
            Program.TrafficLights[0].TrafficLight.Sign = "Red";
            SynchronizationPointAchieved(data.Label, true);
            #endregion //User Code
        }
        // FdAmb_FederationSynchronized
        public override void FdAmb_FederationSynchronized(object sender, HlaFederationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_FederationSynchronized(sender, data);

            #region User Code
            Console.WriteLine("All lights are red. We can start now.");
            Console.WriteLine("Name of This TL : "+Program.name);
            if(Program.name.ToLower() == "east")
            {
                updateEastToGreen();
            }
            #endregion //User Code
        }


        public override void FdAmb_ObjectRemovedHandler(object sender, HlaObjectEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_ObjectRemovedHandler(sender, data);

            #region User Code
            /* object[] snap;
             lock (thisLock)
             {
                 //snap = Program.TrafficLights.ToArray();
             }

             foreach (CTrafficLightHlaObject ship in Program.TrafficLights)
             {
                 if (data.ObjectInstance.Handle == ship.Handle)// Find the Object
                 {
                     Program.TrafficLights.Remove(ship);
                     Console.WriteLine("Number of traffic ligths Now: " + Program.TrafficLights.Count);
                 }
             }*/
            

            int vlremover;

            for (int i = Program.VehicleObject.Count - 1; i >= 0; i--)
            {
                vlremover = Program.VehicleObject.IndexOf(Program.VehicleObject[i]);
                Program.VehicleObject.RemoveAt(vlremover);
            }
            #endregion //User Code
        }

        public bool updateEastToGreen()
        {
            // Add Values
            CTrafficLightHlaObject light = Program.TrafficLights[0];
            light.TrafficLight.Sign = "Yellow";
            light.TrafficLight.Duration = 5;
            light.AddAttributeValue(Som.TrafficLightOC.Sign, light.TrafficLight.Sign);
            light.AddAttributeValue(Som.TrafficLightOC.Duration, light.TrafficLight.Duration);
            light.AddAttributeValue(Som.TrafficLightOC.Position, light.TrafficLight.Position);
            return (UpdateAttributeValues(light, "update/reflect"));
        }

        public bool turnEastToGreen(CTrafficLightHlaObject light)
        {
            if(light.TrafficLight.Sign != "Green")
            {
                light.TrafficLight.Sign = "Green";
                light.TrafficLight.Duration = 30;
                light.AddAttributeValue(Som.TrafficLightOC.Sign, light.TrafficLight.Sign);
                light.AddAttributeValue(Som.TrafficLightOC.Duration, light.TrafficLight.Duration);
                light.AddAttributeValue(Som.TrafficLightOC.Position, light.TrafficLight.Position);
                return (UpdateAttributeValues(light, "update/reflect"));
            }
            return false;
        }
        public bool turnWestToYellow(CTrafficLightHlaObject light)
        {
            /* if (light.TrafficLight.Sign != "Green")
             {*/
               light.TrafficLight.Sign = "Yellow";
                light.TrafficLight.Duration = 5;
                light.AddAttributeValue(Som.TrafficLightOC.Sign, light.TrafficLight.Sign);
                light.AddAttributeValue(Som.TrafficLightOC.Duration, light.TrafficLight.Duration);
                light.AddAttributeValue(Som.TrafficLightOC.Position, light.TrafficLight.Position);
                return (UpdateAttributeValues(light, "update/reflect"));
           /* }
            return false;*/
        }

        public bool turnWestToGreen(CTrafficLightHlaObject light)
        {
            if (light.TrafficLight.Sign != "Green")
            {
                light.TrafficLight.Sign = "Green";
                light.TrafficLight.Duration = 30;
                light.AddAttributeValue(Som.TrafficLightOC.Sign, light.TrafficLight.Sign);
                light.AddAttributeValue(Som.TrafficLightOC.Duration, light.TrafficLight.Duration);
                light.AddAttributeValue(Som.TrafficLightOC.Position, light.TrafficLight.Position);
                return (UpdateAttributeValues(light, "update/reflect"));
            }
            return false;
        }
        public bool turnEastToYellow(CTrafficLightHlaObject light)
        {
            if (light.TrafficLight.Sign != "Yellow")
            {
                light.TrafficLight.Sign = "Yellow";
                light.TrafficLight.Duration = 5;
                light.AddAttributeValue(Som.TrafficLightOC.Sign, light.TrafficLight.Sign);
                light.AddAttributeValue(Som.TrafficLightOC.Duration, light.TrafficLight.Duration);
                light.AddAttributeValue(Som.TrafficLightOC.Position, light.TrafficLight.Position);
                return (UpdateAttributeValues(light, "update/reflect"));
            }
            return false;
        }
        
        public bool changeEastToRed()
        {
            CTrafficLightHlaObject light = Program.TrafficLights[0];
            if (light.TrafficLight.Sign != "Red")
            {
                // Add Values
                light.TrafficLight.Sign = "Red";
                light.TrafficLight.Duration = 0;
                light.AddAttributeValue(Som.TrafficLightOC.Sign, light.TrafficLight.Sign);
                light.AddAttributeValue(Som.TrafficLightOC.Duration, light.TrafficLight.Duration);
                light.AddAttributeValue(Som.TrafficLightOC.Position, light.TrafficLight.Position);
                return (UpdateAttributeValues(light, "update/reflect"));
            }
            return false;
        }
        public bool changeNorthToRed()
        {
            CTrafficLightHlaObject light = Program.TrafficLights[0];
            if (light.TrafficLight.Sign != "Red")
            {
                // Add Values
                light.TrafficLight.Sign = "Red";
                light.TrafficLight.Duration = 0;
                light.AddAttributeValue(Som.TrafficLightOC.Sign, light.TrafficLight.Sign);
                light.AddAttributeValue(Som.TrafficLightOC.Duration, light.TrafficLight.Duration);
                light.AddAttributeValue(Som.TrafficLightOC.Position, light.TrafficLight.Position);
                return (UpdateAttributeValues(light, "update/reflect"));
            }
            return false;
        }
        public bool changeWestToRed()
        {
            CTrafficLightHlaObject light = Program.TrafficLights[0];
            if (light.TrafficLight.Sign != "Red")
            {
                // Add Values
                light.TrafficLight.Sign = "Red";
                light.TrafficLight.Duration = 0;
                light.AddAttributeValue(Som.TrafficLightOC.Sign, light.TrafficLight.Sign);
                light.AddAttributeValue(Som.TrafficLightOC.Duration, light.TrafficLight.Duration);
                light.AddAttributeValue(Som.TrafficLightOC.Position, light.TrafficLight.Position);
                return (UpdateAttributeValues(light, "update/reflect"));
            }
            return false;
        }
        public bool turnNorthToYellow(CTrafficLightHlaObject light)
        {
            if (light.TrafficLight.Sign != "Yellow")
            {
                // Add Values
                light.TrafficLight.Sign = "Yellow";
                light.TrafficLight.Duration = 5;
                light.AddAttributeValue(Som.TrafficLightOC.Sign, light.TrafficLight.Sign);
                light.AddAttributeValue(Som.TrafficLightOC.Duration, light.TrafficLight.Duration);
                light.AddAttributeValue(Som.TrafficLightOC.Position, light.TrafficLight.Position);
                return (UpdateAttributeValues(light, "update/reflect"));
            }
            return false;
        }
        public bool turnSouthToYellow(CTrafficLightHlaObject light)
        {
            if (light.TrafficLight.Sign != "Yellow")
            {
                // Add Values
                light.TrafficLight.Sign = "Yellow";
                light.TrafficLight.Duration = 5;
                light.AddAttributeValue(Som.TrafficLightOC.Sign, light.TrafficLight.Sign);
                light.AddAttributeValue(Som.TrafficLightOC.Duration, light.TrafficLight.Duration);
                light.AddAttributeValue(Som.TrafficLightOC.Position, light.TrafficLight.Position);
                return (UpdateAttributeValues(light, "update/reflect"));
            }
            return false;
        }
        public bool turnSouthToGreen(CTrafficLightHlaObject light)
        {
            if (light.TrafficLight.Sign != "Green")
            {
                // Add Values
                light.TrafficLight.Sign = "Green";
                light.TrafficLight.Duration = 30;
                light.AddAttributeValue(Som.TrafficLightOC.Sign, light.TrafficLight.Sign);
                light.AddAttributeValue(Som.TrafficLightOC.Duration, light.TrafficLight.Duration);
                light.AddAttributeValue(Som.TrafficLightOC.Position, light.TrafficLight.Position);
                return (UpdateAttributeValues(light, "update/reflect"));
            }
            return false;
        }
        public bool turnSouthToRed(CTrafficLightHlaObject light)
        {
            if (light.TrafficLight.Sign != "Red")
            {
                // Add Values
                light.TrafficLight.Sign = "Red";
                light.TrafficLight.Duration = 0;
                light.AddAttributeValue(Som.TrafficLightOC.Sign, light.TrafficLight.Sign);
                light.AddAttributeValue(Som.TrafficLightOC.Duration, light.TrafficLight.Duration);
                light.AddAttributeValue(Som.TrafficLightOC.Position, light.TrafficLight.Position);
                return (UpdateAttributeValues(light, "update/reflect"));
            }
            return false;
        }
        public bool turnNorthToGreen(CTrafficLightHlaObject light)
        {
            if (light.TrafficLight.Sign != "Green")
            {
                // Add Values
                light.TrafficLight.Sign = "Green";
                light.TrafficLight.Duration = 30;
                light.AddAttributeValue(Som.TrafficLightOC.Sign, light.TrafficLight.Sign);
                light.AddAttributeValue(Som.TrafficLightOC.Duration, light.TrafficLight.Duration);
                light.AddAttributeValue(Som.TrafficLightOC.Position, light.TrafficLight.Position);
                return (UpdateAttributeValues(light, "update/reflect"));
            }
            return false;
        }

        public void ChangeLight()
        {
            if (Program.name.ToLower() == "east" && Program.west == 0 && Program.east == 0 && Program.TrafficLights[0].TrafficLight.Sign == "Yellow")
            {
                turnEastToGreen(Program.TrafficLights[0]);
            }

            else if (Program.name.ToLower() == "east" && Program.west == 0 && Program.east == 0 && Program.TrafficLights[0].TrafficLight.Sign == "Green")
            {
                turnEastToYellow(Program.TrafficLights[0]);
            }
            else if (Program.name.ToLower() == "west" && Program.west == 0 && Program.east == 3 && Program.TrafficLights[0].TrafficLight.Sign == "Red")
            {
                turnWestToYellow(Program.TrafficLights[0]);
            }
            else if (Program.name.ToLower() == "west" && Program.west == 0 && Program.east == 4 && Program.TrafficLights[0].TrafficLight.Sign == "Yellow")
            {
                turnWestToGreen(Program.TrafficLights[0]);
            }
            else if (Program.name.ToLower() == "west" && Program.west == 0 && Program.east == 4 && Program.TrafficLights[0].TrafficLight.Sign == "Green")
            {
                turnWestToYellow(Program.TrafficLights[0]);
            }
            else if (Program.name.ToLower() == "north" && Program.north == 3 && Program.TrafficLights[0].TrafficLight.Sign == "Red")
            {
                turnNorthToYellow(Program.TrafficLights[0]);
            }
            else if (Program.name.ToLower() == "north" && Program.north == 4 && Program.TrafficLights[0].TrafficLight.Sign == "Yellow")
            {
                turnNorthToGreen(Program.TrafficLights[0]);
            }
            else if (Program.name.ToLower() == "north" && Program.north == 4 && Program.TrafficLights[0].TrafficLight.Sign == "Green")
            {
                turnNorthToYellow(Program.TrafficLights[0]);
            }
            else if (Program.name.ToLower() == "south" && Program.south == 0 && Program.TrafficLights[0].TrafficLight.Sign == "Red")
            {
                turnSouthToYellow(Program.TrafficLights[0]);
            }
            else if (Program.name.ToLower() == "south" && Program.south == 0 && Program.TrafficLights[0].TrafficLight.Sign == "Yellow")
            {
                turnSouthToGreen(Program.TrafficLights[0]);
            }
            else if (Program.name.ToLower() == "south" && Program.south == 0 && Program.TrafficLights[0].TrafficLight.Sign == "Green")
            {
                turnSouthToRed(Program.TrafficLights[0]);
            }
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

        public void printMsgs()
        {
            foreach (var item in Program.TrafficLights)
            {
                Console.WriteLine(item.Name + " has sign of " + item.TrafficLight.Sign);
                Console.WriteLine(item.Name + " has Duration of " + item.TrafficLight.Duration);
                Console.WriteLine(item.Name + " has Position of " + item.TrafficLight.Position);
            }
        }

    }
}
