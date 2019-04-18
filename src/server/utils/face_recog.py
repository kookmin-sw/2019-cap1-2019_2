import sys
import dlib
import cv2
import face_recognition
import os
import numpy as np

#동일 인물임을 인식하는 코드입니다.
def im_trim(img, left, top, right, bottom, data_path):
    img_trim = img[top:bottom, left:right]
    cv2.imwrite(data_path + '.jpg', img_trim)

class FaceRecog():
    def __init__(self):
        self.known_face_encodings = [] 
            # knowns폴더에 있는 이미지들의 얼굴을 인식한 정보를 저장하는 배열 
        self.known_face_names = [] 
            # knowns폴더에 있는 파일이름들을 저장하는 배열

        dirname = 'knowns'
        files = os.listdir(dirname)
        for filename in files:
            name, ext = os.path.splitext(filename)
            if ext == '.jpg':
                self.known_face_names.append(name)
                pathname = os.path.join(dirname, filename)
                img = face_recognition.load_image_file(pathname)
                face_encoding = face_recognition.face_encodings(img)[0]
                self.known_face_encodings.append(face_encoding)

    def get_frame(self):
        i_d = 1
        dir_name = 'images'
        files = os.listdir(dir_name)
        for file_name in files:
            face_locations = []
            face_encodings = []
            face_names = []
            name, ext = os.path.splitext(file_name)
            if ext == '.jpg':

                data_path = os.path.join(dir_name, file_name)
                img_ = cv2.imread(data_path)
                
                print(data_path)
                face_locations = face_recognition.face_locations(img_, 0, model = "cnn")
                # 얼굴 인식을 cnn모델을 사용하여 합니다.
                print(len(face_locations))
                face_encodings = face_recognition.face_encodings(img_, face_locations)
        
                for i, face in enumerate(face_encodings):
                    distances = face_recognition.face_distance(self.known_face_encodings, face)
                    min_value = min(distances)
                    
                    name = "unknown"
                    if min_value < 0.6 :
                        #거리가 0.6 이하일 경우만 동일인물로 판단합니다.
                        index = np.argmin(distances)
                    
                        name = self.known_face_names[index]
                        (top, right, bottom, left) = face_locations[i]
                        path = dir_name + "/" + "person_" + str(name) + "/"
                        if not(os.path.isdir(path)):
                            os.mkdir(path)
                        tmp_img = cv2.imread(data_path)

                        im_trim(tmp_img, left, top, right, bottom, path + "face_" + str(i_d))

if __name__ == '__main__':

    detector = dlib.get_frontal_face_detector()
    
    data_path = "/Users/csh/Desktop/capstone/"

    base_img = cv2.imread(data_path + "base.jpg")
    
    dets = detector(base_img, 1)

    for i, d in enumerate(dets):
        im_trim(base_img, d.left(), d.top(), d.right(), d.bottom(), data_path + "knowns/" + str(i+1))
    
    face_recog = FaceRecog()
    frame = face_recog.get_frame()

