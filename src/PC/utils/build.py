import sys
import os


ProjectPath = "C:\\Users\\mugcup\\Desktop\\Modeling"
BuildScript = "BuildScript.BuildWindow"
BuildPath = "C:\\Users\\mugcup\\Desktop\\Modeling\\new"
SaveImagePath = "C:\\Users\\mugcup\\Desktop\\Modeling\\Image"
#위의 경로 수정이 필요!!
SourceImageName = "190509_174439"
TargetImageName = "190509_174524"

os.system("unity -quit -batchmode - projectPath {} -executeMethod {}".format(ProjectPath, BuildScript))
os.system("cd {} & myGame.exe -SP {} -SI {} -TI {}".format(BuildPath, SaveImagePath, SourceImageName, TargetImageName))

# import sys
# import os
# from config import *
# ProjectPath = WORKING_PATH + "\\3Dmodeling"
# BuildScript = "BuildScript.BuildWindow"
# BuildPath = WORKING_PATH + "\\3Dmodeling\\new1"
# SaveImagePath = WORKING_PATH + "\\3Dmodeling\\Image"
# #위의 경로 수정이 필요!!
# SourceImageName = "190509_174439"
# TargetImageName = "190509_174524"

# os.system("unity -quit -batchmode - projectPath {} -executeMethod {}".format(ProjectPath, BuildScript))
#os.system("cd {} & myGame.exe -SP {} -SI {} -TI {}".format(BuildPath, SaveImagePath, SourceImageName, TargetImageName))
