using PumpClient.PumpServiceReference;
using System;
using System.IO;
using System.ServiceModel;

namespace PumpClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            InstanceContext instanceContext = new InstanceContext(new CallbackHandler());
            PumpServiceClient client = new PumpServiceClient(instanceContext);

            client.UpdateAndCompileScript($@"C:\Scripts\Sample.script");
            client.RunScript();
            
            Console.WriteLine("Please, Enter to exit ...");
            Console.ReadKey(true);
            client.Close();
        }
    }
}
