using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphingCalc
{
    public class Domain_Parser : MonoBehaviour
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
            input = expression;
            index = 0;
            return ParseExpression(variableValue);
        }

        public float ParseExpression(float variableValue)
        {
            return 1f;
        }
    }
}