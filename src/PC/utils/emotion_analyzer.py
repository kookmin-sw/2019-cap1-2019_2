import sys
import requests
from config import *

def LoadData(filePath):
    # subscription_key = '65b094c8b7084d0b9e072125bfea8ed1'
    subscription_key = 'e958e28cf18a40d7978ede06a69dcdda'
    assert subscription_key

    face_api_url = 'https://westcentralus.api.cognitive.microsoft.com/face/v1.0/detect'

    data = open(filePath, 'rb').read()  

    headers = { 
        'Content-Type': 'application/octet-stream',   # this should be the content type
        'Ocp-Apim-Subscription-Key': subscription_key 
    }
        
    params = {
        'returnFaceId': 'true',
        'returnFaceLandmarks': 'false',
        'returnFaceAttributes': 'emotion',
    }

    response = requests.post(face_api_url, params=params, headers=headers, data = data)
    faces = response.json()
    return faces

def EmotionAnalysis(fileName, emotion='happiness'):
    faces = LoadData("{}/data/image/{}.png".format(WORKING_PATH, fileName))
    face = faces[0]
    emotions = face['faceAttributes'].get('emotion')
    f = open("{}/data/emotion/{}_{}.txt".format(WORKING_PATH, fileName, emotion), "w")
    f.write(str(emotions[emotion]))
    f.close()


if __name__ == '__main__':
    EmotionAnalysis(sys.argv[1]) # params: fileName