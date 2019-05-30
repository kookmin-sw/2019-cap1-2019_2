from django.shortcuts import render
from django.views.decorators.csrf import csrf_exempt
from django.core.files.storage import FileSystemStorage, default_storage
from django.core.files.base import ContentFile
import json
import os
import time

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
        targetImage           = request.FILES['targetImage']
        targetMeshHeadPose    = request.FILES['targetMeshHeadPose']
        targetTextureHeadPose = request.FILES['targetTextureHeadPose']
        targetTextureVertices = request.FILES['targetTextureVertices']
        targetLight           = request.FILES['targetLight']

        sourceImage           = request.FILES['sourceImage']
        sourceMeshHeadPose    = request.FILES['sourceMeshHeadPose']
        sourceMeshVertices    = request.FILES['sourceMeshVertices']
        sourceTextureVertices = request.FILES['sourceTextureVertices']

        remove_duplicate_files('{}/data/image/{}'.format(WORKING_PATH, targetImage.name))
        default_storage.save('{}/data/image/{}'.format(WORKING_PATH, targetImage.name), ContentFile(targetImage.read()))
        remove_duplicate_files('{}/data/mesh/{}'.format(WORKING_PATH, targetMeshHeadPose.name))
        default_storage.save('{}/data/mesh/{}'.format(WORKING_PATH, targetMeshHeadPose.name), ContentFile(targetMeshHeadPose.read()))
        remove_duplicate_files('{}/data/texture/{}'.format(WORKING_PATH, targetTextureHeadPose.name))
        default_storage.save('{}/data/texture/{}'.format(WORKING_PATH, targetTextureHeadPose.name), ContentFile(targetTextureHeadPose.read()))
        remove_duplicate_files('{}/data/texture/{}'.format(WORKING_PATH, targetTextureVertices.name))
        default_storage.save('{}/data/texture/{}'.format(WORKING_PATH, targetTextureVertices.name), ContentFile(targetTextureVertices.read()))
        remove_duplicate_files('{}/data/mesh/{}'.format(WORKING_PATH, targetLight.name))
        default_storage.save('{}/data/mesh/{}'.format(WORKING_PATH, targetLight.name), ContentFile(targetLight.read()))

        remove_duplicate_files('{}/data/image/{}'.format(WORKING_PATH, sourceImage.name))
        default_storage.save('{}/data/image/{}'.format(WORKING_PATH, sourceImage.name), ContentFile(sourceImage.read()))
        remove_duplicate_files('{}/data/mesh/{}'.format(WORKING_PATH, sourceMeshHeadPose.name))
        default_storage.save('{}/data/mesh/{}'.format(WORKING_PATH, sourceMeshHeadPose.name), ContentFile(sourceMeshHeadPose.read()))
        remove_duplicate_files('{}/data/mesh/{}'.format(WORKING_PATH, sourceMeshVertices.name))
        default_storage.save('{}/data/mesh/{}'.format(WORKING_PATH, sourceMeshVertices.name), ContentFile(sourceMeshVertices.read()))
        remove_duplicate_files('{}/data/texture/{}'.format(WORKING_PATH, sourceTextureVertices.name))
        default_storage.save('{}/data/texture/{}'.format(WORKING_PATH, sourceTextureVertices.name), ContentFile(sourceTextureVertices.read()))
        
        print('Receiving data for synthesis is completed.')
        
        sourceName = os.path.splitext(sourceTextureVertices.name)[0]
        sourceName = sourceName[:-9]
        targetName = os.path.splitext(targetTextureVertices.name)[0]
        targetName = targetName[:-9]

        t1 = time.time()
        print('Light Removing...')
        os.system("python -B {}/utils/light_remover.py {}".format(WORKING_PATH, sourceName))
        print('Light Removing is done.')
        t2 = time.time()
        print('time: ', t2- t1)

        print('Unwrapping...')
        os.system("python -B {}/utils/unwrapper.py {}".format(WORKING_PATH, sourceName)) # 텍스처 이미지생성
        print('Unwrapping is done.')
        t3 = time.time()
        print('time: ', t3- t2)

        print('Modeling...')
        os.system("python -B {}/utils/modeler.py {} {}".format(WORKING_PATH, targetName, sourceName))
        print('Modeling is done.')
        t4 = time.time()
        print('time: ', t4- t3)

        print('Synthesis..')
        os.system("python -B {}/utils/synthesizer.py {} {}".format(WORKING_PATH, targetName, sourceName))
        print('Synthesis is done.')
        t5 = time.time()
        print('time: ', t5- t4)
        print('Total Time: ', t5 - t1)
        
        with open("{}/data/image/{}+{}.png".format(WORKING_PATH, targetName, sourceName), "rb") as f:
            return HttpResponse(f.read(), content_type="image/png")

    return HttpResponse("ERROR")
