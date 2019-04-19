import numpy as np
import cv2
import dlib
from config import *

FACE_POINTS       = list(range(17, 68))
JAW_POINTS        = list(range( 0, 17))
RIGHT_BROW_POINTS = list(range(17, 22))
LEFT_BROW_POINTS  = list(range(22, 27))
NOSE_POINTS       = list(range(27, 35))
RIGHT_EYE_POINTS  = list(range(36, 42))
LEFT_EYE_POINTS   = list(range(42, 48))
MOUTH_POINTS      = list(range(48, 61))

OVERLAY_POINTS    = list(range(0, 27))

detector = dlib.get_frontal_face_detector() # using Histogram Of Gradient
predictor = dlib.shape_predictor(WORKING_PATH + '/libs/shape_predictor_68_face_landmarks.dat')

class TooManyFaces(Exception):
	pass

class NoFace(Exception):
	pass

def get_landmarks(img):
	rects = detector(img, 0)

	if len(rects) > 1:
		raise TooManyFaces
	if len(rects) == 0:
		raise NoFace

	return np.matrix([[p.x, p.y] for p in predictor(img, rects[0]).parts()])

def show_landmarks(img, lms):
	img = img.copy()
	lms = np.asarray(lms)

	for pts in enumerate(lms):
		pos = (pts[1][0], pts[1][1])
		cv2.circle(img, pos, 1, color=(0, 255, 255))

	cv2.imshow('landmarks', img)
	cv2.waitKey(0)

def get_mask(img, pts, color=(255, 255, 255)):
	mask = np.zeros(img.shape, dtype=np.uint8)
	cv2.fillConvexPoly(mask, pts, color)
	return mask

# detect and save faces in photo.
def detect_faces(file_name, file_exts):
	img = cv2.imread(WORKING_PATH + '/images/sample/' + file_name + file_exts)
	
	face_rects = detector(img, 1) # upsample the image 1 time.(make everything bigger)
	for face_no, face_rect in zip(range(len(face_rects)), face_rects):
		cv2.imwrite(WORKING_PATH + '/images/face/' + file_name + '#{}'.format(face_no) + file_exts, img[face_rect.top():face_rect.bottom(), face_rect.left():face_rect.right()])

	return len(face_rects)

# extract and save face area in photo.
def extract_faces(file_name, file_exts):
	num_faces = detect_faces(file_name, file_exts)

	for face_no in range(num_faces):
		img = cv2.imread(WORKING_PATH + '/images/face/' + file_name + '#{}'.format(face_no) + file_exts)
		lms = get_landmarks(img)
		lms = np.asarray(lms)

		pts = cv2.convexHull(lms[OVERLAY_POINTS])
		# cv2.drawContours(img, [pts], -1, (0, 255, 0), 1)
		mask = get_mask(img, pts)
		cv2.imwrite(WORKING_PATH + '/images/mask/' + file_name + '#{}'.format(face_no) + file_exts, mask)
		
		face = cv2.bitwise_and(img, mask)
		cv2.imwrite(WORKING_PATH + '/images/face/' + file_name + '#{}_face'.format(face_no) + file_exts, face)

	return num_faces