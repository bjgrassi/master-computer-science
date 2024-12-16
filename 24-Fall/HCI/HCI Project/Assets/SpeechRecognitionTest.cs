using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HuggingFace.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class SpeechRecognitionTest : MonoBehaviour {
    // In the script, define references to your UI components:
    [SerializeField] private Button startButton; // This will start the recording.
    [SerializeField] private Button stopButton; // This will stop the recording.
    [SerializeField] private TextMeshProUGUI text; // This is where the result of the speech recognition will be displayed.

    // Now let's record Microphone input and encode it in WAV format. Start by defining the member variables:
    private AudioClip clip;
    private byte[] bytes;
    private bool recording;
    // Declare the list of the hands to reference on Unity
    public List<GameObject> hands;

    private void Start() {
        // Then, use the Start() method to set up listeners for the start and stop buttons:
        startButton.onClick.AddListener(StartRecording);
        stopButton.onClick.AddListener(StopRecording);
        stopButton.interactable = false;
    }

    private void Update() {
        // In case the recording reaches its maximum length of 10 seconds, we'll want to stop the recording automatically.
        if (recording && Microphone.GetPosition(null) >= clip.samples) {
            StopRecording();
        }
    }

    private void StartRecording() {
        text.color = Color.white;
        text.text = "Recording..."; // Changes the text to inform the user that the audio is being translated.
        startButton.interactable = false;
        stopButton.interactable = true;
        clip = Microphone.Start(null, false, 10, 44100); // This will record up to 10 seconds of audio at 44100 Hz.
        recording = true;
    }

    // Then, in StopRecording(), truncate the recording and encode it in WAV format:
    private void StopRecording() {
        var position = Microphone.GetPosition(null);
        Microphone.End(null);
        var samples = new float[position * clip.channels];
        clip.GetData(samples, 0);
        bytes = EncodeAsWAV(samples, clip.frequency, clip.channels);
        recording = false;
        SendRecording();
    }

    // Finally, we'll need to implement the EncodeAsWAV() method, to prepare the audio data for the Hugging Face API:
    private byte[] EncodeAsWAV(float[] samples, int frequency, int channels) {
        using (var memoryStream = new MemoryStream(44 + samples.Length * 2)) {
            using (var writer = new BinaryWriter(memoryStream)) {
                writer.Write("RIFF".ToCharArray());
                writer.Write(36 + samples.Length * 2);
                writer.Write("WAVE".ToCharArray());
                writer.Write("fmt ".ToCharArray());
                writer.Write(16);
                writer.Write((ushort)1);
                writer.Write((ushort)channels);
                writer.Write(frequency);
                writer.Write(frequency * channels * 2);
                writer.Write((ushort)(channels * 2));
                writer.Write((ushort)16);
                writer.Write("data".ToCharArray());
                writer.Write(samples.Length * 2);

                foreach (var sample in samples) {
                    writer.Write((short)(sample * short.MaxValue));
                }
            }
            return memoryStream.ToArray();
        }
    }

    private void SendRecording() {
        text.color = Color.yellow;
        text.text = "Sending...";
        stopButton.interactable = false;
        HuggingFaceAPI.AutomaticSpeechRecognition(bytes, response => {
            text.color = Color.white;
            text.text = response;
            startButton.interactable = true;
            // Make the Search for the correct hand model
            SearchHandModel(response);
        }, error => {
            text.color = Color.red;
            text.text = error;
            startButton.interactable = true;
        });
    }

    void SearchHandModel(string response)
    {
        response = response.Trim().ToUpper().Replace(".", "").Replace(",", ""); // We need to eliminate any undesired character.

        if (!int.TryParse(response, out var number))
        {
            switch (response)
            {
                case "ONE":
                    number = 1;
                    break;
                case "TWO":
                    number = 2;
                    break;
                case "THREE":
                    number = 3;
                    break;
                case "FOUR":
                    number = 4;
                    break;
                case "FIVE":
                    number = 5;
                    break;
                case "SIX":
                    number = 6;
                    break;
                case "SEVEN":
                    number = 7;
                    break;
                case "EIGHT":
                    number = 8;
                    break;
                case "NINE":
                    number = 9;
                    break;
                case "ZERO":
                    number = 10;
                    break;
                default:
                    number = 0;
                    break;
            }
        }
        
        ShowHandModel(number);
    }
    void ShowHandModel(int number)
    {
        if(number > 0)
        {
            hideAllHands();

            Debug.Log("showing hand number: " + number);

            var arrayNum = number - 1;
            hands[arrayNum].SetActive(true);
        }
    }

    void hideAllHands()
    {
        foreach (var hand in hands)
        {
            hand.SetActive(false);
        }
    }
}
