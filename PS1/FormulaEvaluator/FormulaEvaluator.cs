using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

//namespace Stack 
namespace FormulaEvaluator

  
{
    public static class FormulaEvaluator
    {
        public delegate int Lookup(String v);

        
         Stack<int> values = new Stack<int>();
         Stack<string> operators = new Stack<string>();


        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            //if the array is <= 2 it is not long enough to perform an operation
            if(substrings.Length() <= 2)
            {
                throw new System.Exception("The expression entered was not long enough to perform an expresion.");
            }

            //check that the first part of expresion is a value or "(" and then an operation
            if (!valueRead(0))
            {
                if (!openParenth(0))
                {
                    throw new System.Exception("The expression began with an invalid statemet.");
                }
                values.Push(substrings[0]);
            }


            //begin to read the string
            for(int i = 1; i < substrings.Length; i++)
            {

                if (substrings[i] == "+")
                {
                    //if i = substring.Length - 1 we are at the end of the express, pop value, 
                    //apply addition o the i++ is string[]
                    
                    //else push + to operators 
                }
                else if (substrings[i] == "-")
                {
                    //if i = substring.Length - 1 we are at the end of the express, pop value, 
                    //apply negation to the i++ is string[]

                    //else push + to operators 
                }
            }
            return 0;
        }
  
    }

    /// <summary>
    /// Checks if the next string read is a variable to add to values stack.
    /// </summary>
    /// <param name="i"></param>
    /// The spot in the substring array to look
    /// <returns></returns>
    /// true if it is an integer to add to stack, false if it is not an integer.
    public bool valueRead(int i)
        {
            if (substrings[i] != "(" || substrings[i] != ")" || substrings[i] != "+" || substrings[i] != "-" || substrings[i] != "/" || substrings[i] != "*")
            {
                int newInteger = Lookup(substrings[i]);
                //values.push(newInteger);
            return true;
            }
        return false;
        }

    /// <summary>
    /// Checks if the next string read is an open parenthesi. If it is it will perform all the math within the paranthesis if a closing is found. 
    /// If there is no closing paranthesis or an error is found with in the parenthesis will return false.
    /// </summary>
    /// <param name="i"></param>
    /// The string in the array to look at.
    /// <returns></returns>
    /// true if paranthesi is found and expression in paranthesis are valid, false if no parathesis
    public bool openParenth(int i)
    {
        if(substrings[i] == "(")
        {
            //check there is something to read          
            //if(i++ < substrings.Length())
                   i++;
            if (valueRead(i))
            {
                values.Push(substrings[i]);
                //check there is something to read
                    i++;
                while(substrings[i] != ")")
                {
                    if (operationRead(i))
                    {
                        //check there is something to read
                        i++;
                        performOperation(i);
                    }
                }
                return true;
            }
            else
            {
                if (substrings[i] == "(")
                {
                    //check there is something to read
                    i++;
                    return openParenth(i);
                }
                else
                {
                    return false;
                }
            }

        }
    }
}
