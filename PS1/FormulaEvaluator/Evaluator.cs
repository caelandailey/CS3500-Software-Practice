using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace FormulaEvaluator
{
    /// <summary>
    /// Made to perform simple algebraic expressions.
    /// </summary>
    /// Author: Karina Biancone
    public static class Evaluator
    {
        public delegate int Lookup(String v);



        /// <summary>
        /// Reads and performs an expressions, if valid.
        /// </summary>
        /// <param name="exp"></param>
        /// The expression to be operated
        /// <param name="variableEvaluator"></param>
        /// The delegate that determines if a variable can be converted to a valid integer
        /// <returns></returns>
        /// returns the final value after expression has been succesfully operated on
        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            //the stack that will hold integer values from the expression
            Stack<int> values = new Stack<int>();
            //the stack that will hold operations from the expressions
            //only valid expressions are (,+,-,*,/, or )
            Stack<string> operators = new Stack<string>();

            exp = exp.Replace(" ", String.Empty);
            exp = exp.Replace("\t", String.Empty);
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            foreach (string token in substrings)
            {
                int value = 0;
                if (String.IsNullOrWhiteSpace(token))
                {
                    continue;
                }
                //check if token is an integer
                else if (Int32.TryParse(token, out value))
                {
                    intRead(value,operators, values);
                }
                //check if token is a valid variable
                //char.IsLetter(token.FirstOrDefault()) might be better?
                else if (!String.IsNullOrEmpty(token)&&Char.IsLetter(token[0])&&Char.IsNumber(token[token.Length-1]))
                {
                    int varValue = variableEvaluator(token); //do I need to write my own expression?
                    intRead(varValue, operators, values);
                }

                //check if token is addition or subtrction
                else if (token == "+" || token == "-")
                {
                    if (checkStackSize("o", operators, values))
                    {
                        if(checkAddOrMinus(operators, values))
                        {
                            sumOrSubtract(operators, values);
                        }
                    }
                    operators.Push(token);
                }

                //check if token is multiplication or division
                else if (token == "*" || token == "/")
                {
                    operators.Push(token);
                }

                //check left parathsis
                else if(token == "(")
                {
                    operators.Push(token);
                }

                //check right parenthisis
                else if(token == ")")
                {
                    readRightParanth(operators, values);
                }

                //invalid token
                else
                {
                    throw new System.ArgumentException("ERROR: invalid token read");
                }
            }

            //no more tokens to read, check stacks to get final answer
            //there is only one value to read and nothing in operators stack
            if(values.Count == 1 && operators.Count == 0)
            {
                return values.Pop();
            }
            //there is one more operation to perform
            else if((values.Count == 2) && (operators.Count == 1))
            {
                //it must be addition or subtraction
                if(checkAddOrMinus(operators, values))
                {
                    sumOrSubtract(operators, values);
                    return values.Pop();
                }
                else
                {
                    throw new System.ArgumentException("ERROR: invalid operation for final answer");
                }
            }
            //if neither opptions are true the expression is invalid
            else
            {
                throw new System.ArgumentException("ERROR: expression is invalid");
            }
        }



        /// <summary>
        /// Makes sure that there is at least one token in the stack
        /// </summary>
        /// <param name="s"></param>
        /// The key word to know whether to check the operators stack or the values stack
        /// <returns></returns>
        /// true if there is a token in the stack, false if not
        public static bool checkStackSize(string s, Stack<string> o, Stack<int> v)
        {
            //checks operation stack if string is o
            if(s == "o")
            {
                if(o.Count <= 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            //it will check values stack if any other string is passed
            else
            {
                if(v.Count <= 0)
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
        public static void intRead(int x, Stack<string> o, Stack<int> v)
        {
            if (checkStackSize("o", o, v))
            {
                if (checkMultiOrDiv(o,v))
                {
                    v.Push(x);
                    multiOrDiv(o,v);
                    return;
                }
                else
                {
                    v.Push(x);
                }
            }
            else
            {
                v.Push(x);
            }
            return;
        }

        /// <summary>
        /// Performs operation on two values already recorded. Checks that there are two values to pull and whether to call on addition 
        /// or subtraction.
        /// </summary>
        public static void sumOrSubtract(Stack<string> o, Stack<int> v)
        {
            //pop the two values
            int valOne = popValue(o,v);
            int valTwo = popValue(o,v);
            
            //pop operator 
            int newVal = 0;
            switch (o.Pop())
            {
                case "+":
                    newVal = valOne + valTwo;
                    break;

                case "-":
                    newVal = valTwo - valOne;
                    break;
            }
            v.Push(newVal);
            return;
        }

        /// <summary>
        /// Muliplies or divides two values that have already been added to the stack.
        /// </summary>
        public static void multiOrDiv(Stack<string> o, Stack<int> v)
        {
            //pop the two values
            int valOne = popValue(o,v);
            int valTwo = popValue(o,v);
                       
            int newVal=0;
            switch (o.Pop())
            {
                case "*":
                    newVal = valOne * valTwo;
                    break;
                case "/":
                    if(valOne == 0)
                    {
                        throw new System.ArgumentException("ERROR: division by 0");
                    }
                    newVal = valTwo / valOne;
                    break;
            }
            v.Push(newVal);
        }

        /// <summary>
        /// Pops off the integer at the top of the value stack.
        /// </summary>
        /// <returns></returns>
        /// The integer at the top of the stack if there is one.
        public static int popValue(Stack<string> o, Stack<int> v)
        {
            int val = 0;
            if (checkStackSize("v", o, v))
            {
                val = v.Pop();
            }
            else
            {
                throw new System.ArgumentException("ERROR: can't retreive an integer from values stack");
            }
            return val;
        }

        public static void readRightParanth(Stack<string> o, Stack<int> v)
        {
            //read and perform the operations on stack until a given number of operands read
            for (int i = 0; i < o.Count; i++)
            {
                if (o.Peek() != "(")
                {
                    if (checkAddOrMinus(o,v))
                    {
                        sumOrSubtract(o, v);
                    }
                    if(checkStackSize("o", o, v))
                    {
                        if (checkMultiOrDiv(o,v))
                        {
                            multiOrDiv(o,v);
                        }
                    }
                   
                }
                else
                {
                    break;
                }
            }
            //check for left parenthesis or a multiply or divide operation
            if (checkStackSize("o", o, v))
            {
                if (o.Peek() == "(")
                {
                    o.Pop();
                }
                if (checkStackSize("o", o, v))
                {
                    if (checkMultiOrDiv(o,v))
                     {
                          multiOrDiv(o, v);
                     }
                }
            }
            else
            {
                throw new System.ArgumentException("ERROR: invalid use of parenthisis");
            }
            return;
        }

        public static bool checkAddOrMinus(Stack<string> o, Stack<int> v)
        {
           if(o.Peek() == "+" || o.Peek() == "-")
            {
                return true;
            }
            return false;
        }

        public static bool checkMultiOrDiv(Stack<string> o, Stack<int> v)
        {
            if(o.Peek() == "*" || o.Peek() == "/")
            {
                return true;
            }
            return false;
        }
       
    }

}
