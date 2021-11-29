Scriptable Object Variables as Globals

[The concepts outline here relating to the use of SOs as global variables are originally found here: *Ryan Hipple, [Game Architecture with Scriptable Objects](https://youtu.be/raQ3iHhE_Kk),  Unite Austin 2017*]

A Scriptable Object (SO) is a data container that you can use to store data, independent of class instances.  ScriptableObjects are not associated with GameObjects.  Instead, they are stored as Assets in your Project.  For this reason, they can be used as global variables instead of singletons.  Doing this avoids the problem of co-dependencies between scripts that need to access the same variables.  The classic approach of creating a “GameManager” class that contains shared static variables results in a co-dependence between the GameManager and any other class that accesses its static resources.  This can result in chain-dependencies.  A needs B, but since B needs C and D, porting A to another project requires importing (or fixing the co-dependencies between) B, C and D.

Using a ScriptableObject as a simple variable avoids these dependencies.

To use a ScriptableObject, create a script in your application’s Assets folder that inherits from the ScriptableObject class.  Use the CreateAssetMenu attribute to make it easy to create custom assets from the Editor menu.  For example, the script below adds a “Scriptable Types/Float Variable” entry to the Create menu of the Editor:  in the Project View right click and select Create->Scriptable Types->Float Variable.


|<p>using UnityEngine;</p><p></p><p>[CreateAssetMenu(menuName = "Scriptable Types/Float Variable")]</p><p>public class FloatVariable : ScriptableObject</p><p>{</p><p>`    `public float value;</p><p>}</p>|
| :- |

Doing so creates a ScriptableObject in the Assets window.  In this example, the object (of type FloatVariable) can store a single float value.  To access this value, in another script, create a public “FloatVariable” variable and attach the ScriptableObject to this variable on the Inspector panel.  This can be repeated for any number of scripts that need to access the SO.  Note that, in this way, the scripts depend only on the existence of the SO, not on each other.

Because it isn’t possible to create a generic SO, we would need to create a script similar to the FloatVariable script for each type of SO we want to create.  Later on, this will become a problem when we create NullableReferenceVariables.  Instead, we create a generic abstract base class of type ScriptableVariable from which we can derive our base SO types:

**ScriptableVariable Class**

|<p>using UnityEngine;</p><p></p><p>abstract public class ScriptableVariable<T> : ScriptableObject</p><p>{</p><p>`    `public T value;</p><p>}</p>|
| :- |

Base classes include containers for types float, int, bool, string, Vector2 and Vector3:

**FloatVariable Class**

|<p>using UnityEngine;</p><p></p><p>[CreateAssetMenu(menuName = "Scriptable Types/Float Variable")]</p><p>public class FloatVariable : ScriptableVariable<float> { }</p>|
| :- |

**IntVariable Class**

|<p>using UnityEngine;</p><p></p><p>[CreateAssetMenu(menuName = "Scriptable Types/Int Variable")]</p><p>public class IntVariable : ScriptableVariable<int> { }</p>|
| :- |

**BoolVariable Class**

|<p>using UnityEngine;</p><p></p><p>[CreateAssetMenu(menuName = "Scriptable Types/Bool Variable")]</p><p>public class BoolVariable : ScriptableVariable<bool> { }</p>|
| :- |

**StringVariable Class**

|<p>using UnityEngine;</p><p></p><p>[CreateAssetMenu(menuName = "Scriptable Types/String Variable")]</p><p>public class StringVariable : ScriptableVariable<string> { }</p>|
| :- |

**Vector2Variable Class**

|<p>using UnityEngine;</p><p></p><p>[CreateAssetMenu(menuName = "Scriptable Types/Vector2 Variable")]</p><p>public class Vector2Variable : ScriptableVariable<Vector2> { }</p>|
| :- |

**Vector3Variable Class**

|<p>using UnityEngine;</p><p></p><p>[CreateAssetMenu(menuName = "Scriptable Types/Vector3 Variable")]</p><p>public class Vector3Variable : ScriptableVariable<Vector3> { }</p>|
| :- |

Use any of the templates above to extend the shared SO variables to include any other custom types.

Nullable Reference Variables

At times,  we may want a class to work whether or not a Scriptable Object is connected.  In such cases, the desired behavior is to reference the scriptable object if it is connected and to use a local variable if it is not.  The NullableReferenceVariable class is a helper class that aims to simplify the process of doing this.  Here is the code:


|<p>public class NullableReferenceVariable<T>   //Scriptable Object and Base Type</p><p>{</p><p>`    `public ScriptableVariable<T> mReference\_T; //a reference to a scriptable variable</p><p>`    `T mLocal\_T;        //the local variable used if the scriptable variable is null</p><p> </p><p>`    `//constructor</p><p>`    `public NullableReferenceVariable(ScriptableVariable<T> reference\_T, T initialValueForLocal\_T)</p><p>`    `{</p><p>`        `mReference\_T = reference\_T;   //reference T variable</p><p>`        `mLocal\_T = initialValueForLocal\_T;   //local T variable</p><p>`    `}</p><p></p><p>`    `//value property</p><p>`    `public T value</p><p>`    `{</p><p>`        `get</p><p>`        `{</p><p>`            `//if the scriptable is not null, return it.  Otherwise, return the local variable</p><p>`            `return mReference\_T ? mReference\_T.value : mLocal\_T;</p><p>`        `}</p><p>`        `set</p><p>`        `{</p><p>`            `//if the scriptable is not null, set it</p><p>`            `if (mReference\_T)</p><p>`            `{</p><p>`                `mReference\_T.value = value;</p><p>`            `}</p><p>`            `else</p><p>`            `{</p><p>`                `//otherwise, set the local value</p><p>`                `mLocal\_T = value;</p><p>`            `}</p><p>`        `}</p><p>`    `}</p><p>}</p>|
| :- |


