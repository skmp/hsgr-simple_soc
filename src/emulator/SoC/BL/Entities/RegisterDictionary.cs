using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoC.BL.Entities
{
    public class RegisterDictionary
    {
        public static Dictionary<string, Register> registerDictionary = new Dictionary<string, Register>();

        static RegisterDictionary()
        {
            List<Register> list = new List<Register>();
            list.Add(new Register(0, 0));
            list.Add(new Register(1, 0));
            list.Add(new Register(2, 0));
            list.Add(new Register(3, 0));
            list.Add(new Register(4, 0));
            list.Add(new Register(5, 0));
            list.Add(new Register(6, 0));
            list.Add(new Register(7, 0));
            list.Add(new Register(8, 0));
            list.Add(new Register(9, 0));
            list.Add(new Register(10, 0));
            list.Add(new Register(11, 0));
            list.Add(new Register(12, 0));
            list.Add(new Register(13, 0));
            list.Add(new Register(14, 0));
            list.Add(new Register(15, 0));

            registerDictionary = new Dictionary<string, Register>();
            foreach (Register r in list)
            {
                registerDictionary.Add(r.Name, r);
            }

        }

        public static bool Contains(string s)
        {
            return registerDictionary.ContainsKey(s.ToLower());
        }

        public static bool Contains(int n)
        {
            return registerDictionary.ContainsKey("r" + n.ToString());
        }

        public static Register Get(string s)
        {
            if (registerDictionary.ContainsKey(s.ToLower()))
                return (Register)registerDictionary[s.ToLower()].Clone();

            return null;
        }

        public static Register Get(int n)
        {
            if (registerDictionary.ContainsKey("r" + n.ToString()))
                return (Register)registerDictionary["r" + n.ToString()].Clone();

            return null;
        }

    }
}
