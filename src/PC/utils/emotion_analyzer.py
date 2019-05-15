import sys
import requests
from config import *

def load_data(filePath):
    # subscription_key = '65b094c8b7084d0b9e072125bfea8ed1'
    # subscription_key = 'e958e28cf18a40d7978ede06a69dcdda'
    # subscription_key = 'fe5915d691a941a9858b5d23c8443e81'
    # subscription_key = '355730b2110541b490de53e86ca052a5'
    subscription_key = '32361e22cf5a431289578e359ca410b1'

    assert subscription_key

    # face_api_url = 'https://westcentralus.api.cognitive.microsoft.com/face/v1.0/detect'
    face_api_url = 'https://koreacentral.api.cognitive.microsoft.com/face/v1.0/detect'

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

def emotion_analysis(fileName, emotion='happiness'):
    faces = load_data("{}/data/image/{}.png".format(WORKING_PATH, fileName))
    face = faces[0]
    emotions = face['faceAttributes'].get('emotion')
    f = open("{}/data/emotion/{}_{}.txt".format(WORKING_PATH, fileName, emotion), "w")
    f.write(str(emotions[emotion]))
    f.close()


if __name__ == '__main__':
    emotion_analysis(sys.argv[1]) # params: fileName