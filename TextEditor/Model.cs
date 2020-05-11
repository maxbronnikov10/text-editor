using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor
{
    class Model
    {
        public void SaveFile(string path, string text)
        {
            File.AppendAllText(path, text);
        }

        public string OpenFile(string path)
        {
            return File.ReadAllText(path);
        }

        public void Exit()
        {
            Environment.Exit(0);
        }
        private int Find(string text, string str, int index)
        {
            string sub = (index == 0) ? text : text.Substring(index+str.Length, text.Length-(index+str.Length)-1); 
            if (sub.Contains(str))
                return (index == 0) ? sub.IndexOf(str) : sub.IndexOf(str)+ index + str.Length;
            return -1;
        }

        public IEnumerable<int> FindAll(string text, string str)
        {
            List<int> indexes = new List<int>();
            int i = 0;

            while ((i = Find(text, str, i)) > 0) indexes.Add(i);
            
            return indexes;
        }
    
    }
}
