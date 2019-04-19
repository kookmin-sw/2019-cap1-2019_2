import numpy as np
import cv2
import dlib
from PIL import Image
from config import *

detector = dlib.get_frontal_face_detector()
predictor = dlib.shape_predictor(PREDICTOR_PATH)

class TooManyFaces(Exception):
	pass
class NoFaces(Exception):
	pass

# Get align matrix from two pictures
# @input : 2 images   @output : transform matrix
def transformation_from_points(pts1, pts2):
	pts1 = pts1.astype(np.float64)
	pts2 = pts2.astype(np.float64)
	c1 = np.mean(pts1, axis=0)
	c2 = np.mean(pts2, axis=0)
	pts1 -= c1
	pts2 -= c2
	s1 = np.std(pts1)
	s2 = np.std(pts2)
	pts1 /= s1
	pts2 /= s2
	U, S, Vt = np.linalg.svd(pts1.T * pts2)
	R = (U * Vt).T
	return np.vstack([np.hstack(((s2 / s1) * R, c2.T - (s2 / s1) * R * c1.T)), np.matrix([0., 0., 1.])])

# detect face using dlib detector and get first face's landmark
# @input : image	@output : landmark information
def get_landmarks(img):
	rects = detector(img, 0)
	if len(rects) > 1:
		raise TooManyFaces
	if len(rects) == 0:
		raise NoFaces
	return np.matrix([[p.x, p.y] for p in predictor(img, rects[0]).parts()])

# read image from file and get landmarks
# @input : filename 	@output : image, landmarks
def read_img_and_lms(fname):
	img = cv2.imread(fname, cv2.IMREAD_COLOR)
	lms = get_landmarks(img)
	return img, lms

# draw mask with input points
# @input : image, mask points, mask color
def draw_convex_hull(img, pts, color):
	pts = cv2.convexHull(pts)
	cv2.fillConvexPoly(img, pts, color=color)

# draw mask with img with landmark and index
# @input : image, landmarks, index	@output : mask image
def get_face_mask(img, lms, OVERLAY_POINTS):
	img = np.zeros(img.shape[:2], dtype=np.float64)
	for group in OVERLAY_POINTS:
		draw_convex_hull(img, lms[group], color=1)
	img = np.array([img, img, img]).transpose((1, 2, 0))
	return img

# get widen mouth landmark points and center point
# @input : mouth landmarks, two points to get length of widen scale		@output : widen landmark, center point 
def widen_landmark(lms, lms_1, lms_2):
	center = np.zeros(2, dtype=np.float64)
	for p in lms:
		center += [p.item((0,0)), p.item((0,1))]
	center /= [len(lms), len(lms)]
	my = lms_1.item((0,1))
	ny = lms_2.item((0,1))
	d = int((ny - my)*(2/3))
	dy = [0, d]
	dx = [2.0*d, 0]
	for i in range(len(lms)):
		if i == 0:
			lms[i] +=dx
		elif 1<= i and i<6:
			lms[i] +=dy
		elif i == 6:
			lms[i] -=dx
		else:
			lms[i] -=dy
	center = (int(center[0]),int(center[1]))
	return lms, center

# warp image by transform matrix
# @input : image, transform matrix, image shape		@output : warped image
def warp_img(img, M, dshape):
	output = np.zeros(dshape, dtype=img.dtype)
	cv2.warpAffine(img, M[:2], (dshape[1], dshape[0]),
						dst=output,
						borderMode=cv2.BORDER_TRANSPARENT,
						flags=cv2.WARP_INVERSE_MAP)
	return output

# make img2 color similar to img1
# @input : two images, landmark		@output : color changed image
def correct_colours(img1, img2, lms1):
	blur_amount = COLOUR_CORRECT_BLUR_FRAC * np.linalg.norm(np.mean(lms1[LEFT_EYE_POINTS], axis=0) - np.mean(lms1[RIGHT_EYE_POINTS], axis=0))
	blur_amount = int(blur_amount)
	if blur_amount % 2 == 0:
		blur_amount += 1
	img1_blur = cv2.GaussianBlur(img1, (blur_amount, blur_amount), 0)
	img2_blur = cv2.GaussianBlur(img2, (blur_amount, blur_amount), 0)
	img2_blur += (128 * (img2_blur <= 1.0).astype(img2_blur.dtype))
	return (img2.astype(np.float64) * img1_blur.astype(np.float64) / img2_blur.astype(np.float64))

# use opencv seamless clone to make color correction and save result
# @input : two image path, mask point, mask center point
def seamlessCloning(dstimg_path, srcimg_path, mask_point, mask_center):
	dst, lms_dst = read_img_and_lms(dstimg_path) #sky
	src, lms_src = read_img_and_lms(srcimg_path) #aiplane

	src_mask = np.zeros(src.shape, src.dtype)	
	cv2.fillPoly(src_mask,[mask_point],(255,255,255))

	output = cv2.seamlessClone(src, dst, src_mask, mask_center, cv2.NORMAL_CLONE)
	cv2.imwrite(COLORCORREDIAMGE_PATH,output)
	
# get color from nose point
# @input : two landmark points, image 	@output : color
def find_color(lms_1,lms_2,img):
	find_color_point = (lms_1+lms_2)/2
	return img[int(find_color_point.item(0,0))][int(find_color_point.item(0,1))]


def inpaint(TARGETIMAGE_PATH,MASKIMAGE_PATH):
	trg = cv2.imread(TARGETIMAGE_PATH)
	mask = cv2.imread(MASKIMAGE_PATH)
	mask = cv2.cvtColor(mask,cv2.COLOR_BGR2GRAY)
	dst = cv2.inpaint(trg, mask, 3, cv2.INPAINT_TELEA)
	cv2.imwrite(DELETEMOUTHIMAGE_PATH,dst)


def mouth_swap(trgimg_path, srcimg_path):
	trg, lms_trg = read_img_and_lms(trgimg_path)
	src, lms_src = read_img_and_lms(srcimg_path)

	transform = transformation_from_points(lms_trg[MOUTH_POINTS], lms_src[MOUTH_POINTS])

	mouth = lms_trg[MOUTH_POINTS].astype(np.float64)

	mouth, mouth_center = widen_landmark(mouth, lms_trg[51], lms_trg[33])

	lms_trg[MOUTH_POINTS] = mouth

	color_info = find_color(lms_trg[33], lms_trg[30],trg)

	maskM = get_face_mask(src, lms_src, MOUTH_POINTS)

	warped_maskM = warp_img(maskM, transform, trg.shape)

	combined_maskM = np.max([get_face_mask(trg, lms_trg, [MOUTH_POINTS]), warped_maskM], axis=0)

	warped_src = warp_img(src, transform, src.shape)
	
	warped_corrted_src = correct_colours(trg, warped_src, lms_trg)
	
	output = trg * (1.0 - combined_maskM) + color_info * combined_maskM

	cv2.imwrite(TRANSFORMIAMGE_PATH,warped_src)
	cv2.imwrite(DELETEMOUTHIMAGE_PATH,output)

	seamlessCloning(DELETEMOUTHIMAGE_PATH,TRANSFORMIAMGE_PATH,lms_trg[MOUTH_POINTS], mouth_center)
	

