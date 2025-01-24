using System.Collections;
using TMPro;
using UnityEngine;


public class Day01Solver : MonoBehaviour
{
    [SerializeField] float msPerStep = 5f;
    [SerializeField] GameObject Elevator;
    [SerializeField] GameObject TrailingCubePrefab;

    [SerializeField] private float yPerFloor = 0.75f;

    [SerializeField] TextMeshProUGUI positionText; 
    [SerializeField] TextMeshProUGUI maximumText; 
    [SerializeField] TextMeshProUGUI minimumText; 
    [SerializeField] TextMeshProUGUI basementEnteredText; 

    [SerializeField] GameObject groundCrossedIndicator;

    private int index;
    private int indexTarget;
    private string puzzleInput = "";

    private int maxFloorReached;
    private int minFloorReached;

    private int currentFloor = 0;
    private int indexEnterBasement = -1;

    private bool isSolving = false;


    void Start()
    {
        Solve();
    }

    private void Update()
    {
        Elevator.transform.position = new Vector3(0, currentFloor / yPerFloor, 0);
        positionText.text = $"Position: {index + 1}";
        maximumText.text = $"** Maximum Floor: {maxFloorReached}";
        minimumText.text = $"Minimum Floor: {minFloorReached}";
    }

    void Solve()
    {
        if (!isSolving)
        {
            TextAsset textAsset = Resources.Load<TextAsset>("day01-input");
            puzzleInput = textAsset.text;
            indexTarget = puzzleInput.Length - 1;

            index = 0;
            maxFloorReached = 0;
            minFloorReached = 0;
            currentFloor = 0;
            indexEnterBasement = -1;

            StartCoroutine(Step());
        }

    }

    private IEnumerator Step()
    {
        while (index < indexTarget)
        {
            //yield return null;
            yield return new WaitForSeconds(msPerStep / 1_000f);
            if (puzzleInput[index] == '(')
            {
                currentFloor++;
            }
            else if (puzzleInput[index] == ')')
            {
                currentFloor--;
            }

            if (currentFloor == -1 && indexEnterBasement == -1)
            {
                indexEnterBasement = index + 1;
                groundCrossedIndicator.GetComponent<MeshRenderer>().enabled = true;
                basementEnteredText.text = $"** Basement Entered: {indexEnterBasement}";
            }

            if (currentFloor > maxFloorReached) maxFloorReached = currentFloor;
            if (currentFloor < minFloorReached) minFloorReached = currentFloor;
            index++;

            if (index % 1 == 0)
            {
                Instantiate(TrailingCubePrefab, new Vector3(0, currentFloor/yPerFloor, 0), Quaternion.identity);
            }
        }

        isSolving = false;
        Debug.Log($"Part 1: {currentFloor}");
        Debug.Log($"Part 2: {indexEnterBasement}");

        Debug.Log($"minFloorReached: {minFloorReached}");
        Debug.Log($"maxFloorReached: {maxFloorReached}");
    }

}
