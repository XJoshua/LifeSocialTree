import os
import sys
import stat
from PIL import Image
from struct import *
from zlib import *

def paeth(a, b, c):
    p = a + b -c
    Pa = abs(p - a)
    Pb = abs(p - b)
    Pc = abs(p - c)
    if Pa <= Pb and Pa <= Pc:
        r = a
    elif Pb <= Pc:
        r = b
    else:
        r = c
    return r

def stoi(s):
    if type(s) == str:
        return ord(s)
    return s

def splitImage(file, l, r, t, b):
    left = l + 1
    right = r + 1
    top = t + 1
    bottom = b + 1
    if left == 1 and right == 1 and top == 1 and bottom == 1:
        print("skips file " + file)
        return
    im = Image.open(file[:-4] + ".backup.png")
    xsize, ysize = im.size
    saveim = None
    if left == 1 and right == 1:
        p1 = im.crop((0, ysize-bottom, xsize, ysize))
        im.paste(p1, (0, top, xsize, top+bottom))
        saveim = im.crop((0, 0, xsize, top + bottom))
    elif top == 1 and bottom == 1:
        p1 = im.crop((xsize-right, 0, xsize, ysize))
        im.paste(p1, (left, 0, left+right, ysize))
        saveim = im.crop((0, 0, left + right, ysize))
    else:
        p1 = im.crop((xsize-right, 0, xsize, top))
        p2 = im.crop((0, ysize-bottom, left, ysize))
        p3 = im.crop((xsize-right,  ysize-bottom, xsize, ysize))
        im.paste(p1, (left, 0, left+right, top))
        im.paste(p2, (0, top, left, top+bottom))
        im.paste(p3, (left, top, left+right, top+bottom))
        saveim = im.crop((0, 0, left + right, top + bottom))
    im.close()
    saveim.save(file)
    saveim.close()
    metaFile = file + ".meta"
    f = open(metaFile, "r")
    allLines = f.readlines()
    f.close()
    f = open(metaFile, "w")
    for line in allLines:
        if line.find("spriteBorder") > 0:
            content = "  spriteBorder: {{x: {}, y: {}, z: {}, w: {}}}\r\n".format(l, b, r, t)
            f.write(content)
        else:
            f.write(line)
    f.close()

