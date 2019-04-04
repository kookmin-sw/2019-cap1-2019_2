from config import *
from face_detection import *
from imutils import face_utils

def get_camera_matrix(size):
	focal_length = size[1]
	center = (size[1]/2, size[0]/2)
	return np.array([
		[focal_length, 0, center[0]],
		[0, focal_length, center[1]],
		[0, 0, 1]], dtype="double")

def get_image_points(img):
	lms = get_landmarks(img)
	lms = np.asarray(lms)
	return np.array([
		lms[30],	# Nose tip
		lms[8],		# Chin
		lms[36],	# Left eye left corner
		lms[45],	# Right eye right corner
		lms[48],	# Left mouth corner
		lms[54]		# Right mouth corner
	], dtype="double")

def get_model_points():
	return np.array([
		(0.0, 0.0, 0.0),			# Nose tip
		(0.0, -330.0, -65.0),		# Chin
		(-225.0, 170.0, -135.0),	# Left eye left corner
		(225.0, 170.0, -135.0),		# Right eye right corner
		(-150.0, -150.0, -125.0),	# Left mouth corner
		(150.0, -150.0, -125.0)		# Right mouth corner
	])

def get_head_pose(img):
	camera_mat = get_camera_matrix(img.shape)
	image_pts = get_image_points(img)
	model_pts = get_model_points()
	dist_coeffs = np.zeros((4, 1)) # Assuming no lens distortion

	_, rotation_vec, translation_vec = cv2.solvePnP(model_pts, image_pts, camera_mat, dist_coeffs, flags=cv2.SOLVEPNP_ITERATIVE)
	# reproject_dst, _ = cv2.projectPoints(np.array([(0.0, 0.0, 1000.0)]), rotation_vec, translation_vec, camera_mat, dist_coeffs)
	rotation_mat, _ = cv2.Rodrigues(rotation_vec)
	pose_mat = cv2.hconcat((rotation_mat, translation_vec))
	_, _, _, _, _, _, euler_angle = cv2.decomposeProjectionMatrix(pose_mat)

	return euler_angle