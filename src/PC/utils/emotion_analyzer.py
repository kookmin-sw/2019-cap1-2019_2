import sys
import requests
from config import *


Keys = ['fe5915d691a941a9858b5d23c8443e81', '355730b2110541b490de53e86ca052a5', '32361e22cf5a431289578e359ca410b1', 'd3b1e9aef75843c2b2f33b003d9a9fda']

def load_data(filePath):
    face_api_url = 'https://koreacentral.api.cognitive.microsoft.com/face/v1.0/detect'
    data = open(filePath, 'rb').read()  

    for key in Keys:
        headers = { 
        'Content-Type': 'application/octet-stream',   # this should be the content type
        'Ocp-Apim-Subscription-Key': key
        }
    
        params = {
        'returnFaceId': 'true',
        'returnFaceLandmarks': 'false',
        'returnFaceAttributes': 'emotion',
        }    
    

        response = requests.post(face_api_url, params=params, headers=headers, data = data)

        if(response.status_code == requests.codes.ok):
            faces = response.json()
            return faces

    return false

def emotion_analysis(fileName, emotion='happiness'):
    faces = load_data("{}/data/image/{}.png".format(WORKING_PATH, fileName))
    f = open("{}/data/emotion/{}_{}.txt".format(WORKING_PATH, fileName, emotion), "w")
    try:
        face = faces[0]
        emotions = face['faceAttributes'].get('emotion')
        f.write(str(emotions[emotion]))
        f.close()    
    except TypeError:
        f.write('0.0')
        f.close()

    


if __name__ == '__main__':
    emotion_analysis(sys.argv[1]) # params: fileName