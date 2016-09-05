using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

//namespace Stack 
namespace FormulaEvaluator

  
{
    public static class Evaluator
    {
        public delegate int Lookup(String v);


        static Stack<int> values = new Stack<int>();
        static int valuesSize = 0;
        static Stack<string> operators = new Stack<string>();
        static int operatorsSize = 0;

        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            foreach (string token in substrings)
            {
                //check if token is an integer
                int value;
                if (Int32.TryParse(token, out value))
                {
                    intRead(value);
                }
                //check if token is a valid variable
                else if(token == "")
                {
                    int varValue = variableEvaluator(token);
                    intRead(varValue);
                }
                //check if token is addition or subtrction
                else if (token == "+" || token == "-")
                {
                    if (checkStackSize("operator"))
                    {
                        if(operators.Peek() == "+" || operators.Peek() == "-")
                        {
                            sumOrSubtract();
                        }
                    }
                    operators.Push(token);
                    operatorsSize++;
                }
                //check if token is multiplication or division
                else if (token == "*" || token == "/")
                {
                    operators.Push(token);
                    operatorsSize++;
                }
                //check left parathsis
                else if(token == "(")
                {
                    operators.Push(token);
                    operatorsSize++;
                }
                else if(token == ")")
                {
                    if (checkStackSize("operator"))
                    {
                        if (operators.Peek() == "+" || operators.Peek() == "-")
                        {
                            sumOrSubtract();
                        }
                        if(operators.Peek() == "/" || operators.Peek() == "*")
                        {
                            multiOrDiv();
                        }
                        if(operators.Peek() == "(")
                        {
                            operators.Pop();
                            operatorsSize--;
                        }
                    }
                    if (checkStackSize("operator"))
                    {
                        if(operators.Peek() == "(")
                        {
                            operators.Pop();
                            operatorsSize--;
                        }
                        else
                        {
                            throw System.Exception("ERROR: Paranthesis do not match.");
                        }
                    }
                }
            }
            //no more tokens to read, check stacks to get final answer
            return 0;
        }

        /// <summary>
        /// Makes sure that there is at least one token in the stack
        /// </summary>
        /// <param name="s"></param>
        /// The key word to know whether to check the operators stack or the values stack
        /// <returns></returns>
        /// true if there is a token in the stack, false if not
        public static bool checkStackSize(string s)
        {
            if(s == "operators")
            {
                if(operatorsSize <= 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if(valuesSize <= 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            } 
        }

        /// <summary>
        /// An integer is read, there for check if it needs to be operated on or simply added to the values stack.
        /// </summary>
        /// <param name="x"></param>
        /// the integer that was just read
        public static void intRead(int x)
        {
            if (checkStackSize("operators"))
            {
                if (operators.Peek() == "*" || operators.Peek() == "/")
                {
                    performOperation(x);
                }
            }
            else
            {
                values.Push(x);
                valuesSize++;
            }
        }

        /// <summary>
        /// Performs operation on two values already recorded. Checks that there are two values to pull and whether to call on addition 
        /// or subtraction.
        /// </summary>
        public static void sumOrSubtract()
        {
            //pop the two values
            int valOne;
            int valTwo;
            if (checkStackSize("v"))
            {
                valOne = values.Pop();
                valuesSize--;
            }
            else
            {
                //error
            }
            if (checkStackSize("v"))
            {
                valTwo = values.Pop();
                valuesSize--;
            }
            else
            {
                //error
            }
            //pop operator 
            int newVal;
            switch (operators.Pop())
            {
                case "+":
                    newVal = valOne + valTwo;
                    break;

                case "-":
                    newVal = valTwo - valOne;
                    break;
            }
            operatorsSize--;
            values.Push(newVal);
            valuesSize++;
            return;
        }

        /// <summary>
        /// Muliplies or divides two values that have already been added to the stack.
        /// </summary>
        public static void multiOrDiv()
        {
            //pop the two values
            int valOne;
            int valTwo;
            if (checkStackSize("v"))
            {
                valOne = values.Pop();
                valuesSize--;
            }
            else
            {
                //error
            }
            if (checkStackSize("v"))
            {
                valTwo = values.Pop();
                valuesSize--;
            }
            else
            {
                //error
            }
            int newVal;
            switch (operators.Pop())
            {
                case "*":
                    newVal = valOne * valTwo;
                    break;
                case "/":
                    if(valOne == 0)
                    {
                        //error
                    }
                    newVal = valTwo / valOne;
                    break;
            }
            operatorsSize--;
            values.Push(newVal);
            valuesSize++;
        }
       
    }



}
