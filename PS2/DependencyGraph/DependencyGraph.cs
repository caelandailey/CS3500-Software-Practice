// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        Dictionary<string, HashSet<string>> dependeesGraph;
        Dictionary<string, HashSet<string>> dependentsGraph;
        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            dependeesGraph = new Dictionary<string, HashSet<string>>();
            dependentsGraph = new Dictionary<string, HashSet<string>>();
        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get
            {
                int sum = 0;
                foreach (KeyValuePair<string, HashSet<string>> pairs in dependeesGraph)
                {
                    sum += pairs.Value.Count;
                }
                return sum;
            }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get
            {
                if (dependentsGraph.ContainsKey(s))
                {
                    return dependentsGraph[s].Count;
                }
                else
                {
                    return 0;
                }
            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            //check that there is a dependent for s
            if (dependeesGraph.ContainsKey(s))
            {
                //if there are any values in the set for the key s, then s has dependents
                if (dependeesGraph[s].Count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            //checks that s is a dependent
            if (dependentsGraph.ContainsKey(s))
            {
                //checks that there are values in the set for s
                if (dependentsGraph[s].Count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            //create an empty IEnumerable
            IEnumerable<string> allDependents = new List<string>(0);
            //checks that s has dependents
            if (dependeesGraph.ContainsKey(s))
            {
                //update list of dependents for s
                allDependents = dependeesGraph[s].ToList<string>();

            }
            return allDependents;
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            //initialize an empty IEnumerable
            IEnumerable<string> allDependees = new List<string>(0);
            //check that s has dependees
            if (dependentsGraph.ContainsKey(s))
            {
                //update list with dependees for s
                allDependees = dependentsGraph[s].ToList<string>();

            }
            return allDependees;
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {
            //check if there is no key for s in dependees dictionary
            if (!dependeesGraph.ContainsKey(s))
            {
                //create a new key for the dependency dictionary 
                dependeesGraph.Add(s, new HashSet<string>());
            }
            //add t to the hash set of dependees for s
            dependeesGraph[s].Add(t);

            //update dependents graph as well
            //check if there is a key for t in dependents dictionary
            if (!dependentsGraph.ContainsKey(t))
            {
                //create a new key for the dependents dictionaly
                dependentsGraph.Add(t, new HashSet<string>());
            }
            //add s to the hash set of dependents for t
            dependentsGraph[t].Add(s);
        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            //check that s is a dependee and t is a dependent
            if (dependeesGraph.ContainsKey(s) && dependentsGraph.ContainsKey(t))
            {
                //check that t is a dependent of s
                if (dependeesGraph[s].Contains(t))
                {
                    dependeesGraph[s].Remove(t);
                    //check if s has any other dependents
                    if (dependeesGraph[s].Count == 0)
                    {
                        dependeesGraph.Remove(s);
                    }
                    //update dependent graph for t

                    dependentsGraph[t].Remove(s);
                    if (dependentsGraph[t].Count == 0)
                    {
                        dependentsGraph.Remove(t);
                    }

                }
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            //check that s has dependents
            if (dependeesGraph.ContainsKey(s))
            {
                //remove all dependents of s
                foreach (string r in dependeesGraph[s].ToList())
                {
                    RemoveDependency(s, r);
                }

            }
            //add all new dependents to s
            foreach (string t in newDependents)
            {
                AddDependency(s, t);
            }

        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            //check that s is a dependent
            if (dependentsGraph.ContainsKey(s))
            {
                //remove all values that s depends on
                foreach (string r in dependentsGraph[s].ToList())
                {
                    RemoveDependency(r, s);
                }
            }
            //add new dependees for s
            foreach (string t in newDependees)
            {
                AddDependency(t, s);
            }
        }

    }


}
