# resub
Remove subtitles for lines with only known words in Anime!

Before:

![before](http://i.imgur.com/PKzgzfG.png)

After:

![after](http://i.imgur.com/CBJbls7.png)

すごいですね

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

Multiple dictionaries can be listed, separated by commas with NO SPACES. resub will create a single output file
with multiple subtitle tracks, each corresponding with a dictionary/dictionaryname. 

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

## 4. Limitations

resub is far from perfect

### 4.1 Noise and Music

Speech recognition technology is only so good. If a scene is really noise or has strong background music, resub
will be unable to determine the speech in that scene and cannot remove subtitle lines with known words. 

### 4.2 Signs

resub ignores typeset signs entirely. Plaintext signs might be accidentally removed if they coincide with dialogue
that contains only known words. 

### 4.3 Bad Timing

resub uses subtitle timing to determine the start and end of phrases. If subtitles start after speech starts, or if
subtitles start too far in advance of speech starting, resub will be unable to process correctly. 

### 4.4 Subtitles that don't correspond with spoken text

Sometimes translators rearrange what is being said over multiple subtitle lines. resub does not deal with this 
very well. It will just remove the directly corresponding subtitle line regardless of subtitle content, if the 
speech content only contains known words. 

### 4.5 Network Access

resub uses a web service to do speech to text so it does require pretty good internet access. However, it does log
transcription results in the .ilog file. This means if you want to rerun resub with a different dictionary 
configuration, internet access is not required (nor are your IBM Watson Speech to Text minutes used). 

## 5. Usage

Just open the application. You can click on it or launch it from the command line. 

### 5.1 Interactive Usage

Just type in the path to the file you want to resub and hit enter. You can also drag and drop the file into 
the window and it will fill out the path for you. You can hit enter again to let resub decide where the output
file should go or specify one. Let the program run and make yourself some コーヒー or something. 

![example usage](http://i.imgur.com/M5vLKRc.png)

### 5.2 Command Line Scriptable Usage

resub can be launched with command line arguments so you can write a script that resubs your entire anime 
archive or something (note that IBM Watson is only free for 1000 minutes a month, and after that it's $0.02 a min)

The format is

> resub.exe input.mkv output.mkv

or if you want to let resub decide what to name your output file

> resub.exe input.mkv
