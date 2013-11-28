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
  * FindObject (Async)
  * PutObject
  * PutObject (Async)
  * GetObject
  * GetObject (Async)
  * DeleteObject
  * DeleteObject (Async)
* Amazon DynamoDB
  * GetItem
  * GetItem (Async)
  * PutItem
  * PutItem (Async)
  * UpdateItem
  * UpdateItem (Async)
  * DeleteItem
  * DeleteItem (Async)
  * BatchWritePut
  * BatchWritePut (Async)
  * BatchWriteDelete
  * BatchWriteDelete (Async)
  * Scan
  * Scan (Async)
* Amazon SQS
  * SendMessage
  * SendMessage (Async)
  * SendMessages
  * SendMessages (Async)
  * ReceiveMessage
  * ReceiveMessage (Async)
  * ReceiveMessages
  * ReceiveMessages (Async)
  * DeleteMessage
  * DeleteMessage (Async)
  * DeleteMessages
  * DeleteMessages (Async)
  * ClearQueue
  * ClearQueue (Async)

Update 0.3
* Added Support for .NET 4.5.1 / WinRT (Windows 8.1).
* Improved SimpleDB Library.
* Improved SQS Library.
* Added Async Methods for SQSQueue.
* Improved File Structure.
* Improved Exception Handling.
* Uses the AWSSDK 2.0.2.3.

Update 0.2
* Added Support for .NET 4.5 / Windows Phone 8 / WinRT (Windows 8).
* Added Async Methods.
* Added Unit Tests.
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