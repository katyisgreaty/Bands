# _Band Tracker_

#### _Band and Venue database webpage, 3.3.17_

#### By _**Katy Daviscourt**_

## Description

_This webpage allows the user to manage their database of bands and venues with a many-to-many relationship. In other words, multiple bands can perform at multiple venues, and venues can host multiple bands._

## Setup/Installation Requirements

* Clone the git from the repository at https://github.com/katyisgreaty/Bands.
* In Powershell, navigate to the git's location
* In SQLCMD:
    * > CREATE DATABASE band_tracker;
    * > GO
    * > USE band_tracker;
    * > GO
    * > CREATE TABLE bands (id INT IDENTITY(1,1), name VARCHAR(255));
    * > CREATE TABLE venues (id INT IDENTITY(1,1), name VARCHAR(255));
    * > CREATE TABLE venues (id INT IDENTITY(1,1), band_id INT, venue_id INT);
    * > GO
* Backup and restore database, naming the restored database band_tracker_test
* Type "dnx kestrel" into Powershell
* In the browser, to go "localhost:5004" to view webpage


## Specifications

### Bands:
* The get all method gets a list of all bands
  * ex input: getall
  * ex output: list of all bands

* The user can add a new band and it will save in the database
  * ex input: The Lumineers
  * ex output: saved

* The find method can use the band id to find the band
  * ex input: find band at id 2
  * ex output: The Weeknd

* The get all venues method will get all venues the specified band has played at
  * ex input: find all venues associated with the Lumineers
  * ex output: Marymoor Park, The Moore

### Venues:
* The get all method gets a list of all venues
  * ex input: getall
  * ex output: list of all venues

* The user can add a new venue and it will save in the database
  * ex input: The Moore
  * ex output: saved

* The find method can use the venue id to find the venue
  * ex input: find venue at id 2
  * ex output: Showbox Sodo

* The get all bands method will get all bands the specified venue has hosted
  * ex input: find all bands associated with Marymoor Park
  * ex output: The Lumineers, The Weeknd

* The update method allows the user to change the venue name
  * ex input: old name: Marymoor Park, new name: The Gorge
  * ex output: The Gorge

* The Delete method allows the user to delete a single venue
  * ex input: Delete The Gorge
  * ex output: deleted

* The DeleteAll method will delete all the venues
  * ex input: delete all
  * ex output: blank list


## Ice Box Specifications

* The update band method allows the user to change the band name
  * ex input: old name: Train, new name: Dave Matthews
  * ex output: Dave Matthews

* The Delete band method allows the user to delete a single band
  * ex input: Delete Dave Matthews
  * ex output: deleted

* The DeleteAllBands method will delete all the bands
  * ex input: delete all
  * ex output: blank list




## Known Bugs

_None at this time._

## Support and contact details

_For questions or comments please contact Katy at katy.daviscourt@gmail.com_

## Technologies Used

_C#, MSSQL_

### License

*Available under an MIT License*

Copyright (c) 2017 **_Katy Daviscourt_**
