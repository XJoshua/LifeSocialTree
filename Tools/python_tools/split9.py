import os
import sys
import stat
from PIL import Image

def getFiles():
	files = os.listdir('.')
	for file in files:
		if file[-4:].lower() == ".png":
			checkFiles(file)

def checkFiles(file, l, r, t, b):
	data = file[:-4].split("_")
	if file[-4:].lower() == ".png":
		left = l + 1
		right = r + 1
		top = t + 1
		bottom = b + 1
		if left == 1 and right == 1 and top == 1 and bottom == 1:
			print("skips file " + file)
			return
		im = Image.open(file)
		im.save(file[:-4] + ".backup.png")
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
	else:
		print("skips file " + file)

fileName = sys.argv[1]
l = int(sys.argv[2])
r = int(sys.argv[3])
t = int(sys.argv[4])
b = int(sys.argv[5])
checkFiles(fileName, l, r, t, b)