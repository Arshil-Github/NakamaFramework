const { GoogleGenerativeAI } = require("@google/generative-ai");

const express = require("express")
const bodyParser = require("body-parser")
const {
  personalitiesPrompts,
  promptSuffix
} = require("./serverData.js")

const fs = require('fs');

const app = express();
app.use(bodyParser.json())

const apiKey = "AIzaSyDXzkmPdyywQGJfIpgx5AzlIYFxPLJxUXs";
const genAI = new GoogleGenerativeAI(apiKey);


const model = genAI.getGenerativeModel({
  model: "gemini-1.5-flash",
});

const generationConfig = {
  temperature: 1.5,
  topP: 0.95,
  topK: 64,
  maxOutputTokens: 8192,
  responseMimeType: "application/json",
};

let imageParts = []
let locationArray = ["OrbRoom", "PlantRoom", "Path", "BackRoom"]
locationArray.forEach(element => {
  const imagePath = `./images/${element}.png`;
  const imageMimeType = "image/png";
  const imagePart = fileToGenerativePart(imagePath, imageMimeType);

  imageParts.push(imagePart);
});

let chatSession = model.startChat({
  generationConfig,
    history: [{
      role: 'user',
      parts: [
        imageParts[0],
        imageParts[1],
        imageParts[2],
        imageParts[3],
        { 
        text: personalitiesPrompts.albert_einstein + promptSuffix
        }
    ],
    }
  ],
});



function fileToGenerativePart(path, mimeType) {
  return {
    inlineData: {
      data: Buffer.from(fs.readFileSync(path)).toString("base64"),
      mimeType,
    },
  };
}

app.post('/defineCharacter', async (req, res)=>{

  const character = req.body.character;
  const prompt = `You are ${character}. Act like they would`

  let message = await chatSession.sendMessage(`This is a personaloty change message. Dont follow the normal rules and dont return anything with this. From now on you are to abondon your previous personality. You are now ${character}.`)

  res.status(200).send(message);

})
app.post("/getResponse", async (req, res) =>{

  let userResponse = req.body.userMessage;
  let result = await chatSession.sendMessage(userResponse);
  
  //Check if text() fits in the json 

  try{
    let resultJSON = await JSON.parse(result.response.text());
    
    console.log(userResponse);
  
    //res.status(200).send(userResponse)
    res.status(200).send(resultJSON)
  }
  catch(e)
  {
    res.status(200).send({
      "response" : "I am not in mood to answer that.",
      "options": ["Okay", "Cool", "Next", "You are stupid"],
      "animationName": "idle"
    })
  }
})


// Start the server
const port = 3000;
app.listen(port, () => {
  console.log(`Server is running on http://localhost:${port}`);
});