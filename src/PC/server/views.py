from django.shortcuts import render
from django.views.decorators.csrf import csrf_exempt
from django.core.files.storage import FileSystemStorage, default_storage
from django.core.files.base import ContentFile
import json
import os

# Create your views here.
from django.http import HttpResponse

WORKING_PATH = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))

def remove_duplicate_files(filePath):
    if os.path.isfile(filePath):
        os.remove(filePath)

@csrf_exempt
def happiness(request):
    if request.method == 'POST':
        file = request.FILES['data']
        default_storage.save('{}/data/image/{}'.format(WORKING_PATH, file.name), ContentFile(file.read()))
        print(file.name + " was saved");
        fileName = os.path.splitext(file.name)[0]
        os.system("python -B {}/utils/emotion_analyzer.py {}".format(WORKING_PATH, fileName))
        print("Transfer happiness to FaceCody")

    report = open("{}/data/emotion/{}_happiness.txt".format(WORKING_PATH, fileName), "rb").read()
    return HttpResponse(report, content_type="text/txt")


@csrf_exempt
def synthesis(request):
    if request.method == 'POST':
        targetTextureVertices = request.FILES['targetTextureVertices']
        targetHeadPose        = request.FILES['targetHeadPose']

        sourceTextureVertices = request.FILES['sourceTextureVertices']
        sourceHeadPose        = request.FILES['sourceHeadPose']
        sourceMeshVertices    = request.FILES['sourceMeshVertices']
      
        remove_duplicate_files('{}/data/texture/{}'.format(WORKING_PATH, targetTextureVertices.name))
        default_storage.save('{}/data/texture/{}'.format(WORKING_PATH, targetTextureVertices.name), ContentFile(targetTextureVertices.read()))
        remove_duplicate_files('{}/data/mesh/{}'.format(WORKING_PATH, targetHeadPose.name))
        default_storage.save('{}/data/mesh/{}'.format(WORKING_PATH, targetHeadPose.name), ContentFile(targetHeadPose.read()))

        remove_duplicate_files('{}/data/texture/{}'.format(WORKING_PATH, sourceTextureVertices.name))
        default_storage.save('{}/data/texture/{}'.format(WORKING_PATH, sourceTextureVertices.name), ContentFile(sourceTextureVertices.read()))
        remove_duplicate_files('{}/data/mesh/{}'.format(WORKING_PATH, sourceHeadPose.name))
        default_storage.save('{}/data/mesh/{}'.format(WORKING_PATH, sourceHeadPose.name), ContentFile(sourceHeadPose.read()))
        remove_duplicate_files('{}/data/mesh/{}'.format(WORKING_PATH, sourceMeshVertices.name))
        default_storage.save('{}/data/mesh/{}'.format(WORKING_PATH, sourceMeshVertices.name), ContentFile(sourceMeshVertices.read()))
        
        print('Receiving data for synthesis is completed.')
        
        sourceName = os.path.splitext(sourceTextureVertices.name)[0]
        sourceName = sourceName[:-9]
        targetName = os.path.splitext(targetTextureVertices.name)[0]
        targetName = targetName[:-9]

        os.system("python -B {}/utils/unwrapper.py {}".format(WORKING_PATH, sourceName)) # 텍스처 이미지생성
        print('Unwrapping is done.')
        #os.system() # 3d model
        #os.system() # 합성

    return HttpResponse("OK")
