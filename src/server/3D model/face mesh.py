import bpy 
from bpy.types import Operator 
from bpy_extras.object_utils import AddObjectHelper, object_data_add 
from mathutils import Vector
from math import radians
import os

def createMesh(origin):
#    create mesh and object
    me = bpy.data.meshes.new('FaceMesh')
    ob = bpy.data.objects.new('Face', me)
    ob.location = origin
#    link object to scene   
    scn = bpy.context.scene
    scn.objects.link(ob)
    scn.objects.active = ob
    scn.update()
    
    f = open("/Users/csh/Desktop/vertices.txt", "r")
    verts = []
    faces = []
    while True:
        line = f.readline()
        if not line:
            break
        split = line.split(',')

        v = Vector((float(split[0]), float(split[1]), float(split[2])))
        print(v)
        verts.append(v)
        
    f = open("/Users/csh/Desktop/triangle.txt", "r")
    cnt = 0
    faces = []
    tmp = []
    while True:
        line = f.readline()
        if not line:
            break
        split = line.split()

        num = int(split[0])
        if(cnt == 0):
            tmp.append(num)
            cnt += 1
        elif(cnt == 1):
            tmp.append(num)
            cnt += 1
        else:
            tmp.append(num)
            faces.append(tmp)
            tmp = []
            cnt = 0
            
    me.from_pydata(verts, [], faces)
    
    me.update(calc_edges = True)
    
    texFaces = []
    
    f = open("/Users/csh/Desktop/uv.txt", "r")
    
    while True:
        line = f.readline()
        if not line:
            break
        split = line.split(',')
        tmp = []
        tmp.append(float(split[0]))
        tmp.append(float(split[1]))
        texFaces.append(tmp)

    bpy.ops.mesh.uv_texture_add()
    uvCyl = me.uv_textures.active
    uvCyl.name = 'UVCyl'
    bpy.ops.object.mode_set(mode='EDIT')
    bpy.ops.uv.cylinder_project()
    bpy.ops.object.mode_set(mode = 'OBJECT')
    
    return ob
       
def run(origin):
    ob = createMesh(origin)
    return

def unwrap_face():
    context = bpy.context
    scene = context.scene
    for obj in scene.objects:
        if(obj.type == 'MESH'):
            scene.objects.active = obj
            obj.select = True
            print(obj.name)
            lm = obj.data.uv_textures.get("LightMap")
            if not lm:
                lm = obj.data.uv_textures.new("LightMap")
            lm.active = True
            bpy.ops.object.editmode_toggle()
            bpy.ops.uv.unwrap()
            bpy.ops.uv.select_all(action = 'TOGGLE')
            last_area = bpy.context.area.type
            bpy.context.area.type = 'IMAGE_EDITOR'
            bpy.ops.transform.rotate(value = -1.5708)
            bpy.context.area.type = last_area
            bpy.ops.object.editmode_toggle()
    
if __name__ == "__main__":
    run( (0, 0, 0) )
    unwrap_face()
