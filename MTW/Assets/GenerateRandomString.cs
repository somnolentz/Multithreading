using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

    public class GenerateRandomString : Job<string[]>
{
    private static readonly char[] Letters = "KILMEPLS".ToCharArray();
    public override void ExecuteJob()
    {
        int arraySize = 100;
        string[] results = new string[arraySize];
        System.Random random = new System.Random();
        for (int i = 0; i < arraySize; i++)
        {
            results[i] = RandomLettersGen(random, 4);
        }

        onJobFinishedEvent?.Invoke(results);
    }
    private string RandomLettersGen(System.Random random, int length)
    {
        char[] randomString = new char[length];

        for (int i = 0; i < length; i++)
        {
            randomString[i] = Letters[random.Next(Letters.Length)];
        }
        return new string(randomString);
    }
}