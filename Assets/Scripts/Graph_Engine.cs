using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Rendering;

namespace GraphingCalc
{
    public class Graph_Engine : MonoBehaviour
    {

        [SerializeField]
        private UI_InputWindow inputWindow;

        [SerializeField, Range(10, 1000)]
        int resolution = 10;

        private string previousDomain;
        private string variableName;

        private LineRenderer lineRenderer;

        private void Awake()
        {
            InitializeLineRenderer();
        }

        private void Start()
        {
            //Decipher function to plot
            string previousFunction = inputWindow.GetFunction();
            previousDomain = CheckValidFunction(previousFunction);
            //pass the expression to the graph drawer which will also call the parser
            if (previousDomain != null) //if it's null it was invalid at somepoint already and there is nothing to try and draw
            {
                DrawGraph(previousDomain, variableName);
            }
            
        }
        private void Update()
        {
            //change if the function has changed
            string currentFunction = inputWindow.GetFunction();
            string currentDomain = CheckValidFunction(currentFunction);
            if (!currentDomain.Equals(previousDomain))
            {
                if (currentDomain != null)
                {
                    ClearGraph();
                    DrawGraph(currentDomain, variableName);
                }
                //update previousDomain
                previousDomain = currentFunction;
            }
        }

        public void DrawGraph(string expression, string variableName)
        {
            //get domain to draw for
            //break the domain up into an amount of points equal to the resolution
            //evaluate the domain at each point with the parser
                //store the points in a vector2 format to be used to draw a line
            //draw the line using the points
            Domain_Parser parser = new Domain_Parser(variableName);

            float x = 0f;
            float result = parser.Parse(previousDomain, x);
        }

        public void ClearGraph()
        {
            lineRenderer.positionCount = 0; // Destroy the old line
        }
        
        void InitializeLineRenderer()
        {
            //create the lines between points
            lineRenderer = gameObject.AddComponent<LineRenderer>();

            //decide on line thickness
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;


            //set the material of the line and make it red
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;

            //set the number of points in the Line Renderer to match the array length of points
            lineRenderer.positionCount = resolution;
            lineRenderer.useWorldSpace = true;
        }

        public string CheckValidFunction(string function) //if a function passes all tests it will return a domain to parse, otherwise it will return null
        {

            string mutatedFunction = RemoveSpace(function);
            string domain;
            //implicit expressions can be broken up into a 'range' '=' 'domain'
                //an expression is valid if it consists of a domain only, or all 3 elements
                //a range will always be written as a variable like y, x, z. Or it can be written as a function like f(x), h(x), k(x)
                //we will make sure we recognize unique letter like e, they will not be usable as variables

            //count how many equal symbols exist
            int countEqualSymbols = mutatedFunction.Count(c => c == '=');

            if (countEqualSymbols == 0) //if there is no '=' symbol
            {
                //check the domain
                return CheckValidDomain(mutatedFunction);
            }
            else if (countEqualSymbols == 1) //if there is one '=' symbol
            {
                //split the string on '=' into a range and domain string
                string[] parts = mutatedFunction.Split('=');
                string range = parts[0];
                domain = parts[1];

                //check that the range follows the format of f(x) or y
                string rangeFormat = @"^[a-zA-Z]$|^[a-zA-Z]\([a-zA-Z]\)$";
                if (Regex.IsMatch(range, rangeFormat))
                {
                    string functionFormat = @"^[a-zA-Z]\([a-zA-Z]\)$";
                    if (Regex.IsMatch(range, functionFormat)) //if the format is function like, we need to store the variable in the ( ) charactere
                    {
                        //pass the char to check the validity of the domain
                        return CheckValidDomain(domain, range[2]);
                    }
                    else // otherwise we don't care and can evaluate the domain without a variable
                    {
                        //check the domain
                        return CheckValidDomain(domain);
                    }
                } else
                {
                    return null;
                }
            } else {
                return null;
            }

        }

        public string CheckValidDomain(string domain, char? variable = null)
        {
                //is only one varaible used?
                //do all parenthesis have a match?
                //if not, place in a parenthesis match where you think it should go.
                //do all operators ('+', '-', '*', '/', '^') have values on each end

            //temp to simplify the expression to check if it is a valid expression
            string mutatedDomain = domain;

            //get rid of any extra functions / reduce them down to inner terms for things like tan()
            mutatedDomain = mutatedDomain.Replace("e", "");
            mutatedDomain = mutatedDomain.Replace("pi", "");
            mutatedDomain = mutatedDomain.Replace("sin", "");
            mutatedDomain = mutatedDomain.Replace("cos", "");
            mutatedDomain = mutatedDomain.Replace("tan", "");
            mutatedDomain = mutatedDomain.Replace("cot", "");
            mutatedDomain = mutatedDomain.Replace("sec", "");
            mutatedDomain = mutatedDomain.Replace("csc", "");
            mutatedDomain = mutatedDomain.Replace("log", "");
            mutatedDomain = mutatedDomain.Replace("ln", "");
            mutatedDomain = mutatedDomain.Replace("sqrt", "");

            //we need to know what varaiable is used in the equation if we don't already know, we will always assume it is the first variable to appear
            if (variable == null)
            {
                foreach (char c in mutatedDomain)
                {
                    if (char.IsLetter(c))
                    {
                        variable = c;
                        variableName = $"{variable}"; //this is the variable we will pass to the parser
                        break;
                    }
                }
            }

            //check that the domain now only has the use of some specified varaible
            foreach (char c in mutatedDomain)
            {
                if (char.IsLetter(c) && c != variable)
                {
                    //return null if its invalid
                    Debug.Log("invalid expression");
                    return null;
                }
            }

            //now that we know there is only use of one specified variable we want to validate the parenthesis
            int openCount = 0;

            foreach (char c in mutatedDomain)
            {
                if (c == '(')
                {
                    openCount++;
                } else if (c == ')')
                {
                    openCount--;

                    if (openCount < 0)
                    {
                        Debug.Log("invalid parenthesis - too many closed");
                        return null;
                    }
                }
            }

            if (openCount > 0)
            {
                Debug.Log("invalid parenthesis - too many open");
                return null;
            }

            //we can return this string now that we validated balanced parenthesis and use of only one varaible
            //our parser will handle the rest of the mathematical validation
            return domain;
        }

        //remove whitespace from inputField using Regex
        private static readonly Regex sWhitespace = new Regex(@"\s+");
        public static string RemoveSpace(string input)
        {
            return sWhitespace.Replace(input, string.Empty);
        }

    }

}