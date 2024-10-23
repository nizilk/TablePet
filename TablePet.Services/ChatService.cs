using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Python.Runtime;

namespace TablePet.Services
{
    public class ChatService
    {
        private static string OPENAI_API_KEY = "sk-WM2GGtnoub7lHpmix4ihZX0Si8K1Go5vqyXQDjIwdD3dZdeZ";
        private IntPtr m_threadState;

        public ChatService()
        {
            var osPath = Environment.GetEnvironmentVariable("PATH");
            var venvPath = "D:\\Documents\\GitHub\\TablePet\\.venv\\";
            var binPath = Path.Combine(venvPath, "Scripts");
            var pyPath = "C:\\Users\\ailie\\AppData\\Local\\Programs\\Python\\Python310-32\\";
            var pyPath2 = Path.Combine(pyPath, "python310.dll");

            Environment.SetEnvironmentVariable("PATH", $"{binPath};{osPath}");
            Environment.SetEnvironmentVariable("VIRTUAL_ENV", venvPath);

            Runtime.PythonDLL = pyPath2;

            PythonEngine.PythonPath = $"D:\\Documents\\GitHub\\TablePet\\TablePet.Services\\Python;{venvPath}Lib\\site-packages;{venvPath}Lib;{pyPath}Lib;{pyPath}DLLs";
            PythonEngine.PythonHome = venvPath;
            // PythonEngine.PythonPath = Environment.GetEnvironmentVariable("PYTHONPATH", EnvironmentVariableTarget.Process);

            // PythonEngine.Initialize();
        }


        public string test(string pm)
        {
            string t;
            PythonEngine.Initialize();
            m_threadState = PythonEngine.BeginAllowThreads();
            using (Py.GIL())
            {
                using (PyModule scope = Py.CreateScope())
                {
                    dynamic openai = Py.Import("chat");
                    t = openai.test();
                }
            }
            PythonEngine.EndAllowThreads(m_threadState);
            return t;
        }


    }
}