def parseImage(filename, x0, y0):
    x1 = x0 - 1
    x2 = x0 + 1
    y1 = y0 - 1
    y2 = y0 + 1
    newdata = []
    width = 0
    height = 0
    pngheader = "\x89PNG\r\n\x1a\n"
    file = open(filename, "rb")
    oldPNG = file.read()
    file.close()

    newPNG = oldPNG[:8]
    chunkPos = len(newPNG)
    idatAcc = "".encode()
    # For each chunk in the PNG file
    while chunkPos < len(oldPNG):
        skip = False
        # Reading chunk
        chunkLength = oldPNG[chunkPos:chunkPos+4]
        chunkLength = unpack(">L", chunkLength)[0]
        chunkType = oldPNG[chunkPos+4 : chunkPos+8]
        chunkData = oldPNG[chunkPos+8:chunkPos+8+chunkLength]
        chunkPos += chunkLength + 12
        # Parsing the header chunk
        if chunkType == "IHDR".encode():
            width = unpack(">L", chunkData[0:4])[0]
            height = unpack(">L", chunkData[4:8])[0]
        # Parsing the image chunk
        if chunkType == "IDAT".encode():
            # Store the chunk data for later decompression
            idatAcc = idatAcc + chunkData
            skip = True
        # Add all accumulated IDATA chunks
        if chunkType == "IEND".encode():
            try:
                # Uncompressing the image chunk
                bufSize = width * height * 4 + height
                chunkData = decompress( idatAcc)
            except Exception:
                # The PNG image is normalized
                print("decompress error")
                return 0, 0, 0, 0
            chunkType = "IDAT".encode()
            i = 0
            print(len(chunkData))
            for y in range(height):
                method = stoi(chunkData[i])
                i += 1
                j = 0
                newdata.append([])
                for x in range(width):
                    r = stoi(chunkData[i+0])
                    g = stoi(chunkData[i+1])
                    b = stoi(chunkData[i+2])
                    a = stoi(chunkData[i+3])
                    if method == 0:
                        method = 0
                    elif method == 1:
                        if x != 0:
                            r1 = newdata[y][j-4]
                            g1 = newdata[y][j-3]
                            b1 = newdata[y][j-2]
                            a1 = newdata[y][j-1]
                            r += r1
                            g += g1
                            b += b1
                            a += a1
                            if r > 255:
                                r -= 256
                            if g > 255:
                                g -= 256
                            if b > 255:
                                b -= 256
                            if a > 255:
                                a -= 256
                    elif method == 2:
                        r1 = newdata[y-1][j+0]
                        g1 = newdata[y-1][j+1]
                        b1 = newdata[y-1][j+2]
                        a1 = newdata[y-1][j+3]
                        r += r1
                        g += g1
                        b += b1
                        a += a1
                        if r > 255:
                            r -= 256
                        if g > 255:
                            g -= 256
                        if b > 255:
                            b -= 256
                        if a > 255:
                            a -= 256
                    elif method == 3:
                        if x != 0:
                            r1 = newdata[y][j-4] + newdata[y-1][j+0]
                            g1 = newdata[y][j-3] + newdata[y-1][j+1]
                            b1 = newdata[y][j-2] + newdata[y-1][j+2]
                            a1 = newdata[y][j-1] + newdata[y-1][j+3]
                        else:
                            r1 = newdata[y-1][j+0]
                            g1 = newdata[y-1][j+1]
                            b1 = newdata[y-1][j+2]
                            a1 = newdata[y-1][j+3]
                        r += r1 / 2
                        g += g1 / 2
                        b += b1 / 2
                        a += a1 / 2
                        if r > 255:
                            r -= 256
                        if g > 255:
                            g -= 256
                        if b > 255:
                            b -= 256
                        if a > 255:
                            a -= 256
                    elif method == 4:
                        if x != 0:
                            r0 = newdata[y-1][j-4]
                            r1 = newdata[y][j-4]
                            g0 = newdata[y-1][j-3]
                            g1 = newdata[y][j-3]
                            b0 = newdata[y-1][j-2]
                            b1 = newdata[y][j-2]
                            a0 = newdata[y-1][j-1]
                            a1 = newdata[y][j-1]
                        else:
                            r0 = 0
                            r1 = 0
                            g0 = 0
                            g1 = 0
                            b0 = 0
                            b1 = 0
                            a0 = 0
                            a1 = 0
                        r2 = newdata[y-1][j+0]
                        g2 = newdata[y-1][j+1]
                        b2 = newdata[y-1][j+2]
                        a2 = newdata[y-1][j+3]
                        r += paeth(r1, r2, r0)
                        g += paeth(g1, g2, g0)
                        b += paeth(b1, b2, b0)
                        a += paeth(a1, a2, a0)
                        if r > 255:
                            r -= 256
                        if g > 255:
                            g -= 256
                        if b > 255:
                            b -= 256
                        if a > 255:
                            a -= 256
                    else:
                        method = 0
                    if a == 0:
                        r = 0
                        g = 0
                        b = 0
                    newdata[y].append(r)
                    newdata[y].append(g)
                    newdata[y].append(b)
                    newdata[y].append(a)
                    i += 4
                    j += 4
    line = newdata[y0]
    end = y1
    length = y1 - 1
    endcheck = False
    for y in range(0, end):
        checkline = newdata[length - y]
        for x in range(width*4):
            if abs(line[x] - checkline[x]) > 1:
                endcheck = True
                break
        if endcheck == True:
            break
        y1 -= 1
    start = y2
    endcheck = False
    for y in range(start, height):
        checkline = newdata[y]
        for x in range(width*4):
            if abs(line[x] - checkline[x]) > 1:
                endcheck = True
                break
        if endcheck == True:
            break
        y2 += 1
    row = []
    for y in range(height):
        row.append(newdata[y][x0*4+0])
        row.append(newdata[y][x0*4+1])
        row.append(newdata[y][x0*4+2])
        row.append(newdata[y][x0*4+3])
    end = x1
    length = x1 - 1
    endcheck = False
    for x in range(0, end):
        checkX = length - x
        for y in range(height):
            if abs(row[4*y+0] - newdata[y][checkX*4+0]) > 1:
                endcheck = True
                break
            if abs(row[4*y+1] - newdata[y][checkX*4+1]) > 1:
                endcheck = True
                break
            if abs(row[4*y+2] - newdata[y][checkX*4+2]) > 1:
                endcheck = True
                break
            if abs(row[4*y+3] - newdata[y][checkX*4+3]) > 1:
                endcheck = True
                break
        if endcheck == True:
            break
        x1 -= 1
    start = x2
    endcheck = False
    for x in range(start, width):
        for y in range(height):
            if abs(row[4*y+0] - newdata[y][x*4+0]) > 1:
                endcheck = True
                break
            if abs(row[4*y+1] - newdata[y][x*4+1]) > 1:
                endcheck = True
                break
            if abs(row[4*y+2] - newdata[y][x*4+2]) > 1:
                endcheck = True
                break
            if abs(row[4*y+3] - newdata[y][x*4+3]) > 1:
                endcheck = True
                break
        if endcheck == True:
            break
        x2 += 1
    print(x1,x2,y1,y2)
    return x1+1, width-x2+1, y1+1, height-y2+1

def checkFiles(fileName, x, y):
    im = Image.open(fileName).convert('RGBA')
    backupFile = fileName[:-4] + ".backup.png"
    im.save(backupFile)
    im.close()
    l, r, t, b = parseImage(backupFile, x, y)
    splitImage(fileName, l, r, t, b)

fn = sys.argv[1]
x = int(sys.argv[2])
y = int(sys.argv[3])
checkFiles(fn, x, y)