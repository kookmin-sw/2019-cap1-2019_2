import cv2
import numpy as np
import sys
from config import *

REGION_OF_INTEREST = [  9, 107,  66, 105,  63,  70, 156, 143, 116, 123,
                      147, 213, 192, 214, 135, 136, 150, 149, 176, 148,
                      152, 377, 400, 378, 379, 364, 434, 416, 433, 376,
                      352, 345, 372, 383, 300, 293, 334, 296, 336]

def get_mask(shape, points, color=(255, 255, 255)):
    polygon = np.int32(cv2.convexHull(points))
    mask = np.zeros(shape, dtype=np.uint8)
    cv2.fillConvexPoly(mask, polygon, color)
    kernel = np.ones((5, 5), dtype=np.uint8)
    return cv2.erode(mask, kernel, iterations = 3)

def get_center_height_of_mask(maskImage):
    maxH = 0
    minH = maskImage.shape[0]

    for i in range(maskImage.shape[0]):
        if maskImage[i,:,:].sum() != 0:
            minH = i
            break
    for i in reversed(range(maskImage.shape[0])):
        if maskImage[i,:,:].sum() != 0:
            maxH = i
            break
    return (maxH + minH) // 2

def get_region_of_interest_image(image, points):
    mask = get_mask(image.shape[:2], points)
    return cv2.bitwise_and(image, image, mask=mask)

def synthesis(targetName, sourceName):
    targetImage = cv2.imread("{}/data/image/{}.png".format(WORKING_PATH, targetName))
    sourceImage = cv2.imread("{}/data/texture/{}.png".format(WORKING_PATH, sourceName))

    targetCenter = np.genfromtxt("{}/data/texture/{}_headPose.txt".format(WORKING_PATH, targetName), dtype=np.int)
    sourceVertices = np.genfromtxt("{}/data/texture/{}_vertices.txt".format(WORKING_PATH, sourceName), dtype=np.int)
    
    sourceImage = get_region_of_interest_image(sourceImage, sourceVertices[REGION_OF_INTEREST])
    mask = get_mask(sourceImage.shape[:2], sourceVertices[REGION_OF_INTEREST])
    
    synthesizedImage = cv2.seamlessClone(src=sourceImage, dst=targetImage, mask=mask, p=(targetCenter[0], get_center_height_of_mask(sourceImage)), flags=cv2.NORMAL_CLONE)
    cv2.imwrite("{}/data/image/{}+{}.png".format(WORKING_PATH, targetName, sourceName), synthesizedImage)

if __name__ == '__main__':
    targetName = sys.argv[1]
    sourceName = sys.argv[2]

    synthesis(targetName, sourceName)