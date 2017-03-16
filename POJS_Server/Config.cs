using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POJS_Server
{
    public class Config
    {
        
        private string _serverip = "";
        private string _serverPort = "";
        private string _compilerPath = "";
        private string _workPath = "";
        private string _resourcesPath = "";
        
        public string ServerIP { get { return _serverip; } set { _serverip = value; } }
        public string ServerPort { get{ return _serverPort; } set{ _serverPort = value;} }
        public string CompilerPath { get{ return _compilerPath; } set{ _compilerPath = value; } }
        public string WorkPath { get{ return _workPath; } set{ _workPath = value; } }
        public string ResourcesPath { get{ return _resourcesPath; } set{ _resourcesPath = value; } }

    }
}
