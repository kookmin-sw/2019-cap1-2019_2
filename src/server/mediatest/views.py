from django.shortcuts import render
from django.views.decorators.csrf import csrf_exempt
from django.core.files.storage import FileSystemStorage, default_storage
from django.core.files.base import ContentFile
import json
import os

# Create your views here.
from django.http import HttpResponse

@csrf_exempt
def upload(request):
    if request.method == 'POST':
        myfile = request.FILES['file']
        print(myfile.name)
        path = default_storage.save(myfile.name, ContentFile(myfile.read()))
        
        
        #fs = FileSystemStorage()
        
        #filename = fs.save(myfile.name, myfile, encoding="UTF8")
        #uploaded_file_url = fs.url(filename)

    

    ret = {'success': "true", 'message' : "server-hello"}
    return HttpResponse(json.dumps(ret), "application/json")
