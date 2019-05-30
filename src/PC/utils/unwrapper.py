import cv2
import numpy as np
import sys
from config import *

class Texture:
    def __init__(self, fileName=""):
        if fileName != "":
            self.image = cv2.imread("{}/data/mesh/{}_reduced.png".format(WORKING_PATH, fileName))
            self.vertices = self.load_data("{}/data/texture/{}_vertices.txt".format(WORKING_PATH, fileName))
        else:
            self.image = np.zeros(shape=(1, 1, 3), dtype=np.uint8)
            self.vertices = self.load_data("{}/data/texture/uvs.txt".format(WORKING_PATH))

        self.vertices = self.vertices // RESIZE_RATIO
        self.triangles = self.get_triangles()
        self.rectangles = self.get_rectangles()

    def load_data(self, filePath, dtype=np.int):
        return np.genfromtxt(filePath, dtype=dtype)

    def get_triangles(self):
        triangles = []
        for index in self.load_data("{}/data/texture/triangles.txt".format(WORKING_PATH)):
            triangles.append(np.float32([self.vertices[index[0]],
                                         self.vertices[index[1]],
                                         self.vertices[index[2]]]))
        return np.asarray(triangles)

    def get_rectangles(self):
        rectangles = []
        for index in self.load_data("{}/data/texture/rectangles.txt".format(WORKING_PATH)):
            rectangles.append(np.float32([self.vertices[index[0]],
                                          self.vertices[index[1]],
                                          self.vertices[index[2]],
                                          self.vertices[index[3]]]))
        return np.asarray(rectangles)

def get_mask(shape, points, color=(255, 255, 255)):
    polygon = np.int32(cv2.convexHull(points))
    mask = np.zeros(shape, dtype=np.uint8)
    cv2.fillConvexPoly(mask, polygon, color)
    return mask

def get_region_of_interest_image(image, points):
    mask = get_mask(image.shape[:2], points)
    return cv2.bitwise_and(image, image, mask=mask)

def get_triangular_affine_image(image, sourcePoints, targetPoints):
    transform = cv2.getAffineTransform(sourcePoints, targetPoints)
    return cv2.warpAffine(image, transform, dsize=(image.shape[1], image.shape[0]))

def get_rectangular_affine_image(image, sourcePoints, targetPoints):
    transform = cv2.getPerspectiveTransform(sourcePoints, targetPoints)
    return cv2.warpPerspective(image, transform, dsize=(image.shape[1], image.shape[0]))

def get_area_of_non_zero_region(image):
    gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    _, gray = cv2.threshold(gray, 1, 1, cv2.THRESH_BINARY)
    return gray.sum()

def get_ratio_of_outer_area_to_inner_area(image, points):
    mask = get_mask(image.shape[:2], points)
    maskInv = cv2.bitwise_not(mask)
    innerImage = cv2.bitwise_and(image, image, mask=mask)
    outerImage = cv2.bitwise_and(image, image, mask=maskInv)
    np.seterr(all='raise')
    return get_area_of_non_zero_region(outerImage) / get_area_of_non_zero_region(innerImage)

def remove_of_outer_region(image, points):
    mask = get_mask(image.shape[:2], points)
    return cv2.bitwise_and(image, image, mask=mask)

def add_two_images(baseImage, addImage):
    gray_1 = cv2.cvtColor(baseImage, cv2.COLOR_BGR2GRAY)
    _, mask_1 = cv2.threshold(gray_1, 10, 255, cv2.THRESH_BINARY)
    maskInv_1 = cv2.bitwise_not(mask_1) 

    gray_2 = cv2.cvtColor(addImage, cv2.COLOR_BGR2GRAY)
    _, mask_2 = cv2.threshold(gray_2, 10, 255, cv2.THRESH_BINARY)

    mask = cv2.bitwise_and(maskInv_1, mask_2)
    subImage = cv2.bitwise_and(addImage, addImage, mask=mask)
    return cv2.add(baseImage, subImage), cv2.bitwise_and(mask_1, mask_2)

def get_thicker_mask_image(maskImage, thickness=2):
    thickerImage = maskImage.copy()
    for i in range(1, thickness):
        thickerImage[i: , :] += maskImage[:-i, :]
        thickerImage[:-i, :] += maskImage[i: , :]
        thickerImage[:, i: ] += maskImage[:, :-i]
        thickerImage[:, :-i] += maskImage[:, i: ]
    return thickerImage

def get_resized_image(image, ratio):
    return cv2.resize(image, dsize=(0, 0), fx=ratio, fy=ratio, interpolation=cv2.INTER_LINEAR)

def unwrapping(fileName):
    source = Texture(fileName)
    target = Texture()
    target.image = np.zeros(shape=source.image.shape, dtype=np.uint8)

    inpaintMask = cv2.add(get_mask(target.image.shape[:2], target.rectangles[262]),
                          get_mask(target.image.shape[:2], target.rectangles[263]))
    
    for sourceTriangle, targetTriangle in zip(source.triangles, target.triangles):
        sourceTriangleImage = get_region_of_interest_image(source.image, sourceTriangle)
        affineImage = get_triangular_affine_image(sourceTriangleImage, sourceTriangle, targetTriangle)

        targetTriangleImage = remove_of_outer_region(affineImage, targetTriangle)
        target.image, inpaintMask_ = add_two_images(target.image, targetTriangleImage)
        inpaintMask = cv2.add(inpaintMask, inpaintMask_)

    for sourceRectangle, targetRectangle in zip(source.rectangles, target.rectangles):
        sourceRectangleImage = get_region_of_interest_image(source.image, sourceRectangle)
        affineImage = get_rectangular_affine_image(sourceRectangleImage, sourceRectangle, targetRectangle)

        try:
            if get_ratio_of_outer_area_to_inner_area(affineImage, targetRectangle) > 4.0:
                inpaintMask_ = get_mask(target.image.shape[:2], targetRectangle)
                inpaintMask = cv2.add(inpaintMask, inpaintMask_)
                continue
        except:
            inpaintMask_ = get_mask(target.image.shape[:2], targetRectangle)
            inpaintMask = cv2.add(inpaintMask, inpaintMask_)
            continue

        targetRectangleImage = remove_of_outer_region(affineImage, targetRectangle)
        target.image, inpaintMask_ = add_two_images(target.image, targetRectangleImage)
        inpaintMask = cv2.add(inpaintMask, inpaintMask_)

    inpaintMask = get_thicker_mask_image(inpaintMask)
    target.image = cv2.inpaint(target.image, inpaintMask, 2, cv2.INPAINT_TELEA)
    target.image = get_resized_image(target.image, RESIZE_RATIO)
    cv2.imwrite("{}/data/mesh/{}_unwrapped.png".format(WORKING_PATH, fileName), target.image)

if __name__ == '__main__':
    unwrapping(sys.argv[1]) # params: fileName