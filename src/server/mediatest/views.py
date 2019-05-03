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
        imgfile = request.FILES['data_img']
        path = default_storage.save(imgfile.name, ContentFile(imgfile.read()))
        print(imgfile.name + " saved OK")
        
        txtfile1 = request.FILES['data_tt']
        path = default_storage.save(txtfile1.name, ContentFile(txtfile1.read()))
        print(txtfile1.name + " saved OK")

        txtfile2 = request.FILES['data_tu']
        path = default_storage.save(txtfile2.name, ContentFile(txtfile2.read()))
        print(txtfile2.name + " saved OK")

        txtfile3 = request.FILES['data_tv']
        path = default_storage.save(txtfile3.name, ContentFile(txtfile3.read()))
        print(txtfile3.name + " saved OK")
        
        txtfile4 = request.FILES['data_mt']
        path = default_storage.save(txtfile4.name, ContentFile(txtfile4.read()))
        print(txtfile4.name + " saved OK")

        txtfile5 = request.FILES['data_mu']
        path = default_storage.save(txtfile5.name, ContentFile(txtfile5.read()))
        print(txtfile5.name + " saved OK")

        txtfile6 = request.FILES['data_mv']
        path = default_storage.save(txtfile6.name, ContentFile(txtfile6.read()))
        print(txtfile6.name + " saved OK")
    

    base_img = open("./pbk1.png", "rb").read()
    
    return HttpResponse(base_img, content_type="image/png")
