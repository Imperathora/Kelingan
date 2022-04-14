using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{    
    public class HideMultiPropertyAttribute : PropertyAttribute
    {
        public string ConditionalSourceField = "";
        public string EnumValue = "";

        public bool HideInInspector = false;

        public HideMultiPropertyAttribute(string _conditionalSourceField)
        {
            ConditionalSourceField = _conditionalSourceField;
            HideInInspector = false;
        }

        public HideMultiPropertyAttribute(string _conditionalSourceField, bool _hideInInspector)
        {
            ConditionalSourceField = _conditionalSourceField;
            HideInInspector = _hideInInspector;
        }

        public HideMultiPropertyAttribute(string _conditionalSourceField, string _enumValue)
        {
            ConditionalSourceField = _conditionalSourceField;
            HideInInspector = false;
            EnumValue = _enumValue;
        }

        public HideMultiPropertyAttribute(string _conditionalSourceField, string _enumValue, bool _hideInInspector)
        {
            ConditionalSourceField = _conditionalSourceField;
            HideInInspector = _hideInInspector;
            EnumValue = _enumValue;
        }
    }
}