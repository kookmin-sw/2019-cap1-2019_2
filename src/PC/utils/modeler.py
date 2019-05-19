import cv2
import numpy as np
import sys
from config import *

LEFT_EYE_LEFT_TIP = 130
RIGHT_EYE_RIGHT_TIP = 359

def get_distance(point_1, point_2):
    distance = 0
    for p1, p2 in zip(point_1, point_2):
        distance += np.square(p1 - p2)
    return np.sqrt(distance)

def get_resized_image(image, ratio):
    return cv2.resize(image, dsize=(0, 0), fx=ratio, fy=ratio, interpolation=cv2.INTER_LINEAR)

def get_moved_image(image, dh, dw):
    transform = np.float32([[1, 0, dw], [0, 1, dh]])
    return cv2.warpAffine(image, transform, dsize=(image.shape[1], image.shape[0]))

def get_padded_image(image, shape):
    paddedImage = np.zeros(shape=shape, dtype=np.uint8)
    paddedImage[:image.shape[0], :image.shape[1], ] = image
    return paddedImage

def post_processing(targetName, sourceName):
    targetVertices = np.genfromtxt("{}/data/texture/{}_vertices.txt".format(WORKING_PATH, targetName), dtype=np.float)
    sourceVertices = np.genfromtxt("{}/data/texture/{}_vertices.txt".format(WORKING_PATH, sourceName), dtype=np.float)

    ratio = get_distance(targetVertices[LEFT_EYE_LEFT_TIP], targetVertices[RIGHT_EYE_RIGHT_TIP]) / \
            get_distance(sourceVertices[LEFT_EYE_LEFT_TIP], sourceVertices[RIGHT_EYE_RIGHT_TIP])

    sourceImage = cv2.imread("{}/data/texture/{}.png".format(WORKING_PATH, sourceName))
    sourceImage = get_resized_image(sourceImage, ratio)
    sourceImage = get_padded_image(sourceImage, shape=(IMAGE_HEIGHT, IMAGE_WIDTH, IMAGE_CHANNEL))

    targetCenter = np.genfromtxt("{}/data/texture/{}_headPose.txt".format(WORKING_PATH, targetName), dtype=np.float)
    sourceCenter = np.genfromtxt("{}/data/texture/{}_headPose.txt".format(WORKING_PATH, sourceName), dtype=np.float)
    sourceCenter *= ratio
    distanceToMove = targetCenter - sourceCenter
    
    sourceImage = get_moved_image(sourceImage, dh=distanceToMove[1], dw=distanceToMove[0])
    cv2.imwrite("{}/data/texture/{}.png".format(WORKING_PATH, sourceName), sourceImage)

    for i in range(len(sourceVertices)):
        sourceVertices[i] = sourceVertices[i] * ratio + distanceToMove
    np.savetxt("{}/data/texture/{}_vertices.txt".format(WORKING_PATH, sourceName), X=sourceVertices, fmt='%s')
    
if __name__ == '__main__':
    targetName = sys.argv[1]
    sourceName = sys.argv[2]

    os.system("cd {}\\utils\\modeling\\Build & Modeling.exe {} {}".format(WORKING_PATH, targetName, sourceName))
    post_processing(targetName, sourceName)