using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Python.Runtime;

namespace TablePet.Services.Controllers
{
    public class ChatService
    {
        private static string OPENAI_API_KEY = "sk-WM2GGtnoub7lHpmix4ihZX0Si8K1Go5vqyXQDjIwdD3dZdeZ";
        private IntPtr m_threadState;
        private dynamic GPTInst;

        public ChatService()
        {
            var osPath = Environment.GetEnvironmentVariable("PATH");
            var venvPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\.venv\\");  // "D:\\Documents\\GitHub\\TablePet\\.venv\\";
            var binPath = Path.Combine(venvPath, "Scripts");
            var pyPath0 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\TablePet.Services\\Python\\");
            var pyPath1 = Path.Combine(pyPath0, "Python310-32\\");    // "C:\\Users\\ailie\\AppData\\Local\\Programs\\Python\\Python310-32\\";
            var pyPath2 = Path.Combine(pyPath1, "python310.dll");

            Environment.SetEnvironmentVariable("PATH", $"{binPath};{osPath}");
            Environment.SetEnvironmentVariable("VIRTUAL_ENV", venvPath);

            Runtime.PythonDLL = pyPath2;

            PythonEngine.PythonPath = $"{pyPath0};{venvPath}Lib\\site-packages;{venvPath}Lib;{pyPath1}Lib;{pyPath1}DLLs";
            PythonEngine.PythonHome = venvPath;
            // PythonEngine.PythonPath = Environment.GetEnvironmentVariable("PYTHONPATH", EnvironmentVariableTarget.Process);

            PythonEngine.Initialize();
            Init();
        }


        private void Init()
        {
            m_threadState = PythonEngine.BeginAllowThreads();
            using (Py.GIL())
            {
                using (var scope = Py.CreateScope())
                {
                    dynamic chat = Py.Import("chat");
                    GPTInst = chat.ChatGPT();
                }
            }
            PythonEngine.EndAllowThreads(m_threadState);
        }


        public string AskGpt(string pm)
        {
            string str;
            m_threadState = PythonEngine.BeginAllowThreads();
            using (Py.GIL())
            {
                using (var scope = Py.CreateScope())
                {
                    str = GPTInst.ask_gpt(pm);                    
                }
            }
            PythonEngine.EndAllowThreads(m_threadState);
            return str;
        }


        public string QueryRec(string pm)
        {
            string str;
            m_threadState = PythonEngine.BeginAllowThreads();
            using (Py.GIL())
            {
                using (var scope = Py.CreateScope())
                {
                    str = GPTInst.query_rec(pm);
                }
            }
            PythonEngine.EndAllowThreads(m_threadState);
            return str;
        }


    }
}
