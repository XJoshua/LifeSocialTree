rem @echo off
pip install pyexcel-xlsx
pip install pyexcel
pip install gspread
pip install oauth2client
pip install tk
@REM if exist "C:\Users\wangxinji\Downloads\I2Loc WaterGun Localization.xlsx" (
@REM 	MOVE "C:\Users\wangxinji\Downloads\I2Loc WaterGun Localization.xlsx" .\python_tools
@REM 	echo "rsync to python_tools\I2Loc WaterGun Localization.xlsx"
@REM )
python %~dp0python_tools\GetGoogleSpreadSheet.py local
pause