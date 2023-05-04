using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Checks
{
    public class Observer : MonoBehaviour, IObserveable
    {
        [SerializeField] private PhysicsRaycaster raycaster;
        [SerializeField] private float delayBetweenRepeat;
        [SerializeField] private string fileName;
        [SerializeField] private bool needSerialize;
        [SerializeField] private bool needDeserialize;

        private ISerializable handler;
        private List<string> outputList;

        public event Action<(int, int)> NextStepReady;
        public event Action<(int, int)> NextStepReadyClick;

        private void Awake()
        {
            handler = GetComponent<ISerializable>();
        }

        private void OnEnable()
        {
            handler.StepFinished += OnStepFinished;
        }
        private void OnDisable()
        {
            handler.StepFinished -= OnStepFinished;
        }

        private void Start()
        {
            if (!needDeserialize && needSerialize)
            {
                File.Delete(fileName + ".txt");
            }

            if (needDeserialize && !needSerialize)
            {
                raycaster.enabled = false;
                var output = Deserialize();
                outputList = output.Split(Environment.NewLine).ToList();
                OnStepFinished();
            }
        }

        public async Task Serialize(string input)
        {
            if (!needSerialize && needDeserialize)
            {
                return;
            }

            await using var fileStream = new FileStream(fileName + ".txt", FileMode.Append);
            await using var streamWriter = new StreamWriter(fileStream);

            await streamWriter.WriteLineAsync(input);
        }

        private string Deserialize()
        {
            if (!File.Exists(fileName + ".txt"))
            {
                return null;
            }

            using var fileStream = new FileStream(fileName + ".txt", FileMode.Open);
            using var streamReader = new StreamReader(fileStream);

            var builder = new StringBuilder();

            while (!streamReader.EndOfStream)
            {
                builder.AppendLine(streamReader.ReadLine());

            }

            return builder.ToString();

        }

        private void OnStepFinished()
        {
            if (!needDeserialize && needSerialize)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(outputList[0]))
            {
                needSerialize = true;
                needDeserialize = false;

                Debug.Log("Repeat Game finish");
                raycaster.enabled = true;
                return;
            }

            StartCoroutine(RepeatGame(outputList));

        }

        private IEnumerator RepeatGame(List<string> input)
        {
            foreach(string line in input)
            {
                const string playerCommandPattern = @"Player (\d+) (Move|Click|Remove)";
                const string coordinatePattern = @"(\d+), (\d+)";
                (int, int) destinationPosition = default;
                yield return new WaitForSeconds(delayBetweenRepeat);
                Debug.Log(input);
                var playerCommandMatch = Regex.Match(line, playerCommandPattern);
                var playerIndex = int.Parse(playerCommandMatch.Groups[1].Value);

                var command = playerCommandMatch.Groups[2].Value;

                var coordinateMatches = Regex.Matches(line, coordinatePattern);
                var originPosition = (
                    int.Parse(coordinateMatches[0].Groups[1].Value),
                    int.Parse(coordinateMatches[0].Groups[2].Value)).ToCoordinate();

                if (command == "Move")
                {
                    destinationPosition = (
                        int.Parse(coordinateMatches[1].Groups[1].Value),
                        int.Parse(coordinateMatches[1].Groups[2].Value)).ToCoordinate();
                }

                switch (command)
                {
                    case "Click":
                        Debug.Log($"Player {playerIndex} {command} to {originPosition}");
                        NextStepReadyClick?.Invoke(originPosition);
                        break;
                    case "Move":
                        Debug.Log($"Player {playerIndex} {command} from {originPosition} to {destinationPosition}");
                        NextStepReady?.Invoke(destinationPosition);
                        break;
                    case "Remove":
                        Debug.Log($"Player {playerIndex} {command} chip at {originPosition}");
                        NextStepReady?.Invoke(new(-1, -1));
                        break;
                    default:
                        throw new NullReferenceException("Action is null");
                }

                //Debug.Log(line);
                //Debug.Log(input.Last());

                if (line == input[input.Count - 2])
                {
                    needSerialize = true;
                    needDeserialize = false;

                    Debug.Log("Repeat Game finish");
                    raycaster.enabled = true;

                    StopAllCoroutines();
                }

            }
        }
    }
}