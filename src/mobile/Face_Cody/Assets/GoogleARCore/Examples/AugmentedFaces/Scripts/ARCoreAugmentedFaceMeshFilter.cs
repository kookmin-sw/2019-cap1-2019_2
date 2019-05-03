//-----------------------------------------------------------------------
// <copyright file="ARCoreAugmentedFaceMeshFilter.cs" company="Google">
//
// Copyright 2018 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.AugmentedFaces
{
    using System.Collections.Generic;
    using GoogleARCore;
    using UnityEngine;
    using System.IO;

    /// <summary>
    /// Helper component to update face mesh data.
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    public class ARCoreAugmentedFaceMeshFilter : MonoBehaviour
    {
        /// <summary>
        /// If true, this component will update itself using the first AugmentedFace detected by ARCore.
        /// </summary>
        public bool AutoBind = false;

        private AugmentedFace m_AugmentedFace = null;
        private List<AugmentedFace> m_AugmentedFaceList = null;

        // Keep previous frame's mesh polygon to avoid mesh update every frame.
        private List<Vector3> m_MeshVertices = new List<Vector3>();
        private List<Vector3> m_MeshNormals = new List<Vector3>();
        private List<Vector2> m_MeshUVs = new List<Vector2>();
        private List<int> m_MeshIndices = new List<int>();
        private Mesh m_Mesh = null;
        private bool m_MeshInitialized = false;
        private Camera camera;
        private string path;
        public string filename = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        /// <summary>
        /// Gets or sets the ARCore AugmentedFace object that will be used to update the face mesh data.
        /// </summary>
        public AugmentedFace AumgnetedFace
        {
            get
            {
                return m_AugmentedFace;
            }

            set
            {
                m_AugmentedFace = value;
                Update();
            }
        }

        /// <summary>
        /// The Unity Awake() method.
        /// </summary>
        public void Awake()
        {
            m_Mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = m_Mesh;
            m_AugmentedFaceList = new List<AugmentedFace>();
            camera = GameObject.Find("First Person Camera").GetComponent<Camera>();
            path = Application.persistentDataPath + "/My_Log/";

            if (!Directory.Exists(path + "mesh/"))
            {
                Directory.CreateDirectory(path + "mesh/");
            }
            if (!Directory.Exists(path + "texture/"))
            {
                Directory.CreateDirectory(path + "texture/");
            }
        }

        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        public void Update()
        {
            if (AutoBind && m_AugmentedFace == null)
            {
                m_AugmentedFaceList.Clear();
                Session.GetTrackables<AugmentedFace>(m_AugmentedFaceList, TrackableQueryFilter.All);
                if (m_AugmentedFaceList.Count != 0)
                {
                    m_AugmentedFace = m_AugmentedFaceList[0];
                }
            }

            if (m_AugmentedFace == null)
            {
                return;
            }

            // Update game object position;
            transform.position = m_AugmentedFace.CenterPose.position;
            transform.rotation = m_AugmentedFace.CenterPose.rotation;

            _UpdateMesh();
        }

        /// <summary>
        /// Update mesh with a face mesh vertices, texture coordinates and indices.
        /// </summary>
        private void _UpdateMesh()
        {
            m_AugmentedFace.GetVertices(m_MeshVertices);
            m_AugmentedFace.GetNormals(m_MeshNormals);

            if (!m_MeshInitialized)
            {
                m_AugmentedFace.GetTextureCoordinates(m_MeshUVs);
                m_AugmentedFace.GetTriangleIndices(m_MeshIndices);

                // Only update mesh indices and uvs once as they don't change every frame.
                m_MeshInitialized = true;
            }

            m_Mesh.Clear();
            m_Mesh.SetVertices(m_MeshVertices);
            m_Mesh.SetNormals(m_MeshNormals);
            m_Mesh.SetTriangles(m_MeshIndices, 0);
            m_Mesh.SetUVs(0, m_MeshUVs);

            m_Mesh.RecalculateBounds();
        }

        public void SaveMeshInfo()
        {         
            SaveMeshVertices();
            SaveMeshUVs();
            SaveMeshTriangles();
            img_data.filename = filename;
        }

        public void SaveMeshVertices()
        {
            string name = path + "mesh/" + filename + "_vertices.txt";
            FileStream fs = new FileStream(name, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);


            foreach (Vector3 vertex in m_Mesh.vertices)
            {
                sw.WriteLine("{0} {1} {2}", vertex.x, vertex.y, vertex.z);
            }

            sw.Close();
            fs.Close();
        }

        public void SaveMeshUVs()
        {
            string name = path + "mesh/" + filename + "_uvs.txt";
            FileStream fs = new FileStream(name, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);


            foreach (Vector2 uv in m_Mesh.uv)
            {
                sw.WriteLine("{0} {1}", uv.x, uv.y);
            }

            sw.Close();
            fs.Close();
        }

        public void SaveMeshTriangles()
        {
            string name = path + "mesh/" + filename + "_triangles.txt";
            FileStream fs = new FileStream(name, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);


            foreach (int index in m_Mesh.triangles)
            {
                sw.WriteLine(index);
            }

            sw.Close();
            fs.Close();
        }


        public void SaveTextureInfo()
        {
            SaveTextureVertices();
            SaveTextureUVs();
            SaveTextureTriangles();
        }

        public void SaveTextureVertices()
        {
            string name = path + "texture/" + filename + "_vertices.txt";
            FileStream fs = new FileStream(name, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            foreach (Vector3 vertex in m_Mesh.vertices)
            {
                Vector2 coord = LocalToPixelPoint(vertex);
                sw.WriteLine("{0} {1}", coord.x, coord.y);
            }

            sw.Close();
            fs.Close();
        }

        public void SaveTextureUVs()
        {
            string name = path + "texture/" + filename + "_uvs.txt";
            FileStream fs = new FileStream(name, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            foreach (Vector2 uv in m_Mesh.uv)
            {
                //sw.WriteLine("{0} {1}", uv.x, 1.0 - uv.y);
                sw.WriteLine("{0} {1}", Screen.width * uv.x, Screen.height * (1.0 - uv.y));
            }

            sw.Close();
            fs.Close();
        }

        public void SaveTextureTriangles()
        {
            string name = path + "texture/" + filename + "_triangles.txt";
            FileStream fs = new FileStream(name, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            for (int i = 0; i < m_Mesh.triangles.Length; i+=3)
            {
                sw.WriteLine("{0} {1} {2}", m_Mesh.triangles[i], m_Mesh.triangles[i + 1], m_Mesh.triangles[i + 2]);
            }

            sw.Close();
            fs.Close();
        }

        public Vector2 LocalToPixelPoint(Vector3 coord)
        {
            // local space to world space
            coord = transform.TransformPoint(coord);
            // world space to screen space
            coord = camera.WorldToScreenPoint(coord);
            // screen spcae to pixel space
            return new Vector2(coord.x, Screen.height - coord.y);
        }

    }
}
