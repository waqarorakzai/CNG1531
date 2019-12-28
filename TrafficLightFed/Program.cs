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

        static public CTrafficLightFed trafficLightFed;
        static public BindingList<CTrafficLightHlaObject> TrafficLights;
        static public BindingList<CVehicleHlaObject> VehicleObject;
        static public bool pointAchieved = true;
        static public HlaRegion myAor;
        static public string nextLight;
        static public string currentLight;
        static public string name;
        static public int east = 0;
        static public int west = 0;
        static public int north = 0;
        static public int south = 0;
        static void Main(string[] args)
        {

            // Instantiation
            trafficLightFed = new CTrafficLightFed();

            trafficLightFed.StatusMessageChanged += Federate_StatusMessageChanged;
            trafficLightFed.LogLevel = LogLevel.ALL;
            //trafficLightFed = new CTrafficLightHlaObject(trafficLightFed.Som.TrafficLightOC);

            Console.WriteLine("Traffic Light Position [East, West, North, South]?");
            name = Console.ReadLine();
            // Initialize the federation execution
            trafficLightFed.FederationExecution.FederateName = "TrafficLightFed" + name;
            trafficLightFed.FederationExecution.Name = "tmsFedration";
            trafficLightFed.FederationExecution.FederateType = "TrafficLight";
            trafficLightFed.FederationExecution.ConnectionSettings = "rti://10.143.6.187";
            trafficLightFed.FederationExecution.FDD = @".\TMSFOM.xml";

            // Connect
            trafficLightFed.Connect(CallbackModel.EVOKED, trafficLightFed.FederationExecution.ConnectionSettings);
            // Create federation execution
            bool isCreated = trafficLightFed.CreateFederationExecution(trafficLightFed.FederationExecution.Name, trafficLightFed.FederationExecution.FomModules);
            Console.WriteLine(isCreated);
            
            // Join federation execution
            trafficLightFed.JoinFederationExecution(trafficLightFed.FederationExecution.FederateName, trafficLightFed.FederationExecution.FederateType, trafficLightFed.FederationExecution.Name, trafficLightFed.FederationExecution.FomModules);
            
            var timestamp = trafficLightFed.Time + trafficLightFed.Lookahead;

            TrafficLights = new BindingList<CTrafficLightHlaObject>();
            VehicleObject = new BindingList<CVehicleHlaObject>();

            CTrafficLightHlaObject ctraffic = new CTrafficLightHlaObject(trafficLightFed.Som.TrafficLightOC);
            ctraffic.TrafficLight.setPosition(name);
            Console.WriteLine("Position of This Federate "+ctraffic.TrafficLight.Position);
            Program.TrafficLights.Add(ctraffic);

            CVehicleHlaObject vehicleObj = new CVehicleHlaObject(trafficLightFed.Som.VehicleOC);
            Program.VehicleObject.Add(vehicleObj);

            // Declare Capability
            trafficLightFed.DeclareCapability();

            //Console.WriteLine("Name of Federates "+trafficLightFed.GetFederateName(trafficLightFed.FederateHandle));
            /*if (TrafficLights[0].TrafficLight.Position == DirectionEnum.East)
                {
                    trafficLightFed.CreateEastRegion();
                    myAor = trafficLightFed.aor1;
                }
                else if (TrafficLights[0].TrafficLight.Position == DirectionEnum.West)
                {
                    trafficLightFed.CreateWestRegion();
                    myAor = trafficLightFed.aor2;
                }
                else if (TrafficLights[0].TrafficLight.Position == DirectionEnum.North)
                {
                    trafficLightFed.CreateNorthRegion();
                    myAor = trafficLightFed.aor3;
                }
                else
                {
                    trafficLightFed.CreateSouthRegion();
                    myAor = trafficLightFed.aor4;
                }
                */
            do
            {
                trafficLightFed.Run();

                // Creating sync point

                if(Program.TrafficLights.Count == 4 && name.ToLower() == "east" && pointAchieved)
                {
                    Console.WriteLine("We will call sync");
                    trafficLightFed.RegisterFederationSynchronizationPoint("EveryLightUp", "Syncronization is Requested");
                    pointAchieved = false;
                }



                Console.WriteLine("After 30 Sec passed");
                string yes = Console.ReadLine();
                if (yes == "See")
                {
                    trafficLightFed.printMsgs();
                }

                if(yes == "Send")
                {
                    trafficLightFed.ChangeLight();
                }


                if(yes == "steps")
                {
                    Console.WriteLine(Program.east);
                    Console.WriteLine(Program.west);
                    Console.WriteLine(Program.north);
                    Console.WriteLine(Program.south);
                }

            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);//Console.ReadKey(true).Key != ConsoleKey.Escape

            //vehicle.DisableTimeRegulation();
            //federate.DisableTimeConstrained();
            //vehicle.DisableAsynchronousDelivery();
            // Leave and destroy federation execution
            trafficLightFed.FinalizeFederation(trafficLightFed.FederationExecution, ResignAction.NO_ACTION);

            // Dumb trace log
            System.IO.StreamWriter file = new System.IO.StreamWriter(@".\TraceLog.txt");
            file.WriteLine(trafficLightFed.TraceLog);
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
            Console.WriteLine((sender as CTrafficLightFed).StatusMessage);
        }
    }
}
