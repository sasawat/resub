# resub
Remove subtitles for lines with only known words in Anime!

## 1. Dependencies

### 1.1. IBM Watson 

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
  
### 1.2. ffmpeg

ffmpeg.exe must be in the same directory as resub.exe. You can download ffmpeg from https://www.ffmpeg.org/

### 1.3. MKVToolnix

mkvmerge and mkvextract are required by resub. It is easiest to download the prebuilt executables from 
https://mkvtoolnix.download/downloads.html#windows and put mkvmerge.exe and mkvextract.exe into the same directory as resub.exe, but you can also install them anywhere on your computer and put the paths in resub.conf, or put the
installed path into your PATH environment variable. 

### 1.4. .NET 4.5.2

resub requires .NET framework 4.5.2. It comes preinstalled with Windows 10 and I think earlier Windows versions will 
get it automatically in an update. 

## 2. resub.conf

resub.conf should be placed in the same directory as resub.exe. A template resub.conf has been provided. It assumes
that mkvmerge and mkvextract are either in the PATH environment variable or in the same director as resub.exe. It
also uses the 123M2.txt sample dictionary that has been provided. 

## 3. Dictionaries

resub uses dictionary files to identify what words you know. A sample 123M2.txt dictionary has been provided that
has words based on what a student who has completed Module 2 of Yuta Mori's ASIANLAN 123 class at UMich would know.
Please reference this sample dictionary when writing your own dictionaries. I also plan to have dictionaries 
written for 123 Module 4, 124 Module 3, and 124 Module 5, and possibly high level Japanese classes at UMich. 

The following lists features you can use in dictionary files. These features are limited by my own knowledge of 
Japanese. If you want to cooperate on making more advanced dictionaries and want additional features, please talk 
to me and I will try to implement them. 

### 3.1 Known Numbers

To avoid having to list lots and lots of numbers, a line Numeral,#### should be defined. resub will treat all 
numbers less than or equal to that number as known. Numeral supports only up to 99999. 

### 3.2 Counters

If Numeral,#### has been defined, Counter,K can be used. Counter specifies a counter. Numbers with the counter at
the end will be treated as known. 

### 3.3 Verb Conjugations

VerbConjugate,FORM1,FORM2,... defines which verb conjugations are to be treated as known. The available forms are: 
MASU,PLAIN,TE,TEIRU,TARI,TAI

When defining a word in a dictionary file, use u,word or ru,word to have resub automatically conjugate them. Note
that irregular verbs have to be defined with each conjugation provided as resub cannot automatically conjugate.

### 3.4 Adjective Conjugations

AdjConjugate,FORM1,FORM2,... defines which adjective conjugations are to be treated as known. The available forms 
are: PLAIN,TE,PAST,PASTNEGATIVE,NEGATIVE

When defining a word in a dictionary file, use i,word or na,word to have resub automatically conjugate them. 

### 3.5 Other Words

Just list any other words as a line in the dictionary
