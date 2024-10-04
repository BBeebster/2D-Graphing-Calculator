using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GraphingCalc
{
    public class UI_InputWindow : MonoBehaviour
    {
        public TMP_InputField inputField;
        private void Awake()
        {
            inputField = transform.Find("inputField").GetComponent<TMP_InputField>();

            Show();
            SetDefaultFunction("y = x");
        }

        private void Start()
        {
            
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetDefaultFunction(string inputString)
        {
            inputField.text = inputString;
        }
        public string GetFunction()
        {
            return inputField.text;
        }
    }
}