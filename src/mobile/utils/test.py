import os
from config import *
from face_detection import *
from light_reduction import *

if __name__ == '__main__':
	trg_name, trg_exts = os.path.splitext(os.path.basename(TARGET_IMG))
	src_name, src_exts = os.path.splitext(os.path.basename(SOURCE_IMG))

	print(extract_faces(trg_name, trg_exts))

