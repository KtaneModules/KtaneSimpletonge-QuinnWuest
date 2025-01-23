using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class SimpletongeScript : MonoBehaviour
{
    public KMBombModule Module;
    public KMBombInfo BombInfo;
    public KMAudio Audio;

    public GameObject[] ButtonObjs;
    public GameObject DummyButtonParent;
    public GameObject DummyButton;
    public GameObject PodiumObj;
    public GameObject ModelComponent;
    public Mesh CutOutMeshFilter;

    private KMSelectable[] ButtonSels;
    private TextMesh[] ButtonTexts;

    private int _moduleId;
    private static int _moduleIdCounter = 1;
    private bool _moduleSolved;

    private bool _activated;

    private static readonly string[] _queenPuzzles = new string[] {
        "...#..........#...#............#.#..........#...#............#..",
        "....#....#...........#..#.............#....#...........#..#.....",
        ".....#..#...........#....#.............#..#...........#....#....",
        "..#............#...#..........#.#............#...#..........#...",
        "......#.....#.....#.....#............#.........#.#.........#....",
        ".#.........#.........#.........#..#.....#.............#.....#...",
        "...#.....#.............#.....#..#.........#.........#.........#.",
        "....#.........#.#.........#............#.....#.....#.....#......",
        "....#....#.........#..........#...#............#.....#..#.......",
        "...#..........#.....#....#...........#..#.........#............#",
        "#............#.........#..#...........#....#.....#..........#...",
        ".......#..#.....#............#...#..........#.........#....#....",
        ".......#.#..........#.....#.....#.............#....#.........#..",
        "#.............#....#.........#.........#.#..........#.....#.....",
        ".....#.....#..........#.#.........#.........#....#.............#",
        "..#.........#....#.............#.....#.....#..........#.#.......",
        "...#.....#............#...#..........#.........#....#...#.......",
        "....#.........#..#...........#....#.....#..........#...........#",
        "#...........#..........#.....#....#...........#..#.........#....",
        ".......#...#....#.........#..........#...#............#.....#...",
        ".......#.#.........#....#.............#.....#.....#..........#..",
        "#.............#.....#..........#.#.........#.........#....#.....",
        ".....#....#.........#.........#.#..........#.....#.............#",
        "..#..........#.....#.....#.............#....#.........#.#.......",
        "...#.........#.........#..#.....#.............#.....#....#......",
        "....#.....#.....#............#.........#.#.........#..........#.",
        ".#..........#.........#.#.........#............#.....#.....#....",
        "......#....#.....#.............#.....#..#.........#.........#...",
        "....#..........#...#....#.............#..#...........#....#.....",
        "...#....#...........#..........#.#............#...#..........#..",
        "..#..........#...#............#.#..........#...........#....#...",
        ".....#....#...........#..#.............#....#...#..........#....",
        "..#..........#.........##..........#..........#.....#....#......",
        ".....#....#.....#..............#....#....#.........#..........#.",
        ".#..........#.........#....#....#..............#.....#....#.....",
        "......#....#.....#..........#..........##.........#..........#..",
        "...#...........##...........#.........#..#...........#....#.....",
        "....#...#..............#...#.....#............#...#..........#..",
        "..#..........#...#............#.....#...#..............#...#....",
        ".....#....#...........#..#.........#...........##...........#...",
        "....#.....#............#...#..........#.#............#...#......",
        "...#.........#..#...........#....#.............#..#...........#.",
        ".#...........#..#.............#....#...........#..#.........#...",
        "......#...#............#.#..........#...#............#.....#....",
        ".....#.........#.#.........#....#.............#.....#.....#.....",
        "..#.....#.............#.....#..........#.#.........#.........#..",
        "..#.........#.........#.#..........#.....#.............#.....#..",
        ".....#.....#.....#.............#....#.........#.#.........#.....",
        "....#.........#....#....#.........#............#.....#...#......",
        "...#.....#..........#..........#.....#..#.........#...........#.",
        ".#...........#.........#..#.....#..........#..........#.....#...",
        "......#...#.....#............#.........#....#....#.........#....",
        "...#...........#....#.....#.....#.............#..#...........#..",
        "....#...#..........#.........#.........#.#............#...#.....",
        ".....#...#............#.#.........#.........#..........#...#....",
        "..#...........#..#.............#.....#.....#....#...........#...",
        "...#....#...........#..........#.....#....#...........#..#......",
        "....#..........#...#....#.........#..........#...#............#.",
        ".#............#...#..........#.........#....#...#..........#....",
        "......#..#...........#....#.....#..........#...........#....#...",
        ".#.............#.....#..#.........#.........#.........#....#....",
        "......#.#.........#............#.....#.....#.....#..........#...",
        "...#..........#.....#.....#.....#............#.........#.#......",
        "....#....#.........#.........#.........#..#.....#.............#.",
        "..#..........#.....#....#..............#....#.........#..#......",
        ".....#....#.........#..........##..........#.....#............#.",
        ".#............#.....#..........##..........#.........#....#.....",
        "......#..#.........#....#..............#....#.....#..........#..",
        "...#...........##.........#..........#...#............#.....#...",
        "....#...#..............#.....#....#...........#..#.........#....",
        "....#.........#..#...........#....#.....#..............#...#....",
        "...#.....#............#...#..........#.........##...........#...",
        ".....#...#............#.#..........#...........#....#.....#.....",
        "..#...........#..#.............#....#...#..........#.........#..",
        "..#.........#..........#...#....#.............#..#...........#..",
        ".....#.....#....#...........#..........#.#............#...#.....",
        "...#.....#.............#....#.........#.#.........#..........#..",
        "....#.........#.#..........#.....#.............#.....#....#.....",
        ".....#....#.....#.............#.....#..........#.#.........#....",
        "..#..........#.........#.#.........#....#.............#.....#...",
        "...#..........#.#..............#....#....#...........#....#.....",
        "....#....#.............##..........#..........#...#..........#..",
        "..#..........#...#..........#..........##.............#....#....",
        ".....#....#...........#....#....#..............#.#..........#...",
        "..#..........#.........##...........#.........#..#.........#....",
        ".....#....#.....#..............#...#.....#............#.....#...",
        "...#.....#............#.....#...#..............#.....#....#.....",
        "....#.........#..#.........#...........##.........#..........#..",
        ".....#.....#..........#.#..............#.#..........#.....#.....",
        "..#.........#....#.............##.............#....#.........#..",
        "...#.........#.........#.#............#.#.........#.........#...",
        "....#.....#.....#.............#..#.............#.....#.....#...."
    };

    private const string _victory = "VICTORY!";
    private char[] _grid;
    private int[] _solution;
    private List<int> _input = new List<int>();
    private bool _correctSolutionInputted;

    private void Start()
    {
        _moduleId = _moduleIdCounter++;
        PodiumObj.SetActive(false);
        ButtonSels = new KMSelectable[ButtonObjs.Length];
        ButtonTexts = new TextMesh[ButtonObjs.Length];
        for (int i = 0; i < ButtonObjs.Length; i++)
        {
            ButtonSels[i] = ButtonObjs[i].GetComponent<KMSelectable>();
            ButtonTexts[i] = ButtonObjs[i].GetComponentInChildren<TextMesh>();
            ButtonSels[i].OnInteract += ButtonPress(i);
        }

        tryAgain:
        _grid = Enumerable.Range(0, 64).Select(i => _victory[i % 8]).ToArray().Shuffle();
        var list = new List<int>();
        for (int i = 0; i < _queenPuzzles.Length; i++)
        {
            var ixs = Enumerable.Range(0, 64).Where(x => _queenPuzzles[i][x] == '#').ToArray();
            var arr = ixs.Select(x => _grid[x]).ToArray();
            if (arr.Distinct().Count() == 8)
                list.Add(i);
        }
        if (list.Count != 1)
            goto tryAgain;
        var qp = Enumerable.Range(0, 64).Where(x => _queenPuzzles[list.First()][x] == '#').ToArray();
        var sol = new List<int>();
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < qp.Length; j++)
                if (_grid[qp[j]] == _victory[i])
                    sol.Add(qp[j]);
        _solution = sol.ToArray();

        Debug.LogFormat("[Simpletonge #{0}] Grid: {1}", _moduleId, _grid.Join(""));
        Debug.LogFormat("[Simpletonge #{0}] Press the buttons in this order: {1}.", _moduleId, _solution.Select(i => GetCoord(i)).Join(", "));
    }

    private string GetCoord(int pos)
    {
        return "ABCDEFGH"[pos % 8].ToString() + "12345678"[pos / 8].ToString();
    }

    private KMSelectable.OnInteractHandler ButtonPress(int btn)
    {
        return delegate ()
        {
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.BigButtonPress, ButtonSels[btn].transform);
            ButtonSels[btn].AddInteractionPunch(0.5f);
            if (_moduleSolved || _correctSolutionInputted)
                return false;
            if (!_activated)
            {
                _activated = true;
                SetPuzzle();
                return false;
            }
            _input.Add(btn);
            var valid = IsValidQueenPuzzle(_input, 8);
            if (!valid || _grid[btn] != _victory[_input.Count - 1])
            {
                _input = new List<int>();
                for (int i = 0; i < 64; i++)
                {
                    ButtonObjs[i].GetComponent<MeshRenderer>().material.color = new Color32(0, 0, 255, 255);
                    ButtonTexts[i].color = new Color32(255, 255, 255, 255);
                }
                return false;
            }
            ButtonObjs[btn].GetComponent<MeshRenderer>().material.color = new Color32(255, 255, 255, 255);
            ButtonTexts[btn].color = new Color32(0, 0, 255, 255);
            if (_input.Count == 8)
            {
                _correctSolutionInputted = true;
                StartCoroutine(SolveAnimation());
            }
            return false;
        };
    }

    private void SetPuzzle()
    {
        for (int i = 0; i < ButtonObjs.Length; i++)
        {
            ButtonTexts[i].text = _grid[i].ToString();
            ButtonTexts[i].fontSize = 120;
        }
    }

    private bool IsValidQueenPuzzle(List<int> list, int w)
    {
        var arr = list.ToArray();
        if (arr.Select(i => i % w).Distinct().Count() != arr.Length)
            return false;
        if (arr.Select(i => i / w).Distinct().Count() != arr.Length)
            return false;

        for (int i = 0; i < arr.Length; i++)
        {
            int p = arr[i];
            int oldPos = p;
            while (p % w > 0 && p / w > 0)
            {
                p -= (w + 1);
                if (arr.Contains(p))
                    return false;
            }
            p = oldPos;
            while (p % w < w - 1 && p / w > 0)
            {
                p -= (w - 1);
                if (arr.Contains(p))
                    return false;
            }
            p = oldPos;
            while (p % w > 0 && p / w < (w - 1))
            {
                p += (w - 1);
                if (arr.Contains(p))
                    return false;
            }
            p = oldPos;
            while (p % w < (w - 1) && p / w < (w - 1))
            {
                p += (w + 1);
                if (arr.Contains(p))
                    return false;
            }
        }
        return true;
    }

    private IEnumerator SolveAnimation()
    {
        var btnsToShrink = Enumerable.Range(0, 64).Where(i => !_solution.Contains(i)).ToArray().Shuffle();
        for (int i = 0; i < btnsToShrink.Length; i++)
        {
            StartCoroutine(ShrinkButton(btnsToShrink[i]));
            yield return new WaitForSeconds(0.015f);
        }
        for (int i = 0; i < _solution.Length; i++)
        {
            StartCoroutine(MoveButton(i));
            yield return new WaitForSeconds(0.25f);
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 8; i++)
        {
            Audio.PlaySoundAtTransform("Pop", ButtonObjs[_solution[i]].transform);
            ButtonObjs[_solution[i]].GetComponent<MeshRenderer>().material.color = new Color32(0, 0, 255, 255);
            ButtonTexts[_solution[i]].color = new Color32(255, 255, 255, 255);
            yield return new WaitForSeconds(0.125f);
        }
        yield return new WaitForSeconds(0.5f);
        var duration = 0.5f;
        var elapsed = 0f;
        var btnScale = ButtonObjs[_solution[0]].transform.localScale;
        var btnPoses = Enumerable.Range(0, 8).Select(i => ButtonObjs[_solution[i]].transform.localPosition).ToArray();
        Audio.PlaySoundAtTransform("nyoom", transform);
        while (elapsed < duration)
        {
            for (int i = 0; i < 8; i++)
            {
                ButtonObjs[_solution[i]].transform.localScale = new Vector3(Easing.InOutQuad(elapsed, btnScale.x, 0, duration), Easing.InOutQuad(elapsed, btnScale.y, 0, duration), Easing.InOutQuad(elapsed, btnScale.z, 0, duration));
                ButtonObjs[_solution[i]].transform.localPosition = new Vector3(Easing.InOutQuad(elapsed, btnPoses[i].x, 0, duration), btnPoses[i].y, btnPoses[i].z);
            }
            yield return null;
            elapsed += Time.deltaTime;
        }
        Audio.PlaySoundAtTransform("Victory", transform);
        duration = 0.75f;
        elapsed = 0f;
        while (elapsed < duration)
        {
            DummyButton.transform.localScale = new Vector3(Easing.OutQuad(elapsed, 0, 0.148f, duration), Easing.OutQuad(elapsed, 0, 0.002f, duration), Easing.OutQuad(elapsed, 0, 0.148f, duration));
            yield return null;
            elapsed += Time.deltaTime;
        }
        DummyButton.transform.localScale = new Vector3(0.148f, 0.002f, 0.148f);
        ModelComponent.GetComponent<MeshFilter>().mesh = CutOutMeshFilter;
        Audio.PlaySoundAtTransform("Open", transform);
        duration = 0.5f;
        elapsed = 0f;
        while (elapsed < duration)
        {
            DummyButtonParent.transform.localEulerAngles = new Vector3(0, -45f, Easing.InOutQuad(elapsed, 0, 90, duration));
            yield return null;
            elapsed += Time.deltaTime;
        }
        DummyButtonParent.transform.localEulerAngles = new Vector3(0, -45f, 90);
        PodiumObj.SetActive(true);
        duration = 0.25f;
        elapsed = 0f;
        while (elapsed < duration)
        {
            PodiumObj.transform.localPosition = new Vector3(0, Mathf.Lerp(-0.1f, -0.04f, elapsed / duration), 0);
            yield return null;
            elapsed += Time.deltaTime;
        }
        PodiumObj.transform.localPosition = new Vector3(0, -0.04f, 0);
        _moduleSolved = true;
        Module.HandlePass();
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
        yield break;
    }

    private IEnumerator MoveButton(int i)
    {
        var duration = 1f;
        var elapsed = 0f;
        var btnPos = ButtonObjs[_solution[i]].transform.localPosition;
        while (elapsed < duration)
        {
            ButtonObjs[_solution[i]].transform.localPosition = new Vector3(Easing.InOutQuad(elapsed, btnPos.x, i * 0.02f - 0.07f, duration), btnPos.y, Easing.InOutQuad(elapsed, btnPos.z, 0, duration));
            yield return null;
            elapsed += Time.deltaTime;
        }
        ButtonObjs[_solution[i]].transform.localPosition = new Vector3(i * 0.02f - 0.07f, btnPos.y, 0);
    }

    private IEnumerator ShrinkButton(int i)
    {
        var duration = 0.5f;
        var elapsed = 0f;
        var btnScale = ButtonObjs[i].transform.localScale;
        while (elapsed < duration)
        {
            ButtonObjs[i].transform.localScale = new Vector3(Easing.InOutQuad(elapsed, btnScale.x, 0, duration), Easing.InOutQuad(elapsed, btnScale.y, 0, duration), Easing.InOutQuad(elapsed, btnScale.z, 0, duration));
            yield return null;
            elapsed += Time.deltaTime;
        }
        ButtonObjs[i].transform.localScale = new Vector3(0, 0, 0);
    }

