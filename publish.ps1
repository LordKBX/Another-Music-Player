# load XML file into local variable and cast as XML type.
$ct = Get-Content "D:\\CODES\\VS\\MusicPlayer2\\AnotherMusicPlayer\\AnotherMusicPlayer.csproj" -encoding utf8
$doc = [xml]($ct)

$v = [version]$doc.Project.PropertyGroup.Version
$v = [version]::New($v.Major,$v.Minor,$v.Build,$v.Revision+1)

$AppVersion = ""
$InstallerVersion = "1.0.0"
$ret = Read-Host "Version number(default:" $v.ToString() ")"

if($ret.ToString().Trim() -eq ""){
    $AppVersion = $v.ToString()
}
else{
    $AppVersion = $ret.ToString().Trim()
}
$doc.Project.PropertyGroup.Version = $AppVersion
#$doc.Project.PropertyGroup.AssemblyVersion = $AppVersion
#$doc.Project.PropertyGroup.FileVersion = $AppVersion
$Utf8NoBomEncoding = New-Object System.Text.UTF8Encoding $False
[System.IO.File]::WriteAllLines("D:\\CODES\\VS\\MusicPlayer2\\AnotherMusicPlayer\\AnotherMusicPlayer.csproj", $doc.OuterXml, $Utf8NoBomEncoding)
exit


$confirmation = Read-Host "Installer version(default:" $InstallerVersion ", n=abort compiling)"
if ($confirmation.ToString().Trim() -eq 'n') {
	
}
else {
	if ($confirmation.ToString().Trim() -eq '') {
		}
	else{
		$InstallerVersion = $confirmation.ToString().Trim()
		}
    "Compiling"
    dotnet publish D:\CODES\VS\MusicPlayer2 -p:PublishProfile=AnyCPU
    cmd /C "D:\Program Files (x86)\Inno Setup 6\ISCC.exe" D:\CODES\VS\MusicPlayer2\Installer-AnyCPU.iss /DMyInstallerVersion=$InstallerVersion
}