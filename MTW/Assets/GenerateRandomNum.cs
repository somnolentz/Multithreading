using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
public class GenerateRandomNum : Job<int[]>
{
    public override void ExecuteJob()
    {
        int numberOfKeys = 100;
        int[] results = new int[numberOfKeys]; 
        System.Random random = new System.Random();

        for (int i = 0; i < numberOfKeys; i++)
        {
            results[i] = GenerateNumber(random);
        }

        onJobFinishedEvent?.Invoke(results);
    }

    private int GenerateNumber(System.Random random)
    {
        return random.Next(10000, 100000);
    }
}

