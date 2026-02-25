@echo off
echo ================================================
echo TECHGEAR - CLEAN AND REBUILD SCRIPT
echo ================================================
echo.

echo [1/5] Closing Visual Studio processes...
taskkill /F /IM devenv.exe 2>nul
timeout /t 2 /nobreak >nul

echo [2/5] Cleaning bin and obj folders...
cd /d "%~dp0"
for /d /r . %%d in (bin,obj) do @if exist "%%d" rd /s /q "%%d"

echo [3/5] Cleaning Razor temp files...
del /f /q "%LOCALAPPDATA%\Temp\*.cshtml" 2>nul

echo [4/5] Cleaning NuGet packages cache...
dotnet nuget locals all --clear

echo [5/5] Rebuilding solution...
dotnet build TechGear.sln --configuration Debug --no-incremental

echo.
echo ================================================
echo DONE! Press any key to exit...
echo ================================================
pause >nul
