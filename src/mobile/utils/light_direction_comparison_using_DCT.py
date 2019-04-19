from PIL import Image
import numpy as np
from scipy import fftpack
import cv2
import pandas as pd

# https://github.com/opencv/opencv/tree/master/data/haarcascades
face_cascade = cv2.CascadeClassifier(
    './haarcascades/haarcascade_frontalface_default.xml')


def load_point_light(i):
    path = "../images/point_light/Point Light " + str(i) + ".jpg"
    img = cv2.imread(path)

    return img


def face_detect(img, grayImage, i):
    path = "../images/face/Face" + str(i) + ".jpg"
    faces = face_cascade.detectMultiScale(grayImage, 1.03, 5)

    for (x, y, w, h) in faces:
        cropped = img[y:y + h, x:x + h]

    cv2.imwrite(path, cropped)


def get_image(path='../images/sample/Face1.jpg', size=(128, 128)):
    image = Image.open(path)
    img_color = image.resize(size, 1)
    img_grey = img_color.convert('L')
    img = np.array(img_grey, dtype=np.float)

    return img

def get_2D_dct(img):
    """ Get 2D Cosine Transform of Image
    """
    return fftpack.dct(fftpack.dct(img.T, norm='ortho').T, norm='ortho')

def get_2d_idct(coefficients):
    """ Get 2D Inverse Cosine Transform of Image
    """
    return fftpack.idct(fftpack.idct(coefficients.T, norm='ortho').T, norm='ortho')


def get_reconstructed_image(raw):
    img = raw.clip(0, 255)
    img = img.astype('uint8')
    img = Image.fromarray(img)
    return img


def similarity(img1, img2):
    count = 0
    for i in range(250):
        for j in range(250):
            if (img1[i][j] == img2[i][j]):
                count += 1

    similarity = count / (250 * 250)
    print(similarity)

    return similarity

if __name__ == '__main__':
    for i in range(1,10):
        img = load_point_light(i)
        grayTarget = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
        face_detect(img, grayTarget, i)


    image_path = "../images/face/SrcFace.jpg"

    pixels = get_image(path=image_path, size=(256, 256))
    dct_size = pixels.shape[0]
    dct = get_2D_dct(pixels)
    reconstructed_images = []

    for ii in range(dct_size):
        dct_copy = dct.copy()
        dct_copy[ii:, :] = 0
        dct_copy[:, ii:] = 0

        # Reconstructed image
        r_img = get_2d_idct(dct_copy);
        reconstructed_image = get_reconstructed_image(r_img);

        # Create a list of images
        reconstructed_images.append(reconstructed_image);

    for i in range(1, 10):
        target_path = "../images/face/Face" + str(i) + ".jpg"
        dst_path = "../images/DCT_Result/target" + str(i) + ".jpg"

        pixels = get_image(path=target_path, size=(256, 256))
        dct_size = pixels.shape[0]
        dct = get_2D_dct(pixels)
        reconstructed_images = []

        for ii in range(dct_size):
            dct_copy = dct.copy()
            dct_copy[ii:, :] = 0
            dct_copy[:, ii:] = 0

            # Reconstructed image
            r_img = get_2d_idct(dct_copy);
            reconstructed_image = get_reconstructed_image(r_img);

            # Create a list of images
            reconstructed_images.append(reconstructed_image);

        reconstructed_images[5].save(dst_path)

    sim = []

    src = cv2.imread('../images/DCT_Result/result1.jpg', cv2.IMREAD_GRAYSCALE)

    for i in range(1, 10):
        path = "../images/DCT_Result/target" + str(i) + ".jpg"
        light = cv2.imread(path, cv2.IMREAD_GRAYSCALE)

        sim.append(similarity(light, src))

    for i in range(9):
        if(sim[i] == max(sim)):
            print(i+1)