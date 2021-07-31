
public abstract class AVariableDisplayer<T> : VariableDisplayer
    where T : AVariable
{
    protected T TypedVariable => (T)targetData;

    protected override void Reset()
    {
        SetDevDescription("Observes a " + 
            typeof(T).Name + " Variable and displays its value on events.");
    }
}

