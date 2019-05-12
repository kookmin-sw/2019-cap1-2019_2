from django.shortcuts import render
from django.views.decorators.csrf import csrf_exempt
from django.core.files.storage import FileSystemStorage, default_storage
from django.core.files.base import ContentFile
import json
import os

# Create your views here.
from django.http import HttpResponse

WORKING_PATH = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))

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
def upload(request):
    if request.method == 'POST':
        TargetMesh = request.FILES['TargetMesh']
        TargetTexture = request.FILES['TargetTexture']
        TargetHeadPose = request.FILES['TargetHeadPose']
        SourceMesh = request.FILES['SourceMesh']
        SourceTexture = request.FILES['SourceTexture']

        default_storage.save('{}/data/mesh/{}'.format(WORKING_PATH, TargetMesh.name), ContentFile(TargetMesh.read()))
        default_storage.save('{}/data/mesh/{}'.format(WORKING_PATH, TargetHeadPose.name), ContentFile(TargetHeadPose.read()))
        default_storage.save('{}/data/texture/{}'.format(WORKING_PATH, TargetTexture.name), ContentFile(TargetTexture.read()))
        
        default_storage.save('{}/data/mesh/{}'.format(WORKING_PATH, SourceMesh.name), ContentFile(SourceMesh.read()))
        # default_storage.save('{}/data/mesh/{}'.format(WORKING_PATH, SourceHeadPose.name), ContentFile(SourceMesh.read()))
        default_storage.save('{}/data/texture/{}'.format(WORKING_PATH, SourceTexture.name), ContentFile(SourceTexture.read()))

        print(TargetMesh.name, SourceMesh.name)

        #os.system() # 텍스처 이미지생성
        #os.system() # 3d model
        #os.system() # 합성

    result = open("./black.png", "rb").read()
    return HttpResponse(result, content_type = "image/png")
