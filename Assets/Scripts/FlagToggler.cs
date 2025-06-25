using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace kawanaka
{
    public class FlagToggler : MonoBehaviour
    {
        public void ToggleBoolFlag(GameObject target, string componentName, string flagName)
        {
            Component component = target.GetComponent(componentName);

            FieldInfo field = component.GetType().GetField(flagName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            bool currentValue = (bool)field.GetValue(component);
            field.SetValue(component, !currentValue);
        }
    }
}