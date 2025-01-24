using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class Day03Solver : MonoBehaviour
{

    [SerializeField] float msPerStep = 50f;
    [SerializeField] GameObject presentPrefab;
    [SerializeField] GameObject roboPresentPrefab;


    private int index;
    private int indexTarget;
    private string puzzleInput = "";

    (int, int) currentLocation = (0, 0);
    (int, int) p2santa = (0, 0);
    (int, int) p2robo = (0, 0);
    bool isSantaTurn = true;


    private static Dictionary<(int, int), int> deliveries = new Dictionary<(int, int), int>();
    private static Dictionary<(int, int), int> p2deliveries = new Dictionary<(int, int), int>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Solve();
    }



    // Update is called once per frame
    void Update()
    {

    }

    void Solve()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("day03-input");
        puzzleInput = textAsset.text;
        indexTarget = puzzleInput.Length - 1;

        index = 0;

        incr(deliveries, currentLocation);
        incr(p2deliveries, p2santa);
        placeAt(p2santa, p2deliveries[p2santa], presentPrefab);
        incr(p2deliveries, p2robo);
        placeAt(p2robo, p2deliveries[p2robo], roboPresentPrefab);

        StartCoroutine(Step());
    }

    private IEnumerator Step()
    {
        while (index < indexTarget)
        {
            yield return new WaitForSeconds(msPerStep / 1_000f);

            char c = puzzleInput[index];

            currentLocation = move(c, currentLocation);
            incr(deliveries, currentLocation);

            if (isSantaTurn)
            {
                p2santa = move(c, p2santa);
                incr(p2deliveries, p2santa);
                placeAt(p2santa, p2deliveries[p2santa], presentPrefab);

            }
            else
            {
                p2robo = move(c, p2robo);
                incr(p2deliveries, p2robo);
                placeAt(p2robo, p2deliveries[p2robo], roboPresentPrefab);
            }
            isSantaTurn = !isSantaTurn;


            index++;
        }
    }

    private void placeAt((int, int) loc, int height, GameObject prefab)
    {
        Vector3 position = new Vector3(loc.Item1, loc.Item2, height);
        Instantiate(prefab, position, Quaternion.identity);
    }

    private void incr(Dictionary<(int, int), int> dict, (int, int) loc)
    {
        if (!dict.ContainsKey(loc))
        {
            dict[loc] = 0;
        }

        dict[loc]++;
    }

    private (int, int) move(char c, (int, int) loc)
    {
        (int, int) newLoc;

        switch (c)
        {
            case '^':
                newLoc = (
                    loc.Item1,
                    loc.Item2 - 1
                );
                break;
            case 'v':
                newLoc = (
                    loc.Item1,
                    loc.Item2 + 1
                );
                break;
            case '<':
                newLoc = (
                    loc.Item1 - 1,
                    loc.Item2
                );
                break;
            case '>':
                newLoc = (
                    loc.Item1 + 1,
                    loc.Item2
                );
                break;
            default:
                Debug.LogError($"Unknown input character: {c}");
                newLoc = loc;
                break;
        }

        return newLoc;
    }
}
