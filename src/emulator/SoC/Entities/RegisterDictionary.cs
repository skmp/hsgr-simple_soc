using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoC.Entities
{
    public class RegisterDictionary
    {
        public static Dictionary<string, Register> registerDictionary = new Dictionary<string, Register>();

        static RegisterDictionary()
        {
            List<Register> list = new List<Register>();
            list.Add(new Register("r0", 0));
            list.Add(new Register("r1", 1));
            list.Add(new Register("r2", 2));
            list.Add(new Register("r3", 3));
            list.Add(new Register("r4", 4));
            list.Add(new Register("r5", 5));
            list.Add(new Register("r6", 6));
            list.Add(new Register("r7", 7));
            list.Add(new Register("r8", 8));
            list.Add(new Register("r9", 9));
            list.Add(new Register("r10", 10));
            list.Add(new Register("r11", 11));
            list.Add(new Register("r12", 12));
            list.Add(new Register("r13", 13));
            list.Add(new Register("r14", 14));
            list.Add(new Register("r15", 15));

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

        public static Register Get(string s)
        {
            if (registerDictionary.ContainsKey(s.ToLower()))
                return (Register)registerDictionary[s.ToLower()].Clone();

            return null;
        }

    }
}
