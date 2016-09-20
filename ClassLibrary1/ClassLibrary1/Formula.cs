// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax; variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }

        List<string> tokens = new List<string>();
        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            //check that there is a formula to read
            checkEmpty(formula.Length);

            tokens = GetTokens(formula).ToList();

            List<string> finalFormula = new List<string>();

            //check the first token is a variable, double, or lparenthesis
            checkFirstToken(tokens[0]);

            int lParenthCount = 0;
            int rParenthCount = 0;
            for (int i = 0; i <= tokens.Count; i++)
            {
                //check if a token is a double
                if (isDouble(tokens[i]))
                {
                    if (i != 0)
                    {
                        //check that token is following an operator or left parenthesis
                        previousOperatorOrLParanthToken(tokens[i--], "Number");
                    }
                    //converts and floating points into a double and then a string
                    double value;
                    Double.TryParse(tokens[i], out value);

                    finalFormula.Add(value.ToString());
                }

                //check for a variable
                if (isVariable(tokens[i]))
                {
                    //check variable again
                    normalize(tokens[i]);
                    
                    //check normalized variable is valid
                    if (isValid(normalize(tokens[i])))
                    {
                        if (i != 0)
                        {
                            //check that token is following an operator or left parenthesis
                            previousOperatorOrLParanthToken(tokens[i--], "Variable");
                        }

                       finalFormula.Add(normalize(tokens[i]));
                    }
                    else
                    {
                        throw new FormulaFormatException("Variable is not valid. Be sure that the vaiable begins with a letter or underscore.");
                    }
                }

                //check token is a left parenthesis
                else if (isLeftParenth(tokens[i]))
                {
                    if (i != 0)
                    {
                        //check if token is following an operator or another left parenthesis
                        previousOperatorOrLParanthToken(tokens[i--], "Open parenthesis");
                    }

                    finalFormula.Add(tokens[i]);
                    lParenthCount++;
                }

                //check if token is a valid operator
                else if (isOperator(tokens[i]))
                {
                    //check that the token prior to operator is double, variable, or right parenthesis
                    checkPreviousToken(tokens[i--], "operator");
                    finalFormula.Add(tokens[i]);
                }

                //check if token is a right parenthesis
                else if (isRightParenth(tokens[i--]))
                {
                    //check that the count is acurrate 
                    if (lParenthCount > rParenthCount++)
                    {
                        throw new FormulaFormatException("There are more closing parenthesis than opening. Check that there is an opening for each closing parenthesis.");
                    }
                    //check that the token prior to operator is double, variable, or right parenthesis
                    checkPreviousToken(tokens[i--], "closing parenthesis");
                    finalFormula.Add(tokens[i]);
                    rParenthCount++;
                }
                //token didn't match any expected tokens, thus invalid
                else
                {
                    throw new FormulaFormatException("Invalid token was read. Check that the formula only uses +, -, *, or / operators.");
                }
            }

            //check the last token is a variable, double, or rparenthesis
            if (!(isDouble(tokens.Last()) || isVariable(tokens.Last()) || isLeftParenth(tokens.Last())))
            {
                throw new FormulaFormatException("The last token in the formula is invalid. Be sure that it is a number, valid variable, or closing parenthesis.");
            }
            finalFormula.Add(tokens.Last());

            //check balanced parenthesis 
            if (!(lParenthCount == rParenthCount))
            {
                throw new FormulaFormatException("Parenthesis do not match up. Check that there are the same number of closing and opening paranthesis. ");
            }
            tokens = finalFormula;
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            return null;
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            return null;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            return null;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens, which are compared as doubles, and variable tokens,
        /// whose normalized forms are compared as strings.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            return false;
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            return false;
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return false;
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return 0;
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }

        /// <summary>
        /// Takes the length of the formula entered and checks if it is empty, thus throwing a FormulaFormatException
        /// </summary>
        /// <param name="k"></param>
        private void checkEmpty(int k)
        {
            if (k == 0)
            {
                throw new FormulaFormatException("No formula read. Include a complete formula.");
            }
        }

        /// <summary>
        /// Makes sure that the first token in the formula is a double, variable, or left parenthesis
        /// </summary>
        /// <param name="s"></param>
        private void checkFirstToken(string s)
        {
            if (!(isDouble(s) || isVariable(s) || isLeftParenth(s)))
            {
                throw new FormulaFormatException("The first token in the formula is invalid. Be sure that it is a number, valid variable, or open parenthesis.");
            }
        }

        /// <summary>
        /// Confirms if token is a double.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool isDouble(string s)
        {
            double value;
            if (Double.TryParse(s, out value))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Confirms if a token is a variable. Must begin with a letter or underscore and be followed by a number or
        /// underscore.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool isVariable(string s)
        {
            //the fist character in the variable must be a letter or underscore
            if(Char.IsLetter(s[0]) || (s[0] == '_'))
            {
                //check that the rest of the variable is a letter, number, or variable
                for(int i = 1; i <= s.Length; i++)
                {
                    if(!(Char.IsLetterOrDigit(s[i]) || (s[i] == '_')))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Confirms if a token is a valid operator: +,-,*, or /
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool isOperator(string s)
        {
            if ((s == "+") || (s == "*") || (s == "-") || (s == "/"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Confirms if a token is a left parenthesis.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool isLeftParenth(string s)
        {
            if (s == "(")
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Confirms if a token is a right parenthesis.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool isRightParenth(string s)
        {
            if (s == ")")
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks the token that comes before the token just read. This is to make sure that formula has correct syntax. 
        /// Only called after a number, variable, or left parenthesis is read.
        /// </summary>
        /// <param name="s"></param>
        private void previousOperatorOrLParanthToken(string s, string t)
        {
            if (!(isOperator(s) || isLeftParenth(s)))
            {
                throw new FormulaFormatException(t + " is following an invalid token. Check for a missing operation.");
            }
        }

        /// <summary>
        /// Checks the token that comes before the token just read. This is to make sure that formula has correct syntax.
        /// Only called after an operator or right parenthesis is read.
        /// </summary>
        /// <param name="s"></param>
        private void checkPreviousToken(string s, string t)
        {
            if (isOperator(s) || isLeftParenth(s))
            {
                throw new FormulaFormatException("Invalid token before " + t + ". Check that there are no operators next to one another.");
            }

        }

    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }






        /// <summary>
        /// Reads and performs an expressions, if valid.
        /// </summary>
        /// <param name="exp"></param>
        /// The expression to be operated
        /// <param name="variableEvaluator"></param>
        /// The delegate that determines if a variable can be converted to a valid integer
        /// <returns></returns>
        /// returns the final value after expression has been succesfully operated on
        private double Evaluate(String exp)
        {
            //the stack that will hold integer values from the expression
            Stack<double> values = new Stack<double>();
            //the stack that will hold operations from the expressions
            //only valid expressions are (,+,-,*,/, or )
            Stack<string> operators = new Stack<string>();

            exp = exp.Replace(" ", String.Empty);
            exp = exp.Replace("\t", String.Empty);
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            foreach (string token in substrings)
            {
                double value;
                //check if token is an integer
                if (Double.TryParse(token, out value))
                {
                    intRead(value, operators, values);
                }
                //check if token is a valid variable
                //char.IsLetter(token.FirstOrDefault()) might be better?
                //else if (isVariable(token))
              //  {
                  //  int varValue = variableEvaluator(token);
                //    intRead(varValue, operators, values);
                //}

                //check if token is addition or subtrction
                else if (token == "+" || token == "-")
                {
                    if (checkStackSize("o", operators, values))
                    {
                        if (checkAddOrMinus(operators, values))
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
                else if (token == "(")
                {
                    operators.Push(token);
                }

                //check right parenthisis
                else if (token == ")")
                {
                    readRightParanth(operators, values);
                }

            }
            //no more tokens to read, check stacks to get final answer
            //there is only one value to read and nothing in operators stack
            if (values.Count == 1 && operators.Count == 0)
            {
                return values.Pop();
            }
            //there is one more operation to perform
            else if ((values.Count == 2) && (operators.Count == 1))
            {
                //it must be addition or subtraction
                if (checkAddOrMinus(operators, values))
                {
                    sumOrSubtract(operators, values);
                    return values.Pop();
                }
                else
                {
                    throw new FormatException("ERROR: invalid operation for final answer");
                }
            }
            //if neither opptions are true the expression is invalid
            else
            {
                throw new FormatException("ERROR: expression is invalid");
            }
        }



        /// <summary>
        /// Makes sure that there is at least one token in the stack
        /// </summary>
        /// <param name="s"></param>
        /// The key word to know whether to check the operators stack or the values stack
        /// <returns></returns>
        /// true if there is a token in the stack, false if not
        public static bool checkStackSize(string s, Stack<string> o, Stack<double> v)
        {
            //checks operation stack if string is o
            if (s == "o")
            {
                if (o.Count <= 0)
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
                if (v.Count <= 0)
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
        public static void intRead(double x, Stack<string> o, Stack<double> v)
        {
            if (checkStackSize("o", o, v))
            {
                if (checkMultiOrDiv(o, v))
                {
                    v.Push(x);
                    multiOrDiv(o, v);
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
        public static void sumOrSubtract(Stack<string> o, Stack<double> v)
        {
            //pop the two values
            double valOne = popValue(o, v);
            double valTwo = popValue(o, v);

            //pop operator 
            double newVal = 0;
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
        public static void multiOrDiv(Stack<string> o, Stack<double> v)
        {
            //pop the two values
            double valOne = popValue(o, v);
            double valTwo = popValue(o, v);

            double newVal = 0;
            switch (o.Pop())
            {
                case "*":
                    newVal = valOne * valTwo;
                    break;
                case "/":
                    if (valOne == 0)
                    {
                        throw new FormatException("ERROR: division by 0");
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
        public static double popValue(Stack<string> o, Stack<double> v)
        {
            double val = 0;
            if (checkStackSize("v", o, v))
            {
                val = v.Pop();
            }
            else
            {
                throw new FormatException("ERROR: Too many operations for numbers read.");
            }
            return val;
        }

        public static void readRightParanth(Stack<string> o, Stack<double> v)
        {
            //read and perform the operations on stack until a given number of operands read
            for (int i = 0; i < o.Count; i++)
            {
                if (o.Peek() != "(")
                {
                    if (checkAddOrMinus(o, v))
                    {
                        sumOrSubtract(o, v);
                    }
                    if (checkStackSize("o", o, v))
                    {
                        if (checkMultiOrDiv(o, v))
                        {
                            multiOrDiv(o, v);
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
                    if (checkMultiOrDiv(o, v))
                    {
                        multiOrDiv(o, v);
                    }
                }
            }
            else
            {
                throw new FormatException("ERROR: invalid use of parenthisis");
            }
            return;
        }

        public static bool checkAddOrMinus(Stack<string> o, Stack<double> v)
        {
            if (o.Peek() == "+" || o.Peek() == "-")
            {
                return true;
            }
            return false;
        }

        public static bool checkMultiOrDiv(Stack<string> o, Stack<double> v)
        {
            if (o.Peek() == "*" || o.Peek() == "/")
            {
                return true;
            }
            return false;
        }

    }
}

