NAnt Build Server : Continous Integration with NAnt

*** IMPORTANT NOTES ***
=======================
* The database configured to be at c:\work\nant\extras\BuildServer\db\BuildServer.mdb.  Change the web.config and BuildServer.Tests\DatabaseTest.cs to reflect the actual location.

* In order for ASP.NET to access the database it must have write permission in the database folder.  Under Windows XP you can do this by giving the asp_wp account administer permissions.  If you do not do this you will not be able to share the database, ie, keep the database open in Access AND view the web site.  Problems might also occur if more than one database operations is underway.


What is this?
=============
A web based build server for NAnt.  The idea came from Mozilla's TinderBox and CruiseControl for Java/Ant.  Google for these terms for more info.


FEATURES
========
* start a build from web site
* view build log in progress by refeshing buildqueue.aspx
* build log captured and saved to database
* build result captured and saved as database field
* contents of build queue shown

TODO
====

* main page shows color coded status of project's last build result, age, and date completed (any SQL wizards out there?)
* in build history use Age rather than a date, eg, 3 hours
* anotate a build with comments (ie, message board for a build)
* password protected admin section where projects can be added, edited and deleted
* security to prevent build from destroying server with <delete> or <exec>
* schedule builds to run a specific times (ie, once a night)
* use a server-based timer to query CVS to detect changes and add a build to the queue if detected (make this generic enough to watch *anything* so users can write custom watchers
* do the same for ndoc, mono, and any other projects using nant
* get a <cvs> task for nant


Low Pri
-------
* cleanup html
* cleanup source code
* user documentation
* use css for datagrids and other asp elements instead of hard coding colors/fonts in the .aspx
* add sorting to build history datagrid
* add sorting to project list
