using System;
using System.IO;
using Newtonsoft.Json;


namespace POJS_Server
{
    class LoadConfig
    {
        public static Config config;
        
        public static bool load()
        {
            if (!File.Exists(@"config.json"))
            {
                FileStream configFile = new FileStream(@"config.json", FileMode.OpenOrCreate);
                StreamWriter sw = new StreamWriter(configFile);
                sw.WriteLine("{");
                sw.WriteLine("    \"ServerIP\": \"127.0.0.1\",");
                sw.WriteLine("    \"ServerPort\": \"7777\",");
                sw.WriteLine("    \"CompilerPath\": \"E:\\\\Visual Studio 2015\\\\Projects\\\\POJS\\\\MinGW\\\\bin\\\\gcc.exe\",");
                sw.WriteLine("    \"WorkPath\": \".\\\\\",");
                sw.WriteLine("    \"ResourcesPath\": \".\\\\Resources\\\\\"");
                sw.WriteLine("}");
                sw.Close();
                configFile.Close();
                Console.WriteLine("Please configure the server in the configuration file POJS.config");
                return false;
            }
            else
            {
                FileStream configFile = new FileStream(@"config.json", FileMode.OpenOrCreate);
                StreamReader sr = new StreamReader(configFile);
                string configText = sr.ReadToEnd();
                config = JsonConvert.DeserializeObject<Config>(configText);
            }

            //Console.WriteLine(config.ServerIP);
            //Console.WriteLine(config.ServerPort);
            //Console.WriteLine(config.CompilerPath);
            //Console.WriteLine(config.WorkPath);
            //Console.WriteLine(config.ResourcesPath);

            return true;
        }
    }
}
