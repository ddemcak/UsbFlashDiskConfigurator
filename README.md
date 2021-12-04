# USB Flash Disk Configurator

Handy tool for automatiing the process of USB Flash Disk creation.

### Motivation
* Need to automate process of Boot pendrive creation?
* Or creating different kind of memory stick?

## Features
* Enables to define configuration steps in XML file.
* Allows to choose from avaliable Flash disks.
* Offeres multiple different configurations to choose from.
* Warning safety window to asure the user that all data will be lost when Format step is included.
* [USBKEY] wild string can be used in configuration. It is replaced by current drive letter during process.
* Custom logo can be used.

### List of supported configuration steps
* **format** - Formats USB Flash Drive. Requires 1 parameter - *filesystem*. Supports there options: FAT, FAT32, EXFAT,  NTFS, UDF.
* **replacetext** - Finds and replace string in text file. Requires 3 parameters - *filename*, *findString*, *replaceWithString*.
* **download** - Downloads anything from URL, typically some package like ZIP, TAR, etc. Requires 2 parameters - *remoteSource* and *localFilename*.
* **unpack** - Unpacks ZIP, TAR. Requires 2 parameters - *filename* and *localDirectory* where package will be unpacked.
* **execute** - Runs *.bat* file. Requires 1 parameter - *filename* of the executable.
* **move** - Moves file or directory. Requires 2 parameters - *MoveFromLocation* and *MoveTolocation*.
* **eject** - Ejects USB Flash Drive. No paramters required.
* **userinput** - User can input string during runtime and user it in different step. This step must be defined before the step that uses the data. Currently only *replatetext* step supports it.

## Example of config.xml

```xml
<?xml version="1.0" encoding="utf-8"?>
<AppConfiguration xmlns="http://tempuri.org/ConfigurationFile.xsd">
	<Title>USB bootable drive</Title>
	<Version>100</Version>
	<CompanyLogoPath>ms-logo.png</CompanyLogoPath>
	<Configurations>
		<Configuration Name="Small OS" Description="Minimalistic OS">
			<Step Type="format" Description="Format USB drive">
				<Parameter>FAT32</Parameter>
			</Step>
			<Step Type="download" Description="Download from URL">
				<Parameter>https://examaple.com/small-os.zip</Parameter>
				<Parameter>small-os.zip</Parameter>
			</Step>
			<Step Type="unpack" Description="Unpack archive">
				<Parameter>small-os.zip</Parameter>
				<Parameter>[USBKEY]\</Parameter>
			</Step>
			<Step Type="execute" Description="Make USB drive bootable">
				<Parameter>[USBKEY]\makeboot.bat</Parameter>
			</Step>
			<Step Type="userinput" Description="Enter Timeout">
				<Parameter>[USER_INPUT]</Parameter>
			</Step>
			<Step Type="replacetext" Description="Change boot timeout">
				<Parameter>[USBKEY]\log.txt</Parameter>
				<Parameter>OPTION 0</Parameter>
				<Parameter>TIMEOUT [USER_INPUT]</Parameter>
			</Step>
			<Step Type="eject" Description="Eject USB drive"/>
		</Configuration>
		<Configuration Name="Format to FAT32" Description="Prepare empty USB drive">
			<Step Type="format" Description="Format USB drive">
				<Parameter>FAT32</Parameter>
			</Step>
			<Step Type="eject" Description="Eject USB drive"/>
		</Configuration>
	</Configurations>
</AppConfiguration>

``` 