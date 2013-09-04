Simple AWS SDK C#
=========================

A Simple AWS SDK written in C#

Services under development:
* Amazon DynamoDB
* Amazon SimpleDB
* Amazon Simple Storage Service
* Amazon Simple Queue Service

Current Capabilities:
* Amazon S3
  * FindObject
  * PutObject
  * GetObject
  * DeleteObject
* Amazon DynamoDB
  * GetItem
  * PutItem
  * UpdateItem
  * DeleteItem
  * BatchWrite (Put & Delete)
  * Scan

Update 0.2
* Added Support for .NET 4.5/Windows Phone 8/WinRT.
* Added Async Methods.
* Added Unit Tests
* Uses the AWSSDK 2.0.0.3 beta.
* Restored NuGet.

Update 0.1
* Added Support for Mono / .NET 4.0.
* Added S3 Library.
* Added Tests for DynamoDB Library.
* Improved DynamoDB Library.
* Improved SimpleDB Library.
* Improved SQS Library.
* Removed NuGet.

Furure Changes:
* Bug Fixes
* Improved Tests
* Further Rearchitecting