#pragma warning disable 0414
    private readonly string TwitchHelpMessage = "!{0} press A1 B2 C3 [Press square at column A, row 1] | Columns go from A-H left to right. Rows go from 1-8 top to bottom";
#pragma warning restore 0414
    private IEnumerator ProcessTwitchCommand(string command)
    {
        if (_moduleSolved || _correctSolutionInputted)
        {
            yield return "sendtochaterror You cannot interact with the module now!";
            yield break;
        }
        command = command.Trim().ToUpperInvariant();
        var m = Regex.Match(command, @"^\s*swap(?<coords>(\s+[ABCDEFGH][12345678])+)\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        if (!m.Success)
            yield break;
        var coords = Regex.Split(m.Groups["coords"].Value.Trim(), @"\s+");
        if (coords.Length % 2 != 0)
        {
            yield return "sendtochaterror Odd number of coords found in command. Command ignored";
            yield break;
        }
        var list = new List<int>();
        for (int i = 0; i < coords.Length; i++)
            list.Add((coords[i][1] - '1') * 8 + (coords[i][0] - 'A'));
        yield return null;
        yield return "solve";
        for (int i = 0; i < list.Count; i++)
        {
            ButtonSels[list[i]].OnInteract();
            yield return new WaitForSeconds(0.2f);
            if (_correctSolutionInputted)
                yield break;
        }
    }

    private IEnumerator TwitchHandleForcedSolve()
    {
        if (!_activated)
        {
            ButtonSels[0].OnInteract();
            yield return new WaitForSeconds(0.2f);
        }
        if (_input.Count == 0)
            goto okay;
        var soFar = _solution.Take(_input.Count).ToArray();
        if (_input.SequenceEqual(soFar))
        {
            ButtonSels[0].OnInteract();
            yield return new WaitForSeconds(0.2f);
        }
        okay:;
        for (int i = _input.Count; i < 8; i++)
        {
            ButtonSels[_solution[i]].OnInteract();
            yield return new WaitForSeconds(0.2f);
        }
        while (!_moduleSolved)
            yield return true;
    }
}
