using UnityEngine;
using TMPro;
using System.Text;

public class JobHandler : MonoBehaviour
{
    private bool isFirstJobComplete = false;
    private bool isSecondJobComplete = false;
    private int[] randomNumbersGenerated;
    private string[] secondJobResults;

    private void Start()
    {
        GenerateRandomNum firstJob = new GenerateRandomNum();
        GenerateRandomString secondJob = new GenerateRandomString();

        firstJob.onJobFinishedEvent += FirstJobFinished;
        secondJob.onJobFinishedEvent += SecondJobFinished;

        JobSystem.SubmitJobToPool(firstJob);
        JobSystem.SubmitJobToPool(secondJob);
    }

    private void FirstJobFinished(int[] results)
    {
        foreach (int result in results)
        {
            Debug.Log(result);
            randomNumbersGenerated = results;
        }

        isFirstJobComplete = true;
        DependencyCheck();
    }

    private void SecondJobFinished(string[] results)
    {
        foreach (string result in results)
        {
            Debug.Log(result);
            secondJobResults = results;
        }

        isSecondJobComplete = true;
        DependencyCheck();
    }

    private void DependencyCheck()
    {
        if (isFirstJobComplete && isSecondJobComplete)
        {
            GenerateKey(randomNumbersGenerated, secondJobResults);
        }
    }

    private void GenerateKey(int[] numbers, string[] strings)
    {
        StringBuilder combinedResults = new StringBuilder();

        for (int i = 0; i < Mathf.Min(numbers.Length, strings.Length); i++)
        {
            combinedResults.AppendLine($"Combined Result {i}: {numbers[i]} - {strings[i]}");
        }
        Debug.Log("Generated Keys:\n" + combinedResults.ToString());
    }

}
