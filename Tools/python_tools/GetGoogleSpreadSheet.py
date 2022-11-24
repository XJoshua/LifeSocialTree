# -*- coding: utf-8 -*-
## 下载google spread内容到csv
import os,sys
import argparse
import re
import shutil
import openpyxl
import tkinter
import tkinter.messagebox

import csv
import gspread
from oauth2client.service_account import ServiceAccountCredentials

excelName = "I2Loc Localization.xlsx"
docName = "I2Loc Localization"
thisFilePath = os.path.dirname(__file__)

# SENSER_WORDS = [
#     thisFilePath + "/sensitive-custom.txt",
#     thisFilePath + "/sensitive-政治类.txt"
# ]

def SensitiveFilter(key, content):
    if None != content and isinstance(content, str) and (("Loading" in content) or ("Error" in content)):
        print("Find Loading/Error" + key)

## =GOOGLETRANSLATE\((.*),"zh-cn","en"\)
## =GOOGLETRANSLATE($1;"zh-cn";"en")

def DownloadGData():
    client = gspread.service_account(thisFilePath + "/service_account.json")
    spreadsheet = client.open(docName)
    worksheet_list = spreadsheet.worksheets()
    cacheWorksheet = []
    for i, worksheet in enumerate(worksheet_list):
        ws = {'row':worksheet.get_all_values(), 'title':worksheet.title}
        cacheWorksheet.append(ws)
    print("fetch end, start filter")
    return WriteToTxt(cacheWorksheet)

def ReadLocalExcel():
    excelPath = thisFilePath + "/" + excelName
    if not os.path.exists(excelPath):
        print("not found local file:" + excelPath)
        return False
    wb = openpyxl.load_workbook(filename = excelPath)
    worksheet_list = wb.worksheets
    cacheWorksheet = []
    for i, worksheet in enumerate(worksheet_list):
        rows = []
        for r in worksheet.values:
            row = []
            row.append(r[0])
            row.append(r[1])
            row.append(r[2])
            if type(row[1] == float) and str(row[1]).endswith(".0"):
                row[1] = int(row[1])
            rows.append(row)
        ws = {'row':rows, 'title':worksheet.title}
        cacheWorksheet.append(ws)
    return WriteToTxt(cacheWorksheet)
    # f = open(excelPath, 'rb')
    # print(excelPath)
    # dfDicts = pd.read_excel(f, sheet_name=None)
    # cacheWorksheet = []
    # for dfKey in dfDicts:
    #     # ws = {'row':rows, 'title':i}
    #     df = dfDicts[dfKey]
    #     rows = []
    #     for index, row in df.iterrows():
    #         rows.append(row)
    #     ws = {'row':rows, 'title':dfKey}
    #     cacheWorksheet.append(ws)
    # # print(cacheWorksheet)
    # return WriteToTxt(cacheWorksheet)

def WriteToTxt(cacheWorksheet):
    filename = docName + '.txt'
    f = open(thisFilePath + "/" + filename, 'w', encoding='utf-8', newline = '')
    writer = csv.writer(f)
    isFirstKey = True
    keyCache = []
    for worksheet in cacheWorksheet:
        # if i >= 2:
        #     break
        rows = worksheet['row']
        needSwap = False
        for row in rows:
            if row[0] == "Keys" or row[0] == "Key":
                if row[2].startswith("Chinese"):
                    needSwap = True
                break
        for r in rows:
            if None == r[0]:
                continue
            row = []
            row.append(r[0])
            row.append(r[1])
            row.append(r[2])
            if row[0] == "Keys":
                if isFirstKey:
                    isFirstKey = False
                    row[0] = "Key"
                    row[1] = "Chinese"
                    row[2] = "English (United States)"
                else:
                    continue
            else:
                row[0] = worksheet['title'] + "/" + row[0]
                SensitiveFilter(row[0], row[1])
                # SensitiveFilter(row[0], row[2])
                if needSwap:
                    tmp = row[1]
                    row[1] = row[2]
                    row[2] = tmp
            if row[0] in keyCache:
                print("Duplicate Key:" + row[0] + " " + row[1])
            keyCache.append(row[0])
            #print(row[3])
            #print(row[0:3])
            #break
            writer.writerow(row[0:3])
        # writer.writerows(rows)
        # print(rows)
    f.close()
    return True

def CopyToEditor():
    filename = docName + '.txt'
    csvFilePath = thisFilePath + "/" + filename
    shutil.move(csvFilePath, thisFilePath + "/../../Assets/Editor/" + filename)

def Main(argv):
    ret = False
    try:
        # useLocal = tkinter.messagebox.askokcancel(title='选择', message='直接使用本地文件？需要已经下载下载google文档')
        # if useLocal:
        if len(argv) > 1 and argv[1] == "local":
            ret = ReadLocalExcel()
        else:
            ret = DownloadGData()
    except Exception as e:
        ## exception
        print("Get SpreadSheet exception " + str(e))
    if not ret:
        ret = ReadLocalExcel()
    if ret:
        print("Read Succeed!")
        CopyToEditor()
    else:
        print("Read Error!")
    return ret

if __name__ == "__main__":
    if not Main(sys.argv):
        exit(-1)