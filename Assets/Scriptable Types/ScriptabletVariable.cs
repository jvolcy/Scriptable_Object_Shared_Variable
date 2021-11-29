using UnityEngine;

/*
 * ScriptableVariable
 * Base class for scriptable variable classes FloatVariable, InvVariable,
 * StringVariable, etc...
 * Custom variables are also possible.  Simply derive from this class
 * using the following template:
 * 
 * using UnityEngine;
 * [CreateAssetMenu(menuName = "Scriptable Types/XXX Variable")]
 * public class XXXVariable : ScriptableVariable<xxx> { }
 * 
 * Replace XXX with the name of your time and replace xxx with the
 * type.  Store the declaration in a file with the same name as
 * the template (XXXVariable).
 * 
 * The ScriptableVariable is a generic class with a single class
 * member.  "value" is polymorphically typed to match the type of the
 * derived class.
*/
abstract public class ScriptableVariable<T> : ScriptableObject
{
    public T value;
}

