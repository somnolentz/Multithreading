using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class JobSystem
{
    public static void SubmitJobToPool<T>(Job<T> job)
    {
        ThreadPool.QueueUserWorkItem(new WaitCallback((object state) => { job.ExecuteJob(); }), null); 
    }
}