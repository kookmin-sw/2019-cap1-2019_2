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
