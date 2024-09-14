import pyttsx3
import base64
from pydub import AudioSegment
import os
from fastapi import FastAPI, Body
from pydantic import BaseModel

# Initialize the pyttsx3 engine
engine = pyttsx3.init()

app = FastAPI()

# Function to convert text to speech and save as an audio file
def text_to_speech(text, filename="output.mp3"):
    engine.setProperty('rate', 150)
    engine.setProperty('volume', 1)
    engine.save_to_file(text, filename)
    engine.runAndWait()

# Function to convert an audio file to Base64
def audio_to_base64(filename):
    with open(filename, "rb") as audio_file:
        encoded_string = base64.b64encode(audio_file.read())
        return encoded_string.decode('utf-8')

# Request model for incoming text data
class TextRequest(BaseModel):
    text: str

# Route to get Base64 encoded audio from text
@app.post("/getAudio64")
async def get_audio_base64(request: TextRequest):
    text = request.text
    filename = "output.mp3"
    
    # Convert the text to speech and save as an MP3 file
    text_to_speech(text, filename)
    
    # Convert the MP3 file to Base64
    base64_audio = audio_to_base64(filename)
    
    # Optionally, remove the file after conversion
    os.remove(filename)
    
    # Return the Base64 encoded audio as a JSON response
    return {"base64_audio": base64_audio}

# Start the server on file run
if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000)
