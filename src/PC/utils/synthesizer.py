import cv2
import numpy as np
import sys
from config import *

REGION_OF_INTEREST = [  9, 336, 296, 334, 293, 300, 383, 372, 345, 352,
                      376, 433, 416, 434, 430, 431, 262, 428, 199, 208,
                       32, 211, 210, 214, 192, 213, 147, 123, 116, 143,
                      156,  70,  63, 105,  66, 107]

def get_mask(shape, points, color=(255, 255, 255)):
    points = np.int32(cv2.convexHull(points))
    mask = np.zeros(shape, dtype=np.uint8)
    cv2.fillConvexPoly(mask, points, color)
    return mask

def get_region_of_interest_image(image, points):
    mask = get_mask(image.shape[:2], points)
    return cv2.bitwise_and(image, image, mask=mask)

def synthesis(targetName, sourceName):
    targetImage = cv2.imread("{}/data/image/{}.png".format(WORKING_PATH, targetName))
    sourceImage = cv2.imread("{}/data/texture/{}.png".format(WORKING_PATH, sourceName))

    sourceVertices = np.genfromtxt("{}/data/texture/{}_vertices.txt".format(WORKING_PATH, sourceName), dtype=np.float32)

    sourceImage = get_region_of_interest_image(sourceImage, sourceVertices[REGION_OF_INTEREST])
    mask = get_mask(sourceImage.shape[:2], sourceVertices[REGION_OF_INTEREST])
    maskInv = cv2.bitwise_not(mask)

    targetImage = cv2.bitwise_and(targetImage, targetImage, mask=maskInv)
    synthesizedImage = cv2.add(targetImage, sourceImage)
    cv2.imwrite("{}/data/image/{}+{}.png".format(WORKING_PATH, targetName, sourceName), synthesizedImage)


if __name__ == '__main__':
    targetName = sys.argv[1]
    sourceName = sys.argv[2]

    synthesis(targetName, sourceName)