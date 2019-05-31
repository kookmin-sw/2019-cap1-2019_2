import cv2
import numpy as np
import sys
from config import *

LEFT_EYE_LEFT_TIP = 130
RIGHT_EYE_RIGHT_TIP = 359
FOREHEAD_TIP = 9
CHIN_TIP = 152

def get_distance(point_1, point_2):
    distance = 0
    for p1, p2 in zip(point_1, point_2):
        distance += np.square(p1 - p2)
    return np.sqrt(distance)

def get_resized_image(image, rw, rh):
    return cv2.resize(image, dsize=(0, 0), fx=rw, fy=rh, interpolation=cv2.INTER_LINEAR)

def get_moved_image(image, dw, dh):
    transform = np.float32([[1, 0, dw], [0, 1, dh]])
    return cv2.warpAffine(image, transform, dsize=(image.shape[1], image.shape[0]))

def get_standardized_image(image, shape):
    standardizedImage = np.zeros(shape=shape, dtype=np.uint8)
    if shape[0] < image.shape[0]:
        height = shape[0]
    else:
        height = image.shape[0]

    if shape[1] < image.shape[1]:
        width = shape[1]
    else:
        width = image.shape[1]

    standardizedImage[:height, :width, ] = image[:height, :width, ]
    return standardizedImage

def post_processing(targetName, sourceName):
    targetVertices = np.genfromtxt("{}/data/texture/{}_vertices.txt".format(WORKING_PATH, targetName), dtype=np.float)
    sourceVertices = np.genfromtxt("{}/data/texture/{}_vertices.txt".format(WORKING_PATH, sourceName), dtype=np.float)

    ratioWidth = get_distance(targetVertices[LEFT_EYE_LEFT_TIP], targetVertices[RIGHT_EYE_RIGHT_TIP]) / \
                 get_distance(sourceVertices[LEFT_EYE_LEFT_TIP], sourceVertices[RIGHT_EYE_RIGHT_TIP])

    ratioHeight = get_distance(targetVertices[FOREHEAD_TIP], targetVertices[CHIN_TIP]) / \
                  get_distance(sourceVertices[FOREHEAD_TIP], sourceVertices[CHIN_TIP])

    # ratioWidth *= 1.05
    # ratioHeight *= 1.05

    targetImage = cv2.imread("{}/data/image/{}.png".format(WORKING_PATH, targetName))
    sourceImage = cv2.imread("{}/data/texture/{}.png".format(WORKING_PATH, sourceName))
    # sourceImage = get_resized_image(sourceImage, RESIZE_RATIO, RESIZE_RATIO)
    sourceImage = get_resized_image(sourceImage, ratioWidth, ratioHeight)
    sourceImage = get_standardized_image(sourceImage, shape=targetImage.shape)
    
    targetCenter = np.genfromtxt("{}/data/texture/{}_headPose.txt".format(WORKING_PATH, targetName), dtype=np.float)
    sourceCenter = np.genfromtxt("{}/data/texture/{}_headPose.txt".format(WORKING_PATH, sourceName), dtype=np.float)
    sourceCenter[0] *= ratioWidth
    sourceCenter[1] *= ratioHeight
    sourceVertices[:, 0] *= ratioWidth
    sourceVertices[:, 1] *= ratioHeight

    distanceToMove = targetCenter - sourceCenter
    # distanceToMove[1] = targetVertices[FOREHEAD_TIP, 1] - sourceVertices[FOREHEAD_TIP, 1]
    distanceToMove[1] = targetVertices[CHIN_TIP, 1] - sourceVertices[CHIN_TIP, 1]
    
    sourceImage = get_moved_image(sourceImage, dw=distanceToMove[0], dh=distanceToMove[1])
    cv2.imwrite("{}/data/texture/{}.png".format(WORKING_PATH, sourceName), sourceImage)

    for i in range(len(sourceVertices)):
        sourceVertices[i] += distanceToMove
    np.savetxt("{}/data/texture/{}_vertices.txt".format(WORKING_PATH, sourceName), X=sourceVertices, fmt='%s')
    
if __name__ == '__main__':
    targetName = sys.argv[1]
    sourceName = sys.argv[2]

    sourceImage = cv2.imread("{}/data/mesh/{}_reduced.png".format(WORKING_PATH, sourceName))
    os.system("cd {}\\utils\\modeling\\Build & Modeling.exe {} {} {} {}".format(WORKING_PATH, targetName, sourceName, sourceImage.shape[1], sourceImage.shape[0]))

    post_processing(targetName, sourceName)