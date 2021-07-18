using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Compiler
{
    class List
    {
        public static Queue<string> cll = new Queue<string>();
        public static Queue<int> lin = new Queue<int>();
        public static Node first;
        public static Node last;
        static List()
        {
            first = null;
            last = null;
        }

        public static void insert(string cp, string vp, int line)
        {
            Node newLink = new Node(cp, vp, line);
            if (first == null)
            {
                first = newLink;
                last = newLink;
            }
            else
            {
                last.next = newLink;
                last = last.next;
            }
        }

        public static void display()
        {
            Node current = first;
            while (current != null)
            {
                current.displayNode();
                current = current.next;
            }
            Console.WriteLine("");
        }

        public static void writeIntoFile()
        {
            StreamWriter sw = new StreamWriter("TokenSet.txt");
            Node current = first;
            while (current != null)
            {
                cll.Enqueue(current.classp);
                lin.Enqueue(current.lno);
                sw.WriteLine(current.sData);
                current = current.next;
            }
            sw.Close();
        }
    }
}
