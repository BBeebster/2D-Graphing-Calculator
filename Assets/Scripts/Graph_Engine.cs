using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

        private void Start()
        {
            string f = inputWindow.GetFunction();
            ParseFunction(f);
        }
        private void Update()
        {

        }

        public void ParseFunction(string function)
        {
            Debug.Log("function: " + function);
            string mutatedFunction = RemoveSpace(function);
            Debug.Log("mutated function: " + mutatedFunction);
        }


        private static readonly Regex sWhitespace = new Regex(@"\s+");
        public static string RemoveSpace(string input)
        {
            return sWhitespace.Replace(input, string.Empty);
        }

    }
}