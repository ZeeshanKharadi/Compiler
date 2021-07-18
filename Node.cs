using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class Node
    {
        public string sData;
        public string classp;
        public int lno;
        public Node next;

        public Node()
        {
            sData = null;
            next = null;
        }
        public Node(string cp, string vp, int ln)
        {
            if (cp == "Operator")
            {
                if (vp == "+" || vp == "-")
                {
                    cp = "PM";
                }
                else if (vp == "*" || vp == "/" || vp == "%")
                {
                    cp = "MDM";
                }
                else if (vp == "<" || vp == ">" || vp == "<=" || vp == ">=" || vp == "!=" || vp == "==")
                {
                    cp = "RO";
                }
                else if (vp == "&&")
                {
                    cp = "LOGICAL AND";
                }
                else if (vp == "||")
                {
                    cp = "LOGICAL OR";
                }
                else if (vp == "+=" || vp == "-=" || vp == "/=" || vp == "*=" || vp == "%=")
                {
                    cp = "ASSIGN OP";
                }
                else if (vp == "++" || vp == "--")
                {
                    cp = "INCDEC";
                }
                else
                {
                    cp = vp;
                    // vp = "";
                }
            }
            else if (cp == "Keyword")
            {
                if (vp == "int" || vp == "double" || vp == "char" || vp == "string")
                {
                    cp = "DT";
                }
                else if (vp == "pub" || vp == "pri")
                {
                    cp = "ACCESS MODIFIER";
                }
                else
                {
                    cp = vp;
                    //vp = "";
                }
            }
            else if (cp == "Punctuator")
            {
                cp = vp;
               // vp = "";
            }
            sData = " ( " + cp + " , " + vp + " , " + ln + " )";
            classp = cp;
            lno = ln;
            next = null;
        }

        public void displayNode()
        {
            Console.WriteLine(sData);
        }
    }
}
