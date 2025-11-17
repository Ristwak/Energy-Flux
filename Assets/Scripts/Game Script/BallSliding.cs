using UnityEngine;
using UnityEngine.UI;

public class BallSliding : MonoBehaviour
{
    [Header("Movement / Path")]
    public float minSlideSpeed = 1f;
    public float maxSlideSpeed = 4f;
    public Transform[] pathPoints;

    private float slideSpeed;
    private float[] segmentLengths;
    private float[] pointDistances;
    private float totalPathLength;
    private float distanceTravelled = 0f;
    private float previousDistanceTravelled = 0f;

    // NEW: use height range + precomputed energy map
    private float minY;
    private float maxY;

    private enum EnergyType { Kinetic, Potential }
    private EnergyType[] energyAtPoint;      // energy dominance at each path point
    private int lastStopPathIndex = -1;      // which path point we stopped at last time

    [Header("Question UI")]
    public GameObject questionPanel;
    public Button kineticButton;
    public Button potentialButton;
    public GameObject gameOverPanel;

    private int[] stopPointIndices;
    private int currentStopIndexIdx = 0;
    private bool isSliding = true;
    private bool isStopped = false;
    private bool isGameOver = false;
    private Vector3 startPosition;

    private bool isKineticCorrect = true;

    void Start()
    {
        InitPath();               // build path, compute minY/maxY
        PrecomputeEnergyMap();    // NEW: decide KE/PE at every path point
        SetupRandomStops();

        slideSpeed = Random.Range(minSlideSpeed, maxSlideSpeed);
        transform.position = startPosition;

        if (questionPanel != null)
            questionPanel.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // buttons just check against isKineticCorrect
        kineticButton.onClick.AddListener(() => AnswerQuestion(isKineticCorrect));
        potentialButton.onClick.AddListener(() => AnswerQuestion(!isKineticCorrect));
    }

    void Update()
    {
        if (!isSliding || isGameOver) return;

        previousDistanceTravelled = distanceTravelled;

        UpdateSpeedRandomly();
        SlideAlongPath();
        CheckForStopTrigger();
        CheckForGameOver();
    }

    void UpdateSpeedRandomly()
    {
        float changeAmount = Random.Range(-0.2f, 0.2f);
        slideSpeed += changeAmount;
        slideSpeed = Mathf.Clamp(slideSpeed, minSlideSpeed, maxSlideSpeed);
    }

    void InitPath()
    {
        if (pathPoints == null || pathPoints.Length < 2)
        {
            Debug.LogError("BallSliding: You must assign at least 2 path points.");
            startPosition = transform.position;
            return;
        }

        int count = pathPoints.Length;
        segmentLengths = new float[count - 1];
        pointDistances = new float[count];
        totalPathLength = 0f;

        pointDistances[0] = 0f;

        // init min/max Y from first point
        minY = maxY = pathPoints[0].position.y;

        for (int i = 0; i < count - 1; i++)
        {
            float segLen = Vector3.Distance(pathPoints[i].position, pathPoints[i + 1].position);
            segmentLengths[i] = segLen;
            totalPathLength += segLen;
            pointDistances[i + 1] = totalPathLength;

            float y0 = pathPoints[i].position.y;
            float y1 = pathPoints[i + 1].position.y;

            if (y0 < minY) minY = y0;
            if (y0 > maxY) maxY = y0;
            if (y1 < minY) minY = y1;
            if (y1 > maxY) maxY = y1;
        }

        startPosition = pathPoints[0].position;
    }

    void PrecomputeEnergyMap()
    {
        int count = pathPoints.Length;
        energyAtPoint = new EnergyType[count];

        for (int i = 0; i < count; i++)
        {
            // First and last point → no slope information → treat as potential
            if (i == 0 || i == count - 1)
            {
                energyAtPoint[i] = EnergyType.Potential;
                continue;
            }

            float prevY = pathPoints[i - 1].position.y;
            float currY = pathPoints[i].position.y;
            float nextY = pathPoints[i + 1].position.y;

            // Local peak
            if (currY > prevY && currY > nextY)
            {
                energyAtPoint[i] = EnergyType.Potential;
            }
            // Local valley
            else if (currY < prevY && currY < nextY)
            {
                energyAtPoint[i] = EnergyType.Kinetic;
            }
            // Upslope
            else if (currY < nextY)
            {
                energyAtPoint[i] = EnergyType.Potential;
            }
            // Downslope
            else
            {
                energyAtPoint[i] = EnergyType.Kinetic;
            }
        }
    }

