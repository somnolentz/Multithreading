using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class JobManager : MonoBehaviour
{
    private NativeList<int> randomIntList;
    private NativeArray<char> randomStringList;

    private void Start()
    {
        randomIntList = new NativeList<int>(Allocator.Persistent); //allocates memory for the nativelist to make sure it persists after the current frame, you manually have to call dispose after.
                                                                   //theres stuff like temp, tempjobs etc that is only valid until the end of the frame. they are typically used for effeciency but using persistency here is just easier 
        GenNumbers(100);                                           //native lists are used for thread safety; prevents spilling the milk into the same milk corrupting it :) lists can lead to corruption. also takes less resources 

        //scheduling the jobs ; defining what the job is and what data it will do (actual logic is in the struct on the bottom), 

        randomStringList = new NativeArray<char>(randomIntList.Length * 2, Allocator.Persistent);  // each int in the array is getting 4 characters in conjunction

        for (int i = 0; i < randomIntList.Length; i++) //for each int generate a string
        {
            GenString(i);
        }

        CalculationJob calcJob = new CalculationJob //creating instance of calcjob and initializing it; whats under is describing the datatype were processing, in this case an array.
        {
            RandomIntList = randomIntList.AsDeferredJobArray(), //converts array into deffered array type thats used for unity to manage data
        };

        JobHandle calcHandle = calcJob.Schedule(randomIntList.Length, 20); //job handles are a struct that references the job that has been scheduled, 20 is the batch size, how many iterations of a job


        //now doing the same for our stringgen job!
        int StringLength = 4; 
        int totalCharCount = randomIntList.Length * StringLength;
        StringGen stringJob = new StringGen
        {
            RandomStringList = randomStringList,
            StringLength = 4
        };
        JobHandle stringHandle = stringJob.Schedule(randomIntList.Length, 10);

        JobHandle combinedHandle = JobHandle.CombineDependencies(calcHandle, stringHandle); //making it so the jobs need each other to run
        combinedHandle.Complete(); //ensures job is done before proceeding. acts like a block insurance

        for (int i = 0; i < randomStringList.Length; i += 4)
        {
            Debug.Log(new string(randomStringList.GetSubArray(i, 4).ToArray()));
        }

        randomStringList.Dispose();
        randomIntList.Dispose(); //since the list is frame persistent and no longer relevant, discard it or face sims 3 levels of memory leakage yummers

    }
    //makes it faster nyoom
    private struct CalculationJob : IJobParallelFor //for jobs that divide their work among multiple threads
    {
        [ReadOnly] public NativeArray<int> RandomIntList;

        public void Execute(int index) //just doing random math calcs.
        {
            int randomNum = RandomIntList[index];
            int result = randomNum * randomNum;

            Debug.Log(result);
        }
    }

    private void GenNumbers(int count)
    {
        randomIntList.Clear();

        for (int i = 0; i < count; i++)
        {
            randomIntList.Add(UnityEngine.Random.Range(1, 100));
        }
    }


    private struct StringGen : IJobParallelFor
    {
        public NativeArray<char> RandomStringList;
        public int StringLength;

        public void Execute(int index)
        {
            int startIndex = index * StringLength;
            if (startIndex >= RandomStringList.Length) return;

            for (int i = 0; i < StringLength; i++)
            {
                int arrayIndex = startIndex + i;
                if (arrayIndex < RandomStringList.Length)
                {
                    char charToProcess = RandomStringList[arrayIndex];
                }
            }
        }
    }
    private void GenString(int index)
    {
        int startIndex = index * 4;
        for (int i = 0; i < 4; i++)
        {
            if (startIndex + i < randomStringList.Length)
            {
                char randomChar = (char)UnityEngine.Random.Range(97, 123);
                randomStringList[startIndex + i] = randomChar;
            }
        }
    }
}
    