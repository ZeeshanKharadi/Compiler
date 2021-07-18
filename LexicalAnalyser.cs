using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Compiler
{
    class LexicalAnalyser
    {

        string[] isKeywords = new string[] { "int", "double", "char", "string","boolean", "pub", "pri", "class", "inherit", "abstract", "if", "else", "new", "static", "main", "void", "for", "while" };
        string[] isPunctuators = new string[] { "{", "}", "[", "]", "(", ")", ",", ";", ":" };
        string[] isOperators = new string[] { "+", "-", "*", "/", "%", "!", "=", ".", "<", ">", "&", "|", "&&", "||", "+=", "-=", "*=", "/=", "<=", ">=", "!=", "==", "++", "--" };
        string lc = (@"[\r|\n]");



        public bool isKw(string key)
        {
            for (int i = 0; i < isKeywords.Length; i++)
            {
                if (key == isKeywords[i])
                {
                    return true;
                }
            }
            return false;
        }

        public bool isOpr(string opr)
        {
            for (int i = 0; i < isOperators.Length; i++)
            {
                if (opr == isOperators[i])
                {
                    return true;
                }
            }
            return false;
        }

        public bool isPunc(string punc)
        {
            for (int i = 0; i < isPunctuators.Length; i++)
            {
                if (punc == isPunctuators[i])
                {
                    return true;
                }
            }
            return false;
        }

        public bool isId(string id)
        {
            Regex regex = new Regex(@"^([_]*[a-zA-Z]+[0-9]+[a-zA-Z]*[_]*)+$|^([_]+[a-zA-Z0-9]+[_]*)+$|^([_]*[a-zA-Z]+[_]*)+$");
            Match match = regex.Match(id);
            if (match.Success)
            {
                return true;
            }
            return false;
        }

        public bool isIntConst(string intconst)
        {
            Regex regex2 = new Regex(@"^[-\+][0-9]+$|^[0-9]+$");
            Match match2 = regex2.Match(intconst);
            if (match2.Success)
            {
                return true;
            }
            return false;
        }

        public bool isFloatConst(string floatconst)
        {
            Regex regex3 = new Regex(@"^[-\+][0-9]*[.][0-9]+$|^[0-9]*[.][0-9]+$");
            Match match3 = regex3.Match(floatconst);
            if (match3.Success)
            {
                return true;
            }
            else
                return false;
        }

        public bool isCharConst(string charconst)
        {
            Regex regex4 = new Regex("[^\'a-z]");//char regex missing
            Match match4 = regex4.Match(charconst);
            if (match4.Success)
            {
                return true;
            }
            return false;
        }

        public bool isStrConst(string strconst)
        {
            Regex regex5 = new Regex("^\"([a-zA-Z0-9 ]|(\\\\\\\\)|[~`!@#$%^&*()-=+{}|:;<>,.?/']|\\[|\\]|_|\\\\[abnrtv0f]|\\\\\"|\\\\')*\"$|^\"(\")\"$");
            Match match5 = regex5.Match(strconst);
            if (match5.Success)
            {
                return true;
            }
            return false;
        }

        public bool isLineCarriage(string rn)
        {
            bool ch = Regex.IsMatch(rn, lc);
            if (ch == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // starts reading file 

        StreamReader sr = new StreamReader("Code.txt");
        string temp = "";
        int lstring = 0, scmt = 0, mcmt = 0, lchar = 0, ldot = 0, line_no = 1;
        char curr, next;
        string es = @"\";
        public string[] tok;
        public void splitwords()
        {
            while (!sr.EndOfStream)
            {
                curr = (char)sr.Read();
                next = (char)sr.Peek();
                if (curr.ToString() == "'" && scmt == 0 && mcmt == 0 && lstring == 0 || lchar != 0)
                {
                    temp = temp + curr.ToString();
                    lchar++;
                    if (curr.ToString() == "'" && next.ToString() != es)
                    {
                        next = (char)sr.Read();
                        curr = next;
                        next = (char)sr.Peek();
                        temp = temp + curr.ToString();
                        if (isCharConst(temp) == true)
                        {

                            temp = temp.Remove(0, 1);
                            temp = temp.Remove(temp.Length - 1);
                            List.insert("Char_Const", temp, line_no);
                            temp = "";
                        }
                        else
                        {
                            token(temp, line_no);
                            temp = "";
                        }
                        lchar = 0;
                    }

                    else if (curr.ToString() == "'" && next.ToString() == es)
                    {
                        next = (char)sr.Read();
                        curr = next;
                        next = (char)sr.Peek();
                        temp = temp + curr.ToString();
                        if (isCharConst(temp) == true)
                        {
                            temp = temp.Remove(0, 1);
                            temp = temp.Remove(temp.Length - 1);
                            List.insert("Char_Const", temp, line_no);
                            temp = "";
                        }
                        else
                        {
                            token(temp, line_no);
                            temp = "";
                        }
                        lchar = 0;
                    }
                }
                

                else if (curr != ' ' && curr != '\n' && lstring == 0 && curr != '"' && scmt == 0 && curr != '^' && mcmt == 0 && lchar == 0 && curr.ToString() != "'")
                {
                    temp = temp + curr.ToString();
                    if (curr == '.' && ldot == 0)
                    {
                        if (char.IsNumber(next) == true)
                        {
                            ldot++;
                            temp = temp + next.ToString();
                            next = (char)sr.Read();
                            curr = next;
                            next = (char)sr.Peek();
                            while (char.IsLetterOrDigit(next) == true)
                            {
                                temp = temp + next.ToString();
                                next = (char)sr.Read();
                                curr = next;
                                next = (char)sr.Peek();
                            }
                            token(temp, line_no);
                            temp = "";
                        }
                        else if (char.IsLetter(next) == true && ldot == 0)
                        {
                            token(temp, line_no);
                            temp = "";
                        }
                    

}
                    else if (next == '.' && ldot == 0)
                    {
                        if (char.IsNumber(curr) == true)
                        {
                            ldot++;
                            if (char.IsNumber(curr) == true && next == '.')
                            {
                                next = (char)sr.Read();
                                curr = next;
                                next = (char)sr.Peek();
                                if (char.IsDigit(next) == true)
                                {
                                    temp = temp + curr.ToString() + next.ToString();
                                    next = (char)sr.Read();
                                    curr = next;
                                    next = (char)sr.Peek();
                                    while (char.IsLetterOrDigit(next) == true)
                                    {
                                        temp = temp + next.ToString();
                                        next = (char)sr.Read();
                                        curr = next;
                                        next = (char)sr.Peek();
                                    }
                                    token(temp, line_no);
                                    temp = "";
                                }
                                else if (char.IsNumber(next) == false)
                                {
                                    token(temp, line_no);
                                    token(curr.ToString(), line_no);
                                    temp = "";
                                }
                                else if (isPunc(curr.ToString()) == true)
                                {
                                    token(temp, line_no);
                                    temp = "";
                                }
                            }
                        }
                        else if (ldot == 0)
                        {

                            if (char.IsLetter(curr) == true)
                            {
                                token(temp, line_no);
                                temp = "";
                            }
                            else if (isOpr(temp) == true)
                            {
                                token(temp, line_no);
                                temp = "";
                            }
                            else if (isPunc(curr.ToString()) == true)
                            {
                                token(temp, line_no);
                                temp = "";
                            }
                        }
                    }
                    else if (curr == '.' && char.IsDigit(next) == true)
                    {
                        next = (char)sr.Read();
                        curr = next;
                        next = (char)sr.Peek();
                        temp = temp + curr.ToString();
                        while (char.IsLetterOrDigit(next))
                        {
                            temp = temp + next.ToString();
                            next = (char)sr.Read();
                            curr = next;
                            next = (char)sr.Peek();
                        }
                        token(temp, line_no);
                        temp = "";
                    }

                    else if (curr == '=' && (next == '+' || next == '-'))
                    {
                        List.insert("Operator", temp, line_no);
                        temp = "";
                        {
                            temp = temp + next.ToString();
                            next = (char)sr.Read();
                            curr = next;
                            next = (char)sr.Peek();
                            if (char.IsNumber(curr) == true)
                            {
                                temp = temp + curr.ToString() + next.ToString();
                                next = (char)sr.Read();
                                curr = next;
                                next = (char)sr.Peek();

                                if (char.IsNumber(next) == true)
                                {
                                    temp = temp + curr.ToString() + next.ToString();
                                    next = (char)sr.Read();
                                    curr = next;
                                    next = (char)sr.Peek();
                                    while (char.IsNumber(next) == true)
                                    {
                                        temp = temp + next;
                                        curr = (char)sr.Read();
                                        next = (char)sr.Peek();
                                    }

                                }
                                token(temp, line_no);
                                temp = "";
                            }
                        }
                    }
                    else if (ldot != 0 && next == '.' || curr == '.')
                    {
                        if (isFloatConst(temp) == false)
                        {
                            token(temp, line_no);
                            temp = "";
                        }
                    }
                    else if ((char)sr.Peek() == '\r')
                    {
                        token(temp, line_no);
                        temp = "";
                    }
                    else if (sr.Peek() == -1)
                    {
                        token(temp, line_no);
                        line_no++;
                        temp = "";
                    }
                    else if ((char)sr.Peek() == '\n')
                    {
                        string ww=temp;
                        token(temp, line_no);
                        temp = "";
                        line_no++;
                    }
                    else if (isPunc(next.ToString()) == true || isPunc(curr.ToString()) == true)
                    {
                        token(temp, line_no);
                        temp = "";
                    }
                    else if (isOpr(next.ToString()) == true && isOpr(curr.ToString()) == false)
                    {
                        token(temp, line_no);
                        temp = "";
                    }

                    else if (isOpr(curr.ToString()) == true)
                    {
                        if (isOpr(next.ToString()) == false && isOpr(curr.ToString()) == true)
                        {
                            token(temp, line_no);
                            temp = "";
                        }
                        else if (next == '=')
                        {
                            temp = curr.ToString() + next.ToString();
                            token(temp, line_no);
                            temp = "";
                            next = (char)sr.Read();
                            curr = next;
                            next = (char)sr.Peek();

                        }
                        else if ((curr == '+' && next == '+') || (curr == '-' && next == '-') || (curr == '|' && next == '|') || (curr == '&' && next == '&') || (curr == '=' && next == '='))
                        {
                            temp = curr.ToString() + next.ToString();
                            List.insert("Operator", temp, line_no);
                            temp = "";
                            next = (char)sr.Read();
                            curr = next;
                            next = (char)sr.Peek();

                        }

                        else if (isOpr(curr.ToString()) == true && isOpr(next.ToString()) == true)
                        {
                            token(temp, line_no);
                            temp = "";
                        }

                    }

                }
                else if (curr == ' ' && lchar == 0 && lstring == 0 && scmt == 0 && mcmt == 0 && curr.ToString() != "'")
                {
                    token(temp, line_no);
                    temp = "";
                    ldot = 0;
                }
                else if (curr == '"' && scmt == 0 && mcmt == 0 && lchar == 0 || lstring != 0)
                {
                    lstring++;
                    temp = temp + curr.ToString();
                    if (curr != '"' && curr != '\n')
                    {
                        if (next == '\n')
                        {
                            List.insert("Invalid Lexeme", temp, line_no);
                            temp = "";
                            lstring = 0;
                            line_no++;
                        }

                        else if (next == '\r')
                        {
                            List.insert("Invalid Lexeme", temp, line_no);
                            temp = "";
                            lstring = 0;
                        }
                        else if (sr.Peek() == -1)
                        {
                            List.insert("Invalid Lexeme", temp, line_no);
                            temp = "";
                            lstring = 0;

                        }
                    }
                    else if (isStrConst(temp) == true && lstring > 1)
                    {
                        temp = temp.Remove(0, 1);
                        temp = temp.Remove(temp.Length - 1);
                        List.insert("String_Const", temp, line_no);
                        temp = "";
                        lstring = 0;
                    }

                }

                else if (curr == '^' || scmt != 0 && lchar == 0 && lstring == 0)
                {
                    token(temp, line_no);
                    temp = "";
                    scmt++;
                    if (curr == '\n' && mcmt == 0)
                    {
                        line_no++;
                        scmt = 0;
                    }
                    
                    else if (next == '^')
                    {
                    
                        mcmt++;
                        scmt = 0;
                        line_no++;
                    }
                    else if (curr == '^' && mcmt > 1)
                    {
                        mcmt = 0;
                        scmt = 0;
                        line_no++;
                    }
                    else if (curr == '^' && mcmt > 1 && sr.Peek() == -1)
                    {
                        break;
                    }
                }

            }
            //List.insert("$", "", line_no);
        }


        public void token(string word, int ln)
        {
            if (isOpr(word) == true)
            {
                List.insert("Operator", word, ln);
                word = "";
            }
            else if (isFloatConst(word) == true)
            {
                List.insert("Double_Const", word, ln);
                word = "";

            }
            else if (isPunc(word) == true || word == ".")
            {
                List.insert("Punctuator", word, ln);
                word = "";
            }
            else if (isKw(word) == true)
            {
                List.insert("Keyword", word, ln);
                word = "";
            }
            else if (isId(word) == true)
            {
                List.insert("Id", word, ln);
                word = "";
            }
            else if (isIntConst(word) == true)
            {
                List.insert("Int_Const", word, ln);
                word = "";
            }
            else if (isKw(word) == false && isOpr(word) == false && isPunc(word) == false && isId(word) == false && isIntConst(word) == false && isFloatConst(word) == false && isLineCarriage(word) == false && word != "")
            {
                List.insert("Invalid Lexeme", word, ln);
            }

        }
    }
}
