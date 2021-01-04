#author: chris nelson
#date: 1/3/2021

'''
Using: python 3.8
This script will ingest a binary, vertex, RGB  pointcloud .ply file from intel's d515
(and likely other cameras) lidar scanner and convert the file to ASCII format

note: this is taylor made for intel's realsense-ply export, and has not been tested on other software's
export.

PARAMETERS: required:[binary .ply file with header] required:[valid ply file with header stripped] optional:[name for exported file]
*if no export name supplied default name will be given
'''

import struct
import sys
from pathlib import Path

def main(argv):
    #default out file name
    outFile = "new.ply"

    #user supplied out file name
    if len(argv) > 2:
        outFile = argv[2]

    #get the in file name and test for validity
    try:
        inFileRef = argv[0]
        inFile = argv[1]
        if(not Path(inFileRef).is_file() or not Path(inFile).is_file()):
            print("USER ERROR: file does not exist")
            exit()
    except:
        print("USER ERROR: please supply .ply file to convert as argument")
        exit()

    #read in the header 
    with open(inFileRef, 'rb') as f:
        buffer = f.read(350)

    #string manipulation to get amount vertices and amound faces
    buffer = str(buffer)
    buffer = buffer.split("end_header")
    buffer = buffer[0]
    buffer = buffer.split('\\n')

    #assign verts and faces
    amount_verts = buffer[3]
    amount_face = buffer[10]

    #print

    print("<---------- MODEL METRICS ---------->")
    print("vertices: " + str(amount_verts))
    print("faces: " + str(amount_face))
    print()

    #get int
    amount_verts = amount_verts.split(' ')
    amount_verts = int(amount_verts[2])
    amount_face = amount_face.split(' ')
    amount_face = int(amount_face[2])

    #TODO
    #strip the header to this file and save over or 
    #strip header and save as new
    #prefer new

    f.close()

    with open(inFile, 'rb') as f:
        new_file = open(outFile,'w')

        #loading bar
        const_mod = amount_verts / 40
        mod = const_mod
        print('converting from binary to ASCII')
        print('<', end='', flush=True)

        #hacky appeneding to the file so we can ingest into unity
        for i in range(0,11):
            new_file.write("\n")


        #read the vertices and write to file
        for i in range(0,amount_verts):

            buffer = f.read(12)
            data = struct.unpack('<3f',buffer)

            #loading barg
            if i == mod:
                print('-', end='', flush=True)
                mod += const_mod


            #error check_ likely something went wrong
            for a in data:
                if int(a) > 100 or int(a) < -100:
                    print()
                    print(data)
                    print(int(a))
                    print("we got a problem on line :" + str(i))
                    exit()

            #print(str(i) + ": " +str(data))
            new_file.write(str(data[0])+ ' ' + str(data[1]) + ' ' +str(data[2])+'\n')

            #read the RGB and write to file
            buffer = f.read(3)
            data = struct.unpack('<3B',buffer)
            new_file.write(str(data[0])+ ' ' + str(data[1]) + ' ' +str(data[2])+'\n')

            #print(str(i) + ": " +str(data))


        #close the file
        f.close()
        new_file.close()
        print("->")
        print("file successfully exported to ASCII format as '" + outFile +"'")

if __name__ == "__main__":
    main(sys.argv[1:])
