import cv2
import numpy as np
import sys
from config import *

RESIZE_RATIO = 2.5

def get_resized_image(image, ratio):
    return cv2.resize(image, dsize=(0, 0), fx=ratio, fy=ratio, interpolation=cv2.INTER_LINEAR)

def get_light_removed_image(image, sigma=10, gamma_1=0.9, gamma_2=1.5):
    imageYUV = cv2.cvtColor(image, cv2.COLOR_BGR2YUV)
    imageY, imageU, imageV = cv2.split(imageYUV)
    
    width, height = imageY.shape[:2]

    imageLog = np.log1p(np.array(imageY, dtype='float') / 255)
    # seperate image illumination element with reflectance element
    M = 2*width + 1
    N = 2*height + 1

    (X, Y) = np.meshgrid(np.linspace(0, N-1, N), np.linspace(0, M-1, M))
    Xc = np.ceil(N/2)
    Yc = np.ceil(M/2)
    gaussiannumerator = (X - Xc) ** 2 + (Y - Yc) ** 2

    lpf = np.exp(-gaussiannumerator / (2*sigma*sigma))
    hpf = 1-lpf
    
    lpfShift = np.fft.ifftshift(lpf)
    hpfShift = np.fft.ifftshift(hpf)
    
    imageFFT = np.fft.fft2(imageLog, (M, N))
    imageLF = np.real(np.fft.ifft2(imageFFT * lpfShift, (M, N)))
    imageHF = np.real(np.fft.ifft2(imageFFT * hpfShift, (M, N)))

    iamgeAdjust = gamma_1 * imageLF[:width, :height] + gamma_2 * imageHF[:width, :height]
    # Each lf and hf component is multiplied by a scaling factor(gamma) to
    # control the illumination and reflection values
    imageEXP = np.expm1(iamgeAdjust)
    imageEXP = (imageEXP - np.min(imageEXP)) / (np.max(imageEXP) - np.min(imageEXP))
    imageY = np.array(255 * imageEXP, dtype = 'uint8')

    imageYUV = cv2.merge([imageY, imageU, imageV])
    image = cv2.cvtColor(imageYUV, cv2.COLOR_YUV2BGR)
    return image
    

if __name__ == '__main__':
    fileName = sys.argv[1]
    image = cv2.imread("{}/data/image/{}.png".format(WORKING_PATH, fileName))
    image = get_resized_image(image, ratio=1/RESIZE_RATIO)
    image = get_light_removed_image(image)
    image = get_resized_image(image, ratio=RESIZE_RATIO)
    cv2.imwrite("{}/data/image/{}.png".format(WORKING_PATH, fileName), image)