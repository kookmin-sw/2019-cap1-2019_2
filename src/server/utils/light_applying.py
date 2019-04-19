import cv2
import numpy as np
from config import *

file_path = "/Users/csh/Desktop/point light/"

def light_applying(src_filename, trg_filename, file_exts):
    src_img = cv2.imread(file_path + src_filename + file_exts)
    trg_img = cv2.imread(file_path + trg_filename + file_exts)

    src_yuv = cv2.cvtColor(src_img, cv2.COLOR_BGR2YUV)

    trg_r, trg_g, trg_b = cv2.split(trg_img)
    src_y, src_u, src_v = cv2.split(src_yuv)

    thresh = cv2.threshold(src_y, 150, 255, cv2.THRESH_BINARY)[1]

    rows, cols = thresh.shape[:2]

    alpha = 30 / 255

    for i in range(4):
        thresh = cv2.GaussianBlur(thresh, (9, 9), 10)
    


    for _y in range(rows):
        for _x in range(cols):
            if(thresh[_y][_x] > 100):
                trg_r[_y][_x] = alpha * thresh[_y][_x] + (1-alpha) * trg_r[_y][_x]
                trg_g[_y][_x] = alpha * thresh[_y][_x] + (1-alpha) * trg_g[_y][_x]
                trg_b[_y][_x] = alpha * thresh[_y][_x] + (1-alpha) * trg_b[_y][_x]

    result = cv2.merge([trg_r, trg_g, trg_b])

    return result

cv2.imshow("aa", light_applying("Point Light 1.jpg_trim", "aa11", ".jpg"))
cv2.waitKey(0)
