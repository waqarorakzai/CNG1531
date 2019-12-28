using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Racon
using Racon;
using Racon.RtiLayer;
// Application
using tms.Som;

namespace tms
{
    public class Program
    {
        static public CVehicleFed vehicleFd;
        static public CVehicleHlaObject VehicleObject;
        static public CTrafficLightHlaObject TrafficLights;
        static public HlaRegion myAor;
        static public bool updated = false;

        static void Main(string[] args)
        {

            Console.WriteLine("MainProgram");

            // Instantiation
            vehicleFd = new CVehicleFed();


            vehicleFd.StatusMessageChanged += Federate_StatusMessageChanged;
            vehicleFd.LogLevel = LogLevel.ALL;
            VehicleObject = new CVehicleHlaObject(vehicleFd.Som.VehicleOC);

            // Initialize the federation execution
            vehicleFd.FederationExecution.FederateName = "VehicleFedrate" + VehicleObject.Vehicle.VehicleName;
            vehicleFd.FederationExecution.Name = "tmsFedration";
            vehicleFd.FederationExecution.FederateType = "Vehicle";
            vehicleFd.FederationExecution.ConnectionSettings = "rti://10.143.6.187";
            vehicleFd.FederationExecution.FDD = @".\TMSFOM.xml";

            // Connect
            vehicleFd.Connect(CallbackModel.EVOKED, vehicleFd.FederationExecution.ConnectionSettings);
            // Create federation execution
            vehicleFd.CreateFederationExecution(vehicleFd.FederationExecution.Name, vehicleFd.FederationExecution.FomModules);
            // Join federation execution
            vehicleFd.JoinFederationExecution(vehicleFd.FederationExecution.FederateName, vehicleFd.FederationExecution.FederateType, vehicleFd.FederationExecution.Name, vehicleFd.FederationExecution.FomModules);

            // Declare Capability
            vehicleFd.DeclareCapability();

           /* if (VehicleObject.Vehicle.Direction == DirectionEnum.East)
            {
                vehicleFd.CreateEastRegion();
                myAor = vehicleFd.aor1;
            }
            else if (VehicleObject.Vehicle.Direction == DirectionEnum.West)
            {
                vehicleFd.CreateWestRegion();
                myAor = vehicleFd.aor2;
            }
            else if (VehicleObject.Vehicle.Direction == DirectionEnum.North)
            {
                vehicleFd.CreateNorthRegion();
                myAor = vehicleFd.aor3;
            }
            else
            {
                vehicleFd.CreateSouthRegion();
                myAor = vehicleFd.aor4;
            }
           
            // Creating regions and stuff
            AttributeHandleSetRegionHandleSetPairVector pairs = new AttributeHandleSetRegionHandleSetPairVector();
            List<HlaRegion> regions = new List<HlaRegion>();
            regions.Add(myAor);
            *//*
           pairs.Pairs.Add(new KeyValuePair<List<HlaAttribute>, List<HlaRegion>>(vehicleFd.Som.VehicleOC.Attributes, regions));
           vehicleFd.subscribeObjectClassAttributesWithRegions(vehicleFd.Som.TrafficLightOC, pairs);
           vehicleFd.RequestAttributeValueUpdateWithRegions(vehicleFd.Som.TrafficLightOC, pairs, "user-supplied tag");

           *//*

            vehicleFd.SubscribeInteractionClassWithRegions(vehicleFd.Som.LightMessageIC, regions);*/

            TrafficLights = new CTrafficLightHlaObject(vehicleFd.Som.TrafficLightOC);

            do
            {
                vehicleFd.Run();
                if (!updated)
                {
                    vehicleFd.UpdateVehicleAttribute(VehicleObject);
                    updated = true;
                }
                Console.WriteLine("What do you want to do next?");
                string yes = Console.ReadLine();

                if (yes == "Send")
                {
                    vehicleFd.SendMessage(VehicleObject.Vehicle.VehicleName);
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

            // *************************************************
            // Shutdown
            // *************************************************

            //vehicle.DisableTimeRegulation();
            //federate.DisableTimeConstrained();
            //vehicle.DisableAsynchronousDelivery();
            // Leave and destroy federation execution

            vehicleFd.DeleteObjectInstance(VehicleObject);
            vehicleFd.FinalizeFederation(vehicleFd.FederationExecution, ResignAction.NO_ACTION);

            // Dumb trace log
            System.IO.StreamWriter file = new System.IO.StreamWriter(@".\TraceLog.txt");
            file.WriteLine(vehicleFd.TraceLog);
            file.Close();
            //Console.WriteLine(federate.TraceLog.ToString());

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }


        // Racon Information received
        private static void Federate_StatusMessageChanged(object sender, EventArgs e)
        {
            Console.ResetColor();
            Console.WriteLine((sender as CVehicleFed).StatusMessage);
        }
    }
}
