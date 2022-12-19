# ASP.NET Core 6 (MVC) Event Management Prototype

## Description

This Application is intended to be used by administrative catering company staff. It was designed to allow the user to manage and store, client, employee, and event information, as well as schedule and staff upcoming events. 

## Features 
* Automatic MSSQL database creation and data migration using Entity Framework
* User login authentication (with dynamically created Welcome message and Logout link)
* Dynamic data tables with highlighting selectable rows, and search functionality
* Modal Add and Edit forms
* Modal validation messages, confirmation messages, and alerts
* Schedule validation that prohibits: 
	* Saving event or employee schedules outside of business hours
	* Saving overlapping employee schedules
	* Saving employee schedules outside of employee’s selected availability
* Overtime checking functionality that allows user to view employee overtime on the day or week of an event
* Automated depency deletion upon modification of event dates, or removal of events, employees, or clients.
* Sample NUnit tests for scheduling validations

## Required Environment to Run Application
* Windows 10 or higher
* MSSQL Server 
* Visual Studio 2019 or higher  
* ASP.NET and web development workload for Visual Studio

## Documentation

Documentation including the following can be viewed [HERE](https://drive.google.com/drive/folders/1eW93DNmTUzO45pPOrCRPjzz2Jma5Sw-g?usp=sharing)
* User Guide
* Design Documentation 
	* Class Diagrams
	* Wireframes
* NUnit Testing
	* Running NUnit Tests in Visual Studio
	* Sample Test Walkthrough

