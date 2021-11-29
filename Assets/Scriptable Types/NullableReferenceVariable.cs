/*
class NullableReferenceVariable
Nullable reference variables contain 2 parts: a reference to a scriptable
object variable and an internal local variable both of generic type <T>.
When the SO reference is null, the class uses the internal variable.
That way, code that is based on NullableSharedVariables never
suffer from Null Reference problems even when the SO is not present
or not connected in the Inspector.

This helper class provides a way to leverage the scriptable variable while
making it possible to use a local variable if the scriptable is not connected.
This is helpful for debugging and makes for modular design in the sense that
modules that use this have no absolute dependency on shared variables that
may or may not be present.

To use this class, declare a public ScriptableVariable of type <T>. This should
match the type of the SO you wish to access.  In your Start() or Awake()
function, initiantiate a variable of type NullabeReferenceVariable passing it
the SO variable reference and the initial value for a corresponding local
variable to be created and used if the SO reference is null.  Access the value
of the nullable with the .value member.  Example:

FloatVariable So_Var;
float localInitVal;
NullableReferenceVariable var = new NullableReferenceVariable<float>(So_Var, localInitVal)

Now access with var.value.  If the SO (So_Var) is connected, var.value refers
to the value of So_Var.  If the SO is not connected, var.value refers to a
local variable initialized to the value of localInitVal.
*/
public class NullableReferenceVariable<T>   //Scriptable Object and Base Type
{
    public ScriptableVariable<T> mReference_T; //a reference to a scriptable variable
    T mLocal_T;        //the local variable used if the scriptable variable is null
 
    //constructor
    public NullableReferenceVariable(ScriptableVariable<T> reference_T, T initialValueForLocal_T)
    {
        mReference_T = reference_T;   //reference T variable
        mLocal_T = initialValueForLocal_T;   //local T variable
    }

    //value property
    public T value
    {
        get
        {
            //if the scriptable is not null, return it.  Otherwise, return the local variable
            return mReference_T ? mReference_T.value : mLocal_T;
        }
        set
        {
            //if the scriptable is not null, set it
            if (mReference_T)
            {
                mReference_T.value = value;
            }
            else
            {
                //otherwise, set the local value
                mLocal_T = value;
            }
        }
    }

}
