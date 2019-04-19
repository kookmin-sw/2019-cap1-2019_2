import os
from config import *
from face_detection import *
from head_pose import *

if __name__ == '__main__':
	trg_name, trg_exts = os.path.splitext(os.path.basename(TARGET_IMG))
	src_name, src_exts = os.path.splitext(os.path.basename(SOURCE_IMG))

	print("{} faces extracted.".format(extract_faces(trg_name, trg_exts)))

	trg_face_img = cv2.imread(WORKING_PATH + '/images/face/' + trg_name + '#{}'.format(TARGET_FACE_NO) + trg_exts)
	pose = get_head_pose(trg_face_img)
	print("Head Pose by Euler angles\n\tx: {}\n\ty: {}\n\tz: {}".format(pose[0], pose[1], pose[2]))