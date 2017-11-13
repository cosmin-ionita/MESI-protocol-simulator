using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESI_Simulator
{
    class ProcState
    {
        public int id;
        public char state;
        public bool hasValue;
    }

    class MainClass
    {
        static ProcState[] states;

        private static void InitStates()
        {
            int i = 0;

            for (int x = 0; x < states.Length; x++)
                states[x] = new ProcState();

            foreach (var proc in states)
            {
                proc.id = i;
                proc.state = 'I';
                proc.hasValue = false;

                i++;
            }

        }

        private static void PrintStates(string magState, string dataSource)
        {
            Console.WriteLine("\n");

            foreach (var proc in states)
            {
                Console.Write(proc.state + " ");
            }

            Console.WriteLine("\nActiune Magistrala: " + magState);
            Console.WriteLine("Sursa date: " + dataSource + "\n\n");
        }

        private static void PerformWrite(int processor)
        {
            foreach (var proc in states)
            {
                if (proc.id == processor)
                {
                    proc.state = 'M';
                    proc.hasValue = true;
                }
                else
                {
                    proc.state = 'I';
                    proc.hasValue = false;
                }
            }
        }

        private static void PerformRead(int processor)
        {
            if (states[processor].state == 'M' || states[processor].state == 'S' || states[processor].state == 'E')
            {
                PrintStates("-", "-");
            }
            else if (states[processor].state == 'I')
            {
                var id = states.Where(st => st.hasValue == true).FirstOrDefault();

                if (id == null)
                {
                    states[processor].state = 'E';
                    states[processor].hasValue = true;

                    PrintStates("BusRd", "Mem");
                }
                else if (states[id.id].state == 'M')
                {
                    states[processor].state = 'S';
                    states[id.id].state = 'S';

                    PrintStates("BusRd", "Cache" + id.id);
                }
                else if (states[id.id].state == 'S')
                {
                    states[processor].state = 'S';
                    PrintStates("Flush", "Cache" + id.id);
                }
                else if(states[id.id].state == 'E')
                {
                    states[processor].state = 'S';
                    states[id.id].state = 'S';

                    PrintStates("Flush", "Cache" + id.id);
                }
            }
        }

        public static void Main(string[] args)
        {
            int N = 0;

            Console.WriteLine("Number of processors: ");
            var key = Console.ReadKey();

            N = Int32.Parse(key.KeyChar.ToString());

            states = new ProcState[N];

            InitStates();

            PrintStates("-", "-");

            while (true)
            {
                Console.Write("Id procesor: ");

                var pressedKey = Console.ReadLine();
                var processor = Int32.Parse(pressedKey);

                Console.Write("Actiune: ");

                pressedKey = Console.ReadLine();
                var action = pressedKey.ToString();

                if (action == "W")
                {
                    PerformWrite(processor);
                    PrintStates("BusRdX", "Flush");
                }
                else if (action == "R")
                {
                    PerformRead(processor);     // has printStates inside it
                }
            }
        }
    }
}
