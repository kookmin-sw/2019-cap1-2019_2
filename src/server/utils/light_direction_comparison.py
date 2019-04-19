import cv2

face_cascade = cv2.CascadeClassifier(
    './haarcascades/haarcascade_frontalface_default.xml')


def load_point_light(i):
    path = "../images/point_light/Point Light " + str(i) + ".jpg"
    img = cv2.imread(path)

    return img


def face_detect(img, grayImage, i):
    path = "../images/face/Face" + str(i) + ".jpg"
    faces = face_cascade.detectMultiScale(grayImage, 1.03, 5)

    for (x, y, w, h) in faces:
        cropped = img[y:y + h, x:x + h]

    cv2.imwrite(path, cropped)


def similarity(img1, img2):
    _, dst1 = cv2.threshold(img1, 100, 255, cv2.THRESH_BINARY)
    _, dst2 = cv2.threshold(img2, 100, 255, cv2.THRESH_BINARY)

    resized_img1 = cv2.resize(dst1, dsize=(250, 250), interpolation=cv2.INTER_AREA)
    resized_img2 = cv2.resize(dst2, dsize=(250, 250), interpolation=cv2.INTER_AREA)

    blur1 = cv2.medianBlur(resized_img1, 5)
    blur2 = cv2.medianBlur(resized_img2, 5)

    count = 0
    for i in range(250):
        for j in range(250):
            if (blur1[i][j] == blur2[i][j]):
                count += 1

    similarity = count / (250 * 250)
    print(similarity)

    return similarity

if __name__ == '__main__':
    for i in range(1,10):
        img = load_point_light(i)
        grayTarget = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
        face_detect(img, grayTarget, i)

    src = cv2.imread('../images/sample/Face1.jpg')
    graySrc = cv2.cvtColor(src, cv2.COLOR_BGR2GRAY)
    faces = face_cascade.detectMultiScale(graySrc, 1.03, 5)

    for (x,y,w,h) in faces:
        cropped = src[y:y+h, x:x+h]

    sim = []
    for i in range(1, 10):
        path = "../images/face/Face" + str(i) + ".jpg"
        light = cv2.imread(path, cv2.IMREAD_GRAYSCALE)

        sim.append(similarity(light, src))

    for i in range(9):
        if(sim[i] == max(sim)):
            print(i+1)