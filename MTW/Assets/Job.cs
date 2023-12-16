using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Job<T>
{
    public delegate void JobFinished(T output); // T is a generic Type

    public JobFinished onJobFinishedEvent; // event to call when job is done
    public abstract void ExecuteJob(); //actual job function
}
