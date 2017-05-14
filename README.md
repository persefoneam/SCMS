# SCMS
test for studets course managment system

I created this test app in 2 days. It has a Data access Layer and the MVC layer (this is just a test but in the reallity an application should have a businnes layer with all the nusinnes logic and a service layer in order to access the data and the logic from another kind of interfaces) .

I used Entity Framework 6 (code first), MVC 5 and MS unit testing (I preffer NUnit but it was faster that to install another package and plugin to my VS). 

The entity framework has a seed method that inserts in the dabase 3 users and 1 course. 
User 1. Login: 123 Pass:1234
User 2. Login: 456 Pass:1234
User 3. Login: 789 Pass:1234 (This is the course's teacher). 

This application allow users to upload files grouped in directories for course. And aldo it allows to created evaluated activities in which the students can upload their work and receive a score. 
It should have also (should, it dosnt have) a CRUD to upload teachers and courses and the relations whitin them. I Assume tha the university employees are the ones that enroll and register people. 
It should also have a table with allowed activities for rol to manage it. 

Somethings are pending:
-Profile managment (change user data and passwords).
-More validations 
-More graphics and controls (some UI framework like angular or react). 
-More tests.  
-CRUD for users, courses and rols. 
-Delete auntomatic generated and unused code

I managed the authentication with sessions, but it shoul be managed with httpcontext id.
Thanks. 
Adriana. 
