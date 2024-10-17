using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphingCalc
{
    public class Domain_Parser
    {
        private int index;
        private string input;
        private string variableName;
        public Domain_Parser(string variableName)
        {
            this.variableName = variableName;
        }

        public float Parse(string expression, float variableValue)
        {
            //s represents a symbol, n represents a number, f represent a function, (, ), ^, *, /, +, - are all straight forward
            //evaluate for unique numbers like e or pi and replace them with a float value
            //evaluate subfunctions like sin as f(n)
            //evaluate parenthesis that follow format of (n) where n is a number
            //evaluate exponents 
            return 0f;
        }


    }
}