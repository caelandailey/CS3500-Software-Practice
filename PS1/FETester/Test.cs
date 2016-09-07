using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormulaEvaluator;

namespace FETester
{
    public class VariableTracker
    {
        // For variables, we want mapping between string -> int
        // e.g.
        //  "a1"   -> 50
        //  "ZZ14" -> 9
        // A Dictionary provides such a mapping.
        private Dictionary<String, int> vars;


        /// <summary>
        /// Default constructor.
        /// </summary>
        public VariableTracker()
        {
            vars = new Dictionary<string, int>();
        }

        /// <summary>
        /// Add a variable to the tracker.
        /// </summary>
        /// <param name="var">The name of the variable</param>
        /// <param name="value">The value of the variable</param>
        public void AddVariable(string var, int value)
        {
            vars.Add(var, value);
        }


        /// <summary>
        /// Lookup a variable in the tracker. Throw ArgumentException if the variable doesn't exist.
        /// </summary>
        /// <param name="v">The variable to lookup.</param>
        /// <returns>Returns the value of the variable.</returns>
        public int LookupVariable(String v)
        {
            if (vars.ContainsKey(v))
                return vars[v];
            throw new ArgumentException("Dictionary does not contain " + v);
        }

    }

    // <summary>
    /// Some basic testing of a FormulaEvaluator.
    /// By no means are these tests sufficient.
    /// </summary>
    class SimpleTests
    {
        static void Main(string[] args)
        {

            try
            {
                // Define a couple of variables
                VariableTracker tracker = new VariableTracker();
                tracker.AddVariable("Z6", 10);
                tracker.AddVariable("a1", 9);

                // An expression with no variables, but we still pass it a Lookup delegate.
                Console.WriteLine(Evaluator.Evaluate("5 + 3 * 7 - 8 / (4 + 3) - 2 / 2", tracker.LookupVariable));

                // An expression with a variable that the delegate has a value for.
                Console.WriteLine(Evaluator.Evaluate("1+Z6-4", tracker.LookupVariable));

                // The same expression with a different delegate.
                Console.WriteLine(Evaluator.Evaluate("1+Z6-4", AllZeros));

                // An expression with a variable that the delegate does not have a value for. What should happen?
                // Console.WriteLine(Evaluator.Evaluate("5 + Q9", tracker.LookupVariable));

                //An expression with too many right parenthesis
               // Console.WriteLine(Evaluator.Evaluate("(6-4) + ) 2", tracker.LookupVariable));

                //An expression with too many left parenthesis
               //Console.WriteLine(Evaluator.Evaluate("((6-4) *2", tracker.LookupVariable));

                //A valid expression enclosed in ()
                Console.WriteLine(Evaluator.Evaluate("(3/4)", tracker.LookupVariable));

                //Dividing with 0
                //Console.WriteLine(Evaluator.Evaluate("3/(7-(2+5))", tracker.LookupVariable));

                //All operands without numbers
                //Console.WriteLine(Evaluator.Evaluate("+ / (+)", tracker.LookupVariable));

                //All numbers
                Console.WriteLine(Evaluator.Evaluate("666", tracker.LookupVariable));

                //number in ()
                Console.WriteLine(Evaluator.Evaluate("(6)", tracker.LookupVariable));

                //operation and number in ()
                // Console.WriteLine(Evaluator.Evaluate("(+0)", tracker.LookupVariable));

                //invalid operator
                //Console.WriteLine(Evaluator.Evaluate("3+3 = 6", tracker.LookupVariable));

                //difficult division
                Console.WriteLine(Evaluator.Evaluate("10/(6/3)/2", tracker.LookupVariable));

                //whitespace in expression
                Console.WriteLine(Evaluator.Evaluate("5    5 - 1", tracker.LookupVariable));

                //difficult parenthesis
                Console.WriteLine(Evaluator.Evaluate("(3) + 2", tracker.LookupVariable));

                //negative answer
                Console.WriteLine(Evaluator.Evaluate("(3-6) * 10", tracker.LookupVariable));

                //difficult division
                Console.WriteLine(Evaluator.Evaluate("4/2/9/4/1", tracker.LookupVariable));

                //random 1
                Console.WriteLine(Evaluator.Evaluate("(2 + (4/2+3)- 2)", tracker.LookupVariable));

                //order of operation
                Console.WriteLine(Evaluator.Evaluate("6 + 4 * 3 + 6 / 2", tracker.LookupVariable));

                //each number is in parenthesis
                Console.WriteLine(Evaluator.Evaluate("(((0) + (0) * (0) - (0)))", tracker.LookupVariable));

                //confusing parenthesis
                Console.WriteLine(Evaluator.Evaluate("((1)(()))", tracker.LookupVariable));

                //only parenthesis
                // Console.WriteLine(Evaluator.Evaluate("(()(()))", tracker.LookupVariable));

                //null
                //Console.WriteLine(Evaluator.Evaluate("2+5 null", tracker.LookupVariable));

                //one too operations
                //Console.WriteLine(Evaluator.Evaluate("3*2 +", tracker.LookupVariable));

                //one too many numbers
                //Console.WriteLine(Evaluator.Evaluate("7*2 + 4 (0)", tracker.LookupVariable));

                //random 2
                Console.WriteLine(Evaluator.Evaluate("(3)(4)(2)++", tracker.LookupVariable));

            }

            catch (ArgumentException e)
            {
                Console.WriteLine("exception caught: " + e.Message);
            }

            // Keep the console window open.
            Console.Read();
        }

        /// <summary>
        /// A simple delegate that tracks no variables.
        /// </summary>
        /// <param name="v">The variable to lookup.</param>
        /// <returns></returns>
        public static int NullLookup(String v)
        {
            throw new ArgumentException();
        }

        /// <summary>
        /// A simple delegate that gives all variables a value of 0.
        /// </summary>
        /// <param name="v">The variable to look up.</param>
        /// <returns>Always returns 0</returns>
        public static int AllZeros(String v)
        {
            return 0;
        }

    }



}

