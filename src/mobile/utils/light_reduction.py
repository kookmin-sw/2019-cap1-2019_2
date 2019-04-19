import cv2
import numpy as np
from config import *

### input : face image
### Remove light by using Homomorphic filter 

def light_reduction(file_name, file_exts):
    img = cv2.imread(file_path + file_name + file_exts)
    img_yuv = cv2.cvtColor(img, cv2.COLOR_BGR2YUV)
    y, u, v = cv2.split(img_yuv)

    rows, cols = y.shape[:2]

    img_log = np.log1p(np.array(y, dtype = 'float') / 255)
    # seperate image illumination element with reflectance element

    M = 2*rows + 1
    N = 2*cols + 1

    (X, Y) = np.meshgrid(np.linspace(0, N-1, N), np.linspace(0, M-1, M))

    Xc = np.ceil(N/2)
    Yc = np.ceil(M/2)

    gaussiannumerator = (X - Xc) ** 2 + (Y - Yc) ** 2

    SIGMA = 10
    lpf = np.exp(-gaussiannumerator / (2*SIGMA*SIGMA))
    hpf = 1-lpf

    lpf_shift = np.fft.ifftshift(lpf.copy())
    hpf_shift = np.fft.ifftshift(hpf.copy())

    img_fft = np.fft.fft2(img_log.copy(), (M, N))
    img_lf = np.real(np.fft.ifft2(img_fft.copy() * lpf_shift, (M, N)))
    img_hf = np.real(np.fft.ifft2(img_fft.copy() * hpf_shift, (M, N)))    

    GAMMA1 = 0.5
    GAMMA2 = 1.5
    img_adjusting = GAMMA1 * img_lf[0:rows, 0:cols] + GAMMA2 * img_hf[0:rows, 0:cols]
    # Each lf and hf component is multiplied by a scaling factor(gamma) to
    # control the illumination and reflection values

    img_exp = np.expm1(img_adjusting)
    img_exp = (img_exp - np.min(img_exp)) / (np.max(img_exp) - np.min(img_exp))
    img_out = np.array(255 * img_exp, dtype = 'uint8')

    y = img_out

    result = cv2.merge([y, u, v])
    result = cv2.cvtColor(result, cv2.COLOR_YUV2BGR)

    return result
