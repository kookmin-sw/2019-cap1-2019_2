PREDICTOR_PATH = "./lib/shape_predictor_68_face_landmarks.dat"

TARGETIMAGE_PATH = "./images/pic6.jpg"
SOURCEIMAGE_PATH = "./images/pic8.jpg"

TRANSFORMIAMGE_PATH = "./images/transformimg.jpg"
DELETEMOUTHIMAGE_PATH = "./images/deletedmouth.jpg"
COLORCORREDIAMGE_PATH = "./images/correctedimage.jpg"

SCALE_FACTOR = 1
FEATHER_AMOUNT = 11
COLOUR_CORRECT_BLUR_FRAC = 0.6



FACE_POINTS       = list(range(17, 68))
JAW_POINTS        = list(range( 0, 17))
RIGHT_BROW_POINTS = list(range(17, 22))
LEFT_BROW_POINTS  = list(range(22, 27))
NOSE_POINTS       = list(range(27, 35))
RIGHT_EYE_POINTS  = list(range(36, 42))
LEFT_EYE_POINTS   = list(range(42, 48))
MOUTH_POINTS      = list(range(48, 61))

