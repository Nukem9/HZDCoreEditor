$targetConfig = "Release"

dotnet publish HZDCoreEditor -c $targetConfig /p:DebugType=None /p:DebugSymbols=false /p:PublishProfile=FolderProfile
dotnet publish HZDCoreSearch -c $targetConfig /p:DebugType=None /p:DebugSymbols=false /p:PublishProfile=FolderProfileNoRT
dotnet publish HZDCoreTools -c $targetConfig /p:DebugType=None /p:DebugSymbols=false /p:PublishProfile=FolderProfileNoRT
dotnet publish HZDCoreEditorUI -c $targetConfig /p:DebugType=None /p:DebugSymbols=false /p:PublishProfile=FolderProfileNoRT