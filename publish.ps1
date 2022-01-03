# load XML file into local variable and cast as XML type.
$doc = [xml](Get-Content "D:\\CODES\\VS\\MediaPlayer\\AnotherMusicPlayer\\AnotherMusicPlayer.csproj")

$v = [version]$doc.Project.PropertyGroup.Version
$v = [version]::New($v.Major,$v.Minor,$v.Build,$v.Revision+1)

$ArbitraryVersion = ""
$ret = Read-Host "Version number(default:" $v.ToString() ")"

if($ret.ToString().Trim() -eq ""){
    $ArbitraryVersion = $v.ToString()
}
else{
    $ArbitraryVersion = $ret.ToString().Trim()
}
$doc.Project.PropertyGroup.Version = $ArbitraryVersion
$doc.Project.PropertyGroup.AssemblyVersion = $ArbitraryVersion
$doc.Project.PropertyGroup.FileVersion = $ArbitraryVersion
$doc.save("D:\\CODES\\VS\\MediaPlayer\\AnotherMusicPlayer\\AnotherMusicPlayer.csproj")

$confirmation = Read-Host "Compiling app ?(y+enter|enter = yes, anything else = no)"
if ($confirmation -eq 'y') {
    "Compiling"
    dotnet publish D:\CODES\VS\MediaPlayer -p:PublishProfile=Win-X64
    dotnet publish D:\CODES\VS\MediaPlayer -p:PublishProfile=Win-X86
    cmd /C "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" D:\CODES\VS\MediaPlayer\Installer-x64.iss
    cmd /C "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" D:\CODES\VS\MediaPlayer\Installer-x86.iss
}
if ($confirmation.Trim() -eq '') {
    "Compiling"
    dotnet publish D:\CODES\VS\MediaPlayer -p:PublishProfile=Win-X64
    dotnet publish D:\CODES\VS\MediaPlayer -p:PublishProfile=Win-X86
    cmd /C "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" D:\CODES\VS\MediaPlayer\Installer-x64.iss
    cmd /C "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" D:\CODES\VS\MediaPlayer\Installer-x86.iss
}