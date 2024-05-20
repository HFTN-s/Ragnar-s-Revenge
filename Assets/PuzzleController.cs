using System.Collections;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    public int totalPieces;
    private int piecesInPlace = 0;
    public GameObject runeParent;
    private GameObject[] rune = new GameObject[26];
    public GameObject leftClosetDoor;
    public GameObject rightClosetDoor;
    public int[] correctOrder = { 24, 4, 16, 9 }; // Sequence of rune IDs to be checked
    private int currentOrderIndex = 0;
    private GameObject[] runeLetters = new GameObject[26];
    private GameObject[] gemstones = new GameObject[26];
    public GameObject gemstoneParent;

    private bool[] isHit; // Array to track if each gemstone is being hit
    private Coroutine[] fadeCoroutines; // Array to store active fade coroutines
    private Coroutine[] runeFadeCoroutines; // Array to store rune fade coroutines

    void Start()
    {
        //initialize correct order
        correctOrder = new int[] { 24, 4, 16, 9 };
        Debug.Log("PuzzleController initialized.");

        isHit = new bool[gemstoneParent.transform.childCount];
        fadeCoroutines = new Coroutine[gemstoneParent.transform.childCount];
        runeFadeCoroutines = new Coroutine[runeParent.transform.childCount];

        for (int i = 0; i < runeParent.transform.childCount; i++)
        {
            rune[i] = runeParent.transform.GetChild(i).gameObject;
            Debug.Log($"Rune{i + 1} initialized: {rune[i].name}");
            rune[i].GetComponent<TouchRune>().runeID = i + 1;
            Transform intermediateChild = rune[i].transform.GetChild(0);
            Transform letterTransform = intermediateChild != null ? intermediateChild.Find("Letter") : null;
            if (letterTransform != null)
            {
                runeLetters[i] = letterTransform.gameObject;
                Debug.Log($"Rune Letter{i + 1} found: {runeLetters[i].name}");
            }
            else
            {
                Debug.LogError($"Rune Letter{i + 1} not found!");
            }
        }

        for (int i = 0; i < gemstoneParent.transform.childCount; i++)
        {
            gemstones[i] = gemstoneParent.transform.GetChild(i).gameObject;
        }

        Debug.Log("Gemstones initialized. Count: " + gemstoneParent.transform.childCount);
    }

    public void PiecePlaced()
    {
        piecesInPlace++;
        Debug.Log($"Piece placed. Total pieces in place: {piecesInPlace}/{totalPieces}");
        if (piecesInPlace >= totalPieces)
        {
            CompletePuzzle();
        }
    }

    private void CompletePuzzle()
    {
        Debug.Log("Puzzle completed! Activating rune, and waiting for correct order of runes pressed.");
        rune[1].SetActive(true);
    }

    public void RunePressed(int runeID)
{
    // Turn off all rune colliders for 2 seconds
    DisableAllRuneColliders();
    Invoke(nameof(EnableAllRuneColliders), 2f);

    if (correctOrder[currentOrderIndex] == runeID)
    {
        Debug.Log($"Correct rune pressed: {runeID}");
        SetRuneGlow(runeID, Color.green);
        currentOrderIndex++;

        if (currentOrderIndex == correctOrder.Length)
        {
            Debug.Log("Correct order entered. Opening closet.");
            StartCoroutine(MoveDoors(leftClosetDoor, rightClosetDoor, 3.4f, -3.5f, 2f));
            DisableAllRuneColliders();
        }
    }
    else
    {
        Debug.Log($"Incorrect rune pressed: {runeID}. Resetting sequence.");
        GlowAllRunes(Color.red);
        ResetAllRunes();
        currentOrderIndex = 0;
    }
}

