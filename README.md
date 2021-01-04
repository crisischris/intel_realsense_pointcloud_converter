# intel_realsense_pointcloud_converter
Intel realsense pointcloud converter and ingestion files. This script has been used and *only* tested with the intel l515 lidar camera however any pountcloud output by intel's realsense camera suite should work. 

This repo contains a python binary .ply yo ASCII .ply converter script as well as a C# script for ingestion of the converted .ply file into Unity3D. This script remedies an issue I was having trying to programmatically export to an ASCII version .ply using intel's realsense API.  Currently, export options for the C++ implementation of the realsense API doesn't seem to allow for export to ASCII and instead defaults export to binary.  The convert.py file takes in a binary .ply file and converts to ASCII.    

# converter.py
*NOTE: binary .ply file must be stripped of the .ply format header to be used by this script* 
Expects as argument:<br/>
*required* [valid, binary .ply file]
<br/>
*required* [valid, binary header-stripped .ply file]
<br/>
*optional* [the output file name]
<br/>

# ingest_ply_file.cs
This script should be attached to a unity empty object with a Mesh Renderer.  Use your exported to ASCII .ply file name in the field 'File_name'.
<br/><br/>
unity gizmo pointcloud<br/>
   <img src="https://github.com/crisischris/intel_realsense_pointcloud_converter/blob/main/IMGs/lidar_chris_1.png"><br/>
depth-only pointcloud<br/>
   <img src="https://github.com/crisischris/intel_realsense_pointcloud_converter/blob/main/IMGs/lidar_chris_2.png">
