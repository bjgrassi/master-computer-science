# [How to Convert Speech to Text in Python](https://www.thepythoncode.com/article/using-Assignment2-Part2-Braille-to-convert-speech-to-text-python)
To run this:
- `pip3 install SpeechRecognition pydub` (windows)
- `pip3 install -r requirements.txt`
Source 1: https://thepythoncode.com/article/using-speech-recognition-to-convert-speech-to-text-python 
Source 2: https://github.com/x4nth055/pythoncode-tutorials/tree/master/machine-learning/speech-recognition 
- Change the path 'C:/Projetos/Assignment2-Part2-Braille/...' in the files to yours.

## Step 1: Speech to Text Conversion
Import the library: `pip3 install pyaudio` (windows)
Source: https://pypi.org/project/PyAudio/ 
- To recognize the text from your microphone after talking 5 seconds:
    ```
    python speech_to_text.py 5
    ```
    This will record your talking in 5 seconds and then uploads the audio data to Google to get the desired output.

## Step 2: Text to Braille
`python text_to_braille.py Speech-to-text.txt`

## Step 3: Speech to Braille
`python speech_to_braille.py 5`

## Extra Step: Audio to Text
- To recognize the text of an audio file named `16-122828-0002.wav`:
    ```
    python audio_to_text.py 16-122828-0002.wav
    ```
    **Output**:
    ```
    I believe you're just talking nonsense
    ```