    void SetupRandomStops()
    {
        int count = pathPoints.Length;

        System.Collections.Generic.HashSet<int> chosen = new System.Collections.Generic.HashSet<int>();
        int minIndex = 1;
        int maxIndex = count - 2;

        while (chosen.Count < 4 && chosen.Count < maxIndex - minIndex + 1)
        {
            chosen.Add(Random.Range(minIndex, maxIndex + 1));
        }

        stopPointIndices = new int[chosen.Count];
        chosen.CopyTo(stopPointIndices);
        System.Array.Sort(stopPointIndices);

        currentStopIndexIdx = 0;
    }

    void SlideAlongPath()
    {
        if (totalPathLength <= 0f) return;

        distanceTravelled += slideSpeed * Time.deltaTime;
        distanceTravelled = Mathf.Clamp(distanceTravelled, 0f, totalPathLength);

        float dist = distanceTravelled;
        int segmentIndex = 0;

        while (segmentIndex < segmentLengths.Length && dist > segmentLengths[segmentIndex])
        {
            dist -= segmentLengths[segmentIndex];
            segmentIndex++;
        }

        if (segmentIndex >= segmentLengths.Length)
        {
            transform.position = pathPoints[pathPoints.Length - 1].position;
            return;
        }

        Transform p0 = pathPoints[segmentIndex];
        Transform p1 = pathPoints[segmentIndex + 1];

        float t = segmentLengths[segmentIndex] > 0 ? dist / segmentLengths[segmentIndex] : 0f;
        transform.position = Vector3.Lerp(p0.position, p1.position, t);
    }

    void CheckForStopTrigger()
    {
        if (isStopped) return;
        if (stopPointIndices == null || stopPointIndices.Length == 0) return;
        if (currentStopIndexIdx >= stopPointIndices.Length) return;

        int pathPointIndex = stopPointIndices[currentStopIndexIdx];
        float stopDist = pointDistances[pathPointIndex];

        if (previousDistanceTravelled < stopDist && distanceTravelled >= stopDist)
        {
            transform.position = pathPoints[pathPointIndex].position;

            isStopped = true;
            isSliding = false;
            currentStopIndexIdx++;

            lastStopPathIndex = pathPointIndex;   // remember where we stopped
            DetermineCorrectAnswer();             // look up precomputed energy

            ShowQuestionPanel();
        }
    }

    void ShowQuestionPanel()
    {
        if (questionPanel != null)
            questionPanel.SetActive(true);
    }

    // NEW: simply read from precomputed energyAtPoint
    void DetermineCorrectAnswer()
    {
        if (energyAtPoint == null ||
            lastStopPathIndex < 0 ||
            lastStopPathIndex >= energyAtPoint.Length)
        {
            isKineticCorrect = true; // fallback
            return;
        }

        isKineticCorrect = (energyAtPoint[lastStopPathIndex] == EnergyType.Kinetic);
    }

    public void AnswerQuestion(bool isCorrect)
    {
        if (isCorrect)
        {
            if (questionPanel != null)
                questionPanel.SetActive(false);

            isStopped = false;
            isSliding = true;
        }
        else
        {
            ResetBall();
        }
    }

    void ResetBall()
    {
        distanceTravelled = 0f;
        previousDistanceTravelled = 0f;

        transform.position = startPosition;

        isStopped = false;
        isSliding = true;
        isGameOver = false;

        if (questionPanel != null)
            questionPanel.SetActive(false);

        SetupRandomStops();

        slideSpeed = Random.Range(minSlideSpeed, maxSlideSpeed);
    }

    void CheckForGameOver()
    {
        if (isGameOver) return;

        if (distanceTravelled >= totalPathLength)
        {
            isSliding = false;
            isGameOver = true;
            ShowGameOverPanel();
        }
    }

    void ShowGameOverPanel()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }
}
