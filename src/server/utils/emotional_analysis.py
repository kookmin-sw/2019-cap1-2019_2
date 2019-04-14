import numpy as np
import pandas as pd
import requests

def LoadData():
	subscription_key = 'subscription_key'
	assert subscription_key

	face_api_url = 'https://westcentralus.api.cognitive.microsoft.com/face/v1.0/detect'

	data = open('../images/sample2.jpg', 'rb').read()  

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


def EmotionalAnalysis():
	faces = LoadData()
	faces_str = str(faces)
	data = faces_str.split('}}}, ')

	idx = ['anger', 'contempt', 'disgust', 'fear', 'happiness', 'neutral', 'sadness', 'surprise']
	emotion_df = pd.DataFrame(index = idx)

	for i in range(len(data)):
	    faceInfo = str(data[i])
	    search = 'anger'

	    result = faceInfo.find(search)

	    emotion = faceInfo[result-1:len(faceInfo)]
	    emotion = emotion.replace("\'", "")
	    emotion = emotion.replace("}", "")
	    emotion = emotion.replace("]", "")
	    emotion = emotion.replace("{", "")
	    emotion = emotion.replace("[", "")
	    emotion_li = emotion.split(', ')
	    #print(emotion_li)
	    
	    prob = []
	    for j in range(len(emotion_li)):
	        row = str(emotion_li[j])
	        arr = row.split(': ')
	        prob.append(arr[1])
	        
	    emotion_df[i] = prob
	    
	print(emotion_df)