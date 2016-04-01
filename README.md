# resub
Remove subtitles for lines with only known words in Anime!

1. Dependencies

1.1 IBM Watson 

resub uses the IBM Watson web service Speech to Text API. You will need to create an IBM Bluemix Account and put the 
API Username and Password into resub.conf to use the program. 

To do this:
  1.1.1 Create an IBM Bluemix Account
  1.1.2 From the Bluemix Dashboard, click "Use Services or APIs"
  1.1.3 Select "Speech to Text"
  1.1.4 Click "Create"
  1.1.5 Back from the Bluemix Dashboard, under the "Services" header, select the newly created Speech to Text service
  1.1.6 On the sidebar, select "Service Credentials"
  1.1.7 The username and password for the API service are on the main part of the screen in the box
  
1.2 ffmpeg

ffmpeg.exe must be in the same directory as resub.exe. You can download ffmpeg from https://www.ffmpeg.org/

1.3 MKVToolnix

mkvmerge and mkvextract are required by resub. It is easiest to download the prebuilt executables from 
https://mkvtoolnix.download/downloads.html#windows and put mkvmerge.exe and mkvextract.exe into the same directory as
resub.exe, but you can also install them anywhere on your computer and put the paths in resub.conf

1.4 .NET 4.5.2

resub requires .NET framework 4.5.2. It comes preinstalled with Windows 10 and I think earlier Windows versions will 
get it automatically in an update. 
