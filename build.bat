@echo OFF

:: Regedit stuff to fetch the game installation.
:: TODO: add error checking?

setlocal ENABLEEXTENSIONS
set KEY_NAME=HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 255710
set VALUE_NAME=InstallLocation

FOR /F "tokens=2*" %%A IN ('REG.exe query "%KEY_NAME%" /v "%VALUE_NAME%"') DO (set installDir=%%B)

:: Set environment variables for make

set DIR_GAME=%installDir%
set DIR_GAME_LOCAL=%LOCALAPPDATA%/Colossal Order/Cities_Skylines

make && make install