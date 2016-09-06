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
                //char.IsLetter(token.FirstOrDefault()) might be better?
                else if (!String.IsNullOrEmpty(token)&&Char.IsLetter(token[0]))
                {
                    int varValue = variableEvaluator(token); //do I need to write my own expression?
                    intRead(varValue);
                }

                //check if token is addition or subtrction
                else if (token == "+" || token == "-")
                {
                    if (checkStackSize("o"))
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
 //                   if (checkStackSize("o"))
   //                 {
     //                   if(operators.Peek() == "*" || operators.Peek() == "/")
       //                 {
         //                   multiOrDiv();
           //             }
             //       }
                    operators.Push(token);
                    operatorsSize++;
                }

                //check left parathsis
                else if(token == "(")
                {
                    operators.Push(token);
                    operatorsSize++;
                }

                //check right parenthisis
                else if(token == ")")
                {
                    readRightParanth();
                }

                //invalid token
                else
                {
                    throw new System.Exception("ERROR: invalid token read");
                }
            }

            //no more tokens to read, check stacks to get final answer
            //there is only one value to read and nothing in operators stack
            if(valuesSize == 1 && operatorsSize == 0)
            {
                return values.Pop();
            }
            //there is one more operation to perform
            else if((valuesSize == 2) && (operatorsSize == 1))
            {
                //it must be addition or subtraction
                if(operators.Peek() == "+" || operators.Peek() == "-")
                {
                    sumOrSubtract();
                    return values.Pop();
                }
                else
                {
                    throw new System.Exception("ERROR: invalid operation for final answer");
                }
            }
            //if neither opptions are true the expression is invalid
            else
            {
                throw new System.Exception("ERROR: expression is invalid");
            }
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
            if(s == "o")
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
            if (checkStackSize("o"))
            {
                if (operators.Peek() == "*" || operators.Peek() == "/")
                {
                    values.Push(x);
                    multiOrDiv();
                }
                return;
            }
            else
            {
                values.Push(x);
                valuesSize++;
            }
            return;
        }

        /// <summary>
        /// Performs operation on two values already recorded. Checks that there are two values to pull and whether to call on addition 
        /// or subtraction.
        /// </summary>
        public static void sumOrSubtract()
        {
            //pop the two values
            int valOne = popValue();
            int valTwo = popValue();
            
            //pop operator 
            int newVal = 0;
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
            int valOne = popValue();
            int valTwo = popValue();
                       
            int newVal=0;
            switch (operators.Pop())
            {
                case "*":
                    newVal = valOne * valTwo;
                    break;
                case "/":
                    if(valOne == 0)
                    {
                        throw new System.Exception("ERROR: division by 0");
                    }
                    newVal = valTwo / valOne;
                    break;
            }
            operatorsSize--;
            values.Push(newVal);
            valuesSize++;
        }

        /// <summary>
        /// Pops off the integer at the top of the value stack.
        /// </summary>
        /// <returns></returns>
        /// The integer at the top of the stack if there is one.
        public static int popValue()
        {
            int val = 0;
            if (checkStackSize("v"))
            {
                val = values.Pop();
                valuesSize--;
            }
            else
            {
                throw new System.Exception("ERROR: can't retreive an integer from values stack");
            }
            return val;
        }

        public static void readRightParanth()
        {
            //read and perform the operations on stack until a given number of operands read
            for (int i = 0; i < operatorsSize; i++)
            {
                if (operators.Peek() != "(")
                {
                    if (operators.Peek() == "+" || operators.Peek() == "-")
                    {
                        sumOrSubtract();
                    }
                    if (operators.Peek() == "/" || operators.Peek() == "*")
                    {
                        multiOrDiv();
                    }
                }
                else
                {
                    break;
                }
            }
            //check for left parenthesis or a multiply or divide operation
            if (checkStackSize("o"))
            {
                if (operators.Peek() == "(")
                {
                    operators.Pop();
                    operatorsSize--;
                }
                if (checkStackSize("o"))
                {
                    if (operators.Peek() == "/" || operators.Peek() == "*")
                     {
                          multiOrDiv();
                     }
                }
            }
            else
            {
                throw new System.Exception("ERROR: invalid use of parenthisis");
            }
            return;
        }
       
    }

}