private void EnableAllRuneColliders()
{
    foreach (GameObject runeObject in rune)
    {
        if (runeObject != null)
        {
            Collider collider = runeObject.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = true;
            }
        }
    }
}

    private IEnumerator MoveDoors(GameObject leftDoor, GameObject rightDoor, float leftX, float rightX, float duration)
{
    float timeElapsed = 0;
    Vector3 leftStartPos = leftDoor.transform.localPosition;
    Vector3 rightStartPos = rightDoor.transform.localPosition;

    while (timeElapsed < duration)
    {
        timeElapsed += Time.deltaTime;
        float t = timeElapsed / duration;
        Vector3 leftNewPos = Vector3.Lerp(leftStartPos, new Vector3(leftX, leftStartPos.y, leftStartPos.z), t);
        Vector3 rightNewPos = Vector3.Lerp(rightStartPos, new Vector3(rightX, rightStartPos.y, rightStartPos.z), t);
        leftDoor.transform.localPosition = leftNewPos;
        rightDoor.transform.localPosition = rightNewPos;
        yield return null;
    }
}


    public void DisableColliders()
    {
        Debug.Log("DisableColliders called.");
        foreach (GameObject gemstone in gemstones)
        {
            if (gemstone != null)
            {
                Collider collider = gemstone.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = false;
                    Debug.Log($"Disabled collider on gemstone: {gemstone.name}");
                }
                else
                {
                    Debug.LogWarning($"Collider not found on gemstone: {gemstone.name}");
                }
            }
            else
            {
                Debug.LogWarning("Found a null gemstone in the gemstones array.");
            }
        }
        Invoke(nameof(EnableColliders), 3f);
    }

    public void EnableColliders()
    {
        Debug.Log("EnableColliders called.");
        foreach (GameObject gemstone in gemstones)
        {
            if (gemstone != null)
            {
                Collider collider = gemstone.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = true;
                    Debug.Log($"Enabled collider on gemstone: {gemstone.name}");
                }
                else
                {
                    Debug.LogWarning($"Collider not found on gemstone: {gemstone.name}");
                }
            }
            else
            {
                Debug.LogWarning("Found a null gemstone in the gemstones array.");
            }
        }
    }

    public void OnHitByLaserCorrect()
    {
        for (int i = 0; i < gemstones.Length; i++)
        {
            if (gemstones[i] != null)
            {
                SetGlow(gemstones[i], true);
                isHit[i] = true;
                if (fadeCoroutines[i] != null)
                {
                    StopCoroutine(fadeCoroutines[i]);
                    fadeCoroutines[i] = null;
                }
                Collider collider = gemstones[i].GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = false;
                }
            }
        }
    }

    public void OnHitByLaserIncorrect(RaycastHit hit)
    {
        GameObject hitObject = hit.collider.gameObject;
        Renderer renderer = hitObject.GetComponent<Renderer>();

        if (renderer != null)
        {
            int index = System.Array.IndexOf(gemstones, hitObject);
            isHit[index] = false;
            if (fadeCoroutines[index] != null)
            {
                StopCoroutine(fadeCoroutines[index]);
            }
            fadeCoroutines[index] = StartCoroutine(FadeOutEmission(renderer, 3f));
        }
        else
        {
            Debug.LogWarning("Renderer not found on hit object.");
        }
    }

    private IEnumerator FadeOutEmission(Renderer renderer, float duration)
    {
        Material mat = renderer.material;

        if (!mat.IsKeywordEnabled("_EMISSION"))
        {
            mat.EnableKeyword("_EMISSION");
        }

        Color initialEmissionColor = Color.red * 2f;
        mat.SetColor("_EmissionColor", initialEmissionColor);

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            Color emissionColor = Color.Lerp(initialEmissionColor, Color.black, t);
            mat.SetColor("_EmissionColor", emissionColor);
            yield return null;
        }

        mat.SetColor("_EmissionColor", Color.black);
    }

    private void SetGlow(GameObject gemstone, bool glow)
    {
        Renderer renderer = gemstone.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material mat = renderer.material;

            if (glow)
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", Color.red * 2f);
            }
            else
            {
                mat.SetColor("_EmissionColor", Color.black);
            }
        }
    }

    private void SetRuneGlow(int runeID, Color color)
    {
        int index = runeID - 1;
        if (runeFadeCoroutines[index] != null)
        {
            StopCoroutine(runeFadeCoroutines[index]);
        }
        Renderer renderer = runeLetters[index].GetComponent<Renderer>();
        if (renderer != null)
        {
            Material mat = renderer.material;
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", color * 2f);
        }
    }

    private void GlowAllRunes(Color color)
    {
        for (int i = 0; i < runeLetters.Length; i++)
        {
            if (runeLetters[i] != null)
            {
                Renderer renderer = runeLetters[i].GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material mat = renderer.material;
                    mat.EnableKeyword("_EMISSION");
                    mat.SetColor("_EmissionColor", color * 2f);
                }
            }
        }
    }

    private void ResetAllRunes()
    {
        for (int i = 0; i < runeLetters.Length; i++)
        {
            if (runeLetters[i] != null)
            {
                Renderer renderer = runeLetters[i].GetComponent<Renderer>();
                if (renderer != null)
                {
                    if (runeFadeCoroutines[i] != null)
                    {
                        StopCoroutine(runeFadeCoroutines[i]);
                    }
                    runeFadeCoroutines[i] = StartCoroutine(FadeOutEmission(renderer, 3f));
                }
            }
        }
    }

    private void DisableAllRuneColliders()
    {
        foreach (GameObject runeObject in rune)
        {
            if (runeObject != null)
            {
                Collider collider = runeObject.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = false;
                }
            }
        }
    }

    public void UpdateGemstoneHitStatus(GameObject gemstone, bool isBeingHit)
    {
        int index = System.Array.IndexOf(gemstones, gemstone);
        isHit[index] = isBeingHit;
        if (isBeingHit)
        {
            if (fadeCoroutines[index] != null)
            {
                StopCoroutine(fadeCoroutines[index]);
            }
            SetGlow(gemstone, true);
        }
        else
        {
            if (fadeCoroutines[index] != null)
            {
                StopCoroutine(fadeCoroutines[index]);
            }
            fadeCoroutines[index] = StartCoroutine(FadeOutEmission(gemstone.GetComponent<Renderer>(), 3f));
        }
    }
}